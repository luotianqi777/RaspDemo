﻿/* ==============================================================================
* 功能描述：StringPatcher  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/2 17:35:10
* ==============================================================================*/
using System;
using HarmonyLib;

namespace AgentDemo.Patcher
{
    [HarmonyPatch(typeof(String), nameof(String.Insert))]
    class StringPatcher
    {
        public static void Patch()
        {
            Harmony harmony = new Harmony(nameof(StringPatcher));
        }

        static bool Prefix(ref String __result)
        {
            Debuger.WriteLine("string insert hook success");
            __result = "\nthis string is hooked";
            return false;
        }

    }
}
