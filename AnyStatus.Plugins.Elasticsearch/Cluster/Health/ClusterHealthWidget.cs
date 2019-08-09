using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.Health
{
    [DisplayName("Cluster Health")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows Elasticsearch cluster health")]
    public class ClusterHealthWidget : Widget, IHealthCheck, ISchedulable, IStartable, IStoppable
    {
        [Required]
        [Category("Cluster Health")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Cluster Health")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

        public ClusterHealthWidget()
        {
            Name = "Cluster Health";
            MasterPort = 9200;

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}