namespace Daira.Application.DTOs.RolesDto
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty;


        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
