using Microsoft.EntityFrameworkCore;
using MovieModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieEntityFramework.Interfaces
{
    public interface IApplicationContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Preferences> Preferences { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
