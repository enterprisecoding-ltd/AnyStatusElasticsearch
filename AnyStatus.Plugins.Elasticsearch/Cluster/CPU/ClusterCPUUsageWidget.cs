using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.CPU
{
    [DisplayName("Cluster CPU Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows the percentage of CPU usage for the Elasticsearch Cluster")]
    public class ClusterCPUUsageWidget : Sparkline, ISchedulable
    {
        [Required]
        [Category("Master")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Master")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

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
