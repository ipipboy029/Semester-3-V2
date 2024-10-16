using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer
{
    public class UserService : IUserService
    {
        private readonly IApplicationDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IApplicationDBContext dbContext, PasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterUser(string email, string password)
        {
            // Check if the user already exists
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                throw new Exception("User already exists");
            }

            var user = new User
            {
                Email = email,
            };

            // Hash the password
            user.Password = _passwordHasher.HashPassword(user, password);

            // Add and save user to database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> LoginUser(string email, string password)
        {
            // Retrieve user from the database
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null; // User not found
            }

            // Verify the password hash
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null; // Invalid password
            }

            return user; // Login successful
        }
    }
}
