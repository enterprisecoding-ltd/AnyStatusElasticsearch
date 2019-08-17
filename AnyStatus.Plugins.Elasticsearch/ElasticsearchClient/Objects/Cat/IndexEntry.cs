﻿/*
Anystatus Elasticsearch plugin
Copyright (C) 2019  Enterprisecoding (Fatih Boy)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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