using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.FileSystemUsage
{
    [DisplayName("Cluster File System Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows file system usage for the Elasticsearch Cluster")]
    public class FileSystemUsageWidget : Metric, IElasticsearchWidget, ISchedulable, IReportProgress
    {

        [Required]
        [Category("Cluster File System Usage")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("Cluster File System Usage")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Cluster File System Usage")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Cluster File System Usage")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Cluster File System Usage")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        [Required]
        [Category("Cluster File System Usage")]
        [DisplayName("Percentage Type")]
        public FileSystemPercentageType PercentageType { get; set; }

        [Category("Cluster File System Usage")]
        [DisplayName("Show progress bar")]
        [Description("Should the status show a bar displaying how full the drive is?")]
        public bool ShowProgress { get; set; } = true;

        [Category("Cluster File System Usage")]
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
            ErrorPercentage = 85;
            PercentageType = FileSystemPercentageType.PercentageUsed;

            Name = "Cluster File System Usage";
            Symbol = "%";
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
