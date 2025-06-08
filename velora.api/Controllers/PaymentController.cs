using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using velora.services.Services.CartService.Dto;
using velora.services.Services.PaymentService;

namespace velora.api.Controllers
{
    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        // ✅ Remove the extra space at the end of the secret!
        const string endpointSecret = "whsec_a3eaa61d94e0e1a153475d809fc7c29fdea130fe4c61a4c2fe927d2cc0adafd6";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("create-or-update-intent")]
        public async Task<ActionResult<CustomerCartDto>> CreateOrderUpdatePaymentIntent(CustomerCartDto input)
            => Ok(await _paymentService.CreateOrUpdatePaymentIntent(input));

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"].FirstOrDefault();

            if (string.IsNullOrEmpty(stripeSignature))
            {
                _logger.LogWarning("Missing Stripe-Signature header.");
                return BadRequest("Missing Stripe-Signature header.");
            }

            Event stripeEvent;

            try
            {
                // ✅ Construct the Stripe event (this validates the signature)
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, endpointSecret);
            }
            catch (StripeException ex)
            {
                _logger.LogError("Stripe webhook signature verification failed: {Message}", ex.Message);
                return BadRequest("Stripe signature verification failed.");
            }

            try
            {
                // ✅ Use switch statement for clarity and safety
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        var succeededIntent = stripeEvent.Data.Object as PaymentIntent;
                        _logger.LogInformation("Payment succeeded: {PaymentIntentId}", succeededIntent?.Id);

                        var succeededOrder = await _paymentService.UpdateOrderPaymentSucceeded(succeededIntent.Id);
                        _logger.LogInformation("Order updated to payment succeeded: {OrderId}", succeededOrder.Id);
                        break;

                    case "payment_intent.payment_failed":
                        var failedIntent = stripeEvent.Data.Object as PaymentIntent;
                        _logger.LogInformation("Payment failed: {PaymentIntentId}", failedIntent?.Id);

                        var failedOrder = await _paymentService.UpdateOrderPaymentFailed(failedIntent.Id);
                        _logger.LogInformation("Order updated to payment failed: {OrderId}", failedOrder.Id);
                        break;

                    case "payment_intent.created":
                        _logger.LogInformation("Payment intent created.");
                        break;

                    default:
                        _logger.LogWarning("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                        break;
                }


                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling Stripe event.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
