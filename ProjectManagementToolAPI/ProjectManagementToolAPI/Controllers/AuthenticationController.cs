using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementToolAPI.Models;
using ProjectManagementToolAPI.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace ProjectManagementToolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequest)
        {
            // Validate incoming request
            if(ModelState.IsValid)
            {
                var userExist = await _userManager.FindByEmailAsync(registrationRequest.Email);

                if (userExist != null) 
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>() { "Email already exist" }
                    });
                }

                // Create user
                var user = new IdentityUser()
                {
                    UserName = registrationRequest.Name,
                    Email = registrationRequest.Email
                };

                var isCreated = await _userManager.CreateAsync(user, registrationRequest.Password);
                if (isCreated.Succeeded)
                {
                    // Generate token
                    var token = GenerateJwtToken(user);
                    return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token
                    });
                }

                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() { "Saving user in database failed" }
                });
            }

            return BadRequest();
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            // Token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
