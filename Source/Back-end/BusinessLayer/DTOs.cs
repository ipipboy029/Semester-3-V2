using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class DTOs
    {
        public static PostDTO CreatePostDTO(Post post)
        {
            return new PostDTO { PostNumber = post.PostNumber, Subject = post.Subject, Description = post.Description, ImageURL = post.ImageURL };
        }
    }
}
