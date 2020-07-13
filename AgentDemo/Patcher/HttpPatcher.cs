/* ==============================================================================
* 功能描述：HttpPatcher  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/13 8:45:42
* ==============================================================================*/
using HarmonyLib;
using System;
using System.Net;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Hosting.Internal;

namespace AgentDemo.Patcher
{
    class Http
    {
        [HarmonyPatch(typeof(HttpListener))]
        [HarmonyPatch(nameof(HttpListener.GetContext))]
        class GetContext
        {
            public static bool Prefix()
            {
                Debuger.WriteLine("http request hook success");
                return true;
            }

            public static void Postfix()
            {
                Debuger.WriteLine("http request hook success");
            }
        }

        [HarmonyPatch(typeof(HttpListener))]
        [HarmonyPatch(nameof(HttpListener.GetContextAsync))]
        class GetContextAsync
        {
            public static bool Prefix()
            {
                Debuger.WriteLine("http request hook success");
                return true;
            }

            public static void Postfix()
            {
                Debuger.WriteLine("http request hook success");
            }
        }

    }
}
