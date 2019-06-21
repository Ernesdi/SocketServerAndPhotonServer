using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    abstract class BaseController
    {

        protected RequestCode requestCode = RequestCode.None;

        public RequestCode RequestCode { get { return requestCode; } }

        /// <summary>
        /// 默认都有的方法
        /// </summary>
        /// <param name="data">数据</param>
        public virtual string DefualutHandle(string data, Client client, Server server) { return null; }

    }
}
