using System.Drawing;

namespace silicon_profileProvider.Models;

public class UpdateProfileDetailsRequest
{
    public string UserId { get; set; } = "DEFAULT FROM UPDR";

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Bio { get; set; }
}
