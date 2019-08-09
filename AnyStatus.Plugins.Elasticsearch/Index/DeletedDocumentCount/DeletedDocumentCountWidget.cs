using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Index.DeletedDocumentCount
{
    [DisplayName("Deleted Document Count")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows total deleted document count for the Elasticsearch Cluster")]
    public class DeletedDocumentCountWidget : Metric, ISchedulable
    {
        [Required]
        [Category("Deleted Document Count")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Deleted Document Count")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

        public DeletedDocumentCountWidget()
        {
            Name = "Deleted Document Count";
            MasterPort = 9200;

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
