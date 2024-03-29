﻿/*
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
using System.Xml.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Elasticsearch.FileSystemUsage
{
    [DisplayName("File System Usage")]
    [DisplayColumn("Elasticsearch")]
    [Description("Shows file system usage for the Elasticsearch Node/Cluster")]
    public class FileSystemUsageWidget : Metric, IElasticsearchWidget, ISchedulable, IReportProgress
    {
        [Url]
        [Required]
        [PropertyOrder(10)]
        [Category("File System Usage")]
        [DisplayName("Node Uris")]
        [Description("Elasticsearch node uris to connect")]
        public List<string> NodeUris { get; set; }

        [Category("File System Usage")]
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

        [Category("File System Usage")]
        [Browsable(true)]
        [PropertyOrder(40)]
        [Description("Username to connect Elasticsearch Cluster")]
        public string Username { get; set; }

        [Category("File System Usage")]
        [Browsable(true)]
        [PropertyOrder(50)]
        [Description("Password to connect Elasticsearch Cluster")]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string Password { get; set; }

        [Category("File System Usage")]
        [PropertyOrder(60)]
        [DisplayName("Trust Certificate")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }

        [Category("File System Usage")]
        [PropertyOrder(20)]
        [Description("Elasticsearch node id. Leave empty to watch Cluster File System usage")]
        public string NodeId { get; set; }

        [Required]
        [PropertyOrder(70)]
        [ItemsSource(typeof(FileSystemPercentageTypeItemsSource))]
        [Category("File System Usage")]
        [DisplayName("Percentage Type")]
        public FileSystemPercentageType PercentageType { get; set; }

        [Category("File System Usage")]
        [PropertyOrder(90)]
        [DisplayName("Show progress bar")]
        [Description("Should the status show a bar displaying how full the drive is?")]
        public bool ShowProgress { get; set; } = true;

        [Category("File System Usage")]
        [PropertyOrder(80)]
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

        private bool useBasicAuthentication;

        public FileSystemUsageWidget()
        {
            ErrorPercentage = 85;
            PercentageType = FileSystemPercentageType.PercentageUsed;

            Name = "File System Usage";
            Symbol = "%";
            Interval = 1;
            Units = IntervalUnits.Minutes;
            UseBasicAuthentication = false;
        }
    }
}
