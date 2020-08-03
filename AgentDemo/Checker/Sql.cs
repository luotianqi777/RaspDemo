using AgentDemo.Json;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AgentDemo
{
    public partial class Checker
    {
        public class SQL: IChecker
        {
            // sql关键字列表
            private static readonly string[] sqlCommandKeywordList = "and|exec|insert|select|drop|grant|alter|delete|update|count|chr|mid|master|truncate|char|declare|or|*|;|+|'|%".Split('|')
                // 这个地方为空格匹配(或者%20)
                // .Select(str => { return str.Length == 1 ? str : $" {str} "; })
                .ToArray();

            public bool IsBug(string sqlCommand)
            {
                var userInputStartIndex = sqlCommand.IndexOf("=");
                // 此处无需判断是否找到=，如果找不到index为-1，那么接下来会对整个sqlCommand进行检测
                var userInput = sqlCommand.Substring(userInputStartIndex + 1).ToLower().Trim();
                foreach(var keyWord in sqlCommandKeywordList)
                {
                    if (userInput.IndexOf(keyWord) != -1)
                        return true;
                }
                return false;
            }

        }
    }
}
