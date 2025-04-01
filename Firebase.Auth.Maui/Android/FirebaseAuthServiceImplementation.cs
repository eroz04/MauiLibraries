using Android.Gms.Extensions;
using Plugin.Firebase.Core;
using Plugin.Firebase.Core.Exceptions;
using CrossFirebaseAuthException = Plugin.Firebase.Core.Exceptions.FirebaseAuthException;

// ReSharper disable All

namespace Firebase.Auth.Android;

public sealed class FirebaseAuthServiceImplementation : DisposableBase, IFirebaseAuthService
{
    private readonly FirebaseAuth _firebaseAuth;
    public FirebaseAuthServiceImplementation()
    {
        _firebaseAuth = FirebaseAuth.Instance;
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
    public Task<IFirebaseUser> SignInWithAppleAsync(string idToken, string accessToken)
    {
        throw new PlatformNotSupportedException();
    }
    private async Task<IFirebaseUser> SignInWithCredentialAsync(AuthCredential credential)
    {
        var authResult = await _firebaseAuth.SignInWithCredentialAsync(credential);
        return authResult.User.ToAbstract(authResult.AdditionalUserInfo);
    }
    public async Task<IFirebaseUser> SignInAnonymouslyAsync()
    {
        try
        {
            var authResult = await _firebaseAuth.SignInAnonymouslyAsync();
            return authResult.User.ToAbstract(authResult.AdditionalUserInfo);
        }
        catch (Exception e)
        {
            throw GetFirebaseAuthException(e);
        }
    }
    public async Task<IFirebaseUser> SignInWithEmailAndPasswordAsync(string email, string password)
    {
        try
        {
            var authResult = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
            return authResult.User.ToAbstract(authResult.AdditionalUserInfo);
        }
        catch (Exception e)
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

    private async Task<IFirebaseUser> LinkWithCredentialAsync(AuthCredential credential)
    {
        var authResult = await _firebaseAuth.CurrentUser.LinkWithCredentialAsync(credential);
        return authResult.User.ToAbstract(authResult.AdditionalUserInfo);
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

    private static CrossFirebaseAuthException GetFirebaseAuthException(Exception ex)
    {
        return ex switch
        {
            FirebaseAuthEmailException => new CrossFirebaseAuthException(FIRAuthError.InvalidEmail, ex.Message),
            FirebaseAuthInvalidUserException => new CrossFirebaseAuthException(FIRAuthError.UserNotFound, ex.Message),
            FirebaseAuthWeakPasswordException => new CrossFirebaseAuthException(FIRAuthError.WeakPassword, ex.Message),
            FirebaseAuthInvalidCredentialsException { ErrorCode: "ERROR_WRONG_PASSWORD" } => new CrossFirebaseAuthException(FIRAuthError.WrongPassword, ex.Message),
            FirebaseAuthInvalidCredentialsException => new CrossFirebaseAuthException(FIRAuthError.InvalidCredential, ex.Message),
            FirebaseAuthUserCollisionException { ErrorCode: "ERROR_EMAIL_ALREADY_IN_USE" } => new CrossFirebaseAuthException(FIRAuthError.EmailAlreadyInUse, ex.Message),
            FirebaseAuthUserCollisionException { ErrorCode: "ERROR_ACCOUNT_EXISTS_WITH_DIFFERENT_CREDENTIAL" } => new CrossFirebaseAuthException(FIRAuthError.AccountExistsWithDifferentCredential, ex.Message),
            _ => new CrossFirebaseAuthException(FIRAuthError.Undefined, ex.Message)
        };
    }

    public async Task<IList<string>?> GetSignMethodsByEmail(string email)
    {
        return (await _firebaseAuth?.FetchSignInMethodsForEmail(email)?.AsAsync<ISignInMethodQueryResult>()!)?.SignInMethods?.ToList();
    }
}
