/* ==============================================================================
* 功能描述：HttpPatcher  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/13 8:45:42
* ==============================================================================*/
using HarmonyLib;
using System;
using Microsoft.AspNetCore.Hosting.Internal;

namespace AgentDemo.Patcher
{
    [HarmonyPatch(typeof(HostingApplication))]
    [HarmonyPatch(nameof(HostingApplication.ProcessRequestAsync))]
    [HarmonyPatch(new Type[] { typeof(HostingApplication.Context)})]
    class ProcessRequest
    {
        //'Microsoft.AspNetCore.Hosting.Internal.HostingApplication' from assembly 'Microsoft.AspNetCore.Hosting, 
        public static bool Prefix()
        {
            Debuger.WriteLine("Test");
            return true;
        }
    }

    /*
    class Http
    {

        [HarmonyPatch(typeof(ApplicationBuilderFactory))]
        [HarmonyPatch(nameof(ApplicationBuilderFactory.CreateBuilder))]
        [HarmonyPatch(new Type[] {typeof(IFeatureCollection)})]
        class ApplicationConfigure
        {
            public static bool Prefix()
            {
                Debuger.WriteLine("Test");
                return true;
            }
        }

        #region
        // [HarmonyPatch(typeof(ApplicationBuilder))]
        // [HarmonyPatch(MethodType.Constructor)]
        // [HarmonyPatch(new Type[] {typeof(IServiceProvider), typeof(object)})]
        // // [MyPatch("Microsoft.AspNetCore.Http","Microsoft.AspNetCore.Builder.Internal.ApplicationBuilder",null,new Type[] {typeof(IServiceProvider), typeof(object)})]
        // class ApplicationBuilderConstructor
        // {
        //     public static bool Prefix()
        //     {
        //         Debuger.WriteLine("Test");
        //         return true;
        //     }
        // }

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

        [HarmonyPatch(typeof(HttpClient))]
        [HarmonyPatch(nameof(HttpClient.GetAsync),new Type[] { typeof(string)})]
        class ClientGet { 
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

        [HarmonyPatch(typeof(HttpClient))]
        [HarmonyPatch(nameof(HttpClient.PostAsync),new Type[] { typeof(string),typeof(HttpContent)})]
        class ClientPost { 
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
        #endregion

    }
    */

}
