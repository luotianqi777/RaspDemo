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
        private static MySqlConnection sqlConnection;
        private static MySqlCommand sqlCommand;
        private static class SqlString
        {
            public static string Connect = "server=localhost;port=3306;user=ltq;password=luotianqi;database=test;";
            public static string Query = "select * from movie where id = ";
        }

        /// <summary>
        /// 查询测试
        /// </summary>
        public static string ExecQuery(int? id)
        {
            sqlConnection = new MySqlConnection(SqlString.Connect);
            sqlConnection.Open();
            sqlCommand = new MySqlCommand(SqlString.Query+id, sqlConnection);
            MySqlDataReader mySqlDataReader = sqlCommand.ExecuteReader();
            StringBuilder builder = new StringBuilder();
            while (mySqlDataReader.Read())
            {
                builder.Append(mySqlDataReader[1]);
            }
            mySqlDataReader.Close();
            sqlConnection.Close();
            string result = builder.ToString();
            _ = result.Insert(0, "test: ");
            return result;
        }
    }
}
