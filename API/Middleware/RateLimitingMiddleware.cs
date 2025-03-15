using Application.Services;

namespace API.Middleware
{
    public class RateLimitingMiddleware(RequestDelegate next)
    {
        private static readonly Dictionary<string, RequestInfo> Requests = [];
        private const int RequestLimit = 100;
        private const int TimeFrameInSeconds = 60;

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ip)) return;

            if (!Requests.TryGetValue(ip, out RequestInfo? requestInfo))
            {
                Requests[ip] = new RequestInfo { RequestCount = 1, LastRequestTime = DateTime.UtcNow };
            }
            else
            {
                var elapsed = DateTime.UtcNow - requestInfo.LastRequestTime;

                if (elapsed.TotalSeconds < TimeFrameInSeconds)
                {
                    if (requestInfo.RequestCount >= RequestLimit)
                    {
                        context.Response.StatusCode = 429;
                        return;
                    }
                    requestInfo.RequestCount++;
                }
                else
                {
                    requestInfo.RequestCount = 1;
                    requestInfo.LastRequestTime = DateTime.UtcNow;
                }
            }

            await next(context);
        }

        public class RequestInfo
        {
            public int RequestCount { get; set; }
            public DateTime LastRequestTime { get; set; }
        }
    }

}
