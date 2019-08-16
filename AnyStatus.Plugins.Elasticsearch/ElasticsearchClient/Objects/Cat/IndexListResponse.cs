namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cat
{
    public class IndexListResponse : ElasticsearchResponseBase
    {
        public virtual IndexEntry[] Indices { get; internal set; }
    }
}
