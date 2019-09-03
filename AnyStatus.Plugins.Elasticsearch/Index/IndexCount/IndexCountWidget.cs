/*
Anystatus Elasticsearch plugin
Copyright 2019 Fatih Boy

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */
using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Elasticsearch.Index.IndexCount
{
    /// <summary>
    /// Shows total index count on given Elasticsearch Cluster
    /// </summary>
    [DisplayName("Index Count")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows total document count for the Elasticsearch Cluster")]
    public class IndexCountWidget : Sparkline, IElasticsearchWidget, ISchedulable
    {
        /// <summary>
        /// Elasticsearch nodes uris to connect
        /// </summary>
        [Url]
        [Required]
        [PropertyOrder(10)]
        [Category("Index Count")]
        [DisplayName("Node Uris")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        /// <summary>
        /// Index details to show for each discovered index
        /// </summary>
        [Category("Index Count")]
        [PropertyOrder(20)]
        [ItemsSource(typeof(IndexDetailsItemsSource))]
        [DisplayName("Index Details")]
        [Description("Index detail type to show for each discovered index")]
        public IndexDetail IndexDetails { get; set; }

        /// <summary>
        /// Should we use basic authentication?
        /// </summary>
        [Category("Index Count")]
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

        /// <summary>
        /// Username for basic authentication
        /// </summary>
        [Category("Index Count")]
        [Browsable(true)]
        [PropertyOrder(40)]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        /// <summary>
        /// Password for basic authentication
        /// </summary>
        [Category("Index Count")]
        [Browsable(true)]
        [PropertyOrder(50)]
        [Description("Password to connect Elasticsearch Cluster")]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string Password { get; set; }

        /// <summary>
        /// Should we trust unknown certificates?
        /// </summary>
        [Category("Index Count")]
        [PropertyOrder(60)]
        [DisplayName("Trust Certificate")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        private bool useBasicAuthentication;

        public IndexCountWidget()
        {
            Name = "Index Count";

            IndexDetails = IndexDetail.None;

            Interval = 1;
            Units = IntervalUnits.Minutes;
            UseBasicAuthentication = false;
        }
    }
}
