namespace Facebook.Auth;

public interface IFacebookUser
{
    string Token { get; }
    string Id { get; }
    string? FirstName { get; }
    string LastName { get; }
    string? Email { get; }
    string Picture { get; }
}
