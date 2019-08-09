using System.Collections.Generic;

namespace AnyStatus.Plugins.Elasticsearch.Shared
{
    public interface IElasticsearchWidget
    {
        List<string> NodeUris { get; set; }
        bool UseBasicAuthentication { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool TrustCertificate { get; set; }
    }
}