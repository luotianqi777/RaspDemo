/* ==============================================================================
* 功能描述：StringPatcher  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/2 17:35:10
* ==============================================================================*/
using System;
using HarmonyLib;

namespace AgentDemo.Patcher
{
    // [HarmonyPatch(typeof(String), nameof(String.Insert),new Type[] { typeof(int), typeof(string)})]
    // class StringPatcher:BasePatcher
    // {
    //     static bool Prefix(ref String __instance)
    //     {
    //         Debuger.WriteLine($"string {__instance} hook success");
    //         return true;
    //     }

    // }
}
