namespace silicon_profileProvider.Models
{
    public class UserProfileResult
    {
        public string ProfileImg { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Bio { get; set; }

    }
}
