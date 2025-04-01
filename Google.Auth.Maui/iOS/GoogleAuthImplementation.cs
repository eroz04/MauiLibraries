using BaseLibraries.Helpers;
using Foundation;
using UIKit;
// ReSharper disable All

namespace Google.Auth.iOS;

public sealed class GoogleAuthImplementation : Disposable, IGoogleAuth
{
    private static Lazy<GoogleAuth>? _googleAuth;

    public GoogleAuthImplementation()
    {
        _googleAuth = new Lazy<GoogleAuth>(() => new GoogleAuth());
    }
    public static bool Initialize()
    {
        try
        {
            Google.SignIn.SignIn.SharedInstance.ClientId = NSMutableDictionary.FromFile("GoogleService-Info.plist")?.ValueForKey(new NSString("CLIENT_ID"))?.ToString()!;
        }
        catch (Exception e)
        {
        }
        return false;
        
    }
    public static bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
    {
        return Google.SignIn.SignIn.SharedInstance.HandleUrl(url);
    }
    public Task<GoogleLoginResponse>? LogInAsync()
    {
        return _googleAuth?.Value?.LogInAsync(ViewController);
    }

    public Task LogOutAsync()
    {
        _googleAuth?.Value.LogOut();
        return Task.CompletedTask;
    }

    public Task<IGoogleUser>? GetCurrentUser()
    {
        return _googleAuth?.Value.GetCurrentUser();
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
