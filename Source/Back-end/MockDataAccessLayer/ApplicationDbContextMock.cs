using Moq;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContextMock
{
    public static Mock<ApplicationDbContext> Create()
    {
        var mockContext = new Mock<ApplicationDbContext>();

        // Mock Users DbSet
        var users = new List<User>
        {
            new User { Id = 1, Email = "test@example.com", Password = "hashedpassword1" },
            new User { Id = 2, Email = "user@example.com", Password = "hashedpassword2" }
        }.AsQueryable();

        var mockUsersDbSet = new Mock<DbSet<User>>();
        mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        mockContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);

        // Mock Posts DbSet
        var posts = new List<Post>
        {
            new Post { PostNumber = 1, Subject = "First post" },
            new Post { PostNumber = 2, Subject = "Second post" }
        }.AsQueryable();

        var mockPostsDbSet = new Mock<DbSet<Post>>();
        mockPostsDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
        mockPostsDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
        mockPostsDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
        mockPostsDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

        mockContext.Setup(c => c.Posts).Returns(mockPostsDbSet.Object);

        return mockContext;
    }
}
