using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class DocStats
    {
        [JsonProperty("count")]
        public virtual long Count { get; set; }

        [JsonProperty("deleted")]
        public virtual long Deleted { get; set; }
    }
}
