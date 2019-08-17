using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Index
{
    public class IndicesStats
    {
        [JsonProperty("primaries")]
        public virtual IndexStats Primaries { get; internal set; }
    }
}
