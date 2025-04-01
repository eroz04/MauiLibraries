using Newtonsoft.Json;

// ReSharper disable all

namespace Openpay;

public class OpenPayErrorMessage
{
    [JsonProperty("http_code")]
    public int HttpCode { get; set; }

    [JsonProperty("error_code")]
    public int ErrorCode { get; set; }

    [JsonProperty("category")]
    public string? Castegory { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("request_id")]
    public string? RequestId { get; set; }
}
