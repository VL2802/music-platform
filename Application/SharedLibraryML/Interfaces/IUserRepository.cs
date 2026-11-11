using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraryML.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        User? ReadUser(int userId);
        void UpdateUser(User user);
        void DeleteUser(int userId);
        List<User> ReadAllUsers();
        User FindUserByEmail(string email);
    }
}
