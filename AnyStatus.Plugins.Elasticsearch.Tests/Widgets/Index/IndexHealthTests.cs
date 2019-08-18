using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cluster;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Health;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Shared;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using AnyStatus.Plugins.Elasticsearch.Index.IndexHealth;
using AnyStatus.Plugins.Elasticsearch.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Index
{
    [TestClass]
    public class IndexHealthTests
    {
        private const string indexName = "test";
        [TestMethod]
        public async Task IndexHealthShouldValidWhenClusterStatusGreen()
        {
            var widget = new IndexHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = indexName };

            var indexHealthResponseMock = new Mock<IndexHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            indexHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            indexHealthResponseMock.Setup(response => response.Status).Returns(Health.Green);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexHealthAsync(indexName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexHealthResponseMock.Object));

            var request = HealthCheckRequest.Create(widget);

            var handler = new IndexHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexHealthAsync(indexName, It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task IndexHealthShouldPartiallySucceededWhenClusterStatusYellow()
        {
            var widget = new IndexHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = indexName };

            var indexHealthResponseMock = new Mock<IndexHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            indexHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            indexHealthResponseMock.Setup(response => response.Status).Returns(Health.Yellow);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexHealthAsync(indexName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexHealthResponseMock.Object));

            var request = HealthCheckRequest.Create(widget);

            var handler = new IndexHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.PartiallySucceeded, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexHealthAsync(indexName, It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task IndexHealthShouldFailedWhenClusterStatusRed()
        {
            var widget = new IndexHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = indexName };

            var indexHealthResponseMock = new Mock<IndexHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            indexHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            indexHealthResponseMock.Setup(response => response.Status).Returns(Health.Red);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexHealthAsync(indexName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexHealthResponseMock.Object));

            var request = HealthCheckRequest.Create(widget);

            var handler = new IndexHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Failed, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexHealthAsync(indexName, It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task IndexHealthShouldInvalidWhenResponseIsInvalid()
        {
            var widget = new IndexHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = indexName };

            var indexHealthResponseMock = new Mock<IndexHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            indexHealthResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexHealthAsync(indexName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexHealthResponseMock.Object));

            var request = HealthCheckRequest.Create(widget);

            var handler = new IndexHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexHealthAsync(indexName, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
