using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 用户服务端数据访问对象
    /// </summary>
    class UserServerDao:IUserDao
    {
        public void Add(User user)
        {
            Console.WriteLine("服务端用户数据添加");
        }
    }
}
