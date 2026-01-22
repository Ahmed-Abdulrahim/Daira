namespace Daira.Domain.Models.AuthModel
{
    public class AppRole : IdentityRole<string>
    {
        public string Description { get; set; }
    }
}
