using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using Moq;
using TDD_Opdracht.DbContext;
using TDD_Opdracht.Models;
using TDD_Opdracht.Repositories;
using TDD_Opdracht.Services;
using Xunit;

namespace TestingProject
{
    public class UnitTest1
    {
        public static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }



        //[Fact]
        //public void GetLockedUsers_Invoke_LockedUsers_v2()
        //{
        //    // Arrange
        //    var fixture = new Fixture();
        //    var lockedUser = fixture.Build<User>().With(u => u.Id).Create();
        //    IList<User> users = new List<User>
        //        {
        //            lockedUser,
        //            fixture.Build<User>().With(u => u.Id).Create(),
        //             fixture.Build<User>().With(u => u.Id).Create()
        //        };

        //    var usersMock = CreateDbSetMock(users);

        //    var userContextMock = new Mock<UserDbContext>();
        //    userContextMock.Setup(x => x.Users).Returns(usersMock.Object);

        //    var usersService = new UserDataService(userContextMock.Object);

        //    // Act
        //    var lockedUsers = usersService.Save(user);

        //    // Assert
        //    Assert.Equal(new List<User> { lockedUser }, lockedUsers);
        //}

        [Fact]
        public void Save_NewUserShouldBeAddedToTheListOfUsers()
        {
            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);

            User newUser = new User();

            IList<User> users = new List<User>()
                {
                    new User { Id = 1, FirstName = "C# Piet", LastName = "Achterman", Email = "test@test.nl" },
                    new User { Id = 2, FirstName = "Klaas", LastName = "Klossen", Email = "2@2.nl" },
                    new User { Id = 3, FirstName = "Jan", LastName = "Danger", Email = "3@3.nl" }
                };

            User user = new User { Id = 4, FirstName = "Piet", LastName = "Bimmer", Email = "1@1.nl" };

            userRepository.Setup(x => x.AddUser(user)).Returns(user);
            newUser = userDataService.Save(user);

            Assert.Equal(newUser,  user);
        }

        [Fact]
        public void Validate_UserShouldBeNull()
        {
            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);
            User user = null;

            //userRepository.Setup(p => p.GetById(1)).Returns("");

            Assert.Throws<ArgumentNullException>(() => userDataService.Validate(user));
        }

        [Fact]
        public void Validate_EmailShouldbeUnique()
        {
            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);




            IList<User> users = new List<User>()
                {
                    new User { Id = 1, FirstName = "C# Piet", LastName = "Achterman", Email = "test@test.nl" },
                    new User { Id = 2, FirstName = "Klaas", LastName = "Klossen", Email = "2@2.nl" },
                    new User { Id = 3, FirstName = "Jan", LastName = "Danger", Email = "3@3.nl" }
                };

            User user = new User { Id = 4, FirstName = "Piet", LastName = "Bimmer", Email = "1@1.nl" };

            userRepository.Setup(mr => mr.GetAllUsers()).Returns(users);

            Assert.True(userDataService.Validate(user));


        }

        [Fact]
        public void Validate_EmailShouldbeAnActualEmail()
        {


            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);


            User user = new User { Id = 1, FirstName = "test", LastName = "232", Email = "dwa@dwadw.nl" };

            Assert.True(userDataService.Validate(user));


        }

        



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_UserLastNameShouldNotBeNullOrEmpty(object value)
        {
            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);

            User user = new User();
            user.Email = "test@adwad.nl";
            user.FirstName = "testadwad";
            user.LastName = (string)value;

            Assert.Throws<ArgumentNullException>(() => userDataService.Validate(user));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_UserEmailShouldNotBeNullOrEmpty(object value)
        {
            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);

            User user = new User();
            user.Email = (string)value;
            user.LastName = "dadwadawd";
            user.FirstName = "123123213";

            Assert.Throws<ArgumentNullException>(() => userDataService.Validate(user));
        }

        //[Fact]
        //public void Save_ShouldWriteUserDataToList()
        //{
        //    var userDataService = new UserDataService();
        //    var userRepository = new Mock<IUserRepository>();

        //    IList<User> users = new List<User>
        //        {
        //            new User { Id = 1, FirstName = "C# Unleashed",
        //                LastName = "111", Email = "1@1.nl" },
        //            new User { Id = 2, FirstName = "ASP.Net Unleashed",
        //                LastName = "222", Email = "2@2.nl" },
        //            new User { Id = 3, FirstName = "Silverlight Unleashed",
        //                LastName = "333", Email = "3@3.nl" }
        //        };

        //    User user = new User {Email = "sfdsfes", FirstName = "sfse", LastName = "sefsef" };

        //    userRepository.Setup(p => p.AddUser(user));

        //    Assert.Throws<ArgumentNullException>(() => userDataService.Validate(user));
        //}

    }
}

