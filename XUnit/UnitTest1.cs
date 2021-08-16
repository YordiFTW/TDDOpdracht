using System;
using Xunit;

namespace XUnit
{
    public class UnitTest1
    {
        [Fact]
        public void Validate_UserShouldBeNull()
        {
            var userDataService = new UserDataService();
            User user = null;

            //userRepository.Setup(p => p.GetById(1)).Returns("");

            Assert.Throws<ArgumentNullException>(() => userDataService.Validate(user));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_UserLastNameShouldNotBeNullOrEmpty(object value)
        {
            var userDataService = new UserDataService();

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
            var userDataService = new UserDataService();

            User user = new User();
            user.Email = (string)value;
            user.LastName = "dadwadawd";

            Assert.Throws<ArgumentNullException>(() => userDataService.Validate(user));
        }


    }
}

