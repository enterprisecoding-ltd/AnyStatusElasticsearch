using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Index.DocumentCount
{
    public class DocumentCountMetricQuery : IMetricQuery<DocumentCountWidget>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        public DocumentCountMetricQuery() : this(new ElasticsearchHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal DocumentCountMetricQuery(ElasticsearchHelper elasticsearchHelper)
        {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(MetricQueryRequest<DocumentCountWidget> request, CancellationToken cancellationToken)
        {
            var documentCountWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(documentCountWidget);

            var clusterStatsResponse = await client.StatsAsync("indices.docs.count", cancellationToken);

            if (clusterStatsResponse.IsValid)
            {
                request.DataContext.Value = clusterStatsResponse.Indices.Documents.Count;
                request.DataContext.State = State.Ok;
            }
            else
            {
                documentCountWidget.State = State.Invalid;
            }
        }
    }
}
