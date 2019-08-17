using Newtonsoft.Json;
using System.Collections.Generic;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Index
{
    public class IndicesStatsResponse: ElasticsearchResponseBase
    {
        [JsonProperty("indices")]
        public virtual Dictionary<string, IndicesStats> Indices { get; internal set; }
    }
}
