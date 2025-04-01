using UIKit;
using Foundation;
using FacebookSlim.Login.iOS;

namespace Facebook.Auth.iOS;

public sealed class FacebookAuth
{
    private readonly FacebookLoginManager _loginManager;
    private TaskCompletionSource<FacebookLoginResponse> _tcsLoginResponse = null!;

    public FacebookAuth()
    {
        _loginManager = new FacebookLoginManager();
    }

    public bool IsLoggedIn()
    {
        return _loginManager?.IsLogin() ?? false;
    }

    public Task<FacebookLoginResponse> LogInAsync(UIViewController viewController)
    {
        _tcsLoginResponse = new TaskCompletionSource<FacebookLoginResponse>();
        _loginManager?.Logout();
        _loginManager?.Login(new string[] { "public_profile", "email", }, viewController, (resultEnum, result, error) =>
        {
            if (resultEnum == LoginResultEnum.LoggedIn)
            {
                var facebookUser = result?.ToAbstract();
                if (result.Token != null)
                {
                    FacebookGraphRequest.Shared.MeRequest(
                        new string[]{"id, email, first_name, last_name, picture.width(1000).height(1000)"}, (meResult) =>
                        {
                            if (meResult != null)
                            {
                                //facebookUser.Id = meResult.ValueForKey(new NSString("id"))?.ToString();
                                facebookUser.Email = meResult.ValueForKey(new NSString("email"))?.ToString();
                                facebookUser.FirstName = meResult.ValueForKey(new NSString("first_name"))?.ToString();
                                facebookUser.LastName = meResult.ValueForKey(new NSString("last_name"))?.ToString();
                                facebookUser.Picture = ((meResult.ValueForKey(new NSString("picture")) as NSDictionary)?.ValueForKey(new NSString("data")) as NSDictionary)?.ValueForKey(new NSString("url"))?.ToString();
                            }
                            _tcsLoginResponse.TrySetResult(FacebookLoginResponse.CreateResponse(facebookUser));
                        });
                    return;
                }
                _tcsLoginResponse.TrySetResult(FacebookLoginResponse.CreateResponse(facebookUser));
            }
            else
            {
                if (resultEnum == LoginResultEnum.Error)
                    _tcsLoginResponse.TrySetResult(FacebookLoginResponse.CreateResponse(error.LocalizedDescription));
                if (resultEnum == LoginResultEnum.Cancelled)
                    _tcsLoginResponse.TrySetResult(FacebookLoginResponse.CreateResponse("Inicio de sesión con facebook cancelada"));
                else
                    _tcsLoginResponse.TrySetResult(FacebookLoginResponse.CreateResponse("Error en inicio de sesión con facebook"));
            }
        });

        return _tcsLoginResponse.Task;
    }
    public Task LogOutAsync()
    {
        _loginManager?.Logout();
        return Task.CompletedTask;
    }
}