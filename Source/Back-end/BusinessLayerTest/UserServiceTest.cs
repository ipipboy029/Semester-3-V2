using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq.Expressions;

namespace BusinessLayer.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IApplicationDBContext> _mockDbContext;
        private PasswordHasher<User> _passwordHasher;
        private Mock<IConfiguration> _mockConfiguration;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _mockDbContext = new Mock<IApplicationDBContext>();
            _passwordHasher = new PasswordHasher<User>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Set up mock configuration for JWT
            _mockConfiguration.Setup(config => config["Jwt:SecretKey"]).Returns("TestSecretKey123");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("TestAudience");

            _userService = new UserService(_mockDbContext.Object, _passwordHasher, _mockConfiguration.Object);
        }

        [TestMethod]
        public async Task RegisterUser_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";

            _mockDbContext.Setup(db => db.Users.SingleOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
                          .ReturnsAsync(new User { Email = email });

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => _userService.RegisterUser(email, password));
            Assert.AreEqual("Email is already taken", exception.Message);
        }

        [TestMethod]
        public async Task RegisterUser_ShouldCreateUser_WhenEmailDoesNotExist()
        {
            // Arrange
            var email = "newuser@example.com";
            var password = "Password123!";

            _mockDbContext.Setup(db => db.Users.SingleOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
                          .ReturnsAsync((User)null); // No user found

            var users = new List<User>();
            _mockDbContext.Setup(db => db.Users.Add(It.IsAny<User>()))
                          .Callback<User>(user => users.Add(user));
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1); // Return 1 for successful save

            // Act
            await _userService.RegisterUser(email, password);

            // Assert
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(email, users[0].Email);
            Assert.IsNotNull(users[0].Password);
            _mockDbContext.Verify(db => db.Users.Add(It.IsAny<User>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task LoginUser_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var email = "test@example.com";
            var password = "WrongPassword123!"; // Incorrect password
            var correctPassword = "CorrectPassword123!"; // Correct password

            // Set up in-memory user data with hashed password
            var users = new List<User>
    {
        new User
        {
            Id = 1,
            Email = email,
            Password = _passwordHasher.HashPassword(null, correctPassword) // Hash the correct password
        }
    };

            var usersQueryable = users.AsQueryable(); // Convert to IQueryable for LINQ compatibility

            // Mock DbSet<User>
            var mockUsersDbSet = new Mock<DbSet<User>>();

            // Set up the necessary IQueryable methods for the DbSet
            mockUsersDbSet.As<IQueryable<User>>()
                .Setup(m => m.Provider).Returns(usersQueryable.Provider);
            mockUsersDbSet.As<IQueryable<User>>()
                .Setup(m => m.Expression).Returns(usersQueryable.Expression);
            mockUsersDbSet.As<IQueryable<User>>()
                .Setup(m => m.ElementType).Returns(usersQueryable.ElementType);
            mockUsersDbSet.As<IQueryable<User>>()
                .Setup(m => m.GetEnumerator()).Returns(usersQueryable.GetEnumerator());

            // Mock the SingleOrDefaultAsync method to return a user
            mockUsersDbSet.Setup(d => d.SingleOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Expression<Func<User, bool>> predicate, CancellationToken token) =>
                    users.AsQueryable().SingleOrDefault(predicate)); // Simulate async behavior

            // Setup the mock context to return the mocked DbSet
            _mockDbContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => _userService.LoginUser(email, password));

            // Assert
            Assert.AreEqual("Invalid email or password", exception.Message);
        }

        [TestMethod]
        public async Task LoginUser_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "CorrectPassword123!";

            var user = new User
            {
                Email = email,
                Password = _passwordHasher.HashPassword(null, password)
            };

            _mockDbContext.Setup(db => db.Users.SingleOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
                          .ReturnsAsync(user);

            // Act
            var result = await _userService.LoginUser(email, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Token);
            Assert.AreEqual(email, result.User.Email);
        }
    }
}
