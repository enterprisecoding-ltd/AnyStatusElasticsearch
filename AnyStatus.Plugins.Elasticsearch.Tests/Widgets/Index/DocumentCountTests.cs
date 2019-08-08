using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Index.DocumentCount;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var clusterHealthWidget = new DocumentCountWidget { MasterIp = "127.0.0.1", MasterPort = 9200 };
            var request = MetricQueryRequest.Create(clusterHealthWidget);
            var metricQuery = new DocumentCountMetricQuery();

            await metricQuery.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }
    }
}
