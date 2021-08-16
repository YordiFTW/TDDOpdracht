using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDD_Opdracht.DbContext;
using TDD_Opdracht.Models;
using TDD_Opdracht.Repositories;

namespace TDD_Opdracht.Services
{
    public class UserDataService : IUserDataService
    {

        private readonly UserDbContext userDbContext;
        private readonly IUserRepository userRepo;

        public UserDataService(IUserRepository userRepo, UserDbContext userDbContext)
        {
            this.userRepo = userRepo;
            this.userDbContext = userDbContext;
        }


        public void Save(User user)
        {
            User newUser = new User();

            user.Id = newUser.Id;
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.Email = newUser.Email;

            userRepo.AddUser(newUser);
        }

        public bool Validate(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException();
            }
            if (string.IsNullOrEmpty(user.LastName))
            {
                throw new ArgumentNullException();
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException();
            }

            if (user.Email == new System.Net.Mail.MailAddress(user.Email).ToString())
            {
                return true;
            }

            if (userRepo.GetAllUsers().Where(x => x.Email == user.Email).ToList().Contains(user))
            {
                return true;
            }

            throw new Exception();
        }
    }
}
