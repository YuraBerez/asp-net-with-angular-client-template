using System;
using asp_net_with_angular_client_template.DataContext;
using asp_net_with_angular_client_template.Models.Entity;
using asp_net_with_angular_client_template.Repository.Implementations;
using Microsoft.EntityFrameworkCore;

namespace asp_net_with_angular_client_template.Repository.Interfaces
{
	public class UserRepository: IUserRepository
	{
        private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context)
		{
            _context = context;
		}

        async Task<User> IBaseRepository<User>.AddAsync(User data)
        {
            var result = await _context.Users.AddAsync(data);

            return result.Entity;
        }

        async Task<bool> IBaseRepository<User>.DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);

            return true;
        }

        async Task<List<User>> IBaseRepository<User>.GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        async Task<User?> IUserRepository.GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(p => p.Email == email);
        }

        async Task<User?> IBaseRepository<User>.GetById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        async Task IBaseRepository<User>.SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        User IBaseRepository<User>.Update(User data)
        {
            var result = _context.Users.Update(data);

            return result.Entity;
        }
    }
}

