﻿using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.FileSystemUsage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Cluster
{
    [TestClass]
    public class FileSystemUsageTests
    {
        [TestMethod]
        public async Task FileSystemUsage()
        {
            var fileSystemUsageTestsWidget = new FileSystemUsageWidget {
                MasterIp = "127.0.0.1",
                MasterPort = 9200,
                PercentageType = Shared.FileSystemPercentageType.PercentageUsed
            };
            var request = MetricQueryRequest.Create(fileSystemUsageTestsWidget);
            var metricQuery = new FileSystemUsageQuery();

            await metricQuery.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }
    }
}
