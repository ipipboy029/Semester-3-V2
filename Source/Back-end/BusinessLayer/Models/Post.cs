using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostNumber { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }

        public Post()
        {

        }
    }

}
