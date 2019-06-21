using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using MyGameServer.Tools;
using MyCommon;
using MyGameServer.Manager;
using MyGameServer.Model;

namespace MyGameServer.Handler
{
    class RegisterHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer clientPeer)
        {
            Dictionary<byte, object> data = operationRequest.Parameters;
            string username = data.TryGet((byte)ParameterCode.Username) as String;
            string password = data.TryGet((byte)ParameterCode.Password) as String;

            UserManager userManager = new UserManager();
            User user = userManager.GetByUsername(username);
            //把收到的OperationCode 回调回去
            //如果不写operationRequest.OperationCode  默认为0 然后客户端就是得到第一个回应字典里的Response;
            OperationResponse operationResponse = new OperationResponse(operationRequest.OperationCode);
            if (user==null)
            {
                User user1 = new User() { Username = username, Password = password };
                userManager.Add(user1);
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
