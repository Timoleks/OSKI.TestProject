using Newtonsoft.Json;

namespace AdaptiveRecordKit;

public class DynamicDocument<T>
{
    [JsonProperty("Document")]
    public List<T>? Elements { get; set; }
}