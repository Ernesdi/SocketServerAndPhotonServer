using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using System.Xml.Serialization;
using System.IO;
using MyCommon;

namespace MyGameServer.Handler
{
    class SyncPlayerHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer clientPeer)
        {
            
            List<string> currentOnlineNameExcuteSelf = new List<string>();
            foreach (ClientPeer tempPeer in MyGameServer.loginedClientList)
            {
                if (tempPeer!= clientPeer && string.IsNullOrEmpty(tempPeer.Username) == false)
                {
                    currentOnlineNameExcuteSelf.Add(tempPeer.Username);
                }
            }
            //测试用
            //currentOnlineNameExcuteSelf.Add("Solis25252");

            string dataStr = "";
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<String>));
                xmlSerializer.Serialize(sw, currentOnlineNameExcuteSelf);
                dataStr = sw.ToString();
            }
            //把其他已经上线的客户端角色生成到自己的客户端上
            OperationResponse operationResponse = new OperationResponse(operationRequest.OperationCode);
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.CurrentOnlineNameExcuteSelf, dataStr);
            operationResponse.Parameters = data;
            clientPeer.SendOperationResponse(operationResponse, sendParameters);

            //在其他所有的客户端上创建自己的客户端角色。
            foreach (ClientPeer tempPeer in MyGameServer.loginedClientList)
            {
                if (tempPeer != clientPeer && string.IsNullOrEmpty(clientPeer.Username) == false)
                {
                    MyGameServer.Log.Info("其他客户端上创建自己的客户端角色。");
                    EventData eventData = new EventData((byte)EventCode.SyncPlayer);
                    Dictionary<byte, object> tempData = new Dictionary<byte, object>();
                    tempData.Add((byte)ParameterCode.Username, clientPeer.Username);
                    eventData.Parameters = tempData;
                    //向别人的客户端发送事件
                    tempPeer.SendEvent(eventData, new SendParameters());
                }
            }
        
        }
    }
}
