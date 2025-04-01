// ReSharper disable All
namespace Google.Auth.iOS;

public static class GoogleAuthExtensions
{
    public static GoogleUserWrapper ToAbstract(this Google.SignIn.GoogleUser @this)
    {
        return new GoogleUserWrapper(@this);
    }
}
