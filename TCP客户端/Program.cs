using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace TCP客户端
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket clientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //客户端与服务器端建立连接
            clientScoket.Connect(new IPEndPoint(IPAddress.Parse("192.168.86.1"),55555));

            //接收服务器端消息
            byte[] dataBuffer = new byte[1024]; 
            int count = clientScoket.Receive(dataBuffer);
            string msgRecieve = Encoding.UTF8.GetString(dataBuffer,0,count);
            Console.WriteLine(msgRecieve);


            for (int i = 0; i < 100; i++)
            {
                //Message类中的将int字节和字符字节进行拼接一下发过去，解决粘包问题
                clientScoket.Send(Message.GetBytes(i.ToString()));
            }


            //while (true)
            //{
            //    //向服务器发送消息
            //    string s = Console.ReadLine();
            //    if (s=="Close")
            //    {
            //        clientScoket.Close();
            //        return;
            //    }

            //    clientScoket.Send(Encoding.UTF8.GetBytes(s));
            //}

            Console.ReadKey();
            clientScoket.Close();

        }
    }
}
