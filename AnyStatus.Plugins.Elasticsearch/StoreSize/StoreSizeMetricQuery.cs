using AnyStatus.API;
using System.Threading;
using System.Threading.Tasks;
using AnyStatus.Plugins.Elasticsearch.Helpers;

namespace AnyStatus.Plugins.Elasticsearch.StoreSize
{
    public class StoreSizeMetricQuery : IMetricQuery<StoreSizeWidget>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        public StoreSizeMetricQuery() : this(new ElasticsearchHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal StoreSizeMetricQuery(ElasticsearchHelper elasticsearchHelper)
        {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(MetricQueryRequest<StoreSizeWidget> request, CancellationToken cancellationToken)
        {
            var storeSizeWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(storeSizeWidget);

            ElasticsearchClient.Objects.Stats.ClusterStatsResponse clusterStatsResponse;
            if (string.IsNullOrWhiteSpace(storeSizeWidget.NodeId))
            {
                clusterStatsResponse = await client.StatsAsync("indices.store.size_in_bytes", cancellationToken);
            }
            else
            {
                clusterStatsResponse = await client.StatsAsync("indices.store.size_in_bytes", storeSizeWidget.NodeId, cancellationToken);
            }

            if (clusterStatsResponse.IsValid)
            {
                request.DataContext.Value = FileSizeFormatter.FormatSize(clusterStatsResponse.Indices.Store.SizeInBytes);
                request.DataContext.State = State.Ok;
            }
            else
            {
                storeSizeWidget.State = State.Invalid;
            }
        }
    }
}
