using BaseLibraries.Auth;

namespace Facebook.Auth;

public class FacebookLoginResponse : LoginResponse<IFacebookUser>
{
    public FacebookLoginResponse(IFacebookUser userLogged) : base(userLogged)
    {
    }

    public FacebookLoginResponse(string? errorMessage) : base(errorMessage)
    {
    }

    public static FacebookLoginResponse CreateResponse(IFacebookUser userLogedIn)
    {
        return new FacebookLoginResponse(userLogedIn);
    }

    public static FacebookLoginResponse CreateResponse(string? errorMessage)
    {
        return new FacebookLoginResponse(errorMessage);
    }
}
