using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AuthToken
    {
        public string token { get; set; }
        public DateTime expireDate { get; set; }

    }
}
