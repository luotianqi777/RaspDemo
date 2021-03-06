﻿using System;

namespace AgentDemo.Patcher
{

    class MySql
    {

        [XPatch("MySql.Data", "MySql.Data.MySqlClient.MySqlCommand", "ExecuteReader", new Type[] { typeof(System.Data.CommandBehavior) })]
        public class ExecuteReader : BasePatcher
        {
            public static bool Prefix(ref object __instance)
            {
                Type type = XPatchAttribute.GetPatchedClassType<ExecuteReader>();
                string sqlCommand = type.GetProperty("CommandText").GetValue(__instance).ToString();
                var request = XTool.HttpHelper.GetCurrentHttpRequest();
                // 发送检测请求
                CheckLogic.SendCheckRequest(request, "sql");
                // IAST检测
                CheckLogic.Check(CheckLogic.SQL.IsInject, request, sqlCommand, "Sql调用栈");
                return true;
            }
        }
    }
}
