using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Node.Ram
{
    public class NodeRamUsageHandler : IRequestHandler<MetricQueryRequest<NodeRamUsageWidget>>
    {
        public async Task Handle(MetricQueryRequest<NodeRamUsageWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var client = ElasticsearchHelper.GetElasticClient(clusterHealthWidget);
            var nodeId = new NodeIds(new[] { clusterHealthWidget.NodeId });

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest(nodeId) { FilterPath= new[] { "nodes.os.mem.used_percent" } }, cancellationToken);

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
