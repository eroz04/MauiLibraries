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

public abstract class BaseApiRest
{
    string ApiKey = "a8fd2aee755aeb8ed329eed667d11d2d6893f4f2ce2f62e637300d66af4c9764";
    string Version = "1";
    string Build = "1";
    string Platform = "ios";

    string ApiEndPoint;
    string ApiGetTokenPath;
    string ApiUser = "posada";
    string ApiPass = "posada20191209183100s";
}
