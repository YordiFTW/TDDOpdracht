using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        [Theory]
        [InlineData("test@test.nl")]
        [InlineData("2@2.nl")]
        [InlineData("3@3.nl")]
        [InlineData("4@4.nl")]
        [InlineData("5@5.nl")]
        public void Validate_EmailShouldbeUnique(object value)
        {
            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);

            bool isUnique = false;

            IList<User> users = new List<User>()
                {
                    new User { Id = 1, FirstName = "C# Piet", LastName = "Achterman", Email = "test@test.nl" },
                    new User { Id = 2, FirstName = "Klaas", LastName = "Klossen", Email = "2@2.nl" },
                    new User { Id = 3, FirstName = "Jan", LastName = "Danger", Email = "3@3.nl" }
                };

            User user = new User { Id = 4, FirstName = "Piet", LastName = "Bimmer", Email = value.ToString() };

            userRepository.Setup(mr => mr.GetAllUsers()).Returns(users);


            if (!users.Any(i => i.Email == user.Email || i.Email == user.Email))
            {
                isUnique = true;
            }

            Assert.True(isUnique);

        }

        [Theory]
        [InlineData("yordiappelnl")]
        [InlineData("yordi@.nl")]
        [InlineData("@appel.nl")]
        [InlineData("yordi@nl")]
        [InlineData("yordi@!nl")]
        [InlineData("yordi@@appel..nl")]
        [InlineData("@.nl")]
        [InlineData("yordi@appel.nl")]
        public void Validate_EmailShouldbeAnActualEmail(object value)
        {

            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);


            User user = new User { Id = 1, FirstName = "test", LastName = "232", Email = value.ToString() };

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

        

        [Fact]
        public async Task StoreUsersInMoqDataBase()
        {
            //Arrange  
            var options = new DbContextOptionsBuilder<UserDbContext>()
                 .UseInMemoryDatabase("InMemoryDb")
                 .Options;
            var _dbContext = new UserDbContext(options);

            await _dbContext.Users.AddRangeAsync(
                new User() { Id = 1, FirstName = "Piet", LastName = "Dimmer", Email = "test1@test.nl" },
                new User() { Id = 2, FirstName = "Barry", LastName = "Bimmer", Email = "test2@test.nl" },
                new User() { Id = 3, FirstName = "Jan", LastName = "Zimmer", Email = "test3@test.nl" }
                );
            await _dbContext.SaveChangesAsync();

            //Act  
            var result = await _dbContext.Users.Select(p => p).ToArrayAsync();

            //Assert  
            Assert.Equal(3, _dbContext.Users.Count());
        }

        [Fact]
        public async Task Save_StoreNewUserInMoqDataBase()
        {
            var userRepository = new Mock<IUserRepository>();
            var dbContext = new Mock<UserDbContext>();
            var userDataService = new UserDataService(userRepository.Object, dbContext.Object);

            
            var options = new DbContextOptionsBuilder<UserDbContext>()
                 .UseInMemoryDatabase("InMemoryDbq")
                 .Options;
            var _dbContext = new UserDbContext(options);

            User user = new User { Id = 5, FirstName = "Fliet", LastName = "Klimmer", Email = "test4@test.nl" };

            await _dbContext.Users.AddRangeAsync(
                new User() { Id = 1, FirstName = "Piet", LastName = "Dimmer", Email = "test1@test.nl" },
                new User() { Id = 2, FirstName = "Barry", LastName = "Bimmer", Email = "test2@test.nl" },
                new User() { Id = 3, FirstName = "Jan", LastName = "Zimmer", Email = "test3@test.nl" }
                );
            await _dbContext.SaveChangesAsync();

             
            var result = await _dbContext.Users.Select(p => p).ToArrayAsync();

            

            userDataService.Save(user);


            userRepository.Setup(x => x.AddUser(user)).Returns(_dbContext.Users.Add(user));
            userRepository.Setup(x => x.AddUser(user)).Returns(_dbContext.SaveChanges());
            userRepository.Setup(x => x.AddUser(user)).Returns(_dbContext.Users.ToList());
             
            Assert.Equal(4, _dbContext.Users.Count());
        }

    }
}

