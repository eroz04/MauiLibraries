namespace Facebook.Auth;

public interface IFacebookAuth : IDisposable
{
    bool IsLoggedIn { get; }
    Task LogOutAsync();
    Task<FacebookLoginResponse> LogInAsync();
}
