using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Node.StoreSize
{
    [DisplayName("Node Store Size")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows store size taken by primary & replica shards for given Elasticsearch Node")]
    public class StoreSizeWidget : Metric, ISchedulable
    {
        [Required]
        [Category("Node Store Size")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Node Store Size")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

        [Required]
        [Category("Node Store Size")]
        [Description("Elasticsearch node id")]
        public string NodeId { get; set; }

        public StoreSizeWidget()
        {
            Name = "Node Store Size";
            MasterPort = 9200;

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
