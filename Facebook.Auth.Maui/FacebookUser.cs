namespace Facebook.Auth;

public class FacebookUser : IFacebookUser
{
    public string Token { get; init; } = null!;
    public string Id { get; set; } = null!;
    public string? FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string Picture { get; set; } = null!;
}
