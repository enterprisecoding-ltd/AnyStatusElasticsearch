﻿using AnyStatus.API;
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
                if (fileSystemUsageWidget.PercentageType == Shared.FileSystemPercentageType.PercentageUsed)
                {
                    var usedInBytes = clusterStatsResponse.Nodes.FileSystem.TotalInBytes - clusterStatsResponse.Nodes.FileSystem.AvailableInBytes;
                    request.DataContext.Progress = (int)Math.Round((usedInBytes / (double)clusterStatsResponse.Nodes.FileSystem.TotalInBytes) * 100);
                    request.DataContext.Message = $"Used {request.DataContext.Progress}%{Environment.NewLine}" +
                           $"{FileSizeFormatter.FormatSize(usedInBytes)} used out of {FileSizeFormatter.FormatSize(clusterStatsResponse.Nodes.FileSystem.TotalInBytes)}";

                    request.DataContext.State = request.DataContext.Progress >= request.DataContext.ErrorPercentage ? State.Failed : State.Ok;
                }
                else
                {
                    request.DataContext.Progress = (int)Math.Round((clusterStatsResponse.Nodes.FileSystem.AvailableInBytes / (double)clusterStatsResponse.Nodes.FileSystem.TotalInBytes) * 100);
                    request.DataContext.Message = $"Avaliable {request.DataContext.Progress}%{Environment.NewLine}" +
                           $"{FileSizeFormatter.FormatSize(clusterStatsResponse.Nodes.FileSystem.AvailableInBytes)} available out of {FileSizeFormatter.FormatSize(clusterStatsResponse.Nodes.FileSystem.TotalInBytes)}";

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
