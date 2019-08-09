using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Node.StoreSize
{
    [DisplayName("Node Store Size")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows store size taken by primary & replica shards for given Elasticsearch Node")]
    public class StoreSizeWidget : Metric, IElasticsearchNodeWidget, ISchedulable
    {

        [Required]
        [Category("Node Store Size")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("Node Store Size")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Node Store Size")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Node Store Size")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Node Store Size")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        [Required]
        [Category("Node Store Size")]
        [Description("Elasticsearch node id")]
        public string NodeId { get; set; }

        public StoreSizeWidget()
        {
            Name = "Node Store Size";

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
