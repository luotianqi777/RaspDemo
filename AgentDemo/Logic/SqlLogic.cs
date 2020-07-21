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
        private static class RuleLists
        {
            public static readonly string[] sqlInjectBlackStringList = new string[] { "select", "0x", "or", "!", "/", "*", "=", ";" };
            public static readonly char[] sqlTokenList = new char[] { ' ' };
        }

        /// <summary>
        /// 判断一条sql语句有没有注入风险
        /// </summary>
        /// <param name="sqlCommand">sql语句</param>
        /// <returns>有风险返回true，反之返回false</returns>
        public static bool IsInject(string sqlCommand)
        {
            #region
            // 黑名单
            static bool UseBlackList(string userInput)
            {
                foreach (string keyword in RuleLists.sqlInjectBlackStringList)
                {
                    if (userInput.IndexOf(keyword) != -1)
                    {
                        return true;
                    }
                }
                return false;
            }
            // token
            static bool UseToken(string userInput)
            {
                var tokenString = from subString in userInput.Split(RuleLists.sqlTokenList)
                                  where subString.Length > 0
                                  select subString;
                return tokenString.ToArray().Length > 1;
            }
            #endregion

            var userInputStartIndex = sqlCommand.IndexOf("=");
            if (userInputStartIndex == -1) return false;
            var userInput = sqlCommand.Substring(userInputStartIndex + 1);
            return UseBlackList(userInput) || UseToken(userInput);
        }
        
    }
}
