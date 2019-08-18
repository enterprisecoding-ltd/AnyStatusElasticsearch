/*
Anystatus Elasticsearch plugin
Copyright (C) 2019  Enterprisecoding (Fatih Boy)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
