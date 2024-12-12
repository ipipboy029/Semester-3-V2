using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(string email, string password);

        Task<LoginResponse> LoginUser(string email, string password);
    }
}
