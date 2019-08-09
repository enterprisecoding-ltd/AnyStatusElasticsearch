﻿using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Cluster.StoreSize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Tests.Widgets.Cluster
{
    [TestClass]
    public class StoreSizeTests
    {
        [TestMethod]
        public async Task StoreSize()
        {
            var storeSizeWidget = new StoreSizeWidget { MasterIp = "127.0.0.1", MasterPort = 9200 };
            var request = MetricQueryRequest.Create(storeSizeWidget);
            var metricQuery = new StoreSizeMetricQuery();

            await metricQuery.Handle(request, CancellationToken.None);

            Assert.AreSame(State.Ok, request.DataContext.State);
        }
    }
}