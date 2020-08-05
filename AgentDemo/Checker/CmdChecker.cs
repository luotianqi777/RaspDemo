using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public partial class Checker
    {
        public class Cmd : AbstractChecker
        {
            private readonly static string[] keywords = "dir,|,&".Split(',');
            public override bool CheckInfo(string info, bool isPayload)
            {
                foreach (string keyword in keywords)
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
