using Foundation;
using UIKit;
using BaseLibraries.Helpers;
using FacebookSlim.Login.iOS;

namespace Facebook.Auth.iOS;

public sealed class FacebookAuthImplementation : Disposable, IFacebookAuth
{
    private readonly Lazy<FacebookAuth> _facebookAuth = null;

    public FacebookAuthImplementation()
    {
        _facebookAuth = new Lazy<FacebookAuth>(() => new FacebookAuth());
    }

    public bool IsLoggedIn => _facebookAuth?.Value?.IsLoggedIn() == true;

    public static bool Initialize()
    {
        try
        {
            FacebookCoreKitManager.Shared.InitializeSdk();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return true;
    }

    public static bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
    {
        try
        {
            if (FacebookCoreKitManager.Shared.OpenUrl(app, url, null))
                return true;
        }
        catch
        {

        }
        return false;
    }

    public Task<FacebookLoginResponse> LogInAsync()
    {
        return _facebookAuth?.Value?.LogInAsync(ViewController)!;
    }

    public Task LogOutAsync()
    {
        return _facebookAuth?.Value?.LogOutAsync()!;
    }

    private static UIViewController ViewController
    {
        get
        {
            var rootViewController = Microsoft.Maui.ApplicationModel.Platform.GetCurrentUIViewController();
            if (rootViewController == null)
                throw new NullReferenceException("RootViewController is null");
            return rootViewController.PresentedViewController ?? rootViewController;
        }
    }
}
