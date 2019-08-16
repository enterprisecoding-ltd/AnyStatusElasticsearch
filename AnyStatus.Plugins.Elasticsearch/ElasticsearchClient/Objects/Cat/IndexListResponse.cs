namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cat
{
    public class IndexListResponse : ElasticsearchResponseBase
    {
        public virtual string[] Indices { get; internal set; }
    }
}
