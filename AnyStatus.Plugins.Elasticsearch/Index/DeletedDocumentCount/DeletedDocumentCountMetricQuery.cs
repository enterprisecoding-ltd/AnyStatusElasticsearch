using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Index.DeletedDocumentCount
{
    public class DeletedDocumentCountMetricQuery : IMetricQuery<DeletedDocumentCountWidget>
    {
        private readonly ElasticsearchHelper elasticsearchHelper;

        public DeletedDocumentCountMetricQuery() : this(new ElasticsearchHelper()) { }

        public DeletedDocumentCountMetricQuery(ElasticsearchHelper elasticsearchHelper)
        {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(MetricQueryRequest<DeletedDocumentCountWidget> request, CancellationToken cancellationToken)
        {
            var deleteDocumentCountWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(deleteDocumentCountWidget);

            var clusterStatsResponse = await client.StatsAsync("indices.docs.deleted", cancellationToken);

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
