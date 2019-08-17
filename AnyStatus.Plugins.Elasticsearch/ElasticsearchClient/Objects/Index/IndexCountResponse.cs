using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Index
{
    public class IndexCountResponse : ElasticsearchResponseBase
    {
        [JsonProperty("count")]
        public virtual long Count { get; internal set; }
    }
}
