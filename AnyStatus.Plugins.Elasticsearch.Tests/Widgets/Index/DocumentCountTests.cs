﻿/*
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
        private const string indexName = "index1";

        [TestMethod]
        public async Task IndexDocumentCountShouldValid()
        {
            var widget = new DocumentCountWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = indexName };

            var indicesStatsMock = new Mock<IndicesStats>();
            var indexCountResponseMock = new Mock<IndicesStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            var indicesDisctionaryMock = new Dictionary<string, IndicesStats>{ { indexName, indicesStatsMock.Object} };

            indicesStatsMock.Setup(response => response.Primaries.Documents.Count).Returns(500);
            indexCountResponseMock.SetupGet(response => response.Indices).Returns(indicesDisctionaryMock);
            indexCountResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexStatsAsync(indexName, "indices.*.primaries.docs.count", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexCountResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new DocumentCountMetricQuery(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual((long)500, widget.Value);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexStatsAsync(indexName, "indices.*.primaries.docs.count", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task IndexDocumentCountShouldInvalidWhenResponseIsInvalid()
        {
            var widget = new DocumentCountWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = indexName };

            var indexCountResponseMock = new Mock<IndicesStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            indexCountResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexStatsAsync(indexName, "indices.*.primaries.docs.count", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexCountResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new DocumentCountMetricQuery(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexStatsAsync(indexName, "indices.*.primaries.docs.count", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
