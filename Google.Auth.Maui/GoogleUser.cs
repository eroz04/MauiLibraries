namespace Google.Auth;

public class GoogleUser : IGoogleUser
{
    public string? Id { get; init; }
    public string Email { get; init; }
    public string DisplayName { get; init; }
    public string FamilyName { get; init; }
    public string GivenName { get; init; }
    public string PhotoUrl { get; init; }
    public string ServerAuthCode { get; init; }
    public string AccountName { get; init; }
    public string AccountType { get; init; }
    public string IdToken { get; init; }
    public string AccessToken { get; init; }
}
