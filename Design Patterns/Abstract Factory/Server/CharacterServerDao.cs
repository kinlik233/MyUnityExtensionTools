using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    class CharacterServerDao : ICharacterDao
    {
        public void Add(Character val)
        {
            Console.WriteLine("服务端角色信息添加");
        }
    }
}
