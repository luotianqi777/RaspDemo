using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;

namespace AgentDemo
{
    public partial class Checker
    {

        public class FileRead: AbstractChecker
        {

            private readonly static string[] keywards = "%00|;|../|..\\".Split('|');

            public override bool CheckInfo(string info, bool isPayload)
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

            private readonly static string[] keywards = "html|%00|;".Split('|');

            public override bool CheckInfo(string info, bool isPayload)
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

            public override string PostPayload(HttpRequest request)
            {
                var files = request.Form.Files;
                StringBuilder dataBuilder = new StringBuilder();
                foreach (var formFile in files)
                {
                    string fileName = formFile.FileName;
                    dataBuilder.Append(fileName);
                }
                return dataBuilder.ToString();
            }
        }
    }
}
