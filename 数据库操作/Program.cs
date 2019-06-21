using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace 数据库操作
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

            #region 插入
            //string user = "me";
            //string password = "123456789';delete from login;";
            ////拼接字符串有注入的问题，使用参数的形势就可以了
            ////string cmdStr = "insert into login set user='" + user + "',password='" + password + "'";

            //MySqlCommand cmd = new MySqlCommand("insert into login set user=@us,password=@pwd", conn);
            ////使用@符号来标识参数，然后是用mysqlcommand里的参数来添加一个值。
            //cmd.Parameters.AddWithValue("us", user);
            //cmd.Parameters.AddWithValue("pwd", password);

            ////执行这条命令 执行一条 不查询
            //cmd.ExecuteNonQuery();

            #endregion

            #region 删除
            //MySqlCommand cmd = new MySqlCommand("delete from login where id = @id", conn);
            //cmd.Parameters.AddWithValue("id",8);
            //cmd.ExecuteNonQuery();
            #endregion

            #region 更新
            //MySqlCommand cmd = new MySqlCommand("update login set password = @pwd where id =@id",conn);
            //cmd.Parameters.AddWithValue("pwd", "solis555");
            //cmd.Parameters.AddWithValue("id", 7);
            //cmd.ExecuteNonQuery();

            #endregion

            conn.Close();
            Console.ReadKey();
        }
    }
}
