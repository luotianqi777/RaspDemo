using AgentDemo.Json;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AgentDemo
{
    public partial class Checker
    {
        public class SQL: AbstractChecker
        {
            // sql关键字列表
            private static readonly string[] sqlCommandKeywordList = "and|exec|insert|select|drop|grant|alter|delete|update|count|chr|mid|master|truncate|char|declare|or|*|;|+|'|%".Split('|')
                .ToArray();

            public override bool CheckInfo (string info, bool isPayload)
            {
                var userInputStartIndex = isPayload ? -1 : info.IndexOf("=");
                var checkedInfo = info.Substring(userInputStartIndex + 1).ToLower().Trim();
                foreach(var keyWord in sqlCommandKeywordList)
                {
                    if (checkedInfo.Contains(keyWord))
                        return true;
                }
                return false;
            }

        }
    }
}
