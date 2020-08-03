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
                // 这个地方为空格匹配(或者%20)
                // .Select(str => { return str.Length == 1 ? str : $" {str} "; })
                .ToArray();

            public override bool CheckInfo (string info)
            {
                var userInputStartIndex = info.IndexOf("=");
                var userInput = info.Substring(userInputStartIndex + 1).ToLower().Trim();
                return CheckSqlBug(userInput);
            }

            public override bool CheckPayload(string payload)
            {
                return CheckSqlBug(payload.ToLower().Trim());
            }

            private bool CheckSqlBug(string sqlCommandParma)
            {
                foreach(var keyWord in sqlCommandKeywordList)
                {
                    if (sqlCommandParma.IndexOf(keyWord) != -1)
                        return true;
                }
                return false;
            }
        }
    }
}
