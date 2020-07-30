using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ÒÀÀµ using Microsoft.AspNetCore.Http
            if (env.IsDevelopment())
            {
              //  app.UseDeveloperExceptionPage();
            }

            #region Route
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("sql/{id}", async context =>
                // {
                //     var id = context.Request.RouteValues["id"];
                //     string result = MySqlManger.ExecQuery(id.ToString());
                //     await context.Response.WriteAsync(result);
                // });

                endpoints.MapGet("cmd/{command}", async context =>
                {
                    var command = context.Request.RouteValues["command"];
                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    p.StandardInput.WriteLine($"{command}&exit");
                    p.StandardInput.AutoFlush = true;
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    p.Close();
                    await context.Response.WriteAsync(output);
                });

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello");
                });
            });
            #endregion

        }

        [HttpGet("id")]
        public string Sql(string id)
        {
            return MySqlManger.ExecQuery(id);
        }

    }
}
