using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class ResultDAO
    {


        /// <summary>
        /// 获得战绩表中的数据
        /// </summary>
        public Result GetResultFromData(MySqlConnection conn, int userid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from result where userid =@userid", conn);
                cmd.Parameters.AddWithValue("userId", userid);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalcount = reader.GetInt32("totalcount");
                    int wincount = reader.GetInt32("wincount");
                    //确实有这个用户就返回一个user对象
                    Console.WriteLine("查询到了userid为"+ userid.ToString()+"的战绩");
                    return new Result(id, userid, totalcount, wincount);
                }
                else
                {
                    Console.WriteLine("没有这个玩家的战绩信息，可能是新号");
                    //临时将id设置为-1 因为还没有储存到数据库中，所以-1没关系
                    return new Result(-1, userid, 0, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetResultFromData方法验证用户战绩失败" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;
        }


        /// <summary>
        /// 升级战绩表中的数据
        /// </summary>
        public void UpdateResultFromData(MySqlConnection conn, Result result)
        {
            Console.WriteLine("正在升级用户数据~----");
            try
            {
                MySqlCommand cmd = null;
                //是新号的话就插入
                if (result.Id <= -1)
                {
                    Console.WriteLine("插入战绩表");
                    cmd = new MySqlCommand("insert into result set totalcount =@totalcount,wincount =@wincount,userid = @userid", conn);
                }
                //不是新号咱就升级
                else
                {
                    Console.WriteLine("升级战绩表");
                    cmd = new MySqlCommand("update result set totalcount =@totalcount,wincount =@wincount where userid = @userid", conn);
                }
                cmd.Parameters.AddWithValue("totalcount", result.TotalCount);
                cmd.Parameters.AddWithValue("wincount", result.WinCount);
                cmd.Parameters.AddWithValue("userid", result.UserID);
                cmd.ExecuteNonQuery();
                //防止新号再次进行插入战绩。方法一：
                //新号再次判断，就会执行else语句
                if (result.Id <= -1)
                {
                    //查询到我刚刚插入的那个战绩，把形式参数的id设置为我刚刚查询到的id
                    Result tempRes = GetResultFromData(conn, result.UserID);
                    //修改此函数中的判断条件的值  不是修改传过来的成员变量，是引用传递。不是值传递
                    //我们自己写的类的对象作为参数,一般都是引用传递。string是特殊的引用传递，因为不会修改原来的值。
                    //例如if 一开始是-1<=-1,然后经过这次修改之后就变成了 2<=-1 ，false 然后就执行了else
                    result.Id = tempRes.Id;
                }

                //防止新号再次进行插入战绩。方法二：
                //也可以使用函数返回值的方式，插入完战绩表之后调用GetResultFromData方法把查询到的id再返回去，
                //然后进行修改result.id，这样下次调用这个方法的时候就不会再执行if语句了
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateResultFromData方法升级用户战绩失败" + e);
            }
        
        }
    }
}
