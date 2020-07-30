using AgentDemo.Json;
using Microsoft.VisualBasic;
using System;
using System.Linq;

namespace AgentDemo.Logic
{
    partial class CheckLogic
    {
        public static class SQL
        {
            // sql关键字列表
            private static readonly string[] sqlCommandKeywordList = "and|exec|insert|select|drop|grant|alter|delete|update|count|chr|mid|master|truncate|char|declare|or|*|;|+|'|%".Split('|')
                .Select(str => { return str.Length == 1 ? str : $" {str} "; }).ToArray();

            /// <summary>
            /// 判断一条sql语句有没有注入风险
            /// </summary>
            /// <param name="sqlCommand">sql语句</param>
            /// <returns>有风险返回true，反之返回false</returns>
            public static bool IsInject(string sqlCommand)
            {
                var userInputStartIndex = sqlCommand.IndexOf("=");
                if (userInputStartIndex == -1) return false;
                var userInput = sqlCommand.Substring(userInputStartIndex + 1).ToLower().Trim();
                foreach(var keyWord in sqlCommandKeywordList)
                {
                    if (userInput.IndexOf(keyWord) != -1)
                        return true;
                }
                return false;
                // var tokenString = userInput.Split(sqlCommandKeywordList, StringSplitOptions.RemoveEmptyEntries);
                // return tokenString.Length > 1;
            }

            /// <summary>
            /// 对sql语句进行检查
            /// </summary>
            /// <param name="sqlCommand">sql语句</param>
            public static async void CheckAsync(string sqlCommand)
            {
                if (IsInject(sqlCommand))
                {
                    Debuger.WriteLine("有注入风险");
                    var bugInfo = XJson.BugInfo.GetInstance();
                    Debuger.WriteLine(bugInfo);
                    await XJson.SendJsonMsg(bugInfo, AgentConfig.GetInstance());
                }
            }

        }
    }
}
