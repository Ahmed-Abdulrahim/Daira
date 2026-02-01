using Microsoft.AspNetCore.Authorization;

namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    [Produces("application/json")]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterResponse>> RegisterUser(RegisterDto registerDto)
        {
            var result = await authService.RegisterAsync(registerDto);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //Login
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Login(LoginDto loginDto)
        {
            var result = await authService.LoginAsync(loginDto);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //ConfirmEmail
        [HttpGet("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailDto dto)
        {
            var result = await authService.ConfirmEmailAsync(dto);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //GetNewAccessToken
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RefreshTokenResponse>> GetNewAccessToken(RefreshTokenDto tokenRequest)
        {
            var result = await authService.RefreshTokenAsync(tokenRequest);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
