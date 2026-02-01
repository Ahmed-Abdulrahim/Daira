namespace Daira.Infrastructure.Services.AuthService
{
    public class RoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork, ILogger<RoleService> logger) : IRoleService
    {
        //AssignRoleToUserAsync
        public async Task<AssignRoleResponse> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return AssignRoleResponse.Failure("User not found.");
            }

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                return AssignRoleResponse.Failure($"Role '{roleName}' does not exist.");
            }

            if (await userManager.IsInRoleAsync(user, roleName))
            {
                return AssignRoleResponse.Failure($"User is already in role '{roleName}'.");
            }

            var result = await userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return AssignRoleResponse.Failure(result.Errors.First().Description);
            }

            logger.LogInformation("Assigned role {RoleName} to user {UserId}", roleName, userId);
            return AssignRoleResponse.AssignSuccess(userId, roleName);
        }

        //CreateRole
        public async Task<RoleResponse> CreateRoleAsync(CreateRoleDto dto)
        {
            if (await roleManager.RoleExistsAsync(dto.Name))
            {
                return RoleResponse.Failure($"Role '{dto.Name}' already exists.");
            }
            var role = new AppRole(dto.Name, dto.Description ?? string.Empty);
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return RoleResponse.Failure(result.Errors.Select(e => e.Description));
            }

            logger.LogInformation("Created role: {RoleName}", dto.Name);
            return RoleResponse.Success(MapToDto(role), $"Role '{dto.Name}' created successfully.");
        }

        //Delete Role
        public async Task<RoleResponse> DeleteRoleAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RoleResponse.Failure("Role not found.");
            }

            // Check if any users are assigned to this role
            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                return RoleResponse.Failure($"Cannot delete role '{role.Name}'. It is assigned to {usersInRole.Count} user(s).");
            }

            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return RoleResponse.Failure(result.Errors.Select(e => e.Description));
            }

            logger.LogInformation("Deleted role: {RoleName}", role.Name);
            return RoleResponse.Success(MapToDto(role), $"Role '{role.Name}' deleted successfully.");
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

        //updateRole
        public async Task<RoleResponse> UpdateRoleAsync(string roleId, UpdateRoleDto dto)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RoleResponse.Failure("Role not found.");
            }

            // Check if new name conflicts with existing role
            if (!string.IsNullOrEmpty(dto.Name) && dto.Name != role.Name)
            {
                if (await roleManager.RoleExistsAsync(dto.Name))
                {
                    return RoleResponse.Failure($"Role '{dto.Name}' already exists.");
                }
                role.Name = dto.Name;
            }

            if (dto.Description != null)
            {
                role.Description = dto.Description;
            }


            var result = await roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return RoleResponse.Failure(result.Errors.Select(e => e.Description));
            }

            logger.LogInformation("Updated role: {RoleId}", roleId);
            return RoleResponse.Success(MapToDto(role), "Role updated successfully.");
        }

        //Private
        private static RoleDto MapToDto(AppRole role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                NormalizedName = role.NormalizedName,
                Description = role.Description,

            };
        }
    }
}
