using System;
namespace asp_net_with_angular_client_template.Models.Dto
{
	public class UserDto
	{
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}

