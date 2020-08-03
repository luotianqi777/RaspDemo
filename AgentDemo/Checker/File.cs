using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public partial class Checker
    {

        public class File: IChecker
        {

            public bool IsBug(string info)
            {
                return true;
            }
        }
    }
}
