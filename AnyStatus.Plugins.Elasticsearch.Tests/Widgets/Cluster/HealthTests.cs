using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.Health;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Health;
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
    public class HealthTests
    {
        [TestMethod]
        public async Task ClusterHealthShouldValidWhenClusterStatusGreen()
        {
            var clusterHealthResponseMock = new Mock<ClusterHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            clusterHealthResponseMock.Setup(response => response.Status).Returns(Health.Green);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.HealthAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterHealthResponseMock.Object));

            var widget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = HealthCheckRequest.Create(widget);

            var handler = new ClusterHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.HealthAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterHealthShouldPartiallySucceededWhenClusterStatusYellow()
        {
            var clusterHealthResponseMock = new Mock<ClusterHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            clusterHealthResponseMock.Setup(response => response.Status).Returns(Health.Yellow);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.HealthAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterHealthResponseMock.Object));

            var widget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = HealthCheckRequest.Create(widget);

            var handler = new ClusterHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.PartiallySucceeded, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.HealthAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterHealthShouldFailedWhenClusterStatusRed()
        {
            var clusterHealthResponseMock = new Mock<ClusterHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            clusterHealthResponseMock.Setup(response => response.Status).Returns(Health.Red);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.HealthAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterHealthResponseMock.Object));

            var widget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = HealthCheckRequest.Create(widget);

            var handler = new ClusterHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Failed, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.HealthAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterHealthShouldInvalidWhenResponseIsInvalid()
        {
            var clusterHealthResponseMock = new Mock<ClusterHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterHealthResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.HealthAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterHealthResponseMock.Object));

            var widget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var request = HealthCheckRequest.Create(widget);

            var handler = new ClusterHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.HealthAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
