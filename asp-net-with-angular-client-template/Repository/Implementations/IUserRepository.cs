using System;
using asp_net_with_angular_client_template.Models.Entity;

namespace asp_net_with_angular_client_template.Repository.Implementations
{
	public interface IUserRepository: IBaseRepository<User>
	{
		Task<User?> GetByEmailAsync(string email);
	}
}

