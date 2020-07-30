using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace RaspDemo
{

    public static class Debuger
    {
        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }
    }

    public class MySqlManger
    {
        private static class SqlString
        {
            public static string Connect = "server=localhost;port=3306;user=ltq;password=luotianqi;database=test;";
            public static string Query = "select * from movie where id = ";
        }

        /// <summary>
        /// 查询测试
        /// </summary>
        public static string ExecQuery(string id)
        {
            StringBuilder queryResult = new StringBuilder();
            MySqlConnection sqlConnection;
            try
            {
                sqlConnection = new MySqlConnection(SqlString.Connect);
                sqlConnection.Open();
            }
            catch
            {
                Debuger.WriteLine("数据库连接失败");
                return string.Empty;
            }
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand(SqlString.Query + id, sqlConnection);
                MySqlDataReader mySqlDataReader = sqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    queryResult.Append(mySqlDataReader[1]);
                }
                mySqlDataReader.Close();
            }
            catch
            {
                Debuger.WriteLine("查询失败");
            }
            finally
            {
                sqlConnection.Close();
            }
            return queryResult.ToString();
        }

    }
}
