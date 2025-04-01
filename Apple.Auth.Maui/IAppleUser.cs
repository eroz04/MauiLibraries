namespace Apple.Auth;

public interface IAppleUser
{
    string IdToken { get; }
    string AccessToken { get; }
    string RefreshToken { get; }
    string UserId { get; }
    string Email { get; }
    string Name { get; }
    string RealUserStatus { get; }
}