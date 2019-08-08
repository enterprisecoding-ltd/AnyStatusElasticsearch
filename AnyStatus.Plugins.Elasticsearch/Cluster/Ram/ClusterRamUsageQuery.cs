using AnyStatus.API;
using Elasticsearch.Net;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.Ram
{
    public class ClusterRamUsageQuery : IRequestHandler<MetricQueryRequest<ClusterRamUsageWidget>>
    {
        public async Task Handle(MetricQueryRequest<ClusterRamUsageWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var connectionPool = new SingleNodeConnectionPool(new Uri($"http://{clusterHealthWidget.MasterIp}:{clusterHealthWidget.MasterPort}"));

            var settings = new ConnectionSettings(connectionPool);
            var client = new ElasticClient(settings);

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest { FilterPath= new[] { "nodes.os.mem.used_percent" } }, cancellationToken);

            if (clusterStatsResponse.IsValid)
            {
                request.DataContext.Value = clusterStatsResponse.Nodes.OperatingSystem.Memory.UsedPercent;
                request.DataContext.State = State.Ok;
            }
            else
            {
                clusterHealthWidget.State = State.Invalid;
            }
        }
    }
}
