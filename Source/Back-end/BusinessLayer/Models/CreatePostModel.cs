using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class CreatePostModel
    {
        public string Subject { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }
    }
}
