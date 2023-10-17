using System;
using System.Collections.Generic;
using System.Text;

namespace MyUnityExtensionTools
{
    /// <summary>
    /// 用户客户端数据访问对象
    /// </summary>
    class UserClientDao:IUserDao
    {
        public  void Add(User val)
        {
            Console.WriteLine("客户端用户数据添加");
        }
    }
}
