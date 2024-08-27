using UserManagerCore.Helpers;
using UserManagerCore.Models;

namespace UserManagerCore.Repositories
{
    public interface IUserRepository
    {
        LoginResult LoginUser(string userName, string password);
        List<User> ReadUsersFromFile();
        void SaveToXml();
    }
}