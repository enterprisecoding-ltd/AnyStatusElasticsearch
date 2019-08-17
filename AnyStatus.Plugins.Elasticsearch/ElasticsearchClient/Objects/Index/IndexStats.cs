using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Shared;
using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Index
{
    public class IndexStats
    {
        [JsonProperty("docs")]
        public virtual DocStats Documents { get; internal set; }
    }
}
