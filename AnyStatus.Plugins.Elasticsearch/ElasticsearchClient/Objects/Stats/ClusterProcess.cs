using Newtonsoft.Json;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class ClusterProcess
    {
        [JsonProperty("cpu")]
        public ClusterProcessCpu Cpu { get; internal set; }
    }
}
