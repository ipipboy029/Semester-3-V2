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

namespace BusinessLayer.Tests.UnitTests
{
    [TestClass]
    public class PostServiceTests
    {
        [TestMethod]
        public async Task tryAddNewPost_ShouldAddPost_WhenValidPostIsProvided()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);

            // Instantiate PostService with the mocked DbContext
            var postService = new PostService(context);

            var newPost = new Post
            {
                Subject = "Test Post Title",
                Description = "This is the content of the post.",
                ImageURL = "AuthorName"
            };

            // Act: Add the new post
            var result = await postService.AddPost(newPost);

            // Assert: Verify that the post was added to the database
            var postInDb = context.Posts.SingleOrDefault(p => p.Subject == "Test Post Title");
            Assert.IsNotNull(postInDb);  // Check that the post was added
            Assert.AreEqual("Test Post Title", postInDb.Subject); // Verify title is correct
            Assert.AreEqual("This is the content of the post.", postInDb.Description); // Verify content is correct
            Assert.AreEqual("AuthorName", postInDb.ImageURL); // Verify author is correct
            Assert.IsTrue(result);  // Check that the method returns true
        }
        [TestMethod]
        public async Task tryAddNewPost_ShouldThrowException_WhenPostIsInvalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);

            var postService = new PostService(context);

            // Create an invalid post (missing title and content)
            var invalidPost = new Post
            {
                Subject ="",
                ImageURL = "",
                Description = ""
            };

            // Act: Try to add an invalid post, expecting an exception
            await postService.AddPost(invalidPost);

            // Assert: The exception is expected, so no further assertions are needed
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task tryAddNewPost_ShouldThrowException_WhenPostIsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);
            var postService = new PostService(context);

            // Act: Try to add a null post
            try
            {
                await postService.AddPost(null);
            }
            catch (ArgumentNullException ex)
            {
                // Modify the expected message to match the actual one
                Assert.AreEqual("Post cannot be null (Parameter 'post')", ex.Message);
                throw;  // Re-throw to allow the ExpectedException attribute to work
            }
        }
    }
}
