using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.Ram;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Cluster
{
    [TestClass]
    public class RamUsageTests
    {
        [TestMethod]
        public async Task ClusterRamUsageTest()
        {
            var widget = new ClusterRamUsageWidget { MasterIp = "127.0.0.1", MasterPort = 9200 };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterRamUsageHandler();

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);

            Assert.IsTrue(widget.Value > 0);
        }
    }
}
