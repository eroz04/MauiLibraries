using Android.App;
using AndroidX.Fragment.App;
using Android.Content.PM;
using FacebookSlim.Login.Android;

namespace Facebook.Auth.Android;

public sealed class FacebookAuth
{
    readonly string[] _facebookPackages = new[] { "com.facebook.katana", "com.facebook.lite" };
    private readonly ILoginManager? _loginManager = null!;
    private TaskCompletionSource<FacebookLoginResponse> _tcsLoginResponse = null!;
    private LoginResultWrapper _user = null!;

    public FacebookAuth()
    {
        _loginManager = ILoginManager.CreateInstance();
        _loginManager?.SetLoginBehavior("WebOnly");
    }
    public bool IsLoggedIn()
    {
        return _loginManager!.IsLogin;
    }
    public Task<FacebookLoginResponse> LogInAsync(FragmentActivity activity)
    {
        _tcsLoginResponse = new TaskCompletionSource<FacebookLoginResponse>();

        var facebookCallback = new LoginCallback(
            onCancel: () => _tcsLoginResponse.SetResult(FacebookLoginResponse.CreateResponse("Inicio de sesión con facebook cancelada")),
            onError: error => _tcsLoginResponse.SetResult(FacebookLoginResponse.CreateResponse(error!.Message)),
            onSuccess: result =>
            {
                _user = result!.ToAbstract();
                if (result!.AccessToken != null)
                {
                    var graphRequestCallback = new GraphJSONObjectCallback(
                        onCompleted: (json) =>
                        {
                            if (json != null)
                            {
                                //if (json.Has("id"))
                                //    _facebookUser.Id = json.GetString("id");

                                if (json.Has("first_name"))
                                    _user.FirstName = json.GetString("first_name");

                                if (json.Has("email"))
                                    _user.Email = json.GetString("email");

                                if (json.Has("last_name"))
                                    _user.LastName = json.GetString("last_name");

                                if (json.Has("picture"))
                                {
                                    var p2 = json.GetJSONObject("picture");
                                    if (p2.Has("data"))
                                    {
                                        var p3 = p2.GetJSONObject("data");
                                        if (p3.Has("url"))
                                        {
                                            _user.Picture = p3.GetString("url");
                                        }
                                    }
                                }
                            }
                            _tcsLoginResponse.TrySetResult(FacebookLoginResponse.CreateResponse(_user));
                        });
                    var bundle = new global::Android.OS.Bundle();
                    bundle.PutString("fields", "id, first_name, email, last_name, picture.width(500).height(500)");
                    GraphRequest.NewMeRequest(graphRequestCallback, bundle);
                    return;
                }
                
                _tcsLoginResponse.TrySetResult(FacebookLoginResponse.CreateResponse(_user));
            });

        _loginManager?.Logout();
        _loginManager?.Login(activity, new string[] { "public_profile", "email" }, facebookCallback);
        return _tcsLoginResponse.Task;
    }
    public Task LogOutAsync()
    {
        return Task.Run(() => _loginManager?.Logout());
    }
    public bool IsAppInstalled(FragmentActivity activity)
    {
        if(activity == null) return false;
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            return activity.PackageManager!.GetInstalledPackages(PackageManager.PackageInfoFlags.Of(1))
                    .Join(_facebookPackages,
                    x => x.PackageName,
                    x => x,
                    (installedPackage, fbPackageName) =>
                    {
                        return fbPackageName;
                    }).Any();
        }
        else if (OperatingSystem.IsAndroidVersionAtLeast(21))
        {
            return activity.PackageManager!.GetInstalledPackages(PackageInfoFlags.Activities)
                    .Join(_facebookPackages,
                    x => x.PackageName,
                    x => x,
                    (installedPackage, fbPackageName) =>
                    {
                        return fbPackageName;
                    }).Any();
        }
        return false;
    }
    public void HandleActivityResultAsync(int requestCode, Result resultCode, global::Android.Content.Intent intent)
    {
        var responseOk = false;
        try
        {
            if (_loginManager != null)
                responseOk = _loginManager.OnActivityResult(requestCode, (int)resultCode, intent);
        }
        catch
        {
        }
        if (responseOk)
            return;

        //if (_facebookUser != null)
        //{
        //    var facebookUser = new FacebookUser { Token = _facebookUser.Token, Id = _facebookUser.Id };
        //    var graphResponse = new GraphJSONObjectCallback((j, r) =>
        //    {
        //        if (j != null)
        //        {
        //            if (j.Has("id"))
        //                facebookUser.Id = j.GetString("id");

        //            if (j.Has("first_name"))
        //                facebookUser.FirstName = j.GetString("first_name");

        //            if (j.Has("email"))
        //                facebookUser.Email = j.GetString("email");

        //            if (j.Has("last_name"))
        //                facebookUser.LastName = j.GetString("last_name");

        //            if (j.Has("picture"))
        //            {
        //                var p2 = j.GetJSONObject("picture");
        //                if (p2.Has("data"))
        //                {
        //                    var p3 = p2.GetJSONObject("data");
        //                    if (p3.Has("url"))
        //                    {
        //                        facebookUser.Picture = p3.GetString("url");
        //                    }
        //                }
        //            }
        //        }
        //        _tcsLoginResponse.SetResult(FacebookLoginResponse.CreateResponse(facebookUser));
        //    });
        //    var request = GraphRequest.NewMeRequest(accessToken, graphResponse);
        //    var bundle = new global::Android.OS.Bundle();
        //    bundle.PutString("fields", "id, email, first_name, last_name, picture.width(500).height(500)");
        //    request.Parameters = bundle;
        //    request.ExecuteAsync();
        //}
    }
}