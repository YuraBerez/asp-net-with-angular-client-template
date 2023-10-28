using System;
using System.Data;
using asp_net_with_angular_client_template.Models.Entity;
using asp_net_with_angular_client_template.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace asp_net_with_angular_client_template.DataContext
{
	public class ApplicationDbContext: DbContext
	{
        private readonly IHashService _hashService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHashService hashService) : base(options)
        {
            _hashService = hashService;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            BuildRelation(builder);
            SeedData(builder);
        }

        private void BuildRelation(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany(c => c.RefreshTokens).WithOne(c => c.User).OnDelete(DeleteBehavior.Cascade);
        }

        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("c5ca43f8-16c3-4bce-96d3-a12d63e59b90"),
                    FirstName = "Test",
                    LastName = "User",
                    Email = "user@test.com",
                    PasswordHash = _hashService.GetHash("Password1") // password for user is "Password1"
                }
            );
        }
    }
}

