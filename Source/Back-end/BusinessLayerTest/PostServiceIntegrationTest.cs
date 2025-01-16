using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using BusinessLayer;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BusinessLayerTest.IntegrationTests
{
    // Define your in-memory DbContext
    public class InMemoryDbContextFactory
    {
        public static IApplicationDBContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            return context;
        }
    }

    // Test class
    [TestClass]
    public class PostServiceIntegrationTest
    {
        private IApplicationDBContext _context;
        private PostService _postService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize InMemory database
            _context = InMemoryDbContextFactory.CreateDbContext();
            _postService = new PostService(_context);
        }

        [TestMethod]
        public async Task GetPosts_ReturnsAllPosts()
        {
            // Arrange
            var posts = new List<Post>
        {
            new Post { PostNumber = 1, Subject = "Post 1", Description = "Content 1", ImageURL ="test.png" },
            new Post { PostNumber = 2, Subject = "Post 2", Description = "Content 2", ImageURL ="test.png" }
        };

            await _context.Posts.AddRangeAsync(posts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _postService.GetPosts();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(p => p.Subject == "Post 1" && p.Description == "Content 1"));
            Assert.IsTrue(result.Any(p => p.Subject == "Post 2" && p.Description == "Content 2"));
        }

        [TestMethod]
        public async Task AddPost_AddsPostSuccessfully()
        {
            // Arrange
            var post = new Post { PostNumber = 1, Subject = "New Post", Description = "New Content", ImageURL = "test.png" };

            // Act
            var result = await _postService.AddPost(post);

            // Assert
            Assert.IsTrue(result);
            var addedPost = await _context.Posts.FirstOrDefaultAsync(p => p.Subject == post.Subject);
            Assert.IsNotNull(addedPost);
            Assert.AreEqual("New Post", addedPost.Subject);
            Assert.AreEqual("New Content", addedPost.Description);
        }

        [TestMethod]
        public async Task AddPost_ReturnsFalse_WhenSaveFails()
        {
            // Arrange
            var mockContext = new Mock<IApplicationDBContext>();

            // Mock AddAsync to return default (null, in this case, which is acceptable)
            mockContext.Setup(c => c.Posts.AddAsync(It.IsAny<Post>(), default))
                       .Returns(new ValueTask<EntityEntry<Post>>((EntityEntry<Post>)null));

            // Mock SaveChangesAsync to simulate a failure (returning 0)
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(0);

            var postService = new PostService(mockContext.Object);

            var post = new Post { PostNumber = 2, Subject = "Post 2", Description = "Content 2", ImageURL = "test.png" };

            // Act
            var result = await postService.AddPost(post);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

