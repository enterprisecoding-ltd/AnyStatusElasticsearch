using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Node.StoreSize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Node
{
    [TestClass]
    public class StoreSizeTests
    {
        [TestMethod]
        public async Task StoreSize()
        {
            var storeSizeWidget = new StoreSizeWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, NodeId = "es01" };
            var request = MetricQueryRequest.Create(storeSizeWidget);
            var metricQuery = new StoreSizeMetricQuery();

            await metricQuery.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }

        [TestMethod]
        public async Task StoreSizeSecure()
        {
            var storeSizeWidget = new StoreSizeWidget
            {
                NodeUris = new List<string>() { "https://127.0.0.1:9200" },
                UseBasicAuthentication = true,
                Username = "elastic",
                Password = "changeme",
                TrustCertificate = true,
                NodeId = "es01"
            };
            var request = MetricQueryRequest.Create(storeSizeWidget);
            var metricQuery = new StoreSizeMetricQuery();

            await metricQuery.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }
    }
}
