using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraryML.Interfaces;
using SharedLibraryML.MediaSection;
using SharedLibraryML;


namespace ML_Tests.MockRepository
{
    public class MockUserRepository : IUserRepository
    {
        private List<User> _users = new List<User>
        {
            new User(1, "vvntl", "Test1234567890!", "verautina28@gmail.com", "0615376821", Role.ARTIST, ""),
            new User(2, "brandondiaz", "Test1234567890!", "brandondiaz@gmail.com", "0611111111", Role.LISTENER, "")
        };

        public void CreateUser(User user)
        {
            int lastId = ReadAllUsers().Count;
            user.SetId(lastId++);
            _users.Add(user);
        }

        public User? ReadUser(int userId)
        {
            if (userId == 0)
            { return null; }
            return _users.FirstOrDefault(u => u.GetId() == userId);
        }

        

        public List<User> ReadAllUsers()
        {
            return _users;
        }

        public void UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.GetId() == user.GetId());
            if (existingUser != null)
            {
                _users.Remove(existingUser);
                _users.Add(user);
            }
        }

        public void DeleteUser(int userId)
        {
            User? user = ReadUser(userId);
            _users.Remove(user);
        }

        public bool IsEmailTaken(string email)
        {
            var user = _users.FirstOrDefault(u => u.GetEmail() == email);
            if (user != null)
            {
                return true;  
            }
            return false;
        }

        public User? FindUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.GetEmail() == email);
        }
    }
}
