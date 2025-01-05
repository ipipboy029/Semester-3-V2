using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.AspNet.Identity;
using System;

namespace BusinessLayer.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public async Task RegisterUser_ShouldAddUser_WhenEmailNotTaken()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);

            // Mocking IPasswordHasher<User>
            var passwordHasherMock = new Mock<PasswordHasher<User>>();
            var configurationMock = new Mock<IConfiguration>();

            // Setup mock to hash password
            passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<User>(), "SecurePassword123"))
                              .Returns("hashedPassword123");

            // Instantiate the UserService with the mocked IPasswordHasher<User>
            var userService = new UserService(context, passwordHasherMock.Object, configurationMock.Object);

            // Act
            // Create a new User object and hash the password
            var newUser = new User
            {
                Email = "test@example.com",
                Password = "SecurePassword123" // Assign plain password (this will be hashed in the RegisterUser method)
            };

            // Act: Register the user with the service
            await userService.RegisterUser(newUser.Email, newUser.Password);

            // Assert
            var user = context.Users.SingleOrDefault(u => u.Email == "test@example.com");
            Assert.IsNotNull(user); // Check that the user was created
            Assert.AreEqual("hashedPassword123", user.Password); // Check that the password is hashed correctly
        }




        [TestMethod]
        [ExpectedException(typeof(Exception), "Email is already taken")]
        public async Task RegisterUser_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);

            // Mocking IPasswordHasher<User>
            var passwordHasherMock = new Mock<PasswordHasher<User>>();
            var configurationMock = new Mock<IConfiguration>();

            // Setup mock to hash password
            passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<User>(), "SecurePassword123"))
                              .Returns("hashedPassword123");

            // Instantiate the UserService with the mocked IPasswordHasher<User>
            var userService = new UserService(context, passwordHasherMock.Object, configurationMock.Object);

            // First, register the user with a unique email
            var firstUser = new User
            {
                Email = "test@example.com",
                Password = "SecurePassword123"
            };
            await userService.RegisterUser(firstUser.Email, firstUser.Password); // First user registration

            // Act: Try to register a user with the same email again
            var secondUser = new User
            {
                Email = "test@example.com",
                Password = "SecurePassword123"
            };

            // This should throw an exception because the email already exists
            await userService.RegisterUser(secondUser.Email, secondUser.Password); // Expect exception here

            // The expected exception is declared above, so no need for further assertions
        }
    }
}
