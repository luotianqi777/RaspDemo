using AgentDemo.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(PatcherStartup))]
namespace AgentDemo.Startup
{

    public class PatcherStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((service) =>
            {
                service.AddHostedService<PatchStartupService>();
                // service.AddSingleton<HttpMiddleware>();
                // // 通过这种方式来获取Application行不通
                // // service.BuildServiceProvider()
                // var app = new ApplicationBuilder(builder.Build().Services);
                // app.UseMiddleware<HttpMiddleware>();
                // app.UseHttpService();
            });
            builder.ConfigureAppConfiguration((hostBuilderContext,configureBuilder) =>
            {
            });
        }
    }
}
