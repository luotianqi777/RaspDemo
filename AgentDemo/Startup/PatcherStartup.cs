using AgentDemo.Patcher;
using AgentDemo.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using System;
using Microsoft.AspNetCore.Builder.Internal;
using System.Collections.Generic;

[assembly: HostingStartup(typeof(MyHostingStartup))]
namespace AgentDemo.Startup
{

    public class MyHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostBuilderContext, configureBuilder) =>
            {
                BasePatcher.PatchAll();
            });
            builder.ConfigureServices((service) =>
            {
                service.AddHostedService<PatchStartupService>();
            });
            builder.UseStartup<MyStartup>();
        }
    }

    public class MyStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Use((next) =>{
                return async (context) =>
                {
                    Debuger.WriteLine("Test");
                    await next(context);
                };
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _ = services ?? throw new Exception("no services");
        }
    }

}
