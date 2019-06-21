using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using SiKiedu.Model;
using SiKiedu.Manager;

namespace SiKiedu
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("程序已经启动");

            UserManager userManager = new UserManager();
            ICollection<User> users =  userManager.GetAllUsers();

            foreach (var item in users)
            {
                Console.WriteLine(string.Format("-ID-{0}-用户名为-{1}", item.Id, item.Username));
            }
            Console.WriteLine(userManager.VerifyUser("Solis", "25252"));

            //var configuration =  new Configuration();
            ////解析 不写参数默认解析hibernate.cfg.xml 文件   写了参数就解析指定路径的文件
            //configuration.Configure();

            ////解析映射文件， User.hbm.xml...之类的 因为解析的是一个程序集，单个xml配置文件中写有SiKiedu都能被解析
            //configuration.AddAssembly("SiKiedu");

            ////会话工厂
            //ISessionFactory sessionFactory = null;
            ////会话
            //ISession session = null;
            ////事务（保存数据完整性，在事务内的某一条sql语句执行失败，数据都会回滚到初始状态。）
            //ITransaction transaction = null;

            //try
            //{
            //    //创建一个session工厂
            //    sessionFactory = configuration.BuildSessionFactory();
            //    //打开跟数据库的会话
            //    session = sessionFactory.OpenSession();

            //    //开始事务
            //    transaction = session.BeginTransaction();
            //        //创建一个对象
            //        User user = new User() { Username = "shiwuF", Password = "123" };
            //        //创建一个对象
            //        User user2 = new User() { Username = "shiwuF", Password = "123" };
            //        //保存到数据库中
            //        session.Save(user);
            //        //保存到数据库中
            //        session.Save(user2);
            //    //事务提交
            //    transaction.Commit();

            //    Console.WriteLine("数据保存完毕~");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            //finally
            //{
            //    if (transaction != null)
            //    {
            //        transaction.Dispose();
            //    }

            //    if (session != null)
            //    {
            //        session.Close();
            //    }

            //    if (sessionFactory != null)
            //    {
            //        sessionFactory.Close();
            //    }
            //}
            Console.WriteLine("数据操作完毕~");
            Console.ReadKey();
        }
    }
}
