using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 数据访问对象 工厂
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
        public abstract IUserDao UserDao { get; }

        public abstract ICharacterDao CharacterDao { get; }

    }
}
