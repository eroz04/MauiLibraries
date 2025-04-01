using Foundation;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Auth.Platforms.iOS.Extensions;

// ReSharper disable All

namespace Firebase.Auth.iOS;

public sealed class FirebaseUserWrapper : IFirebaseUser
{
    private readonly User _wrapped;
    private readonly AdditionalUserInfo? _wrappedAdditionalUserInfo;
    public FirebaseUserWrapper(User firebaseUser, AdditionalUserInfo? additionalUserInfo)
    {
        _wrapped = firebaseUser;
        _wrappedAdditionalUserInfo = additionalUserInfo;
    }

    public bool IsNewUser => _wrappedAdditionalUserInfo?.IsNewUser == true;

    public string Uid => _wrapped.Uid;

    public string DisplayName => _wrapped.DisplayName!;

    public string Email
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_wrapped?.Email))
                return _wrapped?.Email!;
            if (!string.IsNullOrWhiteSpace(_wrapped?.ProviderData?.FirstOrDefault()?.Email))
                return _wrapped?.ProviderData?.FirstOrDefault()?.Email!;
            return _wrappedAdditionalUserInfo?.Profile?.ValueForKey(new NSString("email"))?.ToString() ?? "";
        }
    }

    public string FirstName
    {
        get
        {
            string firstNameKey = _wrappedAdditionalUserInfo?.ProviderId?.Contains("google") == true ? "given_name" : "first_name";
            return _wrappedAdditionalUserInfo?.Profile?.ValueForKey(new NSString(firstNameKey))?.ToString() ?? "";
        }
    }

    public string LastName
    {
        get
        {
            var lastNameKey = _wrappedAdditionalUserInfo?.ProviderId?.Contains("google") == true ? "family_name" : "last_name";
            return _wrappedAdditionalUserInfo?.Profile?.ValueForKey(new NSString(lastNameKey))?.ToString() ?? "";
        }
    }


    public string PhotoUrl => _wrapped?.PhotoUrl?.ToString()!;

    public string ProviderId => _wrapped.ProviderId;

    public bool IsEmailVerified => _wrapped.IsEmailVerified;

    public bool IsAnonymous => _wrapped.IsAnonymous;

    public IEnumerable<ProviderInfo> ProviderInfos => _wrapped?.ProviderData?.Select(x => x.ToAbstract())!;

    public Plugin.Firebase.Auth.UserMetadata Metadata => _wrapped.Metadata?.ToAbstract()!;

    public override string ToString()
    {
        return $"[{nameof(FirebaseUserWrapper)}: {nameof(Uid)}={Uid}, {nameof(Email)}={Email}]";
    }

    public Task DeleteAsync()
    {
        return _wrapped.DeleteAsync();
    }

    public async Task<IAuthTokenResult> GetIdTokenResultAsync(bool forceRefresh = false)
    {
        var result = await _wrapped.GetIdTokenResultAsync(forceRefresh);
        return result.ToAbstract();
    }

    public Task SendEmailVerificationAsync(Plugin.Firebase.Auth.ActionCodeSettings actionCodeSettings)
    {
        return _wrapped.SendEmailVerificationAsync(actionCodeSettings?.ToNative()!);
    }

    public Task UnlinkAsync(string providerId)
    {
        return _wrapped.UnlinkAsync(providerId);
    }

    public Task UpdateEmailAsync(string email)
    {
        return _wrapped.UpdateEmailAsync(email);
    }

    public Task UpdatePasswordAsync(string password)
    {
        return _wrapped.UpdatePasswordAsync(password);
    }

    public Task UpdatePhoneNumberAsync(string verificationId, string smsCode)
    {
        return _wrapped.UpdatePhoneNumberCredentialAsync(PhoneAuthProvider.DefaultInstance.GetCredential(verificationId, smsCode));
    }

    public Task UpdateProfileAsync(string displayName = "", string photoUrl = "")
    {
        var request = _wrapped.ProfileChangeRequest();
        if (displayName != "")
        {
            request.DisplayName = displayName;
        }
        if (photoUrl != "")
        {
            request.PhotoUrl = string.IsNullOrWhiteSpace(photoUrl) ? null : new NSUrl(photoUrl);
        }
        return request.CommitChangesAsync();
    }
}
