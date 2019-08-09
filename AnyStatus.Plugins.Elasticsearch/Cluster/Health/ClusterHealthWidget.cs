using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.Health
{
    [DisplayName("Cluster Health")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows Elasticsearch cluster health")]
    public class ClusterHealthWidget : Widget, IElasticsearchWidget, IHealthCheck, ISchedulable, IStartable, IStoppable
    {

        [Required]
        [Category("Cluster Health")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("Cluster Health")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Cluster Health")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Cluster Health")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Cluster Health")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        public ClusterHealthWidget()
        {
            Name = "Cluster Health";

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}