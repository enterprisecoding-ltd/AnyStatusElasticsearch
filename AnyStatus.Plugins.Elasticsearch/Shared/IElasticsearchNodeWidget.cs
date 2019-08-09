namespace AnyStatus.Plugins.Elasticsearch.Shared
{
    public interface IElasticsearchNodeWidget: IElasticsearchWidget
    {
        string NodeId { get; set; }
    }
}