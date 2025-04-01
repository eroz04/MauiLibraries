#if ANDROID
using Facebook.Auth.Android;
#elif IOS
using Facebook.Auth.iOS;
#endif

namespace Facebook.Auth;

public sealed class FacebookAuth
{
    private static Lazy<IFacebookAuth> _implementation = new Lazy<IFacebookAuth>(CreateInstance, System.Threading.LazyThreadSafetyMode.PublicationOnly);

    private static IFacebookAuth CreateInstance()
    {
        //-------------------------------------------
        // Login With WebAuthenticator
        //-------------------------------------------
        //return new CrossFacebookAuthManagerImplementation();




        //-------------------------------------------
        // Login with Platforms Login SDK
        //-------------------------------------------

#if IOS || ANDROID
        return new FacebookAuthImplementation();
#else
#pragma warning disable IDE0022 // Use expression body for methods
        return null;
#pragma warning restore IDE0022 // Use expression body for methods
#endif
    }

    public static bool IsSupported => _implementation?.Value != null;

    public static IFacebookAuth Current
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
            _implementation = new Lazy<IFacebookAuth>(CreateInstance, System.Threading.LazyThreadSafetyMode.PublicationOnly);
        }
    }
}
