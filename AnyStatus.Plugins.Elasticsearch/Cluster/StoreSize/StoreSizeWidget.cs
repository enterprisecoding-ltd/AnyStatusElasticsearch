using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.StoreSize
{
    [DisplayName("Cluster Store Size")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows store size taken by primary & replica shards for the Elasticsearch Cluster")]
    public class StoreSizeWidget : Metric, ISchedulable
    {
        [Required]
        [Category("Cluster Store Size")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Cluster Store Size")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

        public StoreSizeWidget()
        {
            Name = "Store Size";
            MasterPort = 9200;

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
