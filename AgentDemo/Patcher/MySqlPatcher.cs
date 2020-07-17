using System;

namespace AgentDemo.Patcher
{

    class MySql
    {

        #region
        [MyPatch("MySql.Data", "MySql.Data.MySqlClient.BaseCommandInterceptor", "ExecuteNonQuery", null)]
        public class ExecuteNonQuery : BasePatcher
        {
            public static bool Prefix(string sql, ref int returnValue)
            {
                _ = returnValue;
                Debuger.WriteLine(sql);
                return true;
            }
        }
        #endregion

        [MyPatch("MySql.Data", "MySql.Data.MySqlClient.MySqlCommand", "ExecuteReader", new Type[] { typeof(System.Data.CommandBehavior) })]
        public class ExecuteReader : BasePatcher
        {
            public static bool Prefix(ref object __instance)
            {
                Type type = MyPatchAttribute.GetPatchedClassType<ExecuteReader>();
                string sqlCommand = type.GetProperty("CommandText").GetValue(__instance).ToString();
                Debuger.WriteLine(sqlCommand);
                // _ = __instance;
                return true;
            }
        }

    }
}
