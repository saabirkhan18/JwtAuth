using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuth.Models;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtAuth.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {
        private readonly IjwtAuthenticationManager jwtAuthenticationManager;
        private readonly ITokenRefresher tokenRefresher;
        public NameController(IjwtAuthenticationManager jwtAuthenticationManager, ITokenRefresher tokenRefresher)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.tokenRefresher = tokenRefresher;
        }
        // GET: api/<NameValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = jwtAuthenticationManager.Authenticate(userCred.username, userCred.password);
            if (token == null)
            {
                token.message = "unauthorized";
                return Unauthorized();
            }
            else
            {
                token.message = "success";
                return Ok(token);
            }
        }



        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenCred refreshTokenCred)
        {
            try
            {
                var token = tokenRefresher.Refresh(refreshTokenCred);
                if (token == null)
                {
                    token.message = "unauthorized";
                    return Unauthorized(new AuthenticationResponse { jwt_token = "invalid jwt token", refresh_token = "invalid refresh token" });
                }
                else
                {
                    token.message = "success";
                    return Ok(token);
                }
            }
            catch (Exception ex)
            {
                return Unauthorized(new AuthenticationResponse { jwt_token = "invalid jwt token", refresh_token = "invalid refresh token", message = ex.ToString() });
            }
        }
    }
}
