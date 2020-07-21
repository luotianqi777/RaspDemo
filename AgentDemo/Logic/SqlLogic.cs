/* ==============================================================================
* 功能描述：Sql  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/21 9:16:11
* ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AgentDemo.Logic
{
    class SqlLogic
    {
        // 规则列表
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
    }
}
