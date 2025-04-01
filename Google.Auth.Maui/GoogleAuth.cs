#if ANDROID
using Google.Auth.Android;
#elif IOS
using Google.Auth.iOS;
#endif

namespace Google.Auth;

public sealed class GoogleAuth
{
    private static Lazy<IGoogleAuth> _implementation = new Lazy<IGoogleAuth>(CreateInstance, System.Threading.LazyThreadSafetyMode.PublicationOnly);

    private static IGoogleAuth CreateInstance()
    {
#if IOS || ANDROID
        return new GoogleAuthImplementation();
#else
#pragma warning disable IDE0022 // Use expression body for methods
        return null;
#pragma warning restore IDE0022 // Use expression body for methods
#endif
    }

    public static bool IsSupported => _implementation.Value != null;

    public static IGoogleAuth Current
    {
        get
        {
            var ret = _implementation.Value;
            if (ret == null)
            {
                throw NotImplementedInReferenceAssembly();
            }
            return ret;
        }
    }

    private static Exception NotImplementedInReferenceAssembly() =>
        new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

    /// <summary>
    /// Dispose of everything 
    /// </summary>
    public static void Dispose()
    {
        if (_implementation != null && _implementation.IsValueCreated)
        {
            _implementation.Value.Dispose();
            _implementation = new Lazy<IGoogleAuth>(CreateInstance, System.Threading.LazyThreadSafetyMode.PublicationOnly);
        }
    }
}
