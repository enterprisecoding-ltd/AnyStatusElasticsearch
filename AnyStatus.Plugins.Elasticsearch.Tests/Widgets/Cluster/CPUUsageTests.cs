using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.CPU;
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
    public class CPUUsageTests
    {
        [TestMethod]
        public async Task ClusterCPUUsageShouldValid()
        {
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterStatsResponseMock.Setup(response => response.Nodes.Process.Cpu.Percent).Returns(50);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.process.cpu.percent", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new ClusterCPUUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterCPUUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual(50, widget.Value);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.process.cpu.percent", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterCPUUsageShouldInvalidWhenResponseIsInvalid()
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

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.process.cpu.percent", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new ClusterCPUUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = MetricQueryRequest.Create(widget);

            var handler = new ClusterCPUUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>())  , Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.process.cpu.percent", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
