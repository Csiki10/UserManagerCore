﻿using Microsoft.AspNetCore.Identity.Data;
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
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "users.txt");

        public List<UserModel> ReadUsersFromFile()
        {
            var users = new List<UserModel>();

            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("Users file nor exists!");
            }

            var lines = System.IO.File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(';');

                users.Add(new UserModel
                {
                    ID = int.Parse(parts[0]),
                    Username = parts[1],
                    Password = parts[2],
                    LastName = parts[3],
                    FirstName = parts[4],
                    DateOfBirth = DateTime.ParseExact(parts[5], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    PlaceOfBirth = parts[6],
                    PlaceOfResidence = parts[7],
                    PhoneNumber = parts[8]
                });
            }

            return users;
        }

        public UserModel GetUser(int id)
        {
            if (id == 0)
            {
                throw new Exception("The id is null!");
            }

            var users = ReadUsersFromFile();
            var user = users.SingleOrDefault(x => x.ID == id);

            if (user == null)
            {
                throw new Exception("User not found with id: " + id);
            }

            return user;
        }

        public LoginResult LoginUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new Exception("userName or password is null or empty in LoginUser!");
            }

            var users = ReadUsersFromFile();
            var user = users.FirstOrDefault(u => u.Username == userName);

            if (user == null)
            {
                return new LoginResult
                {
                    State = LoginState.InvalidUserName,
                    Error = "Invalid username!"
                };
            }

            if (user.Password != password)
            {
                return new LoginResult
                {
                    State = LoginState.InvalidPassword,
                    Error = "Invalid password!"
                };
            }

            return new LoginResult
            {
                State = LoginState.Succes,
            };
        }

        public void SaveUsersToFile(List<UserModel> users)
        {
            var lines = users.Select(u => $"{u.ID};{u.Username};{u.Password};{u.LastName};{u.FirstName};{u.DateOfBirth:yyyy-MM-dd};{u.PlaceOfBirth};{u.PlaceOfResidence};{u.PhoneNumber}");

            System.IO.File.WriteAllLines(filePath, lines);
        }

        public void SaveToXml()
        {
            var users = ReadUsersFromFile();

            var serializer = new XmlSerializer(typeof(List<UserModel>));
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "users.xml");

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, users);
            }
        }

        public void EditUser(UserModel user)
        {
            var users = ReadUsersFromFile();
            var userToUpdate = users.SingleOrDefault(x => x.ID == user.ID);

            if (userToUpdate == null)
            {
                throw new Exception("User not found with id: " + user.ID);
            }

            userToUpdate.Username = user.Username;
            userToUpdate.LastName = user.LastName;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.PhoneNumber = user.PhoneNumber;
            userToUpdate.DateOfBirth = user.DateOfBirth;
            userToUpdate.PlaceOfBirth = user.PlaceOfBirth;
            userToUpdate.PlaceOfResidence = user.PlaceOfResidence;

            SaveUsersToFile(users);
        }

        public IEnumerable<string> GetPlacesOfBirth()
        {
            return new List<string>()
            {
                "New York",
                "Los Angeles",
                "Chicago",
                "Houston",
                "Phoenix",
                "Philadelphia",
                "Antonio",
                "Diego",
                "Boston",
                "Denver",
                "Detroit",
                "San Antonio"
            };
        }

        public IEnumerable<string> GetPlacesOfResidence()
        {
            return new List<string>()
            {
                "San Francisco",
                "Miami",
                "Austin",
                "Seattle",
                "Denver",
                "Dallas",
                "Portland",
                "Las Vegas",
                "Atlanta",
                "New York",
                "Los Angeles"
            };
        }
    }
}
