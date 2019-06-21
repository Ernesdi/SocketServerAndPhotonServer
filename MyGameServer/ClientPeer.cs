using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MyCommon;
using MyGameServer.Tools;
using MyGameServer.Handler;
using MyGameServer.Threads;

namespace MyGameServer
{
    //管理客户端与服务器端的连接。 继承自 photon.sockerserver中的clientpeer。 相当于是做一些扩展吧
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public string Username;
        public float PosX, PosY, PosZ;

        private SyncPos syncPos;

        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {
            syncPos = new SyncPos(this);
        }

        //客户端断开连接
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            syncPos.StopRun();
            MyGameServer.loginedClientList.Remove(this);
        }

        //客户端向服务器发起的请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            //switch (operationRequest.OperationCode)
            //{
            //    //case 是requestCode.
            //    case 1:
            //        MyGameServer.Log.Info("收到客户端发往服务器端的消息");
            //        //得到客户端发送过来的数据字典
            //        Dictionary<byte, object> ValDic = operationRequest.Parameters;
            //        object intValue; object stringValue;
            //        ValDic.TryGetValue(1, out intValue);
            //        ValDic.TryGetValue(2, out stringValue);
            //        MyGameServer.Log.Info("收到客户端发往服务器端的消息,值为：" + intValue);
            //        MyGameServer.Log.Info("收到客户端发往服务器端的消息,值为：" + stringValue);


            //        //回应
            //        OperationResponse OpResponse = new OperationResponse(1);
            //        Dictionary<byte, object> ValDic2 = new Dictionary<byte, object>();
            //        ValDic2.Add(1, 100);
            //        ValDic2.Add(2, "字符串类型传递");
            //        OpResponse.Parameters = ValDic2;
            //      //向客户端发送回应
            //      SendOperationResponse(OpResponse, sendParameters);


            //        //发送事件
            //        EventData eventData = new EventData(1);
            //        eventData.Parameters = ValDic2;
            //        //直接发送一个事件给客户端，事件可以随时发送，响应只能在客户端发起请求的时候才能给予响应。
            //       SendEvent(eventData, new SendParameters());
            //        break;

            //}
            //根据OpCode查找对应的Handler来进行各自的处理
            OperationCode OpCode =(OperationCode) operationRequest.OperationCode;
            BaseHandler baseHandler = MyGameServer.handlerDic.TryGet(OpCode);
            if (baseHandler != null)
            {
                MyGameServer.Log.Info("找到" + OpCode + "对应的Handler，正在进行分配");
                baseHandler.OnOperationRequest(operationRequest, sendParameters, this);
            }
            else
            {
                MyGameServer.Log.Info("没有找到" + OpCode + "对应的Handler");
            }
           


        }
    }
}
