using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Core.Domain;

namespace ChatApp.Infrastructure.Repositories
{
    public class AplicationDbContext       :DbContext
    {
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ChatApp;Trusted_Connection=True;";

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("ChatApp.Api"));
        }
    }
}
