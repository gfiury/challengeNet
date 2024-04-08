using Microsoft.EntityFrameworkCore;
using MovieEntityFramework.Interfaces;
using MovieModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieEntityFramework
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ChallengeNet");
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Preferences> Preferences { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasOne(a => a.Preferences).WithOne(a => a.User).HasForeignKey<Preferences>(fk => fk.IdUser);
        }
    }
}
