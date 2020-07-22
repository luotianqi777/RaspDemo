using AgentDemo.Logic;
using Microsoft.AspNetCore.Http;
using System;

namespace AgentDemo.Patcher
{

    class MySql
    {

        [MyPatch("MySql.Data", "MySql.Data.MySqlClient.MySqlCommand", "ExecuteReader", new Type[] { typeof(System.Data.CommandBehavior) })]
        public class ExecuteReader : BasePatcher
        {
            public static bool Prefix(ref object __instance)
            {
                Type type = MyPatchAttribute.GetPatchedClassType<ExecuteReader>();
                string sqlCommand = type.GetProperty("CommandText").GetValue(__instance).ToString();
                Debuger.WriteLine(Tool.Http.GetCurrentUrl());
                Debuger.WriteLine("mysql hook id:"+Tool.Http.GetCurrentHttpContext().TraceIdentifier);
                Debuger.WriteLine(sqlCommand);
                CheckLogic.SQL.Check(sqlCommand);
                return true;
            }
        }
    }
}
