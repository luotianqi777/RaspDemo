using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RaspDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>()
                    .Configure((app) =>
                    {
                        app.Use((next) =>
                        {
                            return async (context) =>
                            {
                                Debuger.WriteLine("C");
                                await next(context);
                                Debuger.WriteLine("C");
                            };
                        });
                    })
                    ;
                });
    }
}
