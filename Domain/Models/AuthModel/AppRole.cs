namespace Daira.Domain.Models.AuthModel
{
    public class AppRole : IdentityRole<string>
    {
        public AppRole() : base()
        { }
        public AppRole(string roleName) : base(roleName)
        { }
        public AppRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }
        public string Description { get; set; }
    }
}
