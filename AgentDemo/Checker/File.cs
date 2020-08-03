using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public partial class Checker
    {

        public class FileRead: AbstractChecker
        {

            private readonly static string[] keywards = "..|/|\\".Split('|');

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
        }

        public class FileUpload : AbstractChecker
        {

            public override bool CheckInfo(string info)
            {
                return true;
            }
        }
    }
}
