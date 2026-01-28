namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IAuthService authService) : ControllerBase
    {
        [HttpPost("Change-Password")]
        [Authorize]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.ChangePasswordAsync(userId, changePasswordDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
