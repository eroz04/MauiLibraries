namespace Google.Auth;

public interface IGoogleUser
{
    string? Id { get; }
    string Email { get; }
    string DisplayName { get; }
    string FamilyName { get; }
    string GivenName { get; }
    string PhotoUrl { get; }
    string ServerAuthCode { get; }
    string AccountName { get; }
    string AccountType { get; }
    string IdToken { get; }
    string AccessToken { get; }
}
