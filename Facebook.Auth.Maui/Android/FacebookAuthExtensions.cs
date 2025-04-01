using FacebookSlim.Login.Android;

namespace Facebook.Auth.Android;

public static class FacebookAuthExtensions
{
    public static LoginResultWrapper ToAbstract(this ILoginResult @this)
    {
        return new LoginResultWrapper(@this);
    }
}
