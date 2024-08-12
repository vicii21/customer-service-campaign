using CustomerServiceCampaign.API.Jwt;
using CustomerServiceCampaign.Application.Extensions;
using CustomerServiceCampaign.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensions.Msal;
using System.IdentityModel.Tokens.Jwt;

namespace CustomerServiceCampaign.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly JwtManager _manager;

        public AuthController(JwtManager manager)
        {
            _manager = manager;
        }

        // POST: Create new JWT token based on credentials
        [HttpPost]
        public IActionResult Post([FromBody] AuthRequest req, [FromServices] CustomerServiceCampaignContext context)
        {
            string newToken = _manager.MakeToken(req.Email, req.Password);

            return Ok(new { newToken });
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete([FromServices] ITokenStorage storage)
        {
            var tokenId = this.Request.ExtractTokenClaim("jti"); // "jti" is typically the claim for JWT ID

            if (tokenId != null)
            {
                storage.InvalidateToken(tokenId);
            }

            return NoContent();
        }

        //[Authorize]
        //[HttpDelete]
        //public IActionResult InvalidateToken([FromServices] ITokenStorage tokenStorage)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    var header = HttpContext.Request.Headers["Authorization"];
        //    var token = header.ToString().Split("Bearer ")[1];
        //    var tokenObject = handler.ReadJwtToken(token);

        //    string jti = tokenObject.Claims.FirstOrDefault(e => e.Type == "jti").Value;
        //    tokenStorage.InvalidateToken(jti);

        //    return NoContent();
        //}
    }
}
