using AgentDemo.Logic;
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
                Debuger.WriteLine(sqlCommand);
                if (SqlLogic.IsInject(sqlCommand))
                {
                    Debuger.WriteLine("有注入风险");
                }
                return true;
            }
        }
    }
}
