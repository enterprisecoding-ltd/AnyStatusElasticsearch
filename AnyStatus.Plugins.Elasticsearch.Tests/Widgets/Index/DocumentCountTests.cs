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
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Index;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using AnyStatus.Plugins.Elasticsearch.Index.DocumentCount;
using AnyStatus.Plugins.Elasticsearch.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Index
{
    [TestClass]
    public class DocumentCountTests
    {
        [TestMethod]
        public async Task DocumentCountShouldValid()
        {
            var indexCountResponseMock = new Mock<IndexCountResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            indexCountResponseMock.Setup(response => response.Count).Returns(500);
            indexCountResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexDocsCountAsync("index1", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexCountResponseMock.Object));

            var widget = new DocumentCountWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = "index1" };

            var request = MetricQueryRequest.Create(widget);

            var handler = new DocumentCountMetricQuery(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual((long)500, widget.Value);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexDocsCountAsync("index1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task DocumentCountShouldInvalidWhenResponseIsInvalid()
        {
            var indexCountResponseMock = new Mock<IndexCountResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            indexCountResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexDocsCountAsync("index1", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexCountResponseMock.Object));

            var widget = new DocumentCountWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = "index1" };

            var request = MetricQueryRequest.Create(widget);

            var handler = new DocumentCountMetricQuery(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexDocsCountAsync("index1", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
