namespace silicon_profileProvider.Models
{
    public class GetProfileResult
    {
        public string UserId { get; set; } = null!;
        public string ProfileImg { get; set; } = "";

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Bio { get; set; }

        public string? Address1 { get; set; } = "";

        public string? Address2 { get; set; } = "";

        public string? PostalCode { get; set; } = "";

        public string? City { get; set; } = "";
    }
}
