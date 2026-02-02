namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        //GetAllRoles
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> GetRoles()
        {
            var result = await roleService.GetAllRolesAsync();
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //GetRoleById
        [HttpGet("getById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> GetById(string id)
        {
            var result = await roleService.GetRoleByIdAsync(id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }


        //GetRoleByName
        [HttpGet("getByName/{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> GetByName(string Name)
        {
            var result = await roleService.GetRoleByNameAsync(Name);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //CreateRole
        [HttpPost("CreateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            var result = await roleService.CreateRoleAsync(createRoleDto);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //GetRoleFor Specific User
        [HttpGet("GetUserRole/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserRolesResponse>> GetUserRole(string userId)
        {
            var result = await roleService.GetUserRolesAsync(userId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }


        //GetUsersInRole
        [HttpGet("UsersInRole/{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsersInRoleResponse>> GetUsersInRole(string Name)
        {
            var result = await roleService.GetUsersInRoleAsync(Name);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //RemoveRoleFromUser
        [HttpDelete("user/{userId}/remove/{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AssignRoleResponse>> RemoveRoleFromUser(string userId, string Name)
        {
            var result = await roleService.RemoveRoleFromUserAsync(userId, Name);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //AssignRoleToUser
        [HttpPost("AssignRole/{Name}/User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AssignRoleResponse>> AssignRoleToUser(string userId, string Name)
        {
            var result = await roleService.AssignRoleToUserAsync(userId, Name);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //DeleteRole
        [HttpDelete("deleteRole/{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> DeleteRole(string Name)
        {
            var result = await roleService.DeleteRoleAsync(Name);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //UpdateRole
        [HttpPut("updateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateRole(string roleId, [FromBody] UpdateRoleDto updateRoleDto)
        {
            var result = await roleService.UpdateRoleAsync(roleId, updateRoleDto);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

    }
}
