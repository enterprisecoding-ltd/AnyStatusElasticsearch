using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class StoreStats
    {
        [JsonProperty("size_in_bytes")]
        public virtual double SizeInBytes { get; set; }
    }
}