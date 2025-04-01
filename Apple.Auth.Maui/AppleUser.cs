using System.Diagnostics;
using System.Text;

namespace Apple.Auth;

public class AppleUser : IAppleUser
{
    public string IdToken { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string AuthorizationCode { get; set; }
    public string State { get; set; }
    public string RealUserStatus { get; set; }

    //public string ToQueryParameters()
    //    => $"access_token={AccessToken}&refresh_token={RefreshToken}&id_token={IdJwtToken.Raw}";

    public static AppleUser FromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        var parameters = Util.ParseUrlParameters(url);
        var user = new AppleUser
        {
            IdToken = parameters.ContainsKey("id_token") ? parameters["id_token"] : "",
            AccessToken = parameters.ContainsKey("access_token") ? parameters["access_token"] : "",
            RefreshToken = parameters.ContainsKey("refresh_token") ? parameters["refresh_token"] : "",
            UserId = parameters.ContainsKey("user_id") ? parameters["user_id"] : "",
            Email = parameters.ContainsKey("email") ? parameters["email"] : "",
            Name = parameters.ContainsKey("name") ? parameters["name"] : "",
            RealUserStatus = parameters.ContainsKey("realuserstatus") ? parameters["realuserstatus"] : "",
        };
        GetNameAndEmail(ref user);

        return user;
    }
    public static AppleUser FromWebAuhtenticatorResult(WebAuthenticatorResult res)
    {
        if (res == null)
            return null;
        var user = new AppleUser()
        {
            IdToken = res.IdToken,
            AccessToken = res.AccessToken,
            RefreshToken = res.RefreshToken, // res.Properties.ContainsKey("refresh_token") ? res.Properties["refresh_token"] : "",
            UserId = res.Properties.ContainsKey("user_id") ? res.Properties["user_id"] : "",
            Email = res.Properties.ContainsKey("email") ? res.Properties["email"] : "",
            Name = res.Properties.ContainsKey("name") ? res.Properties["name"] : "",
            State = res.Properties.ContainsKey("state") ? res.Properties["state"] : "",
            AuthorizationCode = res.Properties.ContainsKey("authorization_code") ? res.Properties["authorization_code"] : "",
            RealUserStatus = res.Properties.ContainsKey("realuserstatus") ? res.Properties["realuserstatus"] : "",
        };
        GetNameAndEmail(ref user);
        return user;
    }
    public static void GetNameAndEmail(ref AppleUser appleUser)
    {
        if (appleUser == null)
            return;
        if (string.IsNullOrWhiteSpace(appleUser.IdToken))
            return;
        if (!string.IsNullOrWhiteSpace(appleUser.Email) && !string.IsNullOrWhiteSpace(appleUser.Name))
            return;
        if (!appleUser.IdToken.Contains('.'))
            return;
        try
        {
            var parts = appleUser.IdToken.Split(new char[] { '.' }, 3);
            if (parts?.Length > 1)
            {
                //var headerJson = Encoding.UTF8.GetString(Util.Base64UrlDecode(parts[0]));
                //var header = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(headerJson) ?? new Dictionary<string, object>();

                var payloadJson = Encoding.UTF8.GetString(Util.Base64UrlDecode(parts[1]));
                var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson) ?? new Dictionary<string, object>();

                // var parts3 = parts[2];

                if (payload?.ContainsKey("email") == true && string.IsNullOrWhiteSpace(appleUser.Email))
                    appleUser.Email = payload["email"]?.ToString();
                if (payload?.ContainsKey("name") == true && string.IsNullOrWhiteSpace(appleUser.Name))
                    appleUser.Name = payload["name"]?.ToString();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.StackTrace);
        }
    }
}

public enum AppleSignInCredentialState
{
    Authorized,
    Revoked,
    NotFound,
    Unknown
}