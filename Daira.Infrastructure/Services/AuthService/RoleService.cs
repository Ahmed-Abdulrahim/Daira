using Daira.Application.DTOs.RolesDto;
using Daira.Application.Response.Roles;
using Microsoft.AspNetCore.Identity;

namespace Daira.Infrastructure.Services.AuthService
{
    public class RoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork, ILogger<RoleService> logger) : IRoleService
    {
        public Task<AssignRoleResponse> AssignRoleToUserAsync(string userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<RoleResponse> CreateRoleAsync(CreateRoleDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<RoleResponse> DeleteRoleAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        //GetAllRoles
        public async Task<RoleListResponse> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.OrderBy(r => r.Name).Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name!,
                NormalizedName = r.NormalizedName,
                Description = r.Description,
            }).ToListAsync();

            return RoleListResponse.Success(roles);
        }

        //GetRoleById
        public async Task<RoleResponse> GetRoleByIdAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RoleResponse.Failure("Role not found.");
            }
            var roleDto = new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                NormalizedName = role.NormalizedName,
                Description = role.Description,
            };
            return RoleResponse.Success(roleDto);
        }

        //GetRoleByName
        public async Task<RoleResponse> GetRoleByNameAsync(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is null) return RoleResponse.Failure("Role not found.");
            var roleDto = new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                NormalizedName = role.NormalizedName,
                Description = role.Description,
            };
            return RoleResponse.Success(roleDto);
        }

        //GetRoleForSpecificUser
        public async Task<UserRolesResponse> GetUserRolesAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return UserRolesResponse.Failure("User not found.");
            var roles = await userManager.GetRolesAsync(user);
            return UserRolesResponse.Success(userId, user.Email!, user.FullName, roles);
        }

        //GetUsersInRole
        public async Task<UsersInRoleResponse> GetUsersInRoleAsync(string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                return UsersInRoleResponse.Failure($"Role '{roleName}' does not exist.");
            }

            var users = await userManager.GetUsersInRoleAsync(roleName);

            var userDtos = new List<UserRoleDto>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userDtos.Add(new UserRoleDto
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    FullName = user.FullName,
                    Roles = roles.ToList()
                });
            }

            return UsersInRoleResponse.Success(roleName, userDtos);
        }

        //RemoveRoleFromUser
        public async Task<AssignRoleResponse> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return AssignRoleResponse.Failure("User not found.");
            }

            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                return AssignRoleResponse.Failure($"User is not in role '{roleName}'.");
            }

            var result = await userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return AssignRoleResponse.Failure(result.Errors.First().Description);
            }

            logger.LogInformation("Removed role {RoleName} from user {UserId}", roleName, userId);
            return AssignRoleResponse.RemoveSuccess(userId, roleName);
        }

        public Task<RoleResponse> UpdateRoleAsync(string roleId, UpdateRoleDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
