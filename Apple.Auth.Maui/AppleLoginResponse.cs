using BaseLibraries.Auth;

namespace Apple.Auth;

public class AppleLoginResponse : LoginResponse<IAppleUser>
{
    public AppleLoginResponse(IAppleUser userLogged) : base(userLogged)
    {
    }

    public AppleLoginResponse(string? errorMessage) : base(errorMessage)
    {
    }

    public static AppleLoginResponse CreateResponse(IAppleUser userLogedIn)
    {
        return new AppleLoginResponse(userLogedIn);
    }

    public static AppleLoginResponse ErrorResponse(string? errorMessage)
    {
        return new AppleLoginResponse(errorMessage);
    }
}
