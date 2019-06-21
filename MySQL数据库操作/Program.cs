using System;
using MySql.Data.MySqlClient;


namespace MySQL数据库操作
{
    class Program
    {
        static void Main(string[] args)
        {
            //建立链接
            string connStr = "datasource=localhost;port=3306;user=root;pwd=;database=test007";
            MySqlConnection conn = new MySqlConnection(connStr);
            //打开链接
            conn.Open();

            //建立命令对象
            MySqlCommand cmd = new MySqlCommand("select * from login",conn);
            //执行命令 并返回一个reader对象
            MySqlDataReader reader = cmd.ExecuteReader();

            //如果read的到数据，返回true.不然返回false
            while (reader.Read())
            {
                //输出数据
                string user = reader.GetString("user");
                string password = reader.GetString("password");
                Console.WriteLine(user + ":" + password);
            }

            //关闭
            reader.Close();
            conn.Close();

            Console.ReadKey();
        }
    }
}
