namespace PRM392.Services.DTOs.Account
{
    public class UserAccountDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PictureUrl { get; set; }
        public string[]? Roles { get; set; }

    }
}
