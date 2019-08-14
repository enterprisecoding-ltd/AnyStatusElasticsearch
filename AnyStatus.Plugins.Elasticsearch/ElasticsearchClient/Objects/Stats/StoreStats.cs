using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class StoreStats
    {
        [JsonProperty("size_in_bytes")]
        public double SizeInBytes { get; set; }
    }
}