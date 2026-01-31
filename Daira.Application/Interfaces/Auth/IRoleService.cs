namespace Daira.Application.Interfaces.Auth
{
    public interface IRoleService
    {
        Task<RoleListResponse> GetAllRolesAsync();
        Task<RoleResponse> GetRoleByIdAsync(string roleId);
        Task<RoleResponse> GetRoleByNameAsync(string roleName);
        Task<RoleResponse> CreateRoleAsync(CreateRoleDto dto);
        Task<RoleResponse> UpdateRoleAsync(string roleId, UpdateRoleDto dto);
        Task<RoleResponse> DeleteRoleAsync(string roleId);
        Task<UserRolesResponse> GetUserRolesAsync(string userId);
        Task<AssignRoleResponse> AssignRoleToUserAsync(string userId, string roleName);
        Task<AssignRoleResponse> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<UsersInRoleResponse> GetUsersInRoleAsync(string roleName);
    }
}
