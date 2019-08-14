using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.CPUUsage
{
    [DisplayName("CPU Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows the percentage of CPU usage for the Elasticsearch Cluster")]
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
