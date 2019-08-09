using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Node.Ram
{
    [DisplayName("Node RAM Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows the percentage of RAM usage for the Elasticsearch Cluster")]
    public class NodeRamUsageWidget : Sparkline, IElasticsearchNodeWidget, ISchedulable
    {

        [Required]
        [Category("Node RAM Usage")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("Node RAM Usage")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Node RAM Usage")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Node RAM Usage")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Node RAM Usage")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        [Required]
        [Category("Node RAM Usage")]
        [Description("Elasticsearch node id")]
        public string NodeId { get; set; }

        public NodeRamUsageWidget()
        {
            Name = "Node RAM Usage";
            Symbol = "%";
            MaxValue = 100;
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
