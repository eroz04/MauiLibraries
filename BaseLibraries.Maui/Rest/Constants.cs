using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonApiSerializer;

namespace BaseLibraries.Rest;

public static class Constants
{
    // settings variables
    public const string ApiToken = "ApiToken";
    public const string ReloadToken = "ReloadToken";
    public const string TokenExpirationTime = "TokenExpirationTime";

    public const string ApiTokenOpenPay = "ApiTokenOpenPay";
    public const string ReloadTokenOpenPay = "ReloadTokenOpenPay";
    public const string TokenExpirationTimeOpenPay = "TokenExpirationTimeOpenPay";


    public const float _timeout = 30f;

    public const string ApiKeyString = "api-key";
    public const string AuthorizationString = "Authorization";
    public const string BearerTokenString = "Bearer {0}";
    public const string VersionString = "version";
    public const string BuildString = "build";
    public const string PlatformString = "plataforma";
    public const string UserEmailString = "usuariocorreo";
    public const string UserNameString = "usuarionombre";
}
