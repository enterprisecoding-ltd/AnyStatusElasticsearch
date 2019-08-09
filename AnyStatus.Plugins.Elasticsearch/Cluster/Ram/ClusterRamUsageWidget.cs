using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.Ram
{
    [DisplayName("Cluster RAM Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows the percentage of RAM usage for the Elasticsearch Cluster")]
    public class ClusterRamUsageWidget : Sparkline, ISchedulable
    {
        [Required]
        [Category("Cluster RAM Usage")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Cluster RAM Usage")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

        public ClusterRamUsageWidget()
        {
            Name = "Cluster RAM Usage";
            Symbol = "%";
            MaxValue = 100;
            Interval = 1;
            Units = IntervalUnits.Minutes;

            MasterPort = 9200;
        }
    }
}
