using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.CPU
{
    public class ClusterCPUUsageHandler : IRequestHandler<MetricQueryRequest<ClusterCPUUsageWidget>>
    {
        private readonly ElasticsearchHelper elasticsearchHelper;

        public ClusterCPUUsageHandler() :this(new ElasticsearchHelper()) { }

        public ClusterCPUUsageHandler(ElasticsearchHelper elasticsearchHelper) {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(MetricQueryRequest<ClusterCPUUsageWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(clusterHealthWidget);

            var clusterStatsResponse = await client.StatsAsync("nodes.process.cpu.percent", cancellationToken);

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
