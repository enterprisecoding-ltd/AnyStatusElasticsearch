using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cat
{
    public class IndexEntry
    {
        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("health")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Shared.Health Health { get; set; }

        [JsonProperty("docs.count")]
        public long DocumentCount { get; set; }

        [JsonProperty("docs.deleted")]
        public long DocumentDeletedCount { get; set; }

        [JsonProperty("store.size")]
        public string StoreSize { get; set; }

        [JsonProperty("pri.store.size")]
        public string PrimaryStoreSize { get; set; }
    }
}
