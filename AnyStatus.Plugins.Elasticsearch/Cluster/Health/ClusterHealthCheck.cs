﻿using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Cluster.Health
{
    public class ClusterHealthCheck : ICheckHealth<ClusterHealthWidget>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        public ClusterHealthCheck() : this(new ElasticsearchHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal ClusterHealthCheck(ElasticsearchHelper elasticsearchHelper)
        {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(HealthCheckRequest<ClusterHealthWidget> request, CancellationToken cancellationToken)
        {
            var clusterHealthWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(clusterHealthWidget);

            var clusterHealthResponse = await client.HealthAsync(cancellationToken);

            if (clusterHealthResponse.IsValid)
            {
                switch (clusterHealthResponse.Status)
                {
                    case ElasticsearchClient.Objects.Health.Health.Green:
                        clusterHealthWidget.State = State.Ok;
                        break;
                    case ElasticsearchClient.Objects.Health.Health.Yellow:
                        clusterHealthWidget.State = State.PartiallySucceeded;
                        break;
                    default:
                        clusterHealthWidget.State = State.Failed;
                        break;
                }
            }
            else {
                clusterHealthWidget.State = State.Invalid;
            }
        }
    }
}
