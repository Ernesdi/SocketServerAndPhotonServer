using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using MyCommon;
using MyGameServer.Tools;
using MyGameServer.Event;

namespace MyGameServer.Handler
{
    class SyncPositionHandler : BaseHandler
    {
        SyncPositionEvent syncPositionEvent = new SyncPositionEvent();

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer clientPeer)
        {
            Dictionary<byte, object> data = operationRequest.Parameters;
            float PosX =  (float)data.TryGet((byte)ParameterCode.PosX);
            float PosY =  (float)data.TryGet((byte)ParameterCode.PosY);
            float PosZ =  (float)data.TryGet((byte)ParameterCode.PosZ);
            //自己的位置转存到自个的clientPeer
            clientPeer.PosX = PosX;
            clientPeer.PosY = PosY;
            clientPeer.PosZ = PosZ;

        }
    }
}
