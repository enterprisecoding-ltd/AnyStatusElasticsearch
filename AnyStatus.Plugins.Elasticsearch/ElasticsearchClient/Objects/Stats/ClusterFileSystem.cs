using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterFileSystem
    {
        [JsonProperty("total_in_bytes")]
        public long TotalInBytes { get; set; }

        [JsonProperty("free_in_bytes")]
        public long FreeInBytes { get; set; }

        [JsonProperty("available_in_bytes")]
        public long AvailableInBytes { get; set; }
    }
}
