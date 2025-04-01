using BaseLibraries.Auth;

namespace Google.Auth;

public class GoogleLoginResponse : LoginResponse<IGoogleUser>
{
    public GoogleLoginResponse(IGoogleUser userLogged) : base(userLogged)
    {
    }

    public GoogleLoginResponse(string? errorMessage) : base(errorMessage)
    {
    }

    public static GoogleLoginResponse CreateResponse(IGoogleUser userLogedIn)
    {
        return new GoogleLoginResponse(userLogedIn);
    }

    public static GoogleLoginResponse CreateResponse(string? errorMessage)
    {
        return new GoogleLoginResponse(errorMessage);
    }
}
