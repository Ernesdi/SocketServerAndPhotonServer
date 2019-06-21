using System;
using System.Collections.Generic;
using System.Text;
//是.net 3.5以后新推出的API,主要作用是对集合进行查询。 Concat是他的
using System.Linq;

namespace TCP客户端
{
    class Message
    {
        public static byte[] GetBytes(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            int dataLength = data.Length;
            //转为4个字节的固定长度
            byte[] lengthBytes = BitConverter.GetBytes(dataLength);
            byte[] newBytes = lengthBytes.Concat(data).ToArray();
            return newBytes;
        }

    }
}
