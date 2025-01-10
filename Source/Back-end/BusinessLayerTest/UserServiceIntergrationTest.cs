using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using BusinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayerTest
{
    [TestClass]
    public class UserServiceIntergrationTests
    {
        private UserService _userService;
        private Mock<IConfiguration> _mockConfiguration;
        private PasswordHasher<User> _passwordHasher;
        private ApplicationDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            // Mock IConfiguration
            _mockConfiguration = new Mock<IConfiguration>();

            // Use In-Memory Database for IApplicationDBContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            _dbContext = dbContext;

            // Instantiate PasswordHasher
            _passwordHasher = new PasswordHasher<User>();

            // Instantiate UserService
            _userService = new UserService(_dbContext, _passwordHasher, _mockConfiguration.Object);
        }

        [TestMethod]
        public async Task RegisterUser_ShouldCreateNewUser_WhenEmailIsUnique()
        {
            // Arrange
            var email = "testing@example.com";
            var password = "TestPassword123";

            // Act
            await _userService.RegisterUser(email, password);

            // Assert
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            Assert.IsNotNull(user);
            Assert.AreEqual(email, user.Email);
            Assert.AreNotEqual(password, user.Password); // Password should be hashed
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task RegisterUser_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var email = "test123@example.com";
            var password = "TestPassword123";
            await _userService.RegisterUser(email, password);

            // Act
            await _userService.RegisterUser(email, "NewPassword123");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task LoginUser_ShouldThrowException_WhenEmailDoesNotExist()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var password = "TestPassword123";

            // Act
            await _userService.LoginUser(email, password);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task LoginUser_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var email = "test123@example.com";
            var password = "TestPassword123";
            await _userService.RegisterUser(email, password);

            // Act
            await _userService.LoginUser(email, "WrongPassword");
        }
    }

}
