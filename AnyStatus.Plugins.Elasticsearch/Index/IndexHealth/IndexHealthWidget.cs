/*
Anystatus Elasticsearch plugin
Copyright (C) 2019  Enterprisecoding (Fatih Boy)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Elasticsearch.Index.IndexHealth
{
    [DisplayName("Index Health")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows total document count for the Elasticsearch Cluster")]
    public class IndexHealthWidget : Widget, IElasticsearchWidget, IHealthCheck, ISchedulable, IStartable, IStoppable
    {
        [Url]
        [Required]
        [DisplayName("Node Uris")]
        [PropertyOrder(10)]
        [Category("Index Health")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Required]
        [Category("Index Health")]
        [PropertyOrder(20)]
        [DisplayName("Index Name")]
        [Description("Index name to check health")]
        public string IndexName { get; set; }

        [Category("Index Health")]
        [PropertyOrder(30)]
        [DisplayName("Use Basic Authentication")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Index Health")]
        [PropertyOrder(40)]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Index Health")]
        [PropertyOrder(50)]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Index Health")]
        [PropertyOrder(60)]
        [DisplayName("Trust Certificate")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public string IndexUuid;

        public IndexHealthWidget()
        {
            Name = "Index Health";

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
