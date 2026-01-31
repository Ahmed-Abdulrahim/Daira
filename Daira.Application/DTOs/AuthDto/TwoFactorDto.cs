namespace Daira.Application.DTOs.AuthDto
{
    public class TwoFactorDto
    {
        public string Email { get; set; }
        public int Code { get; set; }
        public bool RememberDevice { get; set; }
    }
}
