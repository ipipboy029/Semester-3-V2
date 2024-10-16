using BusinessLayer;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace osu.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        public readonly PostService _postService;
        public PostController(PostService postService)
        {
            _postService = postService;
        }
        [HttpGet]
        public async Task<IEnumerable<PostDTO>> GetPosts()
        {
            return (await _postService.GetPosts()).Select(post => DTOs.CreatePostDTO(post));
        }
        [HttpPost]
        public async Task<IActionResult> AddPost(CreatePostModel createPostModel)
        {
            Post post = new Post { Subject = createPostModel.Subject, Description = createPostModel.Description, ImageURL = createPostModel.ImageURL };
            return (await _postService.AddPost(post)) ? Ok() : BadRequest();
        }       
    }
}
