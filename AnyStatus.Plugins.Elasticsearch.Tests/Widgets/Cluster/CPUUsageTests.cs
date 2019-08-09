using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Cluster
{
    [TestClass]
    public class CPUUsageTests
    {
        [TestMethod]
        public async Task ClusterCPUUsageTest()
        {
            var widget = new ClusterCPUUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterCPUUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }

        [TestMethod]
        public async Task ClusterCPUUsageTestSecure()
        {
            var widget = new ClusterCPUUsageWidget
            {
                NodeUris = new List<string>() { "https://127.0.0.1:9200" },
                TrustCertificate = true,
                UseBasicAuthentication = true,
                Username = "elastic",
                Password = "changeme"
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterCPUUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }


        [TestMethod]
        public async Task ClusterCPUUsageTestSecureMultiNode()
        {
            var widget = new ClusterCPUUsageWidget
            {
                NodeUris = new List<string>() { "https://127.0.0.1:9200", "https://127.0.0.1:9201" },
                TrustCertificate = true,
                UseBasicAuthentication = true,
                Username = "elastic",
                Password = "changeme"
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterCPUUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }
    }
}
