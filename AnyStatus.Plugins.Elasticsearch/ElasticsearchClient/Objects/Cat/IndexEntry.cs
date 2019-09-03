/*
Anystatus Elasticsearch plugin
Copyright 2019 Fatih Boy

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */
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
