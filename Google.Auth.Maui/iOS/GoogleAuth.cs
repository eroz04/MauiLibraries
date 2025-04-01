using Foundation;
using UIKit;

namespace Google.Auth.iOS;

public class GoogleAuth : NSObject, Google.SignIn.ISignInDelegate
{
    UIViewController _viewController = null!;
    TaskCompletionSource<GoogleLoginResponse> _tcs = null!;

    public GoogleAuth()
    {
        Google.SignIn.SignIn.SharedInstance.Delegate = this;
    }

    public Task<GoogleLoginResponse>? LogInAsync(UIViewController viewController)
    {
        _viewController = viewController;
        _tcs = new TaskCompletionSource<GoogleLoginResponse>();
        Google.SignIn.SignIn.SharedInstance.PresentingViewController = viewController;
        // if (GoogleSignIn.SignIn.SharedInstance.CurrentUser != null)
            LogOut();
        Google.SignIn.SignIn.SharedInstance.SignInUser();

        return _tcs.Task;
    }
    public void LogOut()
    {
        Google.SignIn.SignIn.SharedInstance.SignOutUser();
        Google.SignIn.SignIn.SharedInstance.DisconnectUser();
    }

    public Task<IGoogleUser>? GetCurrentUser()
    {
        var tcsG = new TaskCompletionSource<IGoogleUser>();
        tcsG.TrySetResult(Google.SignIn.SignIn.SharedInstance.CurrentUser.ToAbstract());
        return tcsG.Task;
    }
    public void DidSignIn(Google.SignIn.SignIn signIn, Google.SignIn.GoogleUser? googleUser, NSError? error)
    {
        if (googleUser != null && error == null)
            _tcs.TrySetResult(GoogleLoginResponse.CreateResponse(googleUser.ToAbstract()));
        else
        {
            _tcs.TrySetResult(GoogleLoginResponse.CreateResponse(errorMessage: error?.LocalizedDescription));
            LogOut();
        }
    }

    [Export("signInWillDispatch:error:")]
    public void WillDispatch(Google.SignIn.SignIn signIn, NSError error)
    {
        System.Diagnostics.Debug.WriteLine("WillDispatch");

        _tcs.TrySetResult(GoogleLoginResponse.CreateResponse(Google.SignIn.SignIn.SharedInstance.CurrentUser.ToAbstract()));
    }
    [Export("signIn:presentViewController:")]
    public void PresentViewController(Google.SignIn.SignIn signIn, UIViewController viewController)
    {
        _viewController?.PresentViewController(viewController, true, null);
    }
    [Export("signIn:dismissViewController:")]
    public void DismissViewController(Google.SignIn.SignIn signIn, UIViewController viewController)
    {
        _viewController?.DismissViewController(true, null);
    }
}
