using System;

namespace AgentDemo.Logic
{
    partial class CheckLogic
    {
        public static class SQL
        {
            // sql关键字列表
            public static readonly string[] sqlCommandKeywordList = "and|exec|insert|select|drop|grant|alter|delete|update|count|chr|mid|master|truncate|char|declare|or|*|;|+|'|%".Split('|');

            /// <summary>
            /// 判断一条sql语句有没有注入风险
            /// </summary>
            /// <param name="sqlCommand">sql语句</param>
            /// <returns>有风险返回true，反之返回false</returns>
            public static bool IsInject(string sqlCommand)
            {
                var userInputStartIndex = sqlCommand.IndexOf("=");
                if (userInputStartIndex == -1) return false;
                var userInput = sqlCommand.Substring(userInputStartIndex + 1).ToLower();
                var tokenString = userInput.Split(sqlCommandKeywordList, StringSplitOptions.RemoveEmptyEntries);
                return tokenString.Length > 1;
            }

            /// <summary>
            /// 对sql语句进行检查
            /// </summary>
            /// <param name="sqlCommand">sql语句</param>
            public static void Check(string sqlCommand)
            {
                if (IsInject(sqlCommand))
                {
                    Debuger.WriteLine("有注入风险");
                    Tool.XHttpHelper.GetCurrentHttpContext().Response.Redirect("/");
                }
            }

        }
    }
}
