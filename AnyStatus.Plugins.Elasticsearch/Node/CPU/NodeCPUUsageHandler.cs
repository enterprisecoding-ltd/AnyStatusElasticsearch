using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
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

            var clusterStatsResponse = await client.StatsAsync("nodes.process.cpu.percent", clusterHealthWidget.NodeId, cancellationToken);

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
