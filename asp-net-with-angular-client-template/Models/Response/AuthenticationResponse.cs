using System;
using asp_net_with_angular_client_template.Models.Dto;

namespace asp_net_with_angular_client_template.Models.Response
{
	public class AuthenticationResponse
    {
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public UserDto User { get; set; }
	}
}

