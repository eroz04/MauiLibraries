namespace Apple.Auth;

public interface IAppleAuth : IDisposable
{
    bool IsAvailable();
    Task<AppleLoginResponse> SignInAsync();
    Task<AppleSignInCredentialState> GetCredentialStateAsync(string userid);
}