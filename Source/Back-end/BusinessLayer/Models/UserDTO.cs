using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class UserDTO
    {
        public class UserRegisterDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UserLoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
