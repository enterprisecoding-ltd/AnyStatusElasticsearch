using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterNodesStats
    {
        [JsonProperty("fs")]
        public ClusterFileSystem FileSystem { get; set; }

        [JsonProperty("process")]
        public ClusterProcess Process { get; set; }

        [JsonProperty("os")]
        public ClusterOperatingSystemStats OperatingSystem { get; set; }
    }
}
