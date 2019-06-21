using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP客户端F
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
