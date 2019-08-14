using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterOperatingSystemStats
    {
        [JsonProperty("mem")]
        public OperatingSystemMemoryInfo Memory { get; internal set; }
    }
}
