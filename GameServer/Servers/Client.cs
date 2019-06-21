using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;
using GameServer.Model;
using GameServer.DAO;

namespace GameServer.Servers
{
    class Client
    {
        //服务器端socket
        private Server server;
        //自己的socket
        private Socket clientScoket;
        //消息处理工具类
        private Message msg = new Message();
        //为每个客户端开一个数据库连接
        private MySqlConnection mysqlConn;

        public MySqlConnection MySQLConn {get{ return mysqlConn; }}

        private User user;
        private Result result;

        private ResultDAO resultDAO = new ResultDAO();  

        //客户端所在的房间。
        private Room room;

        public int Hp;
        public bool TakeDamge(int damage)
        {
            Hp -= damage;
            Hp = Math.Max(Hp, 0);
            if (Hp <= 0)
                return true;
            return false;
        }
        public bool IsDie()
        {
            if (Hp <= 0)
                return true;
            return false;
        }

        public void SetUserData(User user, Result result)
        {
            this.user = user;
            this.result = result;
        }

        public string GetUserData()
        {
            return user.Id + "," +user.Username + "," + result.TotalCount + "," + result.WinCount;
        }
        public int GetUserId()
        {
            return user.Id;
        }

        public Room Room
        {
            set { room = value; }
            get { return room; }
        }
        public Client() { }

        public Client(Socket clientScoket,Server server)
        {
            this.clientScoket = clientScoket;
            this.server = server;
            mysqlConn = ConnHelper.ConnDB();
        }


        public void Start()
        {
            if (clientScoket == null || clientScoket.Connected == false)
                return;
            //客户端开始异步接收服务器消息
            clientScoket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainNum, SocketFlags.None, ReceiveCallBack, null);
        }

        /// <summary>
        /// 客户端接收到服务器消息的回调方法。
        /// </summary>
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientScoket == null || clientScoket.Connected == false)
                    return;
                int count = clientScoket.EndReceive(ar);
                //接收数据为0就等于断开了连接
                if (count == 0)
                {
                    Close();
                }
                msg.Receive(count, OnProcessCallBack);
                //客户端重新开始异步接收服务器消息
                Start();
            }
            //非正常断开连接
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        /// <summary>
        /// 处理消息返回的回调方法，因为使用的是Server作为ControllerManager和客户端的中介，所以他们不交互
        /// </summary>
        private void OnProcessCallBack(RequestCode requestCode,ActionCode actionCode ,string dataStr)
        {
            server.HandleRequest(requestCode, actionCode, dataStr, this);
        }


        /// <summary>
        /// 客户端与服务器端断开连接
        /// </summary>
        private void Close()
        {
            ConnHelper.CloseConnDB(mysqlConn);
            if (clientScoket != null)
                clientScoket.Close();
            if (room != null)
            {
                room.QuitRoom(this);
            }
            //Socket要最后关
            server.RemoveClient(this);
          
        }

        /// <summary>
        /// 给客户端发送消息
        /// </summary>
        /// <param name="requestCode">拼接用</param>
        /// <param name="data">拼接用</param>
        public void Send(ActionCode actionCode, string data)
        {
            byte[] sendData = Message.PackData(actionCode, data);
            //发送字节数组
            clientScoket.Send(sendData);
        }

        /// <summary>
        /// 是不是房主
        /// </summary>
        /// <returns></returns>
        public bool IsRoomOwner(Client client)
        {
            return client == client.Room.GetRoomOwnerClient();
        }

        /// <summary>
        /// 升级数据库里的数据
        /// </summary>
        /// <param name="isDie">true为战败</param>
        public void  UpdateResult(bool isDie)
        {
            UpdateResultInDB(isDie);
            UpdateResultInClient();
        }

        /// <summary>
        /// 在数据库 升级战绩 
        /// </summary>
        private void UpdateResultInDB(bool isDie)
        {
            if (!isDie)
            {
                result.WinCount++;
            }
            result.TotalCount++;
            resultDAO.UpdateResultFromData(mysqlConn, result);
        }
        /// <summary>
        ///  升级战绩 在客户端
        /// </summary>
        private void UpdateResultInClient()
        {
            Send(ActionCode.UpdateResult, string.Format("{0},{1}", result.TotalCount, result.WinCount));
        }

    }
}
