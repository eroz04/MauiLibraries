#if ANDROID
using Firebase.Auth.Android;
#elif IOS
using Firebase.Auth.iOS;
#endif

namespace Firebase.Auth;

public sealed class FirebaseAuthService
{
    private static Lazy<IFirebaseAuthService> _implementation = new Lazy<IFirebaseAuthService>(CreateInstance, System.Threading.LazyThreadSafetyMode.PublicationOnly);
    public static IFirebaseAuthService CreateInstance()
    {
#if IOS || ANDROID
        return new FirebaseAuthServiceImplementation();
#else
#pragma warning disable IDE0022 // Use expression body for methods
        return null;
#pragma warning restore IDE0022 // Use expression body for methods
#endif
    }
    public static bool IsSupported => _implementation?.Value != null;

    public static IFirebaseAuthService Current
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


    public static void Dispose()
    {
        if (_implementation != null && _implementation.IsValueCreated)
        {
            _implementation.Value.Dispose();
            _implementation = new Lazy<IFirebaseAuthService>(CreateInstance, System.Threading.LazyThreadSafetyMode.PublicationOnly);
        }
    }
}
