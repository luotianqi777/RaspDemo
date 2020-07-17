using AgentDemo.XHttpLisenter;
using AgentDemo.Patcher;
using AgentDemo.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(PatcherStartup))]
namespace AgentDemo.Startup
{

    public class PatcherStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostBuilderContext,configureBuilder) =>
            {
                BasePatcher.PatchAll();
            });
            builder.ConfigureServices((service) =>
            {
                // XHttpLisenter lisenter = new XHttpLisenter();
                // lisenter.Add("localhost",5000);
                // lisenter.Add("localhost",5001);
                // lisenter.Start();

                // // 通过这种方式来获取Application行不通
                // service.AddSingleton<HttpMiddleware>();
                // // service.BuildServiceProvider()
                // var app = new ApplicationBuilder(builder.Build().Services);
                // app.UseMiddleware<HttpMiddleware>();
                // app.UseHttpService();

                service.AddHostedService<PatchStartupService>();

            });
        }
    }
}
