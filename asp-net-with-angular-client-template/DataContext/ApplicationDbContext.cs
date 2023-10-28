using System;
using asp_net_with_angular_client_template.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace asp_net_with_angular_client_template.DataContext
{
	public class ApplicationDbContext: DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}

