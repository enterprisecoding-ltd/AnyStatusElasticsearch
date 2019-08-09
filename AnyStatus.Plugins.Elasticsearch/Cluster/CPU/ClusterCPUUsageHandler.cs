using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.CPU
{
    public class ClusterCPUUsageHandler : IRequestHandler<MetricQueryRequest<ClusterCPUUsageWidget>>
    {
        public async Task Handle(MetricQueryRequest<ClusterCPUUsageWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var client = ElasticsearchHelper.GetElasticClient(clusterHealthWidget);

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest { FilterPath= new[] { "nodes.process.cpu.percent" } }, cancellationToken);

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
