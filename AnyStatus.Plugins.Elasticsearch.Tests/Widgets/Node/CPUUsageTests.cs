using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Node.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Node
{
    [TestClass]
    public class CPUUsageTests
    {
        [TestMethod]
        public async Task NodeCPUUsage()
        {
            var widget = new NodeCPUUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, NodeId = "es01" };

            var request = MetricQueryRequest.Create(widget);

            var handler = new NodeCPUUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }

        [TestMethod]
        public async Task NodeCPUUsageSecure()
        {
            var widget = new NodeCPUUsageWidget {
                NodeUris = new List<string>() { "https://127.0.0.1:9200" },
                TrustCertificate = true,
                UseBasicAuthentication = true,
                Username = "elastic",
                Password = "changeme",
                NodeId = "es01"
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new NodeCPUUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }
    }
}
