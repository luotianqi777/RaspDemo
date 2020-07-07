using System;

namespace AgentDemo.Patcher
{

    /// <summary>
    /// MySqlPatcher
    /// </summary>
    [MyPatch("MySql.Data.MySqlClient","MySqlCommand","ExecuteReader",new Type[] { typeof(System.Data.CommandBehavior) })]
    class MySqlPatcher: BasePatcher
    {
        public static bool Prefix(ref object __instance)
        {
            Type type = MyPatchAttribute.GetPatchedClassType<MySqlPatcher>();
            string sqlCommand = type.GetProperty("CommandText").GetValue(__instance).ToString();
            Debuger.WriteLine($"sql command {sqlCommand} hook success");
            return true;
        }
    }
}
