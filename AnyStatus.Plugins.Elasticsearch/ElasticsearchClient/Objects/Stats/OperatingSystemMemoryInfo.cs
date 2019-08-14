/*
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
