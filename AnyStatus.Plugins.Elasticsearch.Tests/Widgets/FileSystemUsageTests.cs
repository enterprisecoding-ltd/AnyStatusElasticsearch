﻿/*
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
using AnyStatus.Plugins.Elasticsearch.FileSystemUsage;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using AnyStatus.Plugins.Elasticsearch.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets
{
    [TestClass]
    public class FileSystemUsageTests
    {
        private const string nodeId = "es01";

        [TestMethod]
        public async Task ClusterFileSystemUsageShouldValidPercentageUsed()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageUsed
            };

            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual(70, widget.Progress);
            Assert.AreEqual(70, widget.Value);
            Assert.AreEqual($"Used 70%{Environment.NewLine}70 bytes used out of 100 bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterFileSystemUsageShouldValidPercentageRemaining()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageRemaining,
                ErrorPercentage = 20
            };

            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual(30, widget.Progress);
            Assert.AreEqual(30, widget.Value);
            Assert.AreEqual($"Avaliable 30%{Environment.NewLine}30 bytes available out of 100 bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterFileSystemUsageShouldInvalidWhenPercentageRemainingLessThenErrorPercentage()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageRemaining,
                ErrorPercentage = 40

            };
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Failed, widget.State);
            Assert.AreEqual(30, widget.Progress);
            Assert.AreEqual(30, widget.Value);
            Assert.AreEqual($"Avaliable 30%{Environment.NewLine}30 bytes available out of 100 bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterFileSystemUsageShouldInvalidWhenPercentageUsedHighThenErrorPercentage()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageUsed,
                ErrorPercentage = 20

            };
            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Failed, widget.State);
            Assert.AreEqual(70, widget.Progress);
            Assert.AreEqual(70, widget.Value);
            Assert.AreEqual($"Used 70%{Environment.NewLine}70 bytes used out of 100 bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task ClusterFileSystemUsageShouldInvalidWhenResponseIsInvalid()
        {
            var widget = new FileSystemUsageWidget { NodeUris = new List<string>() { "http://127.0.0.1:9200" } };

            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldValidPercentageUsed()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageUsed,
                NodeId = nodeId
            };

            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual(70, widget.Progress);
            Assert.AreEqual(70, widget.Value);
            Assert.AreEqual($"Used 70%{Environment.NewLine}70 bytes used out of 100 bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldValidPercentageRemaining()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageRemaining,
                NodeId = nodeId,
                ErrorPercentage = 20
            };

            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual(30, widget.Progress);
            Assert.AreEqual(30, widget.Value);
            Assert.AreEqual($"Avaliable 30%{Environment.NewLine}30 bytes available out of 100 bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldInvalidWhenPercentageRemainingLessThenErrorPercentage()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageRemaining,
                NodeId = nodeId,
                ErrorPercentage = 40
            };

            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Failed, widget.State);
            Assert.AreEqual(30, widget.Progress);
            Assert.AreEqual(30, widget.Value);
            Assert.AreEqual($"Avaliable 30%{Environment.NewLine}30 bytes available out of 100 bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldInvalidWhenPercentageUsedHighThenErrorPercentage()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                PercentageType = FileSystemPercentageType.PercentageUsed,
                NodeId = nodeId,
                ErrorPercentage = 20
            };

            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.AvailableInBytes).Returns(30);
            clusterStatsResponseMock.Setup(response => response.Nodes.FileSystem.TotalInBytes).Returns(100);
            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(true);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Failed, widget.State);
            Assert.AreEqual(70, widget.Progress);
            Assert.AreEqual(70, widget.Value);
            Assert.AreEqual($"Used 70%{Environment.NewLine}70 bytes used out of 100 bytes", widget.Message);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NodeFileSystemUsageShouldInvalidWhenResponseIsInvalid()
        {
            var widget = new FileSystemUsageWidget
            {
                NodeUris = new List<string>() { "http://127.0.0.1:9200" },
                NodeId = nodeId
            };

            var clusterStatsResponseMock = new Mock<ClusterStatsResponse>();
            var elasticsearchHelperMock = new Mock<ElasticsearchHelper>();
            var elasticsearchSimpleClientMock = new Mock<ElasticsearchSimpleClient>(MockBehavior.Strict, new object[] { widget });

            clusterStatsResponseMock.Setup(response => response.IsValid).Returns(false);

            elasticsearchHelperMock.Setup(helper => helper.GetElasticClient(It.IsAny<IElasticsearchWidget>()))
                .Returns(elasticsearchSimpleClientMock.Object);

            elasticsearchSimpleClientMock.Setup(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(clusterStatsResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new FileSystemUsageHandler(elasticsearchHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            elasticsearchHelperMock.Verify(client => client.GetElasticClient(It.IsAny<IElasticsearchWidget>()), Times.Once());
            elasticsearchSimpleClientMock.Verify(client => client.StatsAsync("nodes.fs", nodeId, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
