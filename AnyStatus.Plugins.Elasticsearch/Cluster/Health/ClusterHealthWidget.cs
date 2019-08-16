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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.Health
{
    [DisplayName("Cluster Health")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows Elasticsearch cluster health")]
    public class ClusterHealthWidget : Widget, IElasticsearchWidget, IHealthCheck, ISchedulable, IStartable, IStoppable
    {

        [Required]
        [PropertyOrder(10)]
        [Category("Cluster Health")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [PropertyOrder(20)]
        [Category("Cluster Health")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [PropertyOrder(30)]
        [Category("Cluster Health")]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [PropertyOrder(40)]
        [Category("Cluster Health")]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [PropertyOrder(50)]
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