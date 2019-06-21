using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCommon
{
    //客户端向服务器端发送的请求标识码
    public enum OperationCode : byte
    {
        Login,
        Register,
        SyncPosition,
        SyncPlayer
    }
}
