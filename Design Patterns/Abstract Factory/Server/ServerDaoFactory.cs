using System;
using System.Collections.Generic;
using System.Text;

namespace MyUnityExtensionTools
{
    /// <summary>
    /// 服务端 数据访问对象 工厂
    /// </summary>
    class ServerDaoFactory : DaoFactory
    {
        public override IUserDao UserDao
        {
            get { return new UserServerDao(); }
        }

        public override ICharacterDao CharacterDao
        {
            get { return new CharacterServerDao(); }
        }

    }
}
