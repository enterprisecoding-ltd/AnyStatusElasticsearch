using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterNodesStats
    {
        [JsonProperty("fs")]
        public virtual ClusterFileSystem FileSystem { get; internal set; }

        [JsonProperty("process")]
        public virtual ClusterProcess Process { get; internal set; }

        [JsonProperty("os")]
        public virtual ClusterOperatingSystemStats OperatingSystem { get; internal set; }
    }
}
