using BaseLibraries.Helpers;
// ReSharper disable All

namespace Facebook.Auth.Shared;

public class FacebookAuthImplementation : Disposable, IFacebookAuth
{
    static IFacebookUser _facebookUser = null!;
    const string authUrlVersion = "20.0";

    public Task<IFacebookUser> GetCurrentUser()
    {
        return Task.FromResult(_facebookUser);
    }

    public bool IsLoggedIn => true;

    public async Task<FacebookLoginResponse> LogInAsync()
    {
        var authToken = string.Empty;
        try
        {
            var clientId = "1323226151355922";
            var redirectUri = "cielitorico://com.thinkcare.cielitodemo";
            var state = Guid.NewGuid;
            var scope = "public_profile,email";
            var callbackUrl = new Uri(redirectUri);
            var authUrl = new Uri($"https://www.facebook.com/v{authUrlVersion}/dialog/oauth" +
                $"?client_id={clientId}" +
                $"&redirect_uri={redirectUri}" +
                $"&response_type=token" +
                $"&scope={scope}" +
                $"&state={state}");

            WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(
                new WebAuthenticatorOptions
                {
                    Url = authUrl,
                    CallbackUrl = callbackUrl,
                    PrefersEphemeralWebBrowserSession = true
                });

            authToken += authResult?.AccessToken ?? authResult?.IdToken;

            if (authResult!.Properties.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
                authToken += $"Name: {name}{Environment.NewLine}";
            if (authResult.Properties.TryGetValue("email", out var email) && !string.IsNullOrEmpty(email))
                authToken += $"Email: {email}{Environment.NewLine}";

            _facebookUser = new FacebookUser
            {
                FirstName = name,
                Email = email,
                Token = authToken
            };
            return FacebookLoginResponse.CreateResponse(_facebookUser);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Login canceled.");

            authToken = string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed: {ex.Message}");

            authToken = string.Empty;
        }
        return FacebookLoginResponse.CreateResponse("Error");
    }

    public Task LogOutAsync()
    {
        return Task.CompletedTask;
    }
}
