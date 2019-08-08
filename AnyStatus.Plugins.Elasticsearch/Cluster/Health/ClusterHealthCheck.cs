using AnyStatus.API;
using Elasticsearch.Net;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.Health
{
    public class ClusterHealthCheck : ICheckHealth<ClusterHealthWidget>
    {
        public async Task Handle(HealthCheckRequest<ClusterHealthWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var connectionPool = new SingleNodeConnectionPool(new Uri($"http://{clusterHealthWidget.MasterIp}:{clusterHealthWidget.MasterPort}"));
            
            var settings = new ConnectionSettings(connectionPool);
            var client = new ElasticClient(settings);

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
