using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.RamUsage
{
    [DisplayName("RAM Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows the percentage of RAM usage for the Elasticsearch Cluster")]
    public class RamUsageWidget : Sparkline, IElasticsearchWidget, ISchedulable
    {

        [Required]
        [Category("RAM Usage")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("RAM Usage")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("RAM Usage")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("RAM Usage")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("RAM Usage")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        [Category("File System Usage")]
        [Description("Elasticsearch node id. Leave empty to watch cluster CPU usage")]
        public string NodeId { get; set; }

        public RamUsageWidget()
        {
            Name = "RAM Usage";
            Symbol = "%";
            MaxValue = 100;
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
