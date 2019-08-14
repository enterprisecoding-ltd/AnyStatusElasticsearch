using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterProcessCpu
    {
        [JsonProperty("percent")]
        public int Percent { get; set; }
    }
}