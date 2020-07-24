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
            var stacks = new StackTrace().GetFrames();
            Debuger.WriteLine(string.Join("->", (from stack in stacks
                                                 where stack.GetILOffset() != StackFrame.OFFSET_UNKNOWN
                                                 select $"{stack.GetMethod().Name}()")
                                                .Reverse()));
        }
    }
}
