using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

namespace SiKiedu
{
    class NhibernateHelper
    {
        //一个单例
        private static ISessionFactory sessionFactory;
        
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory==null)
                {
                    var configuration = new Configuration();
                    //读取目录（总）配置文件
                    configuration.Configure();
                    //读取程序集配置文件
                    configuration.AddAssembly("SiKiedu");
                    //创建工厂
                    sessionFactory = configuration.BuildSessionFactory();
                }
                return sessionFactory;
            }

        }


        /// <summary>
        /// 打开一个与数据库的对话
        /// </summary>
        /// <returns></returns>
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }



    }
}
