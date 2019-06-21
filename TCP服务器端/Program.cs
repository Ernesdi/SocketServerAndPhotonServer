using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace TCP服务器端
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServerAsync();
        }

        /// <summary>
        /// 异步开启服务，使得Receive不再暂停程序
        /// </summary>
        static void StartServerAsync()
        {
            //创建一个socket 指明协议v4 类型为流类型，协议为tcp
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //使用IPaddress 里面的压入方法获得地址。 路由器每隔几天会换一个地址给电脑，所以必须检查一下。
            IPAddress iPAddress = IPAddress.Parse("192.168.86.1");
            //不要申请80端口，因为是内置端口你申请过来干嘛
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 55555);
            //绑定IP和端口号，同时向系统申请端口
            serverSocket.Bind(iPEndPoint);

            //接收客户端的最大数 0是不限制
            serverSocket.Listen(0);

            //开启异步接收客户端连接，不会终止程序
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

        static Message message = new Message();

        static void AcceptCallBack(IAsyncResult ar)
        {

            //服务器端socket
            Socket serverSocket = ar.AsyncState as Socket;
            //客户端socket
            Socket clientScoket = serverSocket.EndAccept(ar);

            //向客户端发送一条消息
            string msg = "123客户端你好，Hello...";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            clientScoket.Send(data);

            //接收客户端发送过来的消息
            //clientScoket.BeginReceive(dataReceive, 0, 1024, SocketFlags.None, ReceiveCallBack, clientScoket);
            //解决粘包 第一个参数是message类中的字节数组，第二个是开始的位置，第三个是最大限制
            clientScoket.BeginReceive(message.Data, message.StartIndex, message.RemainNum, SocketFlags.None, ReceiveCallBack, clientScoket);
            //重新开启异步接收客户端连接，不会终止程序
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

        }


        //用于接收客户端发过来的消息的数组
        static byte[] dataReceive = new byte[1024];

        /// <summary>
        /// 接收到的消息的回调方法
        /// </summary>
        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientScoket = null;
            try
            {
                clientScoket = ar.AsyncState as Socket;
                int count = clientScoket.EndReceive(ar);
                //正常关闭客户端，因为客户端关闭的时候服务器会一直接受到空消息
                if (count == 0)
                {
                    clientScoket.Close();
                    return;
                }
                //增加长度
                message.AddCount(count);
                //处理接收(主要处理粘包逻辑的编写)
                message.Receive();

                //string msg = Encoding.UTF8.GetString(dataReceive, 0, count);
                //Console.WriteLine("从客户端接收到数据" + msg);

                //重新接收客户端发送过来的消息
                //clientScoket.BeginReceive(dataReceive, 0, 1024, SocketFlags.None, ReceiveCallBack, clientScoket);
                //解决粘包 第一个参数是message类中的字节数组，第二个是开始的位置，第三个是最大限制
                clientScoket.BeginReceive(message.Data, message.StartIndex, message.RemainNum, SocketFlags.None, ReceiveCallBack, clientScoket);
            }
            //非正常关闭客户端
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientScoket!=null)
                {
                    clientScoket.Close();
                }
            }
        }


     

        static void StartServerSync()
        {
            //创建一个socket 指明协议v4 类型为流类型，协议为tcp
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //使用IPaddress 里面的压入方法获得地址。 路由器每隔几天会换一个地址给电脑，所以必须检查一下。
            IPAddress iPAddress = IPAddress.Parse("192.168.86.1");
            //不要申请80端口，因为是内置端口你申请过来干嘛
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 55555);
            //绑定IP和端口号，同时向系统申请端口
            serverSocket.Bind(iPEndPoint);

            //接收客户端的最大数 0是不限制
            serverSocket.Listen(0);
            //客户端socket
            Socket clientScoket = serverSocket.Accept();

            //向客户端发送一条消息
            string msg = "123客户端你好，Hello...";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            clientScoket.Send(data);

            //接收客户端消息
            byte[] dataBuffer = new byte[1024];
            int count = serverSocket.Receive(dataBuffer);
            string msgReceive = Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine(msgReceive);

            Console.ReadKey();

            //关闭客户端的连接以及服务器端
            clientScoket.Close();
            serverSocket.Close();
        }
    }
}
