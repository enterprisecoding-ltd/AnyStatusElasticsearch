using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class OperatingSystemMemoryInfo
    {
        [JsonProperty("free_in_bytes")]
        public long FreeBytes { get; internal set; }

        [JsonProperty("free_percent")]
        public int FreePercent { get; internal set; }

        [JsonProperty("total_in_bytes")]
        public long TotalBytes { get; internal set; }

        [JsonProperty("used_in_bytes")]
        public long UsedBytes { get; internal set; }

        [JsonProperty("used_percent")]
        public int UsedPercent { get; internal set; }
    }
}
