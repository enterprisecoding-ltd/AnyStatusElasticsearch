using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Index.DeletedDocumentCount
{
    public class DeletedDocumentCountMetricQuery : IMetricQuery<DeletedDocumentCountWidget>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        public DeletedDocumentCountMetricQuery() : this(new ElasticsearchHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal DeletedDocumentCountMetricQuery(ElasticsearchHelper elasticsearchHelper)
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
