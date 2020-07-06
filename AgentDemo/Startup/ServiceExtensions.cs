/* ==============================================================================
* 功能描述：ServiceExtensions  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/6 17:05:23
* ==============================================================================*/
using AgentDemo.Patcher;
using HarmonyLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AgentDemo.Startup
{
    public static class ServiceExtensions
    {

        public static void PatchAll()
        {
            Harmony harmony = new Harmony(nameof(ServiceExtensions));
            harmony.UnpatchAll();
            harmony.PatchAll();
            Debuger.WriteLine("Patch all method!\nmethod list:");
            foreach(var value in harmony.GetPatchedMethods())
                Debuger.WriteLine(value.Name);
        }

        public static IServiceCollection AddPatchService(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            // services.AddSingleton<BasePatcher, MySqlPatcher>();
            PatchAll();
            return services;
        }
    }
}
