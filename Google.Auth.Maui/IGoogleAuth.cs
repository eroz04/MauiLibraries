namespace Google.Auth;

public interface IGoogleAuth : IDisposable
{
    Task<GoogleLoginResponse>? LogInAsync();
    Task LogOutAsync();
    Task<IGoogleUser>? GetCurrentUser();
}
