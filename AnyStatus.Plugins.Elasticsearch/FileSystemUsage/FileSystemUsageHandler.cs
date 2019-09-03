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
using AnyStatus.API.Common.Utils;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.FileSystemUsage
{
    public class FileSystemUsageHandler : IRequestHandler<MetricQueryRequest<FileSystemUsageWidget>>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        public FileSystemUsageHandler() : this(new ElasticsearchHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal FileSystemUsageHandler(ElasticsearchHelper elasticsearchHelper)
        {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(MetricQueryRequest<FileSystemUsageWidget> request, CancellationToken cancellationToken)
        {
            var fileSystemUsageWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(fileSystemUsageWidget);

            ElasticsearchClient.Objects.Stats.ClusterStatsResponse clusterStatsResponse;
            if (string.IsNullOrWhiteSpace(fileSystemUsageWidget.NodeId))
            {
                clusterStatsResponse = await client.StatsAsync("nodes.fs", cancellationToken);
            }
            else
            {
                clusterStatsResponse = await client.StatsAsync("nodes.fs", fileSystemUsageWidget.NodeId, cancellationToken);
            }

            if (clusterStatsResponse.IsValid)
            {
                if (fileSystemUsageWidget.PercentageType == FileSystemPercentageType.PercentageUsed)
                {
                    var usedInBytes = clusterStatsResponse.Nodes.FileSystem.TotalInBytes - clusterStatsResponse.Nodes.FileSystem.AvailableInBytes;
                    request.DataContext.Progress = (int)Math.Round((usedInBytes / (double)clusterStatsResponse.Nodes.FileSystem.TotalInBytes) * 100);
                    request.DataContext.Message = $"Used {request.DataContext.Progress}%{Environment.NewLine}" +
                           $"{BytesFormatter.Format(usedInBytes)} used out of {BytesFormatter.Format(clusterStatsResponse.Nodes.FileSystem.TotalInBytes)}";

                    request.DataContext.State = request.DataContext.Progress >= request.DataContext.ErrorPercentage ? State.Failed : State.Ok;
                }
                else
                {
                    request.DataContext.Progress = (int)Math.Round((clusterStatsResponse.Nodes.FileSystem.AvailableInBytes / (double)clusterStatsResponse.Nodes.FileSystem.TotalInBytes) * 100);
                    request.DataContext.Message = $"Avaliable {request.DataContext.Progress}%{Environment.NewLine}" +
                           $"{BytesFormatter.Format(clusterStatsResponse.Nodes.FileSystem.AvailableInBytes)} available out of {BytesFormatter.Format(clusterStatsResponse.Nodes.FileSystem.TotalInBytes)}";

                    request.DataContext.State = request.DataContext.Progress <= request.DataContext.ErrorPercentage ? State.Failed : State.Ok;
                }
            }
            else
            {
                fileSystemUsageWidget.State = State.Invalid;
            }
        }
    }
}
