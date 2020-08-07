using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public partial class Checker
    {
        public class XXE : AbstractChecker
        {
            public static readonly string[] keywords = "ENTITY".Split('|');
            public override bool CheckInfo(string info, bool isPayload = false)
            {
                foreach(string keyword in keywords)
                {
                    if (info.Contains(keyword))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
