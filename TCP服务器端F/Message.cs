using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP服务器端F
{
    class Message
    {
        private byte[] data = new byte[1024];

        public byte[] Data { get { return data; } }

        private int startIndex = 0;

        public int StartIndex { get { return startIndex; } }

        //字节数组剩余的长度
        public int RemainNum { get { return data.Length - startIndex; } }

        //增加长度
        public void AddCount(int count)
        {
            startIndex += count;
        }

        //粘包逻辑处理
        public void Receive()
        {
            while (true)
            {
                //如果4个字节都没有 直接返回，因为前4个字节是记录这条数据有多大
                if (startIndex <= 4) return;
                //获得这条数据的大小，因为是toInt32 所以只会获得前4个字节 0-3刚刚好是我们的记录完整数据的大小。
                int dataCount = BitConverter.ToInt32(data, 0);
                //如果数据总数减去用于标志数据长度的INT32的4个字节大于数据大小，证明有一条完整的数据。
                if (startIndex - 4 >= dataCount)
                {
                    //将字节转换为字符串。
                    string s = Encoding.UTF8.GetString(data, 4, dataCount);
                    Console.WriteLine("获得一条完整的数据:" + s);
                    //拷贝数组，第一个参数是要拷贝的数组，第二个参数是被拷贝数组的开始的位置，
                    //第三个参数是要拷贝到的数组，第四个参数是拷贝到的数组开始的位置，
                    //第五个参数是要拷贝的长度，startIndex其实是这一次包的数据的大小，减去一整条完成的数据长度加上前4个字节的记录字节。就等于剩余的长度
                    Array.Copy(data, dataCount + 4, data, 0, startIndex - dataCount - 4);
                    //更新开始的位置，  dataCount是一条完整的数据大小加上4个字节的int32的字节。
                    startIndex -= (dataCount + 4);
                }
                //如果没有一整条完整的数据，直接break.
                else
                {
                    break;
                }
            }

        }

    }


}
