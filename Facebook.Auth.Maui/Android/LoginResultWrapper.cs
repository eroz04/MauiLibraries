using FacebookSlim.Login.Android;

namespace Facebook.Auth.Android;

public sealed class LoginResultWrapper : IFacebookUser
{
    private readonly ILoginResult _wrapped;
    public LoginResultWrapper(ILoginResult loginResult)
    {
        _wrapped = loginResult;
    }

    public string Token => _wrapped?.AccessToken?.Token!;
    public string Id => _wrapped?.AccessToken?.UserId!;
    public string? FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string Picture { get; set; } = null!;
}
