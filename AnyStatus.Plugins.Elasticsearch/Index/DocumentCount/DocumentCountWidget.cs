using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Index.DocumentCount
{
    [DisplayName("Document Count")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows total document count for the Elasticsearch Cluster")]
    public class DocumentCountWidget : Metric, ISchedulable
    {
        [Required]
        [Category("Master")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("Master")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

        public DocumentCountWidget()
        {
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
