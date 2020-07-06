using System;
using HarmonyLib;
using MySql.Data.MySqlClient;

namespace AgentDemo.Patcher
{

    /// <summary>
    /// MySqlPatcher
    /// </summary>
    // [MyPatch("MySql.Data.MySqlClient","MySqlCommand","ExecuteReader",new Type[] { })]
    [HarmonyPatch(typeof(MySqlCommand), nameof(MySqlCommand.ExecuteReader),new Type[] { })]
    class MySqlPatcher
    {
        public static void Patch()
        {
            Harmony harmony = new Harmony(nameof(MySqlPatcher));
            // if (MyPatchAttribute.IsPatch<MySqlPatcher>())
            // {
            //     Harmony harmony = new Harmony(nameof(MySqlPatcher));
            // }
        }

        public static bool Prefix(ref MySqlCommand __instance)
        {
            // Type type = MyPatchAttribute.GetPatchedClassType<MySqlPatcher>();
            // Debuger.WriteLine("sql command hook success");
            // string sqlCommand = type.GetProperty("CommandText").GetValue(__instance).ToString();
            // Debuger.WriteLine("sql command: "+sqlCommand);
            Debuger.WriteLine(__instance.CommandText);
            return true;
        }
    }
}
