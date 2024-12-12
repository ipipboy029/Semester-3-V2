using BusinessLayer;
using BusinessLayer.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Tests
{
    [TestClass]
    public class PostServiceTests
    {
        private Mock<IApplicationDBContext> _mockDbContext;
        private Mock<DbSet<Post>> _mockPostsDbSet;
        private Mock<EntityEntry<Post>> _mockEntityEntry;
        private PostService _postService;

        [TestInitialize]
        public void Setup()
        {
            _mockDbContext = new Mock<IApplicationDBContext>();
            _mockPostsDbSet = new Mock<DbSet<Post>>();
            _mockEntityEntry = new Mock<EntityEntry<Post>>();
            _postService = new PostService(_mockDbContext.Object);
        }

        [TestMethod]
        public async Task GetPosts_ShouldReturnPosts_WhenPostsExist()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { PostNumber = 1, Subject = "First post" },
                new Post { PostNumber = 2, Subject = "Second post" }
            };

            var queryablePosts = posts.AsQueryable();
            _mockPostsDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(queryablePosts.Provider);
            _mockPostsDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(queryablePosts.Expression);
            _mockPostsDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(queryablePosts.ElementType);
            _mockPostsDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(queryablePosts.GetEnumerator());

            // Setup DbContext to return the mocked DbSet
            _mockDbContext.Setup(c => c.Posts).Returns(_mockPostsDbSet.Object);

            // Act
            var result = await _postService.GetPosts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("First post", result.First().Subject);
        }

        [TestMethod]
        public async Task AddPost_ShouldReturnTrue_WhenPostIsAddedSuccessfully()
        {
            // Arrange
            var newPost = new Post { PostNumber = 3, Subject = "New post" };

            // Mock AddAsync to return an EntityEntry
            _mockPostsDbSet.Setup(m => m.AddAsync(It.IsAny<Post>(), default))
                           .ReturnsAsync(_mockEntityEntry.Object);

            // Mock SaveChangesAsync to return 1 (indicating success)
            _mockDbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(1);

            // Act
            var result = await _postService.AddPost(newPost);

            // Assert
            Assert.IsTrue(result);
            _mockDbContext.Verify(c => c.Posts.AddAsync(It.IsAny<Post>(), default), Times.Once);
            _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task AddPost_ShouldReturnFalse_WhenSaveChangesFails()
        {
            // Arrange
            var newPost = new Post { PostNumber = 3, Subject = "New post" };

            // Mock AddAsync to return an EntityEntry
            _mockPostsDbSet.Setup(m => m.AddAsync(It.IsAny<Post>(), default))
                           .ReturnsAsync(_mockEntityEntry.Object);

            // Mock SaveChangesAsync to return 0 (indicating failure)
            _mockDbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(0);

            // Act
            var result = await _postService.AddPost(newPost);

            // Assert
            Assert.IsFalse(result);
            _mockDbContext.Verify(c => c.Posts.AddAsync(It.IsAny<Post>(), default), Times.Once);
            _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
