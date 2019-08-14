using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient;
using AnyStatus.Plugins.Elasticsearch.Shared;

namespace AnyStatus.Plugins.Elasticsearch.Helpers
{
    public static class ElasticsearchHelper
    {
        public static ElasticsearchSimpleClient GetElasticClient(IElasticsearchWidget elasticsearchWidget)
        {
            var client = new ElasticsearchSimpleClient(elasticsearchWidget.NodeUris, elasticsearchWidget.Username, elasticsearchWidget.Password, elasticsearchWidget.TrustCertificate);
            return client;
        }
    }
}
