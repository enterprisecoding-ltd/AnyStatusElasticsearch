using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterFileSystem
    {
        [JsonProperty("total_in_bytes")]
        public virtual long TotalInBytes { get; internal set; }

        [JsonProperty("free_in_bytes")]
        public virtual long FreeInBytes { get; internal set; }

        [JsonProperty("available_in_bytes")]
        public virtual long AvailableInBytes { get; internal set; }
    }
}
