using System;
using System.Collections.Generic;
using System.Text;

namespace MyUnityExtensionTools
{
    class CharacterClientDao : ICharacterDao
    {
        public void Add(Character val)
        {
            Console.WriteLine("客户端角色信息添加");
        }
    }
}
