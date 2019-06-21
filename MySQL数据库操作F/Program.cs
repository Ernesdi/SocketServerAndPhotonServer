using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQL数据库操作F
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("2323");
            //建立链接
            string connStr = "datasource=localhost;port=3306;user=root;pwd=;database=test007";
            MySqlConnection conn = new MySqlConnection(connStr);
            //打开链接
            conn.Open();


            #region 查询
            ////建立命令对象
            //MySqlCommand cmd = new MySqlCommand("select * from login", conn);
            ////执行命令 并返回一个reader对象
            //MySqlDataReader reader = cmd.ExecuteReader();

            ////如果read的到数据，返回true.不然返回false
            //while (reader.Read())
            //{
            //    //输出数据
            //    string user = reader.GetString("user");
            //    string password = reader.GetString("password");
            //    Console.WriteLine(user + ":" + password);
            //}
            ////关闭
            //reader.Close();
            #endregion

            string user = "me";
            string password = "123456789";
            string cmdStr = "insert into login set user='"+ user +"',password='"+password+"'";
            MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
            //执行这条命令
            cmd.ExecuteNonQuery();

            conn.Close();

            Console.ReadKey();
        }
    }
}
