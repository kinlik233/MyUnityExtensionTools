using System;
using System.Collections.Generic;
using System.Text;

namespace MyUnityExtensionTools
{
    /// <summary>
    /// 客户端 数据访问对象 工厂
    /// </summary>
    class ClientDaoFactory : DaoFactory
    {
        public override IUserDao UserDao
        {
            get { return new UserClientDao(); }
        }

        public override ICharacterDao CharacterDao
        {
            get { return new CharacterClientDao(); }
        }
    }
}
