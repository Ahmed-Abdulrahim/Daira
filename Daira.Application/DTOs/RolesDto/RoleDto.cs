namespace Daira.Application.DTOs.RolesDto
{
    public class RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? NormalizedName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
