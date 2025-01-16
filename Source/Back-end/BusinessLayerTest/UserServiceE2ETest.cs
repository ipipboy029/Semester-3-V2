using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using BusinessLayer.Models;
using BusinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BusinessLayerTest;

[TestClass]
public class UserServiceE2ETests
{
    private DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private ApplicationDbContext _dbContext;
    private PasswordHasher<User> _passwordHasher;
    private Mock<IConfiguration> _configurationMock;

    [TestInitialize]
    public void Setup()
    {
        // Configure in-memory database
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _dbContext = new ApplicationDbContext(_dbContextOptions);

        // Create instance of PasswordHasher<User>
        _passwordHasher = new PasswordHasher<User>();

        // Mock IConfiguration
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["Jwt:SecretKey"]).Returns("test-secret-key-123123278462485762348763248267468723462387423874682164876281943664389716469281647868216046587023645780342687596");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("test-audience");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("test-issuer");
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task RegisterUser_ShouldCreateNewUser_WhenEmailIsUnique()
    {
        // Arrange
        var userService = new UserService(_dbContext, _passwordHasher, _configurationMock.Object);
        var email = "test@example.com";
        var password = "password123";

        // Act
        await userService.RegisterUser(email, password);
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(email, user.Email);
        Assert.IsNotNull(user.Password); // Ensure password is hashed
    }

    [TestMethod]
    public async Task LoginUser_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var userService = new UserService(_dbContext, _passwordHasher, _configurationMock.Object);
        var email = "test@example.com";
        var password = "password123";
        var hashedPassword = _passwordHasher.HashPassword(null, password);

        _dbContext.Users.Add(new User { Email = email, Password = hashedPassword });
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await userService.LoginUser(email, password);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(email, response.User.Email);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Token)); // Token should be generated
    }
}
