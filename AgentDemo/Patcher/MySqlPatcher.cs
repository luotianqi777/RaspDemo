using System;
using AgentDemo.Patcher;
using HarmonyLib;

namespace AgentDemo.Startup
{

    /// <summary>
    /// MySqlPatcher
    /// </summary>
    [MyPatch("MySql.Data.MySqlClient","MySqlCommand","ExecuteReader",new Type[] { })]
    class MySqlPatcher:BasePatcher
    {
        public static void Patch()
        {
            if (MyPatchAttribute.IsPatch<MySqlPatcher>())
            {
                Harmony harmony = new Harmony(nameof(MySqlPatcher));
            }
        }

        public static bool Prefix(ref object __instance)
        {
            Type type = MyPatchAttribute.GetPatchedClassType<MySqlPatcher>();
            Console.WriteLine("sql command: "+type.GetProperty("CommandText").GetValue(__instance));
            return true;
        }
    }
}
