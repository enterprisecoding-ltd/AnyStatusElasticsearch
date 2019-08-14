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
using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.CPUUsage
{
    [DisplayName("CPU Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows the percentage of CPU usage for the Elasticsearch Node/Cluster")]
    public class CPUUsageWidget : Sparkline, IElasticsearchWidget, ISchedulable
    {
        [Required]
        [Category("CPU Usage")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("CPU Usage")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("CPU Usage")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("CPU Usage")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("CPU Usage")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        [Category("CPU Usage")]
        [Description("Elasticsearch node id. Leave empty to watch cluster CPU usage")]
        public string NodeId { get; set; }

        public CPUUsageWidget()
        {
            Name = "CPU Usage";
            Symbol = "%";
            MaxValue = 100;
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
