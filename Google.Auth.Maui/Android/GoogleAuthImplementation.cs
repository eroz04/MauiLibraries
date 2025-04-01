using Android.App;
using Android.Content;
using AndroidX.Fragment.App;
using BaseLibraries.Helpers;
using PlatformAndroid = Microsoft.Maui.ApplicationModel.Platform;
// ReSharper disable All

namespace Google.Auth.Android;

public sealed class GoogleAuthImplementation : Disposable, IGoogleAuth
{
    private static string _requestIdToken = null!;
    private static Lazy<GoogleAuth> _googleAuth = null!;

    public GoogleAuthImplementation()
    {
        _googleAuth = new Lazy<GoogleAuth>(() => new GoogleAuth(Activity, _requestIdToken));
    }
    public static void Initialize(string requestIdToken)
    {
        _requestIdToken = requestIdToken;
    }
    public Task<GoogleLoginResponse> LogInAsync()
    {
        return _googleAuth.Value.LogInAsync(FragmentActivity);
    }

    public Task LogOutAsync()
    {
        return _googleAuth.Value.LogOutAsync();
    }
    public Task<IGoogleUser>? GetCurrentUser()
    {
        return _googleAuth?.Value?.GetCurrentUser();
    }
    public static Task OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        return _googleAuth?.Value?.HandleActivityResultAsync(requestCode, resultCode, data)!;
    }

    private static  Activity Activity =>
        PlatformAndroid.CurrentActivity ?? throw new NullReferenceException("Platform.CurrentActivity is null");

    private static FragmentActivity FragmentActivity =>
        Activity as FragmentActivity ?? throw new NullReferenceException($"Current Activity is either null or not of type {nameof(FragmentActivity)}, which is mandatory for sign in with Google");
}