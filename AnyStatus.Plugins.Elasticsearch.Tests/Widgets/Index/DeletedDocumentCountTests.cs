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
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Index;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using AnyStatus.Plugins.Elasticsearch.Index.IndexDeletedDocumentCount;
using AnyStatus.Plugins.Elasticsearch.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Index
{
    [TestClass]
    public class DeletedDocumentCountTests
    {
        private const string indexName = "index1";

        [TestMethod]
        public async Task IndexDeletedDocumentCountShouldValid()
        {
            var widget = new IndexDeletedDocumentCountWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = indexName };

            var indicesStatsMock = new Mock<IndicesStats>();
            var indexCountResponseMock = new Mock<IndicesStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            var indicesDisctionaryMock = new Dictionary<string, IndicesStats>{ { indexName, indicesStatsMock.Object} };

            indicesStatsMock.Setup(response => response.Primaries.Documents.Deleted).Returns(500);
            indexCountResponseMock.SetupGet(response => response.Indices).Returns(indicesDisctionaryMock);
            indexCountResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexStatsAsync(indexName, "indices.*.primaries.docs.deleted", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexCountResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new IndexDeletedDocumentCountMetricQuery(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual((long)500, widget.Value);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexStatsAsync(indexName, "indices.*.primaries.docs.deleted", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task IndexDeletedDocumentCountShouldInvalidWhenResponseIsInvalid()
        {
            var widget = new IndexDeletedDocumentCountWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" }, IndexName = indexName };

            var indexCountResponseMock = new Mock<IndicesStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            indexCountResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.IndexStatsAsync(indexName, "indices.*.primaries.docs.deleted", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(indexCountResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new IndexDeletedDocumentCountMetricQuery(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.IndexStatsAsync(indexName, "indices.*.primaries.docs.deleted", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
