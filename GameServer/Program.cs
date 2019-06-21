using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Servers;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Server server = new Server("127.0.0.1", 6688);
            //Server server = new Server("192.168.75.107", 6688);
            //老王的WIFI Ip地址
            Server server = new Server("192.168.123.220", 6688);
            server.Start();

            Console.WriteLine("服务器已经启动");
            Console.ReadKey();
        }
    }
}
