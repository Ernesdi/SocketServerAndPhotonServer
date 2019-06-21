using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using MyGameServer.Manager;
using MyGameServer.Model;
using Photon.SocketServer;
using MyCommon;
using MyGameServer.Handler;

namespace MyGameServer
{
    //所有server端 都要集成自applicationbase
    public class MyGameServer : ApplicationBase
    {

        //日志类
        //一个类似Debug.Log的东西  readonly类似一个单例模式
        //ILogger  和 LogManager 这两个类 要使用同一命名空间下的
        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public static Dictionary<OperationCode, BaseHandler> handlerDic = new Dictionary<OperationCode, BaseHandler>();

        //当前已经登录上的客户端list
        public static List<ClientPeer> loginedClientList = new List<ClientPeer>();


        //客户端连接时发起的请求。
        //每一个peerbase表示一个与客户端的连接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            Log.Info("一个客户端连接上了服务器");
            //返回回去，photon会帮我们管理起来这些客户端

            return new ClientPeer(initRequest);

          

        }

        //初始化服务器端
        protected override void Setup()
        {
            //日志的初始化
            //给配置文件更改一个动态参数Photon:ApplicationLogPath  路径为服务器部署的文件路径+bin_Win64\log  日志的名字在配置文件中有写
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(Path.Combine(this.ApplicationRootPath, "bin_Win64"), "log");
            //Dll要读取的日志的配置文件的路径在哪里
            FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (configFileInfo.Exists)
            {
                //设置日志工厂。//让photon知道我们使用的是log4net
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                //让log4net这个插件读取配置文件
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }

            //日志初始化完成
            Log.Info("服务器已经启动，Setup Completed！");
            Log.Info("日志初始化完成了");



            //UserManager userManager = new UserManager();
            //ICollection<User> users = userManager.GetAllUsers();

            //foreach (var item in users)
            //{
            //    Log.Info(string.Format("-ID-{0}-用户名为-{1}", item.Id, item.Username));
            //}
            //Log.Info(userManager.VerifyUser("Solis", "25252"));

            //初始化所有的控制器
            InitHandler();

        }

        private void InitHandler()
        {
            LoginHandler loginHandler = new LoginHandler();
            handlerDic.Add(OperationCode.Login, loginHandler);
            RegisterHandler registerHandler = new RegisterHandler();
            handlerDic.Add(OperationCode.Register, registerHandler);
            SyncPositionHandler syncPositionHandler = new SyncPositionHandler();
            handlerDic.Add(OperationCode.SyncPosition, syncPositionHandler);
            SyncPlayerHandler syncPlayerHandler = new SyncPlayerHandler();
            handlerDic.Add(OperationCode.SyncPlayer, syncPlayerHandler);
        }


        //服务器端关闭时
        protected override void TearDown()
        {
            Log.Info("服务器被关闭了");
        }
    }
}
