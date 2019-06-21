using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class UserDAO
    {
        /// <summary>
        /// 验证用户
        /// </summary>
        public User VerifyUser(MySqlConnection conn,string username,string password)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username =@username and password =@password",conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    //确实有这个用户就返回一个user对象
                    Console.WriteLine("有这个用户,账号密码是对的");
                    return new User(id, username, password);
                }
                else
                {
                    Console.WriteLine("没有这个用户，或者账号密码不对");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("VerifyUser方法验证用户失败" + e);
            }
            finally
            {
                if (reader!=null)
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 检查数据库是否有此用户
        /// </summary>
        /// <returns></returns>
        public bool GetUserFromData(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username =@username", conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("数据库已经有这个用户了");
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUserFromData方法获取用户失败" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return false;
        }


        /// <summary>
        /// 添加用户到数据库
        /// </summary>
        /// <returns></returns>
        public void AddUserToData(MySqlConnection conn, string username, string password)
        {
            try
            {
                //insert into login set user=@us,password=@pwd
                MySqlCommand cmd = new MySqlCommand("insert into user set username =@username,password =@password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                //执行语句（不是查询）
                cmd.ExecuteNonQuery();
                Console.WriteLine("注册用户成功了");
            }
            catch (Exception e)
            {
                Console.WriteLine("AddUserToData方法添加用户失败" + e);
            }
         
        }

    }
}
