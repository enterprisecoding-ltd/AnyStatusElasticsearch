using Newtonsoft.Json;
using System;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects
{
    public class ElasticsearchResponseBase
    {
        [JsonIgnore]
        public bool IsValid { get; set; }

        [JsonIgnore]
        public Exception OriginalException;
    }
}
