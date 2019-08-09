using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.Health
{
    public class ClusterHealthCheck : ICheckHealth<ClusterHealthWidget>
    {
        public async Task Handle(HealthCheckRequest<ClusterHealthWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var client = ElasticsearchHelper.GetElasticClient(clusterHealthWidget);

            var clusterHealthResponse = await client.Cluster.HealthAsync(new ClusterHealthRequest(), cancellationToken);

            if (clusterHealthResponse.IsValid)
            {
                switch (clusterHealthResponse.Status)
                {
                    case global::Elasticsearch.Net.Health.Green:
                        clusterHealthWidget.State = State.Ok;
                        break;
                    case global::Elasticsearch.Net.Health.Yellow:
                        clusterHealthWidget.State = State.PartiallySucceeded;
                        break;
                    default:
                        clusterHealthWidget.State = State.Failed;
                        break;
                }
            }
            else {
                clusterHealthWidget.State = State.Invalid;
            }
        }
    }
}
