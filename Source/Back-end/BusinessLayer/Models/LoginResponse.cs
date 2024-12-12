using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
