﻿using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using AnyStatus.Plugins.Elasticsearch.Node.FileSystemUsage;
using AnyStatus.Plugins.Elasticsearch.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Node
{
    [TestClass]
    public class FileSystemUsageTests
    {
        [TestMethod]
        public async Task NodeFileSystemUsageShouldValidPercentageUsed()
        {
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageUsed,
                NodeId = "es01"
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual(70, widget.Progress);
            Assert.AreEqual($"Used 70%{Environment.NewLine}70.0 Bytes used out of 100.0 Bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldValidPercentageRemaining()
        {
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageRemaining,
                NodeId = "es01",
                ErrorPercentage = 20
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual(30, widget.Progress);
            Assert.AreEqual($"Avaliable 30%{Environment.NewLine}30.0 Bytes available out of 100.0 Bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldInvalidWhenPercentageRemainingLessThenErrorPercentage()
        {
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageRemaining,
                NodeId = "es01",
                ErrorPercentage = 40
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Failed, widget.State);
            Assert.AreEqual(30, widget.Progress);
            Assert.AreEqual($"Avaliable 30%{Environment.NewLine}30.0 Bytes available out of 100.0 Bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldInvalidWhenPercentageUsedHighThenErrorPercentage()
        {
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] {
                new List<string>(),
                string.Empty,
                string.Empty,
                false
            });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageUsed,
                NodeId = "es01",
                ErrorPercentage = 20
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Failed, widget.State);
            Assert.AreEqual(70, widget.Progress);
            Assert.AreEqual($"Used 70%{Environment.NewLine}70.0 Bytes used out of 100.0 Bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldInvalidWhenResponseIsInvalid()
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

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var widget = new FileSystemUsageWidget {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                NodeId = "es01"
            };

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", "es01", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
