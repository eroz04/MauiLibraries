using BaseLibraries.Auth;

namespace Firebase.Auth;

public class FirebaseLoginResponse : LoginResponse<IFirebaseUser>
{
    private FirebaseLoginResponse(bool success) : base(success)
    {
    }
    private FirebaseLoginResponse(IFirebaseUser? user = null) : base(user!)
    {
    }

    private FirebaseLoginResponse(string? errorMessage) : base(errorMessage)
    {
    }

    public static FirebaseLoginResponse CreateResponse(IFirebaseUser user)
    {
        return new FirebaseLoginResponse(user);
    }

    public static FirebaseLoginResponse SuccessResponse()
    {
        return new FirebaseLoginResponse(true);
    }

    public static FirebaseLoginResponse CreateResponse(string? errorMessage)
    {
        return new FirebaseLoginResponse(errorMessage);
    }

    public static FirebaseLoginResponse ErrorResponse(string? errorMessage)
    {
        return new FirebaseLoginResponse(errorMessage);
    }
}
