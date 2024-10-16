using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class PostService
    {
        public PostService() { }
        private readonly IApplicationDBContext _context;

        public PostService(IApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }
        public async Task<bool> AddPost(Post post)
        {
            await _context.Posts.AddAsync(post);
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
