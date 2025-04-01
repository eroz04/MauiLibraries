using FacebookSlim.Login.iOS;

namespace Facebook.Auth.iOS;

public static class FacebookAuthExtensions
{
    public static LoginResultWrapper ToAbstract(this ILoginResult @this)
    {
        return new LoginResultWrapper(@this);
    }
}
