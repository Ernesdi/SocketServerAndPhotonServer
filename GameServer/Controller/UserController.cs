using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;

namespace GameServer.Controller
{
    class UserController :BaseController
    {
        UserDAO userDAO = new UserDAO();
        ResultDAO resultDao = new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        /// <summary>
        /// 登录请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string Login(string data, Client client, Server server)
        {
            Console.WriteLine("收到登录请求");
            string[] dataStrs = data.Split(',');
            User user = userDAO.VerifyUser(client.MySQLConn, dataStrs[0], dataStrs[1]);
            if (user == null)
            {
                //Enum.GetName(typeof(ReturnCode,ReturnCode.Fail));
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Result result = resultDao.GetResultFromData(client.MySQLConn, user.Id);
                //把信息存储到个人客户端
                client.SetUserData(user, result);
                return string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Success).ToString(), user.Username, result.TotalCount, result.WinCount);
            }
        }

        /// <summary>
        /// 登录请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string Register(string data, Client client, Server server)
        {
            Console.WriteLine("收到注册请求");
            string[] dataStrs = data.Split(',');
            bool isHaveUser = userDAO.GetUserFromData(client.MySQLConn, dataStrs[0]);
            if (isHaveUser)
            {
                //有此用户名的账号，返回失败
                return ((int)ReturnCode.Fail).ToString();
            }
            //添加到数据库
            userDAO.AddUserToData(client.MySQLConn, dataStrs[0], dataStrs[1]);
            return ((int)ReturnCode.Success).ToString();
        }

    }
}
