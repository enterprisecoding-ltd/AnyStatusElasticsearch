using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterStatsResponse : ElasticsearchResponseBase
    {
        [JsonProperty("nodes")]
        public ClusterNodesStats Nodes { get; set; }

        [JsonProperty("indices")]
        public ClusterIndicesStats Indices { get; internal set; }
    }
}
