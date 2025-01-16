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
using PasswordVerificationResult = Microsoft.AspNetCore.Identity.PasswordVerificationResult;

namespace BusinessLayer.Tests.UnitTests
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

            // Use the actual PasswordHasher<User>
            var passwordHasher = new PasswordHasher<User>();
            var configurationMock = new Mock<IConfiguration>();

            // Instantiate the UserService with the actual PasswordHasher<User>
            var userService = new UserService(context, passwordHasher, configurationMock.Object);

            // Act
            var newUser = new User
            {
                Email = "test@example.com",
                Password = "SecurePassword123" // Assign plain password (this will be hashed in the RegisterUser method)
            };

            await userService.RegisterUser(newUser.Email, newUser.Password);

            // Assert
            var user = context.Users.SingleOrDefault(u => u.Email == "test@example.com");
            Assert.IsNotNull(user); // Check that the user was created
            Assert.IsTrue(passwordHasher.VerifyHashedPassword(user, user.Password, "SecurePassword123") == PasswordVerificationResult.Success);
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

            // Use the actual PasswordHasher<User>
            var passwordHasher = new PasswordHasher<User>();
            var configurationMock = new Mock<IConfiguration>();

            // Instantiate the UserService with the actual PasswordHasher<User>
            var userService = new UserService(context, passwordHasher, configurationMock.Object);

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

            await userService.RegisterUser(secondUser.Email, secondUser.Password); // Expect exception here
        }
    }
}
