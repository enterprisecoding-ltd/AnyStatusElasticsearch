using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterOperatingSystemStats
    {
        [JsonProperty("mem")]
        public virtual OperatingSystemMemoryInfo Memory { get; internal set; }
    }
}
