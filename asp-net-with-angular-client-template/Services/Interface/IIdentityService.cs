using System;
using asp_net_with_angular_client_template.Models.Dto;
using asp_net_with_angular_client_template.Models.Response;

namespace asp_net_with_angular_client_template.Services.Interface
{
	public interface IIdentityService
	{
        Task<AuthenticationResponse?> AuthenticateAsync(LoginDto loginData);
        Task<AuthenticationResponse?> RefreshTokenAsync(string refreshToken);
        Task RevokeTokenAsync(string token);
    }
}

