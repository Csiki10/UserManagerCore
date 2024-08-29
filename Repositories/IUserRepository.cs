using UserManagerCore.Helpers;
using UserManagerCore.Models;

namespace UserManagerCore.Repositories
{
    public interface IUserRepository
    {
        void EditUser(UserModel user);
        IEnumerable<string> GetPlacesOfBirth();
        IEnumerable<string> GetPlacesOfResidence();
        UserModel GetUser(int id);
        LoginResult LoginUser(string userName, string password);
        List<UserModel> ReadUsersFromFile();
        void SaveToXml();
        void SaveUsersToFile(List<UserModel> users);
    }
}