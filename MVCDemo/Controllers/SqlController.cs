using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MVCDemo.Controllers
{
    public class SqlController : Controller
    {
        [HttpGet]
        public IActionResult Index(string id)
        {
            ViewData["Result"] = ExecQuery(id);
            return View();
        }

        private string ExecQuery(string id)
        {
            string ConnectSql = "server=localhost;port=3306;user=ltq;password=luotianqi;database=test;";
            string QuerySql = "select * from movie where id = ";
            StringBuilder queryResult = new StringBuilder();
            MySqlConnection sqlConnection;
            try
            {
                sqlConnection = new MySqlConnection(ConnectSql);
                sqlConnection.Open();
            }
            catch
            {
                Debuger.WriteLine("数据库连接失败");
                return string.Empty;
            }
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand(QuerySql + id, sqlConnection);
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
