using System;
using MySql.Data.MySqlClient;

namespace AgentDemo.Patcher
{

    class MySql
    {
        /// <summary>
        /// 获取要执行的Sql语句
        /// </summary>
        /// <typeparam name="T">Patcher类</typeparam>
        /// <param name="instance">被hook的对象实例</param>
        /// <returns>sql语句</returns>
        private static string GetCommandText<T>(object instance) where T:BasePatcher
        {
            Type type = MyPatchAttribute.GetPatchedClassType<T>();
            return type.GetProperty("CommandText").GetValue(instance).ToString();
        }

        [MyPatch("MySql.Data","MySql.Data.MySqlClient.MySqlCommand","ExecuteNonQuery",new Type[] {})]
        public class ExecuteNonQuery : BasePatcher
        {
            public static bool Prefix(ref MySqlCommand __instance)
            {
                string sqlCommand = GetCommandText<ExecuteNonQuery>(__instance);
                Debuger.WriteLine(sqlCommand);
                return true;
            }
        }

        [MyPatch("MySql.Data","MySql.Data.MySqlClient.MySqlCommand","ExecuteReader",new Type[] { typeof(System.Data.CommandBehavior) })]
        public class ExecuteReader : BasePatcher
        {
            public static bool Prefix(ref object __instance)
            {
                string sqlCommand = GetCommandText<ExecuteReader>(__instance);
                PrintStack();
                Debuger.WriteLine(sqlCommand);
                return true;
            }
        }

    }
}
