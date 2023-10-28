using System;
using asp_net_with_angular_client_template.Models.Dto;
using asp_net_with_angular_client_template.Models.Response;
using asp_net_with_angular_client_template.Services.Implementation;
using asp_net_with_angular_client_template.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_with_angular_client_template.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController: ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IdentityService identityService)
		{
            _identityService = identityService;
		}

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> Authenticate([FromBody] LoginDto data)
        {
            var result = await _identityService.AuthenticateAsync(data);

            if (result == null)
            {
                return BadRequest(new { message = "Wrong credentials" });
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticationResponse>> RefreshToken([FromBody] RevokeTokenDto model)
        {
            return Ok(await _identityService.RefreshTokenAsync(model.Token));
        }

        [AllowAnonymous]
        [HttpPost("revoke-token")]
        public async Task<ActionResult> RevokeToken([FromBody] RevokeTokenDto model)
        {
            await _identityService.RevokeTokenAsync(model.Token);

            return Ok();
        }
    }
}

