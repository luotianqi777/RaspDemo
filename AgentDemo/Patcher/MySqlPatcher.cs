using System;

namespace AgentDemo.Patcher
{

    class MySql
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
                var context = XTool.HttpHelper.GetCurrentHttpContext();
                Checker.SendCheckRequest(context, "sql");
                // IAST检测
                Checker.Check(new Checker.SQL(), context, sqlCommand, "Sql调用栈");
                return true;
            }
        }
    }
}
