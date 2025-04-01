using FacebookSlim.Login.Android;

namespace Facebook.Auth.Android;

public sealed class LoginCallback : Java.Lang.Object, ILoginCallback
{
    private readonly Action? _onCancel;
    private readonly Action<Exception?>? _onError;
    private readonly Action<ILoginResult?>? _onSuccess;

    public LoginCallback(
        Action? onCancel = null,
        Action<Exception?>? onError = null,
        Action<ILoginResult?>? onSuccess = null)
    {
        _onCancel = onCancel;
        _onError = onError;
        _onSuccess = onSuccess;
    }

    public void OnCanceled()
    {
        _onCancel?.Invoke();
    }

    public void OnError(Java.Lang.Exception? p0)
    {
        _onError?.Invoke(new Exception(p0?.LocalizedMessage));
    }

    public void OnSuccess(ILoginResult? p0)
    {
        _onSuccess?.Invoke(p0);
    }
}
