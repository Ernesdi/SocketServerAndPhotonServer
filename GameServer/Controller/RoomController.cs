using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class RoomController : BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }

        /// <summary>
        /// 创建房间请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string CreateRoom(string data, Client client, Server server)
        {
            server.CreateRoom(client);
            Console.WriteLine("创建房间请求成功");
            //默认蓝是房主
            return ((int)ReturnCode.Success).ToString()+","+((int)RoleType.Blue).ToString();
        }

        /// <summary>
        /// 获取房间列表请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string ListRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Room room in server.GetListRoom())
            {
                //是否是等待加入状态。不是的话不添加进去。
                if (room.GetRoomStateIsWaitingJoin())
                {
                    sb.Append(room.GetHouseOwnerData() + "|");
                }
            }
            //没有房间列表
            if (sb.Length == 0)
            {
                //传个0
                sb.Append("0");
            }
            else
            {
                //移除掉最后的竖杆
                sb.Remove(sb.Length - 1, 1);
            }
            Console.WriteLine("请求的房间列表数据为" + sb.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 加入房间请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string JoinRoom(string data, Client client, Server server)
        {
             Room room = server.GetRoomById(int.Parse(data));
            if (room==null)
            {
                Console.WriteLine("房间为空"+ (int)ReturnCode.NotFound);
                //  (NotFound=2)
                return ((int)ReturnCode.NotFound).ToString();
            }
            else if (room.GetRoomStateIsWaitingJoin() == false)
            {
                Console.WriteLine("房间满员" + (int)ReturnCode.Fail);
                //房间满员 (Fail=1)
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Console.WriteLine("加入成功" + (int)ReturnCode.Success);
                //加入成功
                room.AddUser(client);
                string roomData = room.GetAllClientDataInRoom();
                room.BroadcastMessage(client, ActionCode.UpdateRoom, roomData);
                //返回的数据为： returncord+','+roleType+'-'+id,username,tc,wc+'|'+id,username,tc,wc   (Success=0)
                //默认红色是加入的玩家
                return ((int)ReturnCode.Success).ToString()+","+ ((int)RoleType.Red).ToString() + "-" +roomData;
            }

        }

        /// <summary>
        /// 退出房间请求（枚举类型转成字符串然后用MethodInfo调用这个方法）
        /// </summary>
        public string QuitRoom(string data, Client client, Server server)
        {
            Console.WriteLine("接收到退出房间请求");
            bool isRoomOwner = client.IsRoomOwner(client);
            Room room = client.Room;
            if (isRoomOwner)
            {
                //TODO 是房主的话
                room.BroadcastMessage(client, ActionCode.QuitRoom, ((int)ReturnCode.Success).ToString());
                room.CloseRoom();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                client.Room.RemoveUser(client);
                //TODO 一个玩家退出房间后告诉所有玩家
                room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetAllClientDataInRoom());
                return ((int)ReturnCode.Success).ToString();
            }
           
        }



    }
}
