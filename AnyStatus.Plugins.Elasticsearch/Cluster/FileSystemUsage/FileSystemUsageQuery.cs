using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.FileSystemUsage
{
    public class FileSystemUsageQuery : IRequestHandler<MetricQueryRequest<FileSystemUsageWidget>>
    {
        public async Task Handle(MetricQueryRequest<FileSystemUsageWidget> request, CancellationToken cancellationToken)
        {
            var fileSystemUsageWidget = request.DataContext;

            var client = ElasticsearchHelper.GetElasticClient(fileSystemUsageWidget);

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest() { FilterPath = new[] { "nodes.fs" } }, cancellationToken);

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
