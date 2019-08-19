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

namespace AnyStatus.Plugins.Elasticsearch.Index.IndexDeletedDocumentCount
{
    [DisplayName("Index Deleted Document Count")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows total document count for the Elasticsearch Cluster")]
    public class IndexDeletedDocumentCountWidget : Sparkline, IElasticsearchWidget, ISchedulable
    {
        /// <summary>
        /// Elasticsearch nodes uris to connect
        /// </summary>
        [Url]
        [Required]
        [PropertyOrder(10)]
        [Category("Index Deleted Document Count")]
        [DisplayName("Node Uris")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        /// <summary>
        /// Index name to show deleted document count
        /// </summary>
        [Required]
        [Category("Index Deleted Document Count")]
        [PropertyOrder(20)]
        [DisplayName("Index Name")]
        [Description("Index name to check health")]
        public string IndexName { get; set; }

        /// <summary>
        /// Should we use basic authentication?
        /// </summary>
        [Category("Index Deleted Document Count")]
        [PropertyOrder(30)]
        [DisplayName("Use Basic Authentication")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        /// <summary>
        /// Username for basic authentication
        /// </summary>
        [Category("Index Deleted Document Count")]
        [PropertyOrder(40)]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        /// <summary>
        /// Password for basic authentication
        /// </summary>
        [Category("Index Deleted Document Count")]
        [PropertyOrder(50)]
        [Description("Password to connect Elasticsearch Cluster")]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string Password { get; set; }

        /// <summary>
        /// Should we trust unknown certificates?
        /// </summary>
        [Category("Index Deleted Document Count")]
        [PropertyOrder(60)]
        [DisplayName("Trust Certificate")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        /// <summary>
        /// Tracked index uuid
        /// </summary>
        /// <remarks>
        /// This field used by Index count widget only...
        /// </remarks>
        [XmlIgnore]
        [Browsable(false)]
        public string IndexUuid { get; internal set; }

        public IndexDeletedDocumentCountWidget()
        {
            Name = "Index Deleted Document Count";

            Interval = 1;
            Units = IntervalUnits.Minutes;
        }
    }
}
