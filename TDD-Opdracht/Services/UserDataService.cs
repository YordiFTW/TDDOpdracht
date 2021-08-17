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
            userRepo.AddUser(user);

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

            if (!userRepo.GetAllUsers().Any(i =>i.Email == user.Email || i.Email == user.Email))
            {
                return false;
            }
           
            throw new Exception();
        }
    }
}
