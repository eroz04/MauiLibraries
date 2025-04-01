using BaseLibraries.Helpers;

// ReSharper disable All

namespace Google.Auth.Shared;

public sealed class GoogleAuthImplementation : Disposable, IGoogleAuth
{
    private static IGoogleUser _googleUser = null!;

    public Task<IGoogleUser>? GetCurrentUser()
    {
        return Task.FromResult(_googleUser);
    }
    public async Task<GoogleLoginResponse>? LogInAsync()
    {
        var authToken = string.Empty;
        try
        {
            var clientId = "1323226151355922";
            var redirectUri = "cielitorico://com.thinkcare.cielitodemo";
            var state = Guid.NewGuid;
            var scope = "public_profile,email";
            var callbackUrl = new Uri(redirectUri);
            var authUrl = new Uri($"https://www.facebook.com/v/dialog/oauth" +
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

            _googleUser = new GoogleUser
            {
                DisplayName = name,
                Email = email,
                AccessToken = authToken
            };
            return GoogleLoginResponse.CreateResponse(_googleUser);
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
        return GoogleLoginResponse.CreateResponse("Error");
    }

    public Task LogOutAsync()
    {
        return Task.CompletedTask;
    }
    
}