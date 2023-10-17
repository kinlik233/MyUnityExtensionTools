using System;

namespace MyUnityExtensionTools
{
    class Program
    {
        static void Main(string[] args)
        {
            DaoFactory factory = DaoFactory.Instance;
            IUserDao dao = factory.UserDao;
            dao.Add(new User());

            DaoFactory.Instance.CharacterDao.Add(new Character());
        }
    }
}
