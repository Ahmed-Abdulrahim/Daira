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
        [HttpGet("getById/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> GetById(string roleId)
        {
            var result = await roleService.GetRoleByIdAsync(roleId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }


        //GetRoleByName
        [HttpGet("getByName/{roleName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> GetByName(string roleName)
        {
            var result = await roleService.GetRoleByNameAsync(roleName);
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
        [HttpGet("UsersInRole/{roleName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsersInRoleResponse>> GetUsersInRole(string roleName)
        {
            var result = await roleService.GetUsersInRoleAsync(roleName);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //RemoveRoleFromUser
        [HttpDelete("user/{userId}/remove/{roleName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AssignRoleResponse>> RemoveRoleFromUser(string userId, string roleName)
        {
            var result = await roleService.RemoveRoleFromUserAsync(userId, roleName);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //AssignRoleToUser
        [HttpPost("AssignRole/{roleName}/User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AssignRoleResponse>> AssignRoleToUser(string userId, string roleName)
        {
            var result = await roleService.AssignRoleToUserAsync(userId, roleName);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //DeleteRole
        [HttpDelete("deleteRole/{roleName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> DeleteRole(string roleName)
        {
            var result = await roleService.DeleteRoleAsync(roleName);
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
