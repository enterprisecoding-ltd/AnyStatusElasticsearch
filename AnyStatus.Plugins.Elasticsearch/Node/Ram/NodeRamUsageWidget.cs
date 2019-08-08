﻿using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Node.Ram
{
    [DisplayName("Node RAM Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows the percentage of RAM usage for the Elasticsearch Cluster")]
    public class NodeRamUsageWidget : Sparkline, ISchedulable
    {
        [Required]
        [Category("Master")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Master")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

        [Required]
        [Category("Node")]
        [Description("Elasticsearch node id")]
        public string NodeId { get; set; }

        public NodeRamUsageWidget()
        {
            Name = "Node RAM Usage";
            Symbol = "%";
            MaxValue = 100;
            Interval = 1;
            Units = IntervalUnits.Minutes;

            MasterPort = 9200;
        }
    }
}
