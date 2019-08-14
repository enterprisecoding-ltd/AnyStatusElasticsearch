using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.Ram;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using AnyStatus.Plugins.Elasticsearch.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Cluster
{
    [TestClass]
    public class RamUsageTests
    {
        [TestMethod]
        public async Task ClusterRamUsageShouldValid()
        {
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterStatsResponseMock.Setup(response => response.Nodes.OperatingSystem.Memory.UsedPercent).Returns(50);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.os.mem.used_percent", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new ClusterRamUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterRamUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual(50, widget.Value);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.os.mem.used_percent", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterRamUsageShouldInvalidWhenResponseIsInvalid()
        {
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.os.mem.used_percent", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new ClusterRamUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterRamUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.os.mem.used_percent", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
