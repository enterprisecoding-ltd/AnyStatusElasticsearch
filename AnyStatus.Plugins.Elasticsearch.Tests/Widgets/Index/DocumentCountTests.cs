using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Index.DocumentCount;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Index
{
    [TestClass]
    public class DocumentCountTests
    {
        [TestMethod]
        public async Task DocumentCount()
        {
            var clusterHealthWidget = new DocumentCountWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };
            var request = MetricQueryRequest.Create(clusterHealthWidget);
            var metricQuery = new DocumentCountMetricQuery();

            await metricQuery.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }

        [TestMethod]
        public async Task DocumentCountSecure()
        {
            var clusterHealthWidget = new DocumentCountWidget
            {
                NodeUris = new List<string>() { "https://127.0.0.1:9200" },
                TrustCertificate = true,
                UseBasicAuthentication = true,
                Username = "elastic",
                Password = "changeme"
            };
            var request = MetricQueryRequest.Create(clusterHealthWidget);
            var metricQuery = new DocumentCountMetricQuery();

            await metricQuery.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }
    }
}
