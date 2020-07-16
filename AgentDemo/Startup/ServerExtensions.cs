/* ==============================================================================
* 功能描述：ServerExtensions  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/10 16:41:16
* ==============================================================================*/
using AgentDemo.Patcher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace AgentDemo.Startup
{
    public class PatchStartupService : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int maxLoopTimes = 2;
            int currentLoopTimes = 0;
            while (!stoppingToken.IsCancellationRequested && currentLoopTimes++ < maxLoopTimes)
            {
                await Task.Run(() => BasePatcher.PatchAll());
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    /// <summary>
    /// Http中间件
    /// </summary>
    class HttpMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Debuger.WriteLine(context.Request.Body);
            await next(context);
        }
    }

    /// <summary>
    /// 拓展服务
    /// </summary>
    public static class HttpExtensions
    {
        public static void UseHttpService(this IApplicationBuilder app)
        {
            RequestDelegate middle(RequestDelegate next)
            {
                return context => {
                    Debuger.WriteLine(context.Request.Path);
                    return next(context); };
            }
            app.Use(middle);
        }
    }


}
