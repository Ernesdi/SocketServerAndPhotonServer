using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCommon;

namespace MyGameServer.Event
{
    public class SyncPositionEvent
    {
        //向自己以外的客户端发送自己的位置
        public void SendPosExcuteSelf(ClientPeer excutePeer)
        {
            String data = excutePeer.Username + "|" + excutePeer.PosX + "," + excutePeer.PosY + "," + excutePeer.PosZ;
            MyGameServer.Log.Info("我的位置：" + data);
            foreach (ClientPeer clientPeer in MyGameServer.loginedClientList)
            {
                if (excutePeer != clientPeer && string.IsNullOrEmpty(clientPeer.Username) == false)
                {
                    EventData eventData = new EventData((byte)EventCode.SyncPos);
                    Dictionary<byte, object> dataDic = new Dictionary<byte, object>();
                    dataDic.Add((byte)ParameterCode.Position, data);
                    eventData.Parameters = dataDic;
                    clientPeer.SendEvent(eventData, new SendParameters());
                }
            }
        }

    }
}
