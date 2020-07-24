﻿using AgentDemo.Patcher;
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

[assembly: HostingStartup(typeof(XHostingStartup))]
namespace AgentDemo.Startup
{

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
                service.AddTransient<IStartupFilter, HttpStartupFilter>();
            });
        }
    }

    #region
    /// <summary>
    /// 服务启动后的服务
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
    #endregion

    /// <summary>
    /// 中间件注册服务
    /// </summary>
    public class HttpStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Use(next =>
                {
                    return async context =>
                    {
                        // Debuger.WriteLine("middleware id:"+Tool.Http.GetUrl(context.Request)+context.TraceIdentifier);
                        string id = "NUZFOFSZCWUXGPLT";
                        string key = "EAB1OGMIFVGRGICI";
                        string nonce = "RODO:random";
                        string ip = "https://192.168.172.138:9090";
                        // XJson xJson = XJson.GetXJson(id, XRequest.GetInstance(context.Request));
                        // Debuger.WriteLine(xJson);
                        Tool.Http.RequestForward(ip, id, key, nonce, context.Request);
                        await next(context);
                    };
                });
                next(app);
            };
        }
    }

}