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
            Console.WriteLine("Patch all method!\nmethod list:");
            foreach(var value in harmony.GetPatchedMethods())
            {
                Console.WriteLine(value.Name);
            }
        }

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async (service) =>
            {
                // first patch
                PatchAll();
                // wait website startup
                await Task.Delay(2000);
                // i have no idea why must patch again
                // if not do that, patch don't working
                PatchAll();
            });
            
        }
    }
}
