namespace BaseLibraries.Auth;

public abstract class LoginResponse<T>
{
    public bool Success { get; private set; }
    public int ErrorCode { get; private set; }
    public string? ErrorMessage { get; private set; } = null!;
    public T UserLoggedIn { get; private set; }

    protected LoginResponse(bool success)
    {
        Success = success;
        UserLoggedIn = default!;
    }

    protected LoginResponse(T userLogged)
    {
        if (userLogged == null)
        {
            Success = false;
            ErrorMessage = "No user logged";
            UserLoggedIn = default!;
        }
        else
        {
            Success = true;
            ErrorMessage = string.Empty;
            UserLoggedIn = userLogged;
        }
        ErrorCode = 0;
    }

    protected LoginResponse(string? errorMessage, int errorCode = 0)
    {
        Success = false;
        UserLoggedIn = default!;
        ErrorCode = errorCode;
        if (string.IsNullOrWhiteSpace(errorMessage))
            ErrorMessage = "Error on login";
        else
            ErrorMessage = errorMessage;
    }

    //public static LoginResponse<T> CreateResponse(T userLogedIn)
    //{
    //    return new LoginResponse<T>(userLogedIn);
    //}

    //public static LoginResponse<T> CreateResponse(string errorMessage, int errorCode = 0)
    //{
    //    return new LoginResponse<T>(errorMessage);
    //}
}
