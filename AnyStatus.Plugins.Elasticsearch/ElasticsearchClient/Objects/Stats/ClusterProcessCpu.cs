using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterProcessCpu
    {
        [JsonProperty("percent")]
        public virtual int Percent { get; internal set; }
    }
}