using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RaspDemo
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ÒÀÀµ using Microsoft.AspNetCore.Http
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("test/{id}", async context =>
                 {
                     var id = context.Request.RouteValues["id"];
                     string result = MySqlManger.ExecQuery(id.ToString());
                     await context.Response.WriteAsync(result);
                 });

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello");
                });
            });
        }
    }
}
