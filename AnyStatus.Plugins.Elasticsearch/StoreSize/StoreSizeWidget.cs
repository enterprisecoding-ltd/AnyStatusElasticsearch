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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Elasticsearch.StoreSize
{
    [DisplayName("Store Size")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows store size taken by primary & replica shards for the Elasticsearch Node/Cluster")]
    public class StoreSizeWidget : Sparkline, IElasticsearchWidget, ISchedulable
    {
        [Url]
        [Required]
        [PropertyOrder(10)]
        [Category("Store Size")]
        [DisplayName("Node Uris")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("Store Size")]
        [PropertyOrder(20)]
        [DisplayName("Node Id")]
        [Description("Elasticsearch node id. Leave empty to watch Cluster Store Size")]
        public string NodeId { get; set; }

        [Category("Store Size")]
        [PropertyOrder(30)]
        [RefreshProperties(RefreshProperties.All)]
        [DisplayName("Use Basic Authentication")]
        [Description("Use Basic Authentication to connect Elasticsearch Cluster")]
        public bool UseBasicAuthentication
        {
            get => useBasicAuthentication;
            set
            {
                useBasicAuthentication = value;

                OnPropertyChanged();

                SetPropertyVisibility(nameof(Username), useBasicAuthentication);
                SetPropertyVisibility(nameof(Password), useBasicAuthentication);
            }
        }

        [Browsable(true)]
        [Category("Store Size")]
        [PropertyOrder(40)]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Browsable(true)]
        [Category("Store Size")]
        [PropertyOrder(50)]
        [Description("Password to connect Elasticsearch Cluster")]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string Password { get; set; }

        [Category("Store Size")]
        [PropertyOrder(60)]
        [DisplayName("Trust Certificate")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        private bool useBasicAuthentication;

        public StoreSizeWidget()
        {
            Name = "Store Size";

            Interval = 1;
            Units = IntervalUnits.Minutes;
            UseBasicAuthentication = false;
        }

        public override string ToString()
        {
            return BytesFormatter.Format(Convert.ToInt64(Value));
        }
    }
}
