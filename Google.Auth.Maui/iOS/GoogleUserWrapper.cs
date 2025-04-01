// ReSharper disable All

namespace Google.Auth.iOS;

public sealed class GoogleUserWrapper : IGoogleUser
{
    private readonly Google.SignIn.GoogleUser _wrapped;
    public GoogleUserWrapper(Google.SignIn.GoogleUser googleUser)
    {
        _wrapped = googleUser;
    }
    public string Id => _wrapped?.UserId!;
    public string Email => _wrapped?.Profile?.Email!;
    public string DisplayName => _wrapped?.Profile?.Name!;
    public string FamilyName => _wrapped?.Profile?.FamilyName!;
    public string GivenName => _wrapped?.Profile?.GivenName!;
    public string ServerAuthCode => _wrapped?.ServerAuthCode!;
    public string AccountName => _wrapped?.HostedDomain!;
    public string AccountType => _wrapped?.HostedDomain!;
    public string IdToken => _wrapped?.Authentication?.IdToken!;
    public string AccessToken => _wrapped?.Authentication?.AccessToken!;
    public string PhotoUrl => (_wrapped?.Profile?.HasImage == true ? _wrapped.Profile.GetImageUrl(500)?.ToString()! : null)!;
}
