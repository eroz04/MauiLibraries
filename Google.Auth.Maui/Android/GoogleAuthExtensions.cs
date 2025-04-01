using Android.Gms.Auth.Api.SignIn;
// ReSharper disable All

namespace Google.Auth.Android;

public static class GoogleAuthExtensions
{
    public static GoogleUserWrapper ToAbstract(this GoogleSignInAccount @this)
    {
        return new GoogleUserWrapper(@this);
    }
}
