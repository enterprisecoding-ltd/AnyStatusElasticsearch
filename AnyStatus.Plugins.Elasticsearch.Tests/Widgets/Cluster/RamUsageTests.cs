using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.Ram;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Cluster
{
    [TestClass]
    public class RamUsageTests
    {
        [TestMethod]
        public async Task ClusterRamUsage()
        {
            var widget = new ClusterRamUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterRamUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }

        [TestMethod]
        public async Task ClusterRamUsageSecure()
        {
            var widget = new ClusterRamUsageWidget {
                NodeUris = new List<string>() { "https://127.0.0.1:9200" },
                TrustCertificate = true,
                UseBasicAuthentication = true,
                Username = "elastic",
                Password = "changeme"
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterRamUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }
    }
}
