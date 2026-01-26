namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AuthUseCase authUseCase) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<RegisterUserResponseDto>> Register(RegisterUserDto registerUser)
        {
            if (registerUser is null) return BadRequest();
            var result = await authUseCase.RegisterUser(registerUser);
            return Ok(result);
        }
    }
}
