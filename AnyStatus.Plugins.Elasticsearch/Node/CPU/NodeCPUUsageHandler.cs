using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Node.CPU
{
    public class NodeCPUUsageHandler : IRequestHandler<MetricQueryRequest<NodeCPUUsageWidget>>
    {
        public async Task Handle(MetricQueryRequest<NodeCPUUsageWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var client = ElasticsearchHelper.GetElasticClient(clusterHealthWidget);
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
