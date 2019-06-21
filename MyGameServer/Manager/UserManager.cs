using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGameServer.Model;
using NHibernate;
using NHibernate.Criterion;

namespace MyGameServer.Manager
{
    class UserManager : IUserManager
    {
        public void Add(User user)
        {
            //这种写法也是使用完就关闭，不需要写close() 或者dispose()
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction =  session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
           
        }

        public User GetById(int id)
        {
            //这种写法也是使用完就关闭，不需要写close() 或者dispose()
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    User user =  session.Get<User>(id);
                    transaction.Commit();
                    return user;
                }
            }
        }

        public void Remove(User user)
        {
            //这种写法也是使用完就关闭，不需要写close() 或者dispose()
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    //根据主键来进行升级，所以传过来的信息一定要有主键
                    session.Delete(user);
                    transaction.Commit();
                }
            }
        }

        public void Update(User user)
        {
            //这种写法也是使用完就关闭，不需要写close() 或者dispose()
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    //根据主键来进行升级，所以传过来的信息一定要有主键
                    session.Update(user);
                    transaction.Commit();
                }
            }
        }


        /// <summary>
        /// 复杂查询，session.CreateCriteria 创建查询标准
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetByUsername(string username)
        {
            //这种写法也是使用完就关闭，不需要写close() 或者dispose().查询操作是不需要用到事务的
            using (ISession session = NhibernateHelper.OpenSession())
            {
                //第一个参数是创建标准类型为 User,
                //第二个参数是严格匹配(Restrictions是NHibernate.Criterion命名空间下的)  User类中的Username属性 与 传过来的名字。
                //因为已经建立了连接，所以Nhibernate知道实体类中的那个属性对应着数据库中的那个属性
                //第三个是得到的唯一结果。
                User user =  session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", username))
                    .UniqueResult<User>();
                return user;
            }
        }


        public ICollection<User> GetAllUsers()
        {
            //这种写法也是使用完就关闭，不需要写close() 或者dispose()
            using (ISession session = NhibernateHelper.OpenSession())
            {
                //第一个参数是创建标准类型为 User,
                //第二个参数是返回一个列表
                IList<User> users = session.CreateCriteria(typeof(User)).List<User>();
                return users;
            }
        }



        public bool VerifyUser(string username,string password)
        {
            //这种写法也是使用完就关闭，不需要写close() 或者dispose()
            using (ISession session = NhibernateHelper.OpenSession())
            {
                User user = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", username))
                    .Add(Restrictions.Eq("Password", password))
                    .UniqueResult<User>();
                if (user == null)
                {
                    return false;
                }
            }
            return true;

        }

    }
}
