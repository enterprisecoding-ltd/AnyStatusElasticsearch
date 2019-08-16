using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cluster
{
    public class IndexHealthResponse : ElasticsearchResponseBase
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual Shared.Health Status { get; internal set; }
    }
}
