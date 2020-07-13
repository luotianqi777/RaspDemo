using AgentDemo.Startup;
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
                // 通过这种方式来获取Application行不通
                // service.AddSingleton<HttpMiddleware>();
                // var app = new ApplicationBuilder(service.BuildServiceProvider());
                // app.UseMiddleware<HttpMiddleware>();
                // app.UseHttpService();
            });
            builder.ConfigureAppConfiguration((hostBuilderContext,configureBuilder) =>
            {
            });
            
        }
    }
}
