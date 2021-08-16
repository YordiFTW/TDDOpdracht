using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TDD_Opdracht.Repositories;
using TDD_Opdracht.Services;

namespace UnitTest
{
    public class Base
    {

        public Base()
        {
            var userRepository = new Mock<IUserRepository>();
            UserDataService userDataService = new UserDataService();
        }
    }
}
