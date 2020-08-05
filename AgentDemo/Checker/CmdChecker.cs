using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public partial class Checker
    {
        public class Cmd : AbstractChecker
        {
            public override bool CheckInfo(string info, bool isPayload)
            {
                return true;
            }

        }
    }
}
