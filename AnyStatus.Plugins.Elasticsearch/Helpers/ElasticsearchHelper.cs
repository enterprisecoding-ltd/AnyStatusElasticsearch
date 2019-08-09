using AnyStatus.Plugins.Elasticsearch.Shared;
using Elasticsearch.Net;
using Nest;
using System;
using System.Linq;

namespace AnyStatus.Plugins.Elasticsearch.Helpers
{
    public static class ElasticsearchHelper
    {
        public static ElasticClient GetElasticClient(IElasticsearchWidget elasticsearchWidget) {
            var connectionPool = new StaticConnectionPool(elasticsearchWidget.NodeUris.Select(nodeUri => new Uri(nodeUri)).ToArray());

            var settings = new ConnectionSettings(connectionPool);
            if (elasticsearchWidget.UseBasicAuthentication)
            {
                settings.BasicAuthentication(elasticsearchWidget.Username, elasticsearchWidget.Password); 
            }

            if (elasticsearchWidget.TrustCertificate)
            {
                settings.ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
            }

            return new ElasticClient(settings);
        }
    }
}
