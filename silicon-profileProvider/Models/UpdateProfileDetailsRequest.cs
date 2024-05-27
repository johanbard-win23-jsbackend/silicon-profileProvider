namespace silicon_profileProvider.Models;

public class UpdateProfileDetailsRequest
{
    public string UserId { get; set; } = "";
    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string? Phone { get; set; }

    public string? Bio { get; set; }
}
