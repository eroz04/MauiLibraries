using FacebookSlim.Login.Android;
using Org.Json;

namespace Facebook.Auth.Android;

public sealed class GraphJSONObjectCallback : Java.Lang.Object, IGraphJSONObjectCallback
{
    private readonly Action<JSONObject?>? _onCompleted;

    public GraphJSONObjectCallback(Action<JSONObject?>? onCompleted = null)
    {
        _onCompleted = onCompleted;
    }

    public void OnCompleted(JSONObject? p0)
    {
        _onCompleted?.Invoke(p0);
    }
}
