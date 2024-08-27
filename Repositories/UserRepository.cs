using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using UserManagerCore.Helpers;
using UserManagerCore.Models;

namespace UserManagerCore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string file_path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "users.txt");

        public List<User> ReadUsersFromFile()
        {
            var users = new List<User>();

            if (!System.IO.File.Exists(file_path))
            {
                throw new Exception("Users file nor exists!");
            }

            var lines = System.IO.File.ReadAllLines(file_path);

            foreach (var line in lines)
            {
                var parts = line.Split(';');

                users.Add(new User
                {
                    ID = int.Parse(parts[0]),
                    Username = parts[1],
                    Password = parts[2],
                    LastName = parts[3],
                    FirstName = parts[4],
                    DateOfBirth = DateTime.ParseExact(parts[5], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    PlaceOfBirth = parts[6],
                    PlaceOfResidence = parts[7]
                });
            }

            return users;
        }

        public LoginResult LoginUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return new LoginResult
                {
                    State = LoginState.Error,
                    Error = "userName or password is null or empty in LoginUser!"
                };

            }

            var users = ReadUsersFromFile();
            var user = users.FirstOrDefault(u => u.Username == userName);

            if (user == null)
            {
                return new LoginResult
                {
                    State = LoginState.Invalid,
                    Error = "No user found with username: " + userName
                };
            }

            if (user.Password != password)
            {
                return new LoginResult
                {
                    State = LoginState.Invalid,
                    Error = "Invalid password!"
                };
            }

            return new LoginResult
            {
                State = LoginState.Succes,
            };
        }

        public void SaveToXml()
        {
            var users = ReadUsersFromFile();

            var serializer = new XmlSerializer(typeof(List<User>));
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "users.xml");

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, users);
            }
        }
    }
}
