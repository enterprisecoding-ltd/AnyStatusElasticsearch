using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Node.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Node
{
    [TestClass]
    public class CPUUsageTests
    {
        [TestMethod]
        public async Task NodeCPUUsageTest()
        {
            var widget = new NodeCPUUsageWidget { MasterIp = "127.0.0.1", MasterPort = 9200, NodeId = "es01" };

            var request = MetricQueryRequest.Create(widget);

            var handler = new NodeCPUUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }
    }
}
