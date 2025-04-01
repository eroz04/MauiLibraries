using Android.App;
using Android.Provider;
using Android.Webkit;
using Java.Util;
using AndroidWebView = Android.Webkit.WebView;
using Openpay.Shared;

// ReSharper disable all

namespace Openpay;

public sealed class Openpay : OpenpayBase
{
    /// <summary>
    /// Implementación por/plataforma de la solicitud del identificador de la sesión.
    /// </summary>
    /// <param name="merchantId">El identificador del cliente</param>
    /// <param name="apiKey">La llave pública del API del cliente</param>
    /// <param name="baseUrl">El URL al que se debe conectar la plataforma.</param>
    protected override Task<string> CreateDeviceSessionIdInternal(string? merchantId, string? apiKey, string? baseUrl)
    {
        if (null == Activity)
        {
            throw new InvalidOperationException("Activity has not been initialized.");
        }

        var sessionId = UUID.RandomUUID()?.ToString();
        sessionId = sessionId?.Replace("-", string.Empty);

        var identifierForVendor = Settings.Secure.GetString(Activity.ContentResolver, Settings.Secure.AndroidId);
        var identifierForVendorScript = $"var identifierForVendor = '{identifierForVendor}';";

        using (var webView = new AndroidWebView(Activity))
        {
            webView.SetWebViewClient(new WebViewClient());
            webView.Settings.JavaScriptEnabled = true;
            webView.EvaluateJavascript(identifierForVendorScript, null);

            var url = $"{baseUrl}/oa/logo.htm?m={merchantId}&s={sessionId}";
            webView.LoadUrl(url);

            return Task.FromResult(sessionId)!;
        }
    }
    internal static Activity? Activity { get; set; }
    public static void Initialize(Activity? activity) => Openpay.Activity = activity ?? throw new ArgumentNullException(nameof(activity));
}