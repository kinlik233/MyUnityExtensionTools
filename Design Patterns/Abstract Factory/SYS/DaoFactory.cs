using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 数据访问对象 抽象工厂
    /// 不关注数据的创建过程，方便修改具体的子类创建工厂
    /// 无法修改工厂要创建的数据种类
    /// </summary>
    abstract class DaoFactory
    {
        private static DaoFactory instance;
        public static DaoFactory Instance
        {
            get
            {
                //if (GameMain.Type == "Client")
                //{
                //    return new ClientDaoFactory();
                //}
                //else
                //    return new ServerDaoFactory();

                if (instance == null)
                {
                    //格式
                    Type type = Type.GetType("Common." + GameMain.Type + "DaoFactory");
                    instance = Activator.CreateInstance(type) as DaoFactory;
                }
                return instance;
            }
        }
        //用户数据抽象
        public abstract IUserDao UserDao { get; }
        //角色数据抽象
        public abstract ICharacterDao CharacterDao { get; }

    }
}
