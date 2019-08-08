using AnyStatus.API;
using Elasticsearch.Net;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Index.DocumentCount
{
    public class DocumentCountMetricQuery : IMetricQuery<DocumentCountWidget>
    {
        public async Task Handle(MetricQueryRequest<DocumentCountWidget> request, CancellationToken cancellationToken)
        {
            var documentCountWidget = request.DataContext;

            var connectionPool = new SingleNodeConnectionPool(new Uri($"http://{documentCountWidget.MasterIp}:{documentCountWidget.MasterPort}"));

            var settings = new ConnectionSettings(connectionPool);
            var client = new ElasticClient(settings);

            var clusterStatsResponse = await client.Cluster.StatsAsync(new ClusterStatsRequest { FilterPath = new[] { "indices.docs.count" } }, cancellationToken);

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
