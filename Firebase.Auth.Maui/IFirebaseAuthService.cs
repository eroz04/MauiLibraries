namespace Firebase.Auth;

public interface IFirebaseAuthService : IDisposable
{
    Task<IFirebaseUser> SignInWithGoogleAsync(string idToken, string accessToken);
    Task<IFirebaseUser> SignInWithFacebookAsync(string accessToken);
    Task<IFirebaseUser> SignInWithAppleAsync(string idToken, string accessToken);


    Task<IFirebaseUser> SignInWithEmailAndPasswordAsync(string email, string password);
    Task<IFirebaseUser> SignInAnonymouslyAsync();

    Task<IList<string>?> GetSignMethodsByEmail(string email);


    Task<Plugin.Firebase.Auth.IFirebaseUser> LinkWithFacebookAsync(string accessToken);
    Task<Plugin.Firebase.Auth.IFirebaseUser> LinkWithGoogleAsync(string idToken, string accessToken);
    Task<bool> SendResetPasswordAsync(string email);
    Task<(bool, string)> VerifyResetPasswordAsync(string code);
    Task<(bool, string)> ConfirmResetPasswordAsync(string code, string newPassword);
}
