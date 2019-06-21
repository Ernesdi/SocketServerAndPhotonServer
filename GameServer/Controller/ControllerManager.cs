using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
//反射，用字符串调用方法
using System.Reflection;
using GameServer.Servers;

namespace GameServer.Controller
{
    class ControllerManager
    {

        private Dictionary<RequestCode, BaseController> controllerDic = new Dictionary<RequestCode, BaseController>();

        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }


        private void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDic.Add(defaultController.RequestCode,defaultController);
            UserController userController = new UserController();
            controllerDic.Add(userController.RequestCode, userController);
            RoomController roomController = new RoomController();
            controllerDic.Add(roomController.RequestCode, roomController);
            GameController gameController  = new GameController();
            controllerDic.Add(gameController.RequestCode, gameController);
        }

        /// <summary>
        ///  处理请求
        /// </summary>
        /// <param name="requestCode">使用那个控制器</param>
        /// <param name="actionCode">使用控制器中的哪个方法</param>
        /// <param name="data">数据</param>
        /// <param name="client">消息要反馈的客户端</param>
        public void HandleRequest(RequestCode requestCode,ActionCode actionCode,string data,Client client)
        {
            BaseController baseController;
            bool isGet = controllerDic.TryGetValue(requestCode, out baseController);
            if (isGet==false)
            {
                Console.WriteLine("无法得到["+requestCode+"]所对应的controller，无法处理请求额");
                return;
            }
            //把一个枚举类型转化为字符串  第一个参数是枚举是什么类型的，第二参数枚举的名字是啥
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            //获得方法信息。
            MethodInfo mi = baseController.GetType().GetMethod(methodName);
            if (mi==null)
            {
                Console.WriteLine("【警告】在controller【"+ baseController.GetType() + "】中没有对应的处理方法：【"+methodName+"】");
                return;
            }
            object[] parameters = new object[] { data, client, server };
            //调用控制器函数。
            object returnValue = mi.Invoke(baseController, parameters);
            if (returnValue==null|| string.IsNullOrEmpty(returnValue as string))
                return;
            //如果有返回值，就给客户端响应
            server.SendResponse(client, actionCode, returnValue as string);
        }


    }
}
