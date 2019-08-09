using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Node.FileSystemUsage
{
    public class FileSystemUsageQuery : IRequestHandler<MetricQueryRequest<FileSystemUsageWidget>>
    {
        public async Task Handle(MetricQueryRequest<FileSystemUsageWidget> request, CancellationToken cancellationToken)
        {
            var fileSystemUsageWidget = request.DataContext;

            var connectionPool = new SingleNodeConnectionPool(new Uri($"http://{fileSystemUsageWidget.MasterIp}:{fileSystemUsageWidget.MasterPort}"));

            var settings = new ConnectionSettings(connectionPool);
            var client = new ElasticClient(settings);
            var nodeId = new NodeIds(new[] { fileSystemUsageWidget.NodeId });

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest(nodeId), cancellationToken);

            if (clusterStatsResponse.IsValid)
            {
                if (fileSystemUsageWidget.PercentageType== Shared.FileSystemPercentageType.PercentageUsed)
                {
                    var usedInBytes = clusterStatsResponse.Nodes.FileSystem.TotalInBytes - clusterStatsResponse.Nodes.FileSystem.AvailableInBytes;
                    request.DataContext.Progress = (int)Math.Round((usedInBytes / (double)clusterStatsResponse.Nodes.FileSystem.TotalInBytes) * 100);
                    request.DataContext.Message = $"Used {request.DataContext.Progress}%{Environment.NewLine}" +
                           $"{FileSizeFormatter.FormatSize(usedInBytes)} used out of {FileSizeFormatter.FormatSize(clusterStatsResponse.Nodes.FileSystem.TotalInBytes)}";
                }
                else
                {
                    request.DataContext.Progress = (int)Math.Round((clusterStatsResponse.Nodes.FileSystem.AvailableInBytes / (double)clusterStatsResponse.Nodes.FileSystem.TotalInBytes) * 100);
                    request.DataContext.Message = $"Avaliable {request.DataContext.Progress}%{Environment.NewLine}" +
                           $"{FileSizeFormatter.FormatSize(clusterStatsResponse.Nodes.FileSystem.AvailableInBytes)} available out of {FileSizeFormatter.FormatSize(clusterStatsResponse.Nodes.FileSystem.TotalInBytes)}";
                }

                request.DataContext.State = request.DataContext.Progress >= request.DataContext.ErrorPercentage ? State.Failed : State.Ok;
            }
            else
            {
                fileSystemUsageWidget.State = State.Invalid;
            }
        }
    }
}
