using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Threading;

namespace GameServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }
    class Room
    {
        private List<Client> clientRoom = new List<Client>();
        private RoomState roomState = RoomState.WaitingJoin;
        private Server server;

        //最大血量
        private const int MAX_HP = 20;

        public Room(Server server)
        {
            this.server = server;
        }
        public void AddUser(Client client)
        {
            clientRoom.Add(client);
            client.Room = this;
            client.Hp = MAX_HP;
            if (clientRoom.Count >= 2)
            {
                roomState = RoomState.WaitingBattle;
            }
        }
        public void RemoveUser(Client client)
        {
            client.Room = null;
            clientRoom.Remove(client);
            if (clientRoom.Count >= 2)
            {
                roomState = RoomState.WaitingBattle;
            }
            else
            {
                roomState = RoomState.WaitingJoin;
            }
        }

        public bool GetRoomStateIsWaitingJoin()
        {
            return roomState == RoomState.WaitingJoin;
        }
        /// <summary>
        /// 获取房主信息 由名字，总场数，胜利数组成。
        /// </summary>
        /// <returns></returns>
        public string GetHouseOwnerData()
        {
            return clientRoom[0].GetUserData();
        }

        //获得房主的client
        public Client GetRoomOwnerClient()
        {
            return clientRoom[0];
        }
        /// <summary>
        /// 获取房间的id
        /// </summary>
        /// <returns></returns>
        public int GetRoomId()
        {
            if (clientRoom.Count > 0)
            {
                return clientRoom[0].GetUserId();
            }
            return -1;
        }
        /// <summary>
        /// 获取房间中所有的用户的资料
        /// </summary>
        /// <returns></returns>
        public string GetAllClientDataInRoom()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Client client in clientRoom)
            {
                sb.Append(client.GetUserData() + "|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 只有房主退出才会关闭房间,其他人退出移出当前房间集合就行了 （非正常关闭）
        /// </summary>
        public void QuitRoom(Client client)
        {
            if (client == clientRoom[0])
            {
                CloseRoom();
            }
            else
            {
                clientRoom.Remove(client);
            }
        }

        /// <summary>
        /// 房主退出时的关闭
        /// </summary>
        public void CloseRoom()
        {
            foreach (Client client in clientRoom)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
        }

        /// <summary>
        /// 广播消息给房间内所有的客户端，房间加入了一个玩家,除了我们剔除的那个不广播（也就是加入的那个）= =
        /// 需要广播的消息。就是现在房间内每个玩家的数据。
        /// </summary>
        public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data)
        {
            foreach (Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
            //Console.WriteLine("房间更新信息:" + data + "。广播完毕");
        }

        /// <summary>
        /// 开始倒计时
        /// </summary>
        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }

        private void RunTimer()
        {
            //先暂停0.5秒等加载面板
            Thread.Sleep(500);
            for (int i = 3; i > 0; i--)
            {
                //给房间内所有的客户端发送计时
                BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
                //停一秒
                Thread.Sleep(1000);
            }
            //传一个r免得控数据异常
            BroadcastMessage(null, ActionCode.StartPlay, "r");
        }


        /// <summary>
        /// 造成伤害
        /// </summary>
        public void TakeDamage(Client excludeClient, int damage)
        {
            bool isOneDie = false;
            foreach (Client client in clientRoom)
            {
                if (client!= excludeClient)
                {
                    //如果返回值为true  有一方死亡了
                    if (client.TakeDamge(damage))
                    {
                        isOneDie = true;
                    }
                }
            }
            if (isOneDie == false)
                return;
            //告诉双方战斗结果
            foreach (Client client in clientRoom)
            {
                if (client.IsDie())
                {
                    //传过去1
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                    client.UpdateResult(true);
                }
                else
                {
                    Console.WriteLine("你赢了~");
                    //传过去0
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                    client.UpdateResult(false);
                }
            }
            //关闭当前房间
            CloseRoom();
        }

    }
}
