/* ==============================================================================
* 功能描述：BasePatcher  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/4 17:39:26
* ==============================================================================*/
using HarmonyLib;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
            harmony.UnpatchAll();
            harmony.PatchAll();
            Debuger.WriteLine("Patch all method!\nmethod list:");
            foreach (var value in harmony.GetPatchedMethods())
            {
                Debuger.WriteLine("-\t" + value.Name);
            }
        }

        public static void PrintStack()
        {
            Debuger.WriteLine(string.Join("->", (from stack in new StackTrace().GetFrames()
                                                 where stack.GetILOffset() != StackFrame.OFFSET_UNKNOWN && stack.GetNativeOffset() != StackFrame.OFFSET_UNKNOWN
                                                 select $"{stack.GetMethod().Name}()")
                                                .Reverse()));
        }
    }
}
