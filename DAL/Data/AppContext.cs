using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class AppDataContext : IdentityDbContext<AppUser>
    {
        private readonly IConfiguration _configuration;

        public AppDataContext(DbContextOptions<AppDataContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=RikMortyApp;Integrated Security=True");
            //optionsBuilder.UseSqlServer(_configuration["AppDB"]);
        }

        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           /* var character =modelBuilder.Entity<Character>();
            character
                .HasOne(u => u.User)
                .WithMany(ch => ch.Characters)
                .HasForeignKey(u => u.UserId);*/
        }
    }
}
