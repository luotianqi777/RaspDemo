﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public static class Debuger
    {
        public static void WriteLine(object message)
        {
            FileLog.Log(message.ToString());
            Console.WriteLine(message);
            // System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
