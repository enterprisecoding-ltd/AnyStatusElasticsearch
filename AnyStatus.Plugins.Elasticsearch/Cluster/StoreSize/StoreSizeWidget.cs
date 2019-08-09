using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.StoreSize
{
    [DisplayName("Cluster Store Size")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows store size taken by primary & replica shards for the Elasticsearch Cluster")]
    public class StoreSizeWidget : Metric, IElasticsearchWidget, ISchedulable
    {

        [Required]
        [Category("Cluster Store Size")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("Cluster Store Size")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Cluster Store Size")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Cluster Store Size")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Cluster Store Size")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        public StoreSizeWidget()
        {
            Name = "Store Size";

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
