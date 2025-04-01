namespace Apple.Auth;

public class AppleAccount
{
    public string Email { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string RealUserStatus { get; init; } = null!;
    public string UserId { get; init; } = null!;

    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
    public string Token { get; set; }

    public string Idtoken { get; set; } = null!;

    // public JwtToken IdToken { get; set; }

    public static AppleAccount FromLibAppleAccount(AppleAccount aa)
    {
        return new AppleAccount
        {
            AccessToken = aa.AccessToken,
            Email = aa.Email,
            //IdToken = aa.IdToken,
            Name = aa.Name,
            RealUserStatus = aa.RealUserStatus,
            RefreshToken = aa.RefreshToken,
            UserId = aa.UserId
        };
    }


    public static AppleAccount FromWebAuhtenticatorResult(WebAuthenticatorResult res)
    {
        AppleAccount user = null;
        if (res != null)
        {
            user = new AppleAccount()
            {
                AccessToken = res.AccessToken,
                Idtoken = res.IdToken,
                //Idtoken = res.Properties.ContainsKey("id_token") ? res.Properties["id_token"] : "",
                UserId = res.Properties.ContainsKey("user_id") ? res.Properties["user_id"] : "",
                Email = res.Properties.ContainsKey("email") ? res.Properties["email"] : "",
                Name = res.Properties.ContainsKey("name") ? res.Properties["name"] : "",
                RealUserStatus = res.Properties.ContainsKey("realuserstatus") ? res.Properties["realuserstatus"] : "",
            };
        }
        return user;
    }

    public static AppleAccount FromUrl(string url)
    {
        var parameters = Util.ParseUrlParameters(url);

        var idToken = JwtToken.Decode(parameters["id_token"]);

        return new AppleAccount
        {
            // IdToken = idToken,
            UserId = idToken.Subject,
            Email = idToken.Payload.ContainsKey("email") ? idToken.Payload["email"]?.ToString() : null,
            Name = idToken.Payload.ContainsKey("name") ? idToken.Payload["name"]?.ToString() : null,
            AccessToken = parameters.ContainsKey("access_token") ? parameters["access_token"] : null,
            RefreshToken = parameters.ContainsKey("refresh_token") ? parameters["refresh_token"] : null,
        };
    }
}
