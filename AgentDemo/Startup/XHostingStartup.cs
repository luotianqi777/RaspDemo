using AgentDemo.Patcher;
using AgentDemo.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.AspNetCore.Http;
using AgentDemo.Json;
using Newtonsoft.Json;

[assembly: HostingStartup(typeof(XHostingStartup))]
namespace AgentDemo.Startup
{

    #region HostingStartup
    public class XHostingStartup : IHostingStartup
    {
        // 服务启动前的服务
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostBuilderContext, configureBuilder) =>
            {
                BasePatcher.PatchAll();
            });
            builder.ConfigureServices((service) =>
            {
                service.AddHttpContextAccessor();
                service.AddHostedService<PatchStartupService>();
                service.AddHostedService<DokiStartupService>();
                service.AddTransient<IStartupFilter, HttpStartupFilter>();
            });
        }
    }
    #endregion

    #region BackgroundService
    /// <summary>
    /// 注册Hook
    /// </summary>
    public class PatchStartupService : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 只PatchAll一次不能保证成功
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
    /// 注册心跳
    /// </summary>
    public class DokiStartupService : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int interTime = 1000 * 10;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(async () =>
                {
                    var doki = XJson.Doki.Sender.GetInstance();
                    Debuger.WriteLine(doki);
                    // var response = 
                    await XJson.SendJsonMsg(doki);
                    // var rdoki = XJson.GetResponseJsonData(response, out _);
                    // Debuger.WriteLine(rdoki);
                });
                await Task.Delay(interTime, stoppingToken);
            }
        }
    }

    #endregion

    #region StartupFilter
    /// <summary>
    /// 中间件注册服务
    /// </summary>
    public class HttpStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Use(XMiddlewares.TestMain);
                next(app);
            };
        }
    }
    #endregion

}
