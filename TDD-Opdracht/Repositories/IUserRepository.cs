using System;
using System.Collections.Generic;
using System.Text;
using TDD_Opdracht.Models;

namespace TDD_Opdracht.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int userId);
        User AddUser(User user);
        User UpdateUser(User user);
        void DeleteUser(int userId);





    }
}
