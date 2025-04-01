using Foundation;
using Plugin.Firebase.Core;
using Plugin.Firebase.Core.Exceptions;
using FirebaseAuth = global::Firebase.Auth.Auth;


namespace Firebase.Auth.iOS;

public sealed class FirebaseAuthServiceImplementation : DisposableBase, IFirebaseAuthService
{
    private readonly FirebaseAuth _firebaseAuth;
    public FirebaseAuthServiceImplementation()
    {
        _firebaseAuth = FirebaseAuth.DefaultInstance!;
    }

    public async Task<IFirebaseUser> SignInWithGoogleAsync(string idToken, string accessToken)
    {
        try
        {
            var credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            return await SignInWithCredentialAsync(credential);
        }
        catch
        {
            throw new Exception("No se puede iniciar sesión en firebase con la cuenta de google");
        }
    }
    public async Task<IFirebaseUser> SignInWithFacebookAsync(string accessToken)
    {
        try
        {
            var credential = FacebookAuthProvider.GetCredential(accessToken);
            return await SignInWithCredentialAsync(credential);
        }
        catch
        {
            throw new Exception("No se puede iniciar sesión en firebase con la cuenta de facebook");
        }
    }
    public async Task<IFirebaseUser> SignInWithAppleAsync(string idToken, string accessToken)
    {
        try
        {
            var credential = OAuthProvider.GetCredential("apple.com", idToken, accessToken);
            return await SignInWithCredentialAsync(credential);
        }
        catch
        {
            throw new Exception("No se puede iniciar sesión en firebase con la cuenta de apple");
        }
    }
    private async Task<IFirebaseUser> SignInWithCredentialAsync(AuthCredential credential)
    {
        var authResult = await _firebaseAuth.SignInWithCredentialAsync(credential);
        return authResult.User.ToAbstract(authResult.AdditionalUserInfo!);
    }

    public async Task<IFirebaseUser> SignInWithEmailAndPasswordAsync(string email, string password)
    {
        try
        {
            var authResult = await _firebaseAuth.SignInWithPasswordAsync(email, password);
            return authResult.User.ToAbstract(authResult.AdditionalUserInfo!);
        }
        catch (NSErrorException e)
        {
            throw GetFirebaseAuthException(e);
        }
    }

    public async Task<IFirebaseUser> SignInAnonymouslyAsync()
    {
        try
        {
            var authResult = await _firebaseAuth.SignInAnonymouslyAsync();
            return authResult.User.ToAbstract(authResult.AdditionalUserInfo!);
        }
        catch (NSErrorException e)
        {
            throw GetFirebaseAuthException(e);
        }
    }


    public async Task<Plugin.Firebase.Auth.IFirebaseUser> LinkWithGoogleAsync(string idToken, string accessToken)
    {
        try
        {
            var credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            return await LinkWithCredentialAsync(credential);
        }
        catch
        {
            throw new Exception("No se pudo relacionar la cuenta actual con la cuenta de google");
        }
    }
    public async Task<Plugin.Firebase.Auth.IFirebaseUser> LinkWithFacebookAsync(string accessToken)
    {
        try
        {
            var credential = FacebookAuthProvider.GetCredential(accessToken);
            return await LinkWithCredentialAsync(credential);
        }
        catch
        {
            throw new Exception("No se pudo relacionar la cuenta actual con la cuenta de facebook");
        }
    }
    private async Task<Plugin.Firebase.Auth.IFirebaseUser> LinkWithCredentialAsync(AuthCredential credential)
    {
        var authResult = await _firebaseAuth.CurrentUser?.LinkAsync(credential)!;
        return authResult.User.ToAbstract(authResult.AdditionalUserInfo!);
    }



    public Task<bool> SendResetPasswordAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<(bool, string)> VerifyResetPasswordAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task<(bool, string)> ConfirmResetPasswordAsync(string code, string newPassword)
    {
        throw new NotImplementedException();
    }

    private static FirebaseAuthException GetFirebaseAuthException(NSErrorException ex)
    {
        AuthErrorCode errorCode;
        if (IntPtr.Size == 8)
        { // 64 bits devices
            errorCode = (AuthErrorCode)(long)ex.Error.Code;
        }
        else
        { // 32 bits devices
            errorCode = (AuthErrorCode)(int)ex.Error.Code;
        }

        Enum.TryParse(errorCode.ToString(), out FIRAuthError authError);
        return new FirebaseAuthException(authError, ex.Error.LocalizedDescription);
    }

    public async Task<IList<string>?> GetSignMethodsByEmail(string email)
    {
        return (await _firebaseAuth.FetchSignInMethodsAsync(email))?.ToList()!;
    }
}
