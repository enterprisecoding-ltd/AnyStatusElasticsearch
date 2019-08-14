using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.CPUUsage
{
    public class CPUUsageHandler : IRequestHandler<MetricQueryRequest<CPUUsageWidget>>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        public CPUUsageHandler() :this(new ElasticsearchHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal CPUUsageHandler(ElasticsearchHelper elasticsearchHelper) {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(MetricQueryRequest<CPUUsageWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(clusterHealthWidget);

            ElasticsearchClient.Objects.Stats.ClusterStatsResponse clusterStatsResponse;
            if (string.IsNullOrWhiteSpace(clusterHealthWidget.NodeId))
            {
                clusterStatsResponse = await client.StatsAsync("nodes.process.cpu.percent", cancellationToken);
            }
            else
            {
                clusterStatsResponse = await client.StatsAsync("nodes.process.cpu.percent", clusterHealthWidget.NodeId, cancellationToken);
            }
            

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
