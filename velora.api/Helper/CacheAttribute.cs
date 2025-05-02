using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using velora.services.CacheService;

namespace velora.api.Helper
{
    public class CacheAttribute : Attribute , IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute( int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await _cacheService.GetCacheResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/html",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;

            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult response) 
            {
                await _cacheService.SetCacheResponseAsync(cacheKey,response.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }

        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
           StringBuilder cashKey = new StringBuilder();
            cashKey.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                cashKey.Append($"|{key}-{value}");

            return cashKey.ToString();
        }
    }
}
