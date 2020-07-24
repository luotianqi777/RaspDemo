using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.Logic
{
    partial class Logic
    {
        public static class Cmd
        {
            public static bool IsInject(string cmd)
            {
                return false;
            }

            public static bool Check(string cmd)
            {
                return IsInject(cmd);
            }
        }
    }
}
