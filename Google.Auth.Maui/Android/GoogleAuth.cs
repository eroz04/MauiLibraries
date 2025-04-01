using Android.App;
using Android.Gms.Auth.Api.SignIn;
using GmsTask = Android.Gms.Tasks.Task;
using Android.Gms.Extensions;
using Android.Content;
using AndroidX.Fragment.App;


namespace Google.Auth.Android;

public sealed class GoogleAuth
{
    private const int GOOGLE_SIGNIN_REQUEST_CODE = 1199;
    private readonly GoogleSignInClient _signInClient;
    private TaskCompletionSource<GoogleLoginResponse>? _tcs;

    public GoogleAuth(Activity activity, string requestIdToken)
    {
        _signInClient = GoogleSignIn.GetClient(activity, CreateGoogleSignInOptions(requestIdToken));
    }
    private static GoogleSignInOptions CreateGoogleSignInOptions(string requestIdToken)
    {
        return new GoogleSignInOptions
            .Builder(GoogleSignInOptions.DefaultSignIn)
            .RequestIdToken(requestIdToken)
            .RequestEmail()
            .Build();
    }
    public Task<GoogleLoginResponse> LogInAsync(FragmentActivity activity)
    {
        _tcs = new TaskCompletionSource<GoogleLoginResponse>();
        activity.StartActivityForResult(_signInClient?.SignInIntent, GOOGLE_SIGNIN_REQUEST_CODE);
        return _tcs.Task;
    }
    public Task LogOutAsync()
    {
        return _signInClient?.SignOutAsync()!;
    }
    public async Task<IGoogleUser?> GetCurrentUser()
    {
        try
        {
            var currentUser = await _signInClient?.SilentSignInAsync()!;
            return currentUser?.ToAbstract()!;
        }
        catch
        {
        }
        return null;
    }
    public async Task HandleActivityResultAsync(int requestCode, Result resultCode, Intent data)
    {
        if (requestCode == GOOGLE_SIGNIN_REQUEST_CODE)
            await HandleSignInResultAsync(GoogleSignIn.GetSignedInAccountFromIntent(data));
    }
    private async Task HandleSignInResultAsync(GmsTask signInAccountTask)
    {
        if (signInAccountTask.IsSuccessful)
        {
            var signInAccount = await signInAccountTask.AsAsync<GoogleSignInAccount>();
            if (signInAccount != null)
            {
                _tcs?.TrySetResult(GoogleLoginResponse.CreateResponse(signInAccount.ToAbstract()));
                return;
            }
        }
        _tcs?.SetException(signInAccountTask.Exception);
    }
}
