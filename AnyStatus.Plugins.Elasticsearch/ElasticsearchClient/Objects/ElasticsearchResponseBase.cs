using Newtonsoft.Json;
using System;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects
{
    public class ElasticsearchResponseBase
    {
        [JsonIgnore]
        public virtual bool IsValid { get; internal set; }

        [JsonIgnore]
        public Exception OriginalException;
    }
}
