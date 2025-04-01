using Android.Gms.Auth.Api.SignIn;

// ReSharper disable All

namespace Google.Auth.Android;

public sealed class GoogleUserWrapper : IGoogleUser
{
    private readonly GoogleSignInAccount _wrapped;
    public GoogleUserWrapper(GoogleSignInAccount signInAccount)
    {
        _wrapped = signInAccount;
    }
    public string Id =>  _wrapped?.Id!;
    public string Email => _wrapped?.Email!;
    public string DisplayName => _wrapped?.DisplayName!;
    public string FamilyName => _wrapped?.FamilyName!;
    public string GivenName => _wrapped?.GivenName!;
    public string ServerAuthCode => _wrapped?.ServerAuthCode!;
    public string AccountName => _wrapped?.Account?.Name!;
    public string AccountType => _wrapped?.Account?.Type!;
    public string IdToken => _wrapped?.IdToken!;
    public string AccessToken => _wrapped?.IdToken!;
    public string PhotoUrl => _wrapped?.PhotoUrl?.ToString()!;
}
