using AgentDemo.Logic;
using Microsoft.AspNetCore.Http;
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
                Type type = XPatchAttribute.GetPatchedClassType<ExecuteReader>();
                string sqlCommand = type.GetProperty("CommandText").GetValue(__instance).ToString();
                CheckLogic.SQL.CheckAsync(sqlCommand);
                return true;
            }
        }
    }
}
