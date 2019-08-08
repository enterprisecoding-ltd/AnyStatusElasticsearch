using AnyStatus.API;
using Elasticsearch.Net;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Node.CPU
{
    public class NodeCPUUsageQuery : IRequestHandler<MetricQueryRequest<NodeCPUUsageWidget>>
    {
        public async Task Handle(MetricQueryRequest<NodeCPUUsageWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var connectionPool = new SingleNodeConnectionPool(new Uri($"http://{clusterHealthWidget.MasterIp}:{clusterHealthWidget.MasterPort}"));

            var settings = new ConnectionSettings(connectionPool);
            var client = new ElasticClient(settings);
            var nodeId = new NodeIds(new[] { clusterHealthWidget.NodeId });

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest(nodeId) { FilterPath= new[] { "nodes.process.cpu.percent" } }, cancellationToken);

            if (clusterStatsResponse.IsValid)
            {
                request.DataContext.Value = clusterStatsResponse.Nodes.Process.Cpu.Percent;
                request.DataContext.State = State.Ok;
            }
            else
            {
                clusterHealthWidget.State = State.Invalid;
            }
        }
    }
}
