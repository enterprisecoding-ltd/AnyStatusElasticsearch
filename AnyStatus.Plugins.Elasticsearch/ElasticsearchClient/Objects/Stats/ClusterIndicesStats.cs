using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterIndicesStats
    {
        [JsonProperty("store")]
        public virtual StoreStats Store { get; internal set; }


        [JsonProperty("docs")]
        public virtual DocStats Documents { get; internal set; }
    }
}
