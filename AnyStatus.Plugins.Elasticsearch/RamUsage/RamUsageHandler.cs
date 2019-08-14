using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.RamUsage
{
    public class RamUsageHandler : IRequestHandler<MetricQueryRequest<RamUsageWidget>>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        public RamUsageHandler() : this(new ElasticsearchHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal RamUsageHandler(ElasticsearchHelper elasticsearchHelper)
        {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(MetricQueryRequest<RamUsageWidget> request, CancellationToken cancellationToken)
        {
            var ramUsageWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(ramUsageWidget);

            ElasticsearchClient.Objects.Stats.ClusterStatsResponse clusterStatsResponse;
            if (string.IsNullOrWhiteSpace(ramUsageWidget.NodeId))
            {
                clusterStatsResponse = await client.StatsAsync("nodes.os.mem.used_percent", cancellationToken);
            }
            else
            {
                clusterStatsResponse = await client.StatsAsync("nodes.os.mem.used_percent", ramUsageWidget.NodeId, cancellationToken);
            }

            if (clusterStatsResponse.IsValid)
            {
                request.DataContext.Value = clusterStatsResponse.Nodes.OperatingSystem.Memory.UsedPercent;
                request.DataContext.State = State.Ok;
            }
            else
            {
                ramUsageWidget.State = State.Invalid;
            }
        }
    }
}
