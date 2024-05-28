namespace silicon_profileProvider.Models;

public class UpdateProfileAddressRequest
{
    public string UserId { get; set; } = "DEFAULT FROM UPAR";

    public string Email { get; set; } = null!;

    public string? Address1 { get; set; } = "";

    public string? Address2 { get; set; } = "";

    public string? PostalCode { get; set; } = "";

    public string? City { get; set; } = "";
}
