using AgentDemo.Startup;
using HarmonyLib;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

[assembly: HostingStartup(typeof(PatcherStartup))]
namespace AgentDemo.Startup
{
    public class PatcherStartup : IHostingStartup
    {

        public static void PatchAll()
        {
            Harmony harmony = new Harmony(nameof(PatcherStartup));
            harmony.UnpatchAll();
            harmony.PatchAll();
            Debuger.WriteLine("Patch all method!\nmethod list:");
            foreach(var value in harmony.GetPatchedMethods())
            {
                Debuger.WriteLine("-\t" + value.Name);
            }
        }

        public void Configure(IWebHostBuilder builder)
        {
            //builder.UseConfiguration(async (context,config)=>
            // builder.ConfigureAppConfiguration(async (context, config) =>
            // {
            //     // first patch
            //     PatchAll();
            //     // wait website startup
            //     await Task.Delay(2000);
            //     // i have no idea why must patch again
            //     // if not do that, patch don't working
            //     PatchAll();
            // });
            builder.ConfigureServices(async (service) =>
            {
                // first patch
                PatchAll();
                // wait website startup
                // service.AddPatchService();
                await Task.Delay(2000);
                // service.AddPatchService();
                // i have no idea why must patch again
                // if not do that, patch don't working
                PatchAll();
            });
            
        }
    }
}
