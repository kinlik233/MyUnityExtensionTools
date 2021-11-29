using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 用户数据访问对象
    /// </summary>
    interface IUserDao
    {
        void Add(User val);
    }
}
