using AnyStatus.API;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using AnyStatus.Plugins.Elasticsearch.Helpers;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.StoreSize
{
    public class StoreSizeMetricQuery : IMetricQuery<StoreSizeWidget>
    {
        public async Task Handle(MetricQueryRequest<StoreSizeWidget> request, CancellationToken cancellationToken)
        {
            var storeSizeWidget = request.DataContext;

            var client = ElasticsearchHelper.GetElasticClient(storeSizeWidget);

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest { FilterPath = new[] { "indices.store.size_in_bytes" } }, cancellationToken);

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
