using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.CPU
{
    [DisplayName("Cluster CPU Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows the percentage of CPU usage for the Elasticsearch Cluster")]
    public class ClusterCPUUsageWidget : Sparkline, IElasticsearchWidget, ISchedulable
    {
        [Required]
        [Category("Cluster CPU Usage")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("Cluster CPU Usage")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Cluster CPU Usage")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Cluster CPU Usage")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Cluster CPU Usage")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        public ClusterCPUUsageWidget()
        {
            Name = "Cluster CPU Usage";
            Symbol = "%";
            MaxValue = 100;
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
