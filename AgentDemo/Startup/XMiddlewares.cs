using Microsoft.AspNetCore.Http;

namespace AgentDemo.Startup
{
    public class XMiddlewares
    {
        public static RequestDelegate TestMain(RequestDelegate next)
        {
            return async context =>
            {
                Debuger.WriteLine(XTool.HttpHelper.GetUrl(context.Request));

                await next(context);
            };
        }
    }
}
