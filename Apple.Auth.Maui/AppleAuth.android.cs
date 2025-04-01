using BaseLibraries.Helpers;

namespace Apple.Auth;

public class AppleAuth : Disposable, IAppleAuth
{
    public bool IsAvailable() => false;

    public Task<AppleSignInCredentialState> GetCredentialStateAsync(string userid)
    {
        return Task.FromResult<AppleSignInCredentialState>(default);
    }

    public Task<AppleLoginResponse> SignInAsync()
    {
        return Task.FromResult<AppleLoginResponse>(default!);
    }
}
