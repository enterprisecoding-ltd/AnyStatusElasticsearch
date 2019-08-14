using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterStatsResponse : ElasticsearchResponseBase
    {
        [JsonProperty("nodes")]
        public virtual ClusterNodesStats Nodes { get; internal set; }

        [JsonProperty("indices")]
        public virtual ClusterIndicesStats Indices { get; internal set; }
    }
}
