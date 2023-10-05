using Microsoft.AspNetCore.Mvc;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Services.Interface;

namespace PrioritiZe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegistrationDTO registrationDTO)
        {
            if (registrationDTO != null)
            {
                var (token, userId, result) = await _authService.RegisterAsync(registrationDTO);
                
                if (result.Succeeded)
                {
                    return Ok(new { Token = token, Message = "Registration successful.", Id = userId });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
                
            }
            else
            {
                return BadRequest("Registration model is null.");
            }

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            if (loginDTO != null)
            {
                var (token, userId, result) = await _authService.LoginAsync(loginDTO);

                if (result.Succeeded)
                {
                    return Ok(new { Token = token, Message = "Login successful.", Id = userId });
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            else
            {
                return BadRequest("Login model is null.");
            }
        }
    }
}
