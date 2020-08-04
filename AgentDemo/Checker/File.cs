using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public partial class Checker
    {

        public class FileRead: AbstractChecker
        {

            private readonly static string[] keywards = "%00|;|../|..\\".Split('|');

            public override bool CheckInfo(string info)
            {
                foreach(string keyward in keywards)
                {
                    if (info.IndexOf(keyward) != -1)
                    {
                        return true;
                    }
                }
                return false;
            }

            public override string GetPayload()
            {
                throw new NotImplementedException();
            }
        }

        public class FileUpload : AbstractChecker
        {

            private readonly static string[] keywards = "cshtml|%00|;".Split('|');

            public override bool CheckInfo(string info)
            {
                foreach(string keyward in keywards)
                {
                    if (info.IndexOf(keyward) != -1)
                    {
                        return true;
                    }
                }
                return false;
            }

            public override string GetPayload()
            {
                throw new NotImplementedException();
            }
        }
    }
}
