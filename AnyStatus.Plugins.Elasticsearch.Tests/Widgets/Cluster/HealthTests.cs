using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.Health;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Cluster
{
    [TestClass]
    public class HealthTests
    {
        [TestMethod]
        public async Task ClusterHealth()
        {
            var clusterHealthWidget = new ClusterHealthWidget { MasterIp = "127.0.0.1", MasterPort = 9200 };
            var request = HealthCheckRequest.Create(clusterHealthWidget);
            var handler = new ClusterHealthCheck();

            await handler.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }
    }
}
