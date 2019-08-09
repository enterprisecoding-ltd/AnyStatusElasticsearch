using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.Health;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
            var clusterHealthWidget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };
            var request = HealthCheckRequest.Create(clusterHealthWidget);
            var handler = new ClusterHealthCheck();

            await handler.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }

        [TestMethod]
        public async Task ClusterHealthSecure()
        {
            var clusterHealthWidget = new ClusterHealthWidget {
                NodeUris = new List<string>() { "https://127.0.0.1:9200" },
                TrustCertificate = true,
                UseBasicAuthentication = true,
                Username = "elastic",
                Password = "changeme"
            };
            var request = HealthCheckRequest.Create(clusterHealthWidget);
            var handler = new ClusterHealthCheck();

            await handler.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }
    }
}
