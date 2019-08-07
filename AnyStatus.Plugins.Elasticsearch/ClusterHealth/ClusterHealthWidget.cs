using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.ClusterHealth
{
    [DisplayName("Cluster Health")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows Elasticsearch cluster health")]
    public class ClusterHealthWidget : Widget, IHealthCheck, ISchedulable, IStartable, IStoppable
    {
        [Required]
        [Category("Master")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Master")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }
    }
}