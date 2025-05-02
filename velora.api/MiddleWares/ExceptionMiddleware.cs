using velora.services.HandlerResponses;
using System.Net;
using System.Text.Json;

namespace velora.api.MiddleWares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment; 
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next  , IHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) {

                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //500

                var response = _environment.IsDevelopment()
                    ? new CustomException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new CustomException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);

            }
        }
    }
}
