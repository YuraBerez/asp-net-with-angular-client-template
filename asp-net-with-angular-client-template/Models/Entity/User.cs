using System;
using System.ComponentModel.DataAnnotations;
using asp_net_with_angular_client_template.Models.Dto;

namespace asp_net_with_angular_client_template.Models.Entity
{
	public class User
	{
		[Key]
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }

        public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; }

		public UserDto ToDto() =>
			new UserDto
			{
				Id = Id,
				Email = Email,
				FirstName = FirstName,
				LastName = LastName
			};
    }
}

