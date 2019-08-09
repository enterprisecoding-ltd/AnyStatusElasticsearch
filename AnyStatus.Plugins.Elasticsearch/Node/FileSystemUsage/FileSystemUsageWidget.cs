using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace AnyStatus.Plugins.Elasticsearch.Node.FileSystemUsage
{
    [DisplayName("Node File System Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows file system usage for given Elasticsearch Node")]
    public class FileSystemUsageWidget : Metric, ISchedulable, IReportProgress
    {
        [Required]
        [Category("File System Usage")]
        [Description("Elasticsearch master server ip")]
        public string MasterIp { get; set; }

        [Required]
        [Category("File System Usage")]
        [Description("Elasticsearch master server port")]
        public int MasterPort { get; set; }

        [Required]
        [Category("File System Usage")]
        [Description("Elasticsearch node id")]
        public string NodeId { get; set; }

        [Required]
        [Category("File System Usage")]
        [DisplayName("Percentage Type")]
        public FileSystemPercentageType PercentageType { get; set; }

        [Category("File System Usage")]
        [DisplayName("Show progress bar")]
        [Description("Should the status show a bar displaying how full the drive is?")]
        public bool ShowProgress { get; set; } = true;

        [Category("File System Usage")]
        [DisplayName("Error percentage")]
        [Description("At what percentage should this Node error?")]
        public int ErrorPercentage { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public int Progress
        {
            get => (Value is int) ? (int)Value : -1;
            set
            {
                Value = value;

                OnPropertyChanged();
            }
        }

        public FileSystemUsageWidget()
        {
            MasterPort = 9200;
            ErrorPercentage = 100;

            Name = "Node File System Usage";
            Symbol = "%";
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
