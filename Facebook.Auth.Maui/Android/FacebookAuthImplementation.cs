using Android.App;
using Android.Content;
using AndroidX.Fragment.App;
using BaseLibraries.Helpers;
using PlatformAndroid = Microsoft.Maui.ApplicationModel.Platform;

namespace Facebook.Auth.Android;

public sealed class FacebookAuthImplementation : Disposable, IFacebookAuth
{
    private static Lazy<FacebookAuth> _facebookAuth = null!;

    public FacebookAuthImplementation()
    {
        _facebookAuth = new Lazy<FacebookAuth>(() => new FacebookAuth());
    }

    public bool IsLoggedIn => _facebookAuth?.Value?.IsLoggedIn() == true;

    public static void Initialize()
    {
    }

    public Task<FacebookLoginResponse> LogInAsync()
    {
        return _facebookAuth?.Value?.LogInAsync(FragmentActivity)!;
    }

    public Task LogOutAsync()
    {
        return _facebookAuth?.Value?.LogOutAsync()!;
    }

    public static Task OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        _facebookAuth?.Value?.HandleActivityResultAsync(requestCode, resultCode, data);
        return Task.CompletedTask;
    }

    private static Activity Activity =>
        PlatformAndroid.CurrentActivity ?? throw new NullReferenceException("Platform.CurrentActivity is null");

    private static FragmentActivity FragmentActivity =>
        Activity as FragmentActivity ?? throw new NullReferenceException($"Current Activity is either null or not of type {nameof(FragmentActivity)}, which is mandatory for sign in with Google");
}
