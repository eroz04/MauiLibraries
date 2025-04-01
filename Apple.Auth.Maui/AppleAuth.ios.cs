using System.Diagnostics;
using AuthenticationServices;
using BaseLibraries.Helpers;

namespace Apple.Auth;

public class AppleAuth : Disposable, IAppleAuth
{
    public bool IsAvailable() => OperatingSystem.IsIOSVersionAtLeast(13);

    public async Task<AppleSignInCredentialState> GetCredentialStateAsync(string userid)
    {
        if (OperatingSystem.IsIOSVersionAtLeast(13))
        {
#pragma warning disable CA1416
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var credentialState = await appleIdProvider.GetCredentialStateAsync(userid);
            switch (credentialState)
            {
                case ASAuthorizationAppleIdProviderCredentialState.Authorized:
                    // The Apple ID credential is valid.
                    return AppleSignInCredentialState.Authorized;
                case ASAuthorizationAppleIdProviderCredentialState.Revoked:
                    // The Apple ID credential is revoked.
                    return AppleSignInCredentialState.Revoked;
                case ASAuthorizationAppleIdProviderCredentialState.NotFound:
                    // No credential was found, so show the sign-in UI.
                    return AppleSignInCredentialState.NotFound;
                default:
                    return AppleSignInCredentialState.Unknown;
            }
#pragma warning restore CA1416
        }
        return AppleSignInCredentialState.Unknown;
    }

    public async Task<AppleLoginResponse> SignInAsync()
    {
        if (!IsAvailable()) // Fallback to web for older iOS versions
            return await SignInOlderVersionsAsync();
        else
            return await SignInNativeAsync();
    }
    static async Task<AppleLoginResponse> SignInNativeAsync()
    {
        try
        {
            var signinResult = await AppleSignInAuthenticator.AuthenticateAsync(
                    new AppleSignInAuthenticator.Options()
                    {
                        IncludeEmailScope = true,
                        IncludeFullNameScope = true
                    });
            return AppleLoginResponse.CreateResponse(AppleUser.FromWebAuhtenticatorResult(signinResult));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.StackTrace);
            return AppleLoginResponse.ErrorResponse(ex.Message);
        }
    }
    static async Task<AppleLoginResponse> SignInOlderVersionsAsync()
    {
        try
        {
            var webAuth = new AppleSignInClient(
                    serverId: "com.thinkcare.cielitodemo.singin",
                    keyId: "DZWWJTYW7C",
                    teamId: "8L5U474VZF",
                    redirectUri: new Uri("https://cielito-8c4ea.firebaseapp.com/__/auth/handler"),
                    p8FileContents: @"-----BEGIN PRIVATE KEY-----
            MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgOFnkV1BQl2MZlWCf
            i6GIrJ/3kgKTpEh9V7mZWLSsCkOgCgYIKoZIzj0DAQehRANCAARS+h6DhFHF1+D0
            /aOBtJGcPtfo2JOGlmutFvPJoaXEmEfcglXOEHT/VoyooBr4yyDjdeNCeg5RnWHn
            6m2abmvj
            -----END PRIVATE KEY-----",
                    state: Util.GenerateState(),
                    nonce: Util.GenerateNonce());


            var callbackUrl = new Uri("cielitoapp://");
            var webAuthenticatorResult = await WebAuthenticator.AuthenticateAsync(webAuth.GenerateAuthorizationUrl(), callbackUrl);

            if (webAuthenticatorResult != null)
                return AppleLoginResponse.CreateResponse(AppleUser.FromWebAuhtenticatorResult(webAuthenticatorResult));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.StackTrace);
        }
        return AppleLoginResponse.ErrorResponse("Error");
    }
}
