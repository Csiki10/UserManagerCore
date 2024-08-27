using UserManagerCore.Helpers;
using UserManagerCore.Models;

namespace UserManagerCore.Repositories
{
    public interface IUserRepository
    {
        UserModel GetUser(int id);
        LoginResult LoginUser(string userName, string password);
        List<UserModel> ReadUsersFromFile();
        void SaveToXml();
        void SaveUsersToFile(List<UserModel> users);
    }
}