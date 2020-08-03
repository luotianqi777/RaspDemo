﻿using System;

namespace AgentDemo.Patcher
{

    class Sql
    {

        [XPatch("MySql.Data", "MySql.Data.MySqlClient.MySqlCommand", "ExecuteReader", new Type[] { typeof(System.Data.CommandBehavior) })]
        public class ExecuteReader : BasePatcher
        {
            public static bool Prefix(ref object __instance)
            {
                // 获取sql语句
                Type type = XPatchAttribute.GetPatchedClassType<ExecuteReader>();
                string sqlCommand = type.GetProperty("CommandText").GetValue(__instance).ToString();
                // 发送检测请求
                Checker.SendCheckRequest("sql");
                // IAST检测
                Checker.Check(new Checker.SQL(), sqlCommand, GetStackTrace());
                return true;
            }
        }
    }
}