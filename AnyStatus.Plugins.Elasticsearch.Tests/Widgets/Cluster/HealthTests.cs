/*
Anystatus Elasticsearch plugin
Copyright 2019 Fatih Boy

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */
using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.Health;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Health;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Shared;
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
            var widget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var clusterHealthResponseMock = new Mock<ClusterHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            clusterHealthResponseMock.Setup(response => response.Status).Returns(Health.Green);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.HealthAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterHealthResponseMock.Object));

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
            var widget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var clusterHealthResponseMock = new Mock<ClusterHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            clusterHealthResponseMock.Setup(response => response.Status).Returns(Health.Yellow);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.HealthAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterHealthResponseMock.Object));

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
            var widget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var clusterHealthResponseMock = new Mock<ClusterHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterHealthResponseMock.Setup(response => response.IsValid).Returns(true);
            clusterHealthResponseMock.Setup(response => response.Status).Returns(Health.Red);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.HealthAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterHealthResponseMock.Object));

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
            var widget = new ClusterHealthWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var clusterHealthResponseMock = new Mock<ClusterHealthResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterHealthResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.HealthAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterHealthResponseMock.Object));

            var request = HealthCheckRequest.Create(widget);

            var handler = new ClusterHealthCheck(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, request.DataContext.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.HealthAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
