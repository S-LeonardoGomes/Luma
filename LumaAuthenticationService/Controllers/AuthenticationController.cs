using LumaAuthenticationService.Models;
using LumaAuthenticationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LumaAuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {

            _authenticationService= authenticationService;
        }

        [HttpPost("Register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            try
            {
                _authenticationService.RegisterUser(user);
                return Ok("Usuário cadastrado com sucesso. Por favor, confirme seu e-email!");
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ConfirmEmail/{userId}")]
        public IActionResult ConfirmEmail([FromRoute] string userId)
        {
            try
            {
                _authenticationService.ConfirmEmail(userId);
                return Ok("Email confirmado com sucesso!");
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            try
            {
                string token = _authenticationService.Login(user);
                return Ok(JsonSerializer.Serialize(new { Token = token, ValidadoAte = $"{DateTime.UtcNow.AddHours(2)}", Timezone = "UTC" }));
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
