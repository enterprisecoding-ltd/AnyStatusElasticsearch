using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Node.Ram;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Node
{
    [TestClass]
    public class RamUsageTests
    {
        [TestMethod]
        public async Task NodeRamUsage()
        {
            var widget = new NodeRamUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, NodeId = "es01" };

            var request = MetricQueryRequest.Create(widget);

            var handler = new NodeRamUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }

        [TestMethod]
        public async Task NodeRamUsageSecure()
        {
            var widget = new NodeRamUsageWidget {
                NodeUris = new List<string>() { "https://127.0.0.1:9200" },
                UseBasicAuthentication = true,
                Username = "elastic",
                Password = "changeme",
                TrustCertificate = true,
                NodeId = "es01"
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new NodeRamUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }
    }
}
