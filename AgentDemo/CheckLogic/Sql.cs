using AgentDemo.Json;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AgentDemo
{
    public partial class CheckLogic
    {
        public static class SQL
        {
            // sql关键字列表
            private static readonly string[] sqlCommandKeywordList = "and|exec|insert|select|drop|grant|alter|delete|update|count|chr|mid|master|truncate|char|declare|or|*|;|+|'|%".Split('|')
                // 这个地方为空格匹配(或者%20)
                // .Select(str => { return str.Length == 1 ? str : $" {str} "; })
                .ToArray();

            /// <summary>
            /// 判断一条sql语句有没有注入风险
            /// </summary>
            /// <param name="sqlCommand">sql语句</param>
            /// <returns>有风险返回true，反之返回false</returns>
            public static bool IsInject(string sqlCommand)
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
