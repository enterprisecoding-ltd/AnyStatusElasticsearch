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

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats
{
    public class OperatingSystemMemoryInfo
    {
        [JsonProperty("free_in_bytes")]
        public virtual long FreeBytes { get; internal set; }

        [JsonProperty("free_percent")]
        public virtual int FreePercent { get; internal set; }

        [JsonProperty("total_in_bytes")]
        public virtual long TotalBytes { get; internal set; }

        [JsonProperty("used_in_bytes")]
        public virtual long UsedBytes { get; internal set; }

        [JsonProperty("used_percent")]
        public virtual int UsedPercent { get; internal set; }
    }
}
