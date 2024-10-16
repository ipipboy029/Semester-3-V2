using BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IApplicationDBContext
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }

        public Task Save();

        public Task<int> SaveChangesAsync();
    }
}
