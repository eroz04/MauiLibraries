using Android.Gms.Extensions;
using Android.Runtime;
using Uri = Android.Net.Uri;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Auth.Platforms.Android.Extensions;

// ReSharper disable All

namespace Firebase.Auth.Android;

public sealed class FirebaseUserWrapper : IFirebaseUser
{
    private readonly FirebaseUser? _wrapped;
    private readonly IAdditionalUserInfo? _wrappedAdditionalUserInfo;
    public FirebaseUserWrapper(FirebaseUser firebaseUser, IAdditionalUserInfo? additionalUserInfo = null)
    {
        _wrapped = firebaseUser;
        _wrappedAdditionalUserInfo = additionalUserInfo;
    }

    public bool IsNewUser => _wrappedAdditionalUserInfo?.IsNewUser == true;

    public string? Uid => _wrapped?.Uid;

    public string? DisplayName => _wrapped?.DisplayName;

    public string Email
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_wrapped?.Email))
                return _wrapped.Email;
            if (!string.IsNullOrWhiteSpace(_wrapped?.ProviderData?.FirstOrDefault()?.Email))
                return _wrapped.ProviderData.FirstOrDefault()?.Email!;
            return _wrappedAdditionalUserInfo?.Profile?.ContainsKey("email") == true ? _wrappedAdditionalUserInfo.Profile["email"].ToString() : "";
        }
    }

    public string? FirstName
    {
        get
        {
            var firstNameKey = _wrappedAdditionalUserInfo?.ProviderId?.Contains("google") == true ? "given_name" : "first_name";
            return _wrappedAdditionalUserInfo?.Profile?.ContainsKey(firstNameKey) == true ? _wrappedAdditionalUserInfo?.Profile[firstNameKey]?.ToString() : "";
        }
    }

    public string? LastName
    {
        get
        {
            var lastNameKey = _wrappedAdditionalUserInfo?.ProviderId.Contains("google") == true ? "family_name" : "last_name";
            return _wrappedAdditionalUserInfo?.Profile?.ContainsKey(lastNameKey) == true ? _wrappedAdditionalUserInfo?.Profile[lastNameKey]?.ToString() : "";
        }
    }

    public string? PhotoUrl => _wrapped?.PhotoUrl?.ToString();

    public string? ProviderId => _wrapped?.ProviderId;

    public bool IsEmailVerified => _wrapped?.IsEmailVerified == true;

    public bool IsAnonymous => _wrapped?.IsAnonymous == true;

    public IEnumerable<ProviderInfo>? ProviderInfos => _wrapped?.ProviderData?.Select(x => x.ToAbstract());

    public UserMetadata? Metadata => _wrapped?.Metadata?.ToAbstract();

    public override string ToString()
    {
        return $"[{nameof(FirebaseUserWrapper)}: {nameof(Uid)}={Uid}, {nameof(Email)}={Email}]";
    }

    public Task? DeleteAsync()
    {
        return _wrapped?.DeleteAsync();
    }

    public async Task<IAuthTokenResult?> GetIdTokenResultAsync(bool forceRefresh = false)
    {
        var result = (await _wrapped?.GetIdToken(forceRefresh))?.JavaCast<GetTokenResult>();
        return result?.ToAbstract();
    }

    public Task? SendEmailVerificationAsync(Plugin.Firebase.Auth.ActionCodeSettings? actionCodeSettings = null)
    {
        return _wrapped?.SendEmailVerificationAsync(actionCodeSettings?.ToNative());
    }

    public Task? UnlinkAsync(string providerId)
    {
        return _wrapped?.UnlinkAsync(providerId);
    }

    public Task? UpdateEmailAsync(string email)
    {
        return _wrapped?.UpdateEmailAsync(email);
    }

    public Task? UpdatePasswordAsync(string password)
    {
        return _wrapped?.UpdatePasswordAsync(password);
    }

    public Task? UpdatePhoneNumberAsync(string verificationId, string smsCode)
    {
        return _wrapped?.UpdatePhoneNumberAsync(PhoneAuthProvider.GetCredential(verificationId, smsCode));
    }

    public Task? UpdateProfileAsync(string displayName = "", string photoUrl = "")
    {
        var builder = new UserProfileChangeRequest.Builder();
        if (displayName != "")
        {
            builder.SetDisplayName(displayName);
        }
        if (photoUrl != "")
        {
            builder.SetPhotoUri(photoUrl == null ? null : Uri.Parse(photoUrl));
        }
        return _wrapped?.UpdateProfileAsync(builder.Build());
    }
}
