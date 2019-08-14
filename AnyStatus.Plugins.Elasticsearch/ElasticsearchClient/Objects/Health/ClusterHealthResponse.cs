using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Health
{
    public class ClusterHealthResponse : ElasticsearchResponseBase
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual Health Status { get; internal set; }
    }
}
