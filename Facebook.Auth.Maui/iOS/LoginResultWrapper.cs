using FacebookSlim.Login.iOS;

namespace Facebook.Auth.iOS;

public sealed class LoginResultWrapper : IFacebookUser
{
    private readonly ILoginResult _wrapped;
    public LoginResultWrapper(ILoginResult loginResult)
    {
        _wrapped = loginResult;
    }

    public string Token => _wrapped?.Token!;
    public string Id => _wrapped?.UserId;
    public string? FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string Picture { get; set; } = null!;
}
