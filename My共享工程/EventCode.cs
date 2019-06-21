using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCommon
{
    //服务器向客户端发送的事件枚举。免得123这些数字不知道代表什么意思，可以使用枚举转成int再传输。
    //不是继承，是指定 EventCode 的类型， public enum EventCode : byte，这个表示枚举元素可以转换为 byte 。
    public enum EventCode : byte
    {
        SyncPlayer,
        SyncPos

    }
}
