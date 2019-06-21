using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using MyGameServer.Tools;
using MyCommon;
using MyGameServer.Manager;
using MyGameServer.Threads;

namespace MyGameServer.Handler
{
    class LoginHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer clientPeer)
        {
            Dictionary<byte,object> data = operationRequest.Parameters;
            string username =  data.TryGet((byte)ParameterCode.Username) as String;
            string password =  data.TryGet((byte)ParameterCode.Password) as String;

            UserManager userManager = new UserManager();
            bool temp =  userManager.VerifyUser(username, password);
            //如果不写operationRequest.OperationCode  默认为0 然后客户端就是得到第一个回应字典里的Response;
            OperationResponse operationResponse = new OperationResponse(operationRequest.OperationCode);
            if (temp)
            {
                //存起来 因为是引用传递，所以直接改
                clientPeer.Username = username;
                MyGameServer.loginedClientList.Add(clientPeer);
                operationResponse.ReturnCode = (short)ReturnCode.Success;
            }
            else
            {
                operationResponse.ReturnCode = (short)ReturnCode.Fail;
            }
            clientPeer.SendOperationResponse(operationResponse, sendParameters);
        }
    }
}
