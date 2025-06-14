using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using velora.services.Services.CartService.Dto;
using velora.services.Services.PaymentService;
using velora.services.Services.PaymentService.Dto;
namespace velora.api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController :APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private readonly IStripeClient _stripeClient;
        private readonly string _endpointSecret;

        public PaymentController(
     IPaymentService paymentService,
     ILogger<PaymentController> logger,
     IStripeClient stripeClient,
     IOptions<StripeSettings> stripeSettings)
        {
            _paymentService = paymentService;
            _logger = logger;
            _stripeClient = stripeClient;
            _endpointSecret = stripeSettings.Value.WebhookSecret;
        }

        [HttpPost("create-or-update-intent")]
        public async Task<ActionResult<CustomerCartDto>> CreateOrderUpdatePaymentIntent(CustomerCartDto input)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be resolved from token.");
            }

            input.UserId = userId;
            var result = await _paymentService.CreateOrUpdatePaymentIntent(input);
            return Ok(result);
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentRequest request)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency,
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService(_stripeClient);
            var paymentIntent = await service.CreateAsync(options);

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
        private async Task ProcessStripeEventAsync(Event stripeEvent)
        {
            try
            {
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        var intent = stripeEvent.Data.Object as PaymentIntent;
                        await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                        break;
                    case "payment_intent.payment_failed":
                        // handle failure
                        break;
                        // other event types...
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Stripe event");
            }
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // ✅ Log the remote IP address (already done, optional)
            _logger.LogInformation("Received webhook request from: {RemoteIpAddress}", HttpContext.Connection.RemoteIpAddress);

            // ✅ NEW: Log the Stripe-Signature header
            var stripeSignature = Request.Headers["Stripe-Signature"].FirstOrDefault();
            _logger.LogInformation("Stripe-Signature: {Signature}", stripeSignature);

            //// ✅ Optional: Log all headers
            //foreach (var header in Request.Headers)
            //{
            //    _logger.LogInformation("Header: {Key} = {Value}", header.Key, header.Value);
            //}

            // ✅ Check if Stripe-Signature is missing
            if (string.IsNullOrEmpty(stripeSignature))
            {
                _logger.LogWarning("Missing Stripe-Signature header.");
                return BadRequest("Missing Stripe-Signature header.");
            }

            // ✅ Use EventUtility to verify
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _endpointSecret);
            }
            catch (StripeException ex)
            {
                _logger.LogError("Stripe webhook signature verification failed: {Message}", ex.Message);
                return BadRequest("Stripe signature verification failed.");
            }

            // Fire-and-forget background handling
            await ProcessStripeEventAsync(stripeEvent);

            try
            {
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        var succeededIntent = stripeEvent.Data.Object as PaymentIntent;
                        _logger.LogInformation("✅ Payment succeeded: {PaymentIntentId}", succeededIntent?.Id);

                        var succeededOrder = await _paymentService.UpdateOrderPaymentSucceeded(succeededIntent.Id);
                        _logger.LogInformation("✅ Order updated to payment succeeded: {OrderId}", succeededOrder.Id);
                        break;

                    case "payment_intent.payment_failed":
                        var failedIntent = stripeEvent.Data.Object as PaymentIntent;
                        _logger.LogInformation("❌ Payment failed: {PaymentIntentId}", failedIntent?.Id);

                        var failedOrder = await _paymentService.UpdateOrderPaymentFailed(failedIntent.Id);
                        _logger.LogInformation("❌ Order updated to payment failed: {OrderId}", failedOrder.Id);
                        break;

                    default:
                        _logger.LogWarning("⚠️ Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error handling Stripe event.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}