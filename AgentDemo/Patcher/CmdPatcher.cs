using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using HarmonyLib;

namespace AgentDemo.Patcher
{
    class Cmd
    {

        [HarmonyPatch(typeof(Process))]
        [HarmonyPatch(nameof(Process.Start), new Type[] { })]
        public class ProcessStart
        {
            public static bool Prefix()
            {
                return true;
            }

        }

        // [HarmonyPatch(typeof(StreamWriter))]
        // [HarmonyPatch("WriteLine", new Type[] {typeof(string)})]
        // public class StreamWriter:BasePatcher
        // {
        //     public static bool Prefix(string value)
        //     {
        //         if (value is null)
        //         {
        //             throw new ArgumentNullException(nameof(value));
        //         }

        //         Checker.SendCheckRequest("cmd");
        //         Checker.Check(new Checker.Cmd(), value, GetStackTrace());
        //         return true;
        //     }

        // }

    }
}
