﻿/* ==============================================================================
* 功能描述：BasePatcher  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/4 17:39:26
* ==============================================================================*/
using HarmonyLib;
using System;

namespace AgentDemo.Patcher
{
    /// <summary>
    /// Patcher基类
    /// </summary>
    public abstract class BasePatcher
    {
        public static void PatchAll()
        {
            Harmony harmony = new Harmony(nameof(BasePatcher));
            try
            {
                harmony.UnpatchAll();
                harmony.PatchAll();
            }
            catch(Exception e)
            {
                Debuger.WriteLine(e.Message);
            }
            Debuger.WriteLine("Patch all method!\nmethod list:");
            foreach (var value in harmony.GetPatchedMethods())
            {
                Debuger.WriteLine("-\t" + value.Name);
            }
        }
    }
}
