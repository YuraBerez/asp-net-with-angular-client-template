using System;
using System.Security.Claims;
using System.Security.Cryptography;
using asp_net_with_angular_client_template.Models.Dto;
using asp_net_with_angular_client_template.Models.Entity;
using asp_net_with_angular_client_template.Models.Response;
using asp_net_with_angular_client_template.Repository.Implementations;
using asp_net_with_angular_client_template.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using asp_net_with_angular_client_template.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace asp_net_with_angular_client_template.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;

        public IdentityService(IUserRepository userRepository,
            IHashService hashService)
		{
            _userRepository = userRepository;
            _hashService = hashService;
        }

        public async Task<AuthenticationResponse?> AuthenticateAsync(LoginDto loginData)
        {
            var user = await _userRepository.GetByEmailAsync(loginData.Email);
            if (user == null || _hashService.GetHash(loginData.Password) != user.PasswordHash)
            {
                return null;
            }

            return await GenerateAuthenticateResponce(user);
        }

        public Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task RevokeTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        #region private methods

        private async Task<AuthenticationResponse> GenerateAuthenticateResponce(User user)
        {

            if (user.RefreshTokens == null)
            {
                user.RefreshTokens = new List<UserRefreshToken>();
            }

            var newRefreshToken = GenerateRefreshToken();
            newRefreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);

            _userRepository.Update(user);
            await _userRepository.SaveAsync();

            var jwtToken = GenerateJwtToken(user);

            return new AuthenticationResponse
            {
                AccessToken = jwtToken,
                RefreshToken = newRefreshToken.Token,
                User = user.ToDto()

            };
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
                };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private string GenerateJwtToken(User user)
        {
            var identity = GetIdentity(user);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }



        private UserRefreshToken GenerateRefreshToken()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new UserRefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddMonths(1), // lifetime the token
                Created = DateTime.UtcNow
            };
        }
        #endregion
    }
}

