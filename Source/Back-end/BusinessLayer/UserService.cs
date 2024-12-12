using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserService : IUserService
    {
        private readonly IApplicationDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(IApplicationDBContext dbContext, PasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task RegisterUser(string email, string password)
        {
            // Step 1: Check if the email already exists
            var existingUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                throw new Exception("Email is already taken"); // Email already in use
            }

            // Step 2: Hash the password
            var hashedPassword = _passwordHasher.HashPassword(null, password);

            // Step 3: Create the user object
            var user = new User
            {
                Email = email,
                Password = hashedPassword
                // Add other properties as necessary (e.g., Name, DateCreated, etc.)
            };

            // Step 4: Save the user to the database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(); // Persist the user to the database
        }

        public async Task<LoginResponse> LoginUser(string email, string password)
        {
            // Retrieve user from the database
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("Invalid email or password"); // User not found
            }

            // Verify the password hash
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid email or password"); // Invalid password
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Return the user and the generated JWT token (as part of a response object)
            return new LoginResponse
            {
                User = user,
                Token = token
            };
        }

        private string GenerateJwtToken(User user)
        {
            // Define JWT claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Get the secret key from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define token expiration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"]
            };

            // Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
