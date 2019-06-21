using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{
    class Server
    {

        private IPEndPoint ipEndPoint;
        private Socket serverScoket;

        private List<Client> clientList = new List<Client>();
        private List<Room> listRoom = new List<Room>();

        private ControllerManager controllerManager;

        /// <summary>
        /// 构造方法
        /// </summary>
        public Server() { }
        public Server(string ipStr,int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr,port);
        }

        /// <summary>
        /// 绑定端口
        /// </summary>
        /// <param name="ipStr">传过来的ip服务器地址</param>
        /// <param name="port">服务器端口</param>
        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }


        public void Start()
        {
            //绑定
            serverScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverScoket.Bind(ipEndPoint);
            //最大限制监听
            serverScoket.Listen(0);
            //开始异步监听
            serverScoket.BeginAccept(AcceptCallBack, null);

        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private void AcceptCallBack(IAsyncResult ar)
        {
            //获得客户端scoket
            Socket clientScoket = serverScoket.EndAccept(ar);
            Client client = new Client(clientScoket, this);
            client.Start();
            //添加到客户端列表中管理
            clientList.Add(client);
            Console.WriteLine("一个用户已经连接上服务器Socket");
            //再次异步监听
            serverScoket.BeginAccept(AcceptCallBack, null);
        }


        /// <summary>
        /// 客户端断开连接，把客户端列表中断开的那个移除掉
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(Client client)
        {
            //上锁？？？？
            //lock的解释：https://www.cnblogs.com/wangfuyou/p/5098778.html
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }

        /// <summary>
        /// 向客户端响应
        /// </summary>
        /// <param name="client">那个客户端</param>
        /// <param name="requestCode">来自那个处理器</param>
        /// <param name="data">数据</param>
        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            //给客户端反应
            client.Send(actionCode, data);
        }

        /// <summary>
        /// 中介方法
        /// </summary>
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="client"></param>
        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddUser(client);
            listRoom.Add(room);
        }
        public List<Room> GetListRoom()
        {
            return listRoom;
        }
        public Room GetRoomById(int id)
        {
            foreach (Room room in listRoom)
            {
                if (room.GetRoomId()==id)
                {
                    return room;
                }
            }
            return null;
        }

        /// <summary>
        /// 移除房间
        /// </summary>
        public void RemoveRoom(Room room)
        {
            if (listRoom!=null&&room!=null)
            {
                listRoom.Remove(room);
            }
        }
    }
}
