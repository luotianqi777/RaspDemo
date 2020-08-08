using AgentDemo.Json;
using Microsoft.AspNetCore.Http;

namespace AgentDemo.Startup
{
    public class XMiddlewares
    {
        public static RequestDelegate TestMain(RequestDelegate next)
        {
            return async context =>
            {
                var request = XTool.HttpHelper.GetCurrentHttpRequest();
                Debuger.WriteLine($"url:{XTool.HttpHelper.GetUrl(request)}, method:{request.Method}");
                Debuger.WriteLine(XTool.HttpHelper.GetRequestBody(request));
                // Checker.SendCheckRequest();
                await next(context);
            };
        }
    }
}
