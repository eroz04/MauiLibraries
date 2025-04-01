// ReSharper disable All

namespace Firebase.Auth.Android;

public static class FirebaseAuthExtensions
{
    public static FirebaseUserWrapper ToAbstract(this FirebaseUser @this, IAdditionalUserInfo? additionalUserInfo = null)
    {
        return new FirebaseUserWrapper(@this, additionalUserInfo);
    }
}
