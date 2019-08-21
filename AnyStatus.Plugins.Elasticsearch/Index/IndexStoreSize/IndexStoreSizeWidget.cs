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
using AnyStatus.API.Common.Utils;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Elasticsearch.Index.IndexStoreSize
{
    [DisplayName("Index Store Size")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows primary/total store size of a given elasticserch index")]
    public class IndexStoreSizeWidget : Sparkline, IElasticsearchWidget, ISchedulable
    {
        [Url]
        [Required]
        [PropertyOrder(10)]
        [Category("Index Store Size")]
        [DisplayName("Node Uris")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Required]
        [Category("Index Store Size")]
        [PropertyOrder(20)]
        [DisplayName("Index Name")]
        [Description("Index name to check health")]
        public string IndexName { get; set; }

        [Required]
        [Category("Index Store Size")]
        [PropertyOrder(30)]
        [DisplayName("Size Type")]
        [Description("Store size type to display")]
        public StoreSizeType SizeType { get; set; }

        [Category("Index Store Size")]
        [PropertyOrder(40)]
        [DisplayName("Use Basic Authentication")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication { get; set; }

        [Category("Index Store Size")]
        [PropertyOrder(50)]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("Index Store Size")]
        [PropertyOrder(60)]
        [Description("Password to connect Elasticsearch Cluster")]
        public string Password { get; set; }

        [Category("Index Store Size")]
        [PropertyOrder(70)]
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

        public IndexStoreSizeWidget()
        {
            Name = "Index Store Size";

            SizeType = StoreSizeType.Primary;
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }

        public override string ToString()
        {
            return BytesFormatter.Format(Convert.ToInt64(Value));
        }
    }
}
