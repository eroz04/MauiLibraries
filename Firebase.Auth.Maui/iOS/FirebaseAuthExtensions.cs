namespace Firebase.Auth.iOS;

public static class FirebaseAuthManagerExtensions
{
    public static FirebaseUserWrapper ToAbstract(this User @this, AdditionalUserInfo? additionalUserInfo = null)
    {
        return new FirebaseUserWrapper(@this, additionalUserInfo);
    }
}
