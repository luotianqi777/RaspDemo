/* ==============================================================================
* 功能描述：Debuger  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/6 18:03:22
* ==============================================================================*/
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public static class Debuger
    {
        public static void WriteLine(string message)
        {
            FileLog.Log(message);
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
