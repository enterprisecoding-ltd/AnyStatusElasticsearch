using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Health;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.ElasticsearchClient
{
    public class ElasticsearchSimpleClient
    {
        private readonly List<HttpClient> httpClients;

        public ElasticsearchSimpleClient(IEnumerable<string> uris, string username = null, string password = null, bool trustCertificate = false)
        {
            httpClients = new List<HttpClient>(uris.Count());
            foreach (var uri in uris)
            {
                var httpClient = new HttpClient { BaseAddress = new Uri(uri) };
                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    var authenticationHeaderBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationHeaderBytes));
                }

                httpClients.Add(httpClient);
            }

            if (trustCertificate)
            {
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidation;
            }
        }

        internal async Task<ClusterHealthResponse> HealthAsync(CancellationToken cancellationToken)
        {
            ClusterHealthResponse result;
            try
            {
                HttpResponseMessage responseMessage = await GetAsync("/_cluster/health", null, cancellationToken);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<ClusterHealthResponse>(response);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result = new ClusterHealthResponse { IsValid = false, OriginalException = ex };
            }

            return result;
        }

        private bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public async Task<ClusterStatsResponse> StatsAsync(string filterPath, CancellationToken cancellationToken) {
            return await StatsAsync(filterPath, null, cancellationToken);
        }

        public async Task<ClusterStatsResponse> StatsAsync(string filterPath, string nodeId, CancellationToken cancellationToken)
        {
            ClusterStatsResponse result;
            try
            {
                HttpResponseMessage responseMessage;
                if (string.IsNullOrWhiteSpace(nodeId))
                {
                    responseMessage = await GetAsync("/_cluster/stats", filterPath, cancellationToken);
                }
                else
                {
                    responseMessage = await GetAsync($"/_cluster/stats/nodes/{nodeId}", filterPath, cancellationToken);
                }
                
                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<ClusterStatsResponse>(response);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result = new ClusterStatsResponse { IsValid = false, OriginalException = ex };
            }

            return result;
        }

        private async Task<HttpResponseMessage> GetAsync(string path, string filterPath, CancellationToken cancellationToken)
        {
            var requestUri = path;
            if (!string.IsNullOrWhiteSpace(filterPath))
            {
                requestUri = $"{requestUri}?filter_path={filterPath}";
            }

            Exception exception = null;
            foreach (var httpClient in httpClients)
            {
                try
                {
                    var responseMessage = await httpClient.GetAsync(requestUri, cancellationToken);

                    if (responseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        continue;
                    }

                    responseMessage.EnsureSuccessStatusCode();

                    return responseMessage;
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            throw exception ?? new InvalidOperationException("No clients available to service the request.");
        }

        public void Dispose()
        {
            ServicePointManager.ServerCertificateValidationCallback -= RemoteCertificateValidation;

            foreach (var httpClient in httpClients)
            {
                httpClient.Dispose();
            }
        }
    }
}
