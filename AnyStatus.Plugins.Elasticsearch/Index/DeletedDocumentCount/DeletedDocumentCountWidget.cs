using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Index.DeletedDocumentCount
{
    [DisplayName("Deleted Document Count")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows total deleted document count for the Elasticsearch Cluster")]
    public class DeletedDocumentCountWidget : Metric, IElasticsearchWidget, ISchedulable
    {

        [Required]
        [Category("Deleted Document Count")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("Deleted Document Count")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Deleted Document Count")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Deleted Document Count")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Deleted Document Count")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        public DeletedDocumentCountWidget()
        {
            Name = "Deleted Document Count";

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
