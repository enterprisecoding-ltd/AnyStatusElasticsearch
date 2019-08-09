using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Index.DeletedDocumentCount
{
    public class DeletedDocumentCountMetricQuery : IMetricQuery<DeletedDocumentCountWidget>
    {
        public async Task Handle(MetricQueryRequest<DeletedDocumentCountWidget> request, CancellationToken cancellationToken)
        {
            var deleteDocumentCountWidget = request.DataContext;

            var client = ElasticsearchHelper.GetElasticClient(deleteDocumentCountWidget);

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest { FilterPath = new[] { "indices.docs.deleted" } }, cancellationToken);

            if (clusterStatsResponse.IsValid)
            {
                request.DataContext.Value = clusterStatsResponse.Indices.Documents.Deleted;
                request.DataContext.State = State.Ok;
            }
            else
            {
                deleteDocumentCountWidget.State = State.Invalid;
            }
        }
    }
}
