using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class GameController :BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        /// <summary>
        /// 开始游戏请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string StartGame(string data, Client client, Server server)
        {
           
            //先判断是不是房主
            if (client.IsRoomOwner(client))
            {
                Room room = client.Room;
                room.BroadcastMessage(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString());
                Console.WriteLine("您是房主，正在开始倒计时。");
                room.StartTimer();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                Console.WriteLine("您不是房主，无法开始游戏。");
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 客户端发送过来的个人位置请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string Move(string data, Client client, Server server)
        {
            //把个人位置信息广播给除去自己以为的所有人，返回给自己一个null 
            Room room = client.Room;
            if (room!=null)
                room.BroadcastMessage(client, ActionCode.Move, data);
            //返回值为Null时不会发送到客户端 ControllerManager的HandleRequest方法有写
            return null;
        }

        /// <summary>
        /// 客户端发送过来的个人的箭的位置请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string Shoot(string data, Client client, Server server)
        {
            //把个人位置信息广播给除去自己以为的所有人，返回给自己一个null 
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Shoot, data);
            //返回值为Null时不会发送到客户端 ControllerManager的HandleRequest方法有写
            return null;
        }


        /// <summary>
        /// 客户端发送过来的造成伤害请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// 不需要返回东西给发过来的客户端
        /// </summary>
        public string Attack(string data, Client client, Server server)
        {
            int damage = int.Parse(data);
            Room room = client.Room;
            if (room == null)
                return null;
            room.TakeDamage(client, damage);
            return null;
        }


        /// <summary>
        /// 客户端发送过来的退出战斗请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// 不需要返回东西给发过来的客户端
        /// </summary>
        public string QuitBattle(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
            {
                room.BroadcastMessage(null, ActionCode.QuitBattle, data);
                room.CloseRoom();
            }
            return null;
        }

    }
}
