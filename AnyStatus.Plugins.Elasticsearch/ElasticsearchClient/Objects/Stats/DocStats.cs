using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class DocStats
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("deleted")]
        public long Deleted { get; set; }
    }
}
