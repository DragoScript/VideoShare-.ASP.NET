using COMP367group6.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace COMP367group6.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
       public DbSet<Video> Videos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// If the primary key is not named "Id", specify it explicitly
			modelBuilder.Entity<User>().HasKey(u => u.Id);  // Id is the primary key
			modelBuilder.Entity<Video>().HasKey(v => v.VidId); // VidId is the primary key

		}
	}
}
