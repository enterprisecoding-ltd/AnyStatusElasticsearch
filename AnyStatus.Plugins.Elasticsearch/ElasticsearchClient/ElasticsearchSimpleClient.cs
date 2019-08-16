/*
Anystatus Elasticsearch plugin
Copyright (C) 2019  Enterprisecoding (Fatih Boy)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cluster;
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
    /// <summary>
    /// Simple Elasticsearch client
    /// </summary>
    /// <remarks>
    /// This class only implements limited Elasticsearch client functionality required 
    /// by Anystatus elasticsearch plugin.
    /// </remarks>
    public class ElasticsearchSimpleClient
    {
        /// <summary>
        /// List of http clients used to connect Elasticsearch cluster members
        /// </summary>
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

        public virtual async Task<ClusterHealthResponse> HealthAsync(CancellationToken cancellationToken)
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

        public virtual async Task<IndexHealthResponse> IndexHealthAsync(string indexName, CancellationToken cancellationToken)
        {
            IndexHealthResponse result;
            try
            {
                var responseMessage = await GetAsync($"/_cluster/health/{indexName}", null, cancellationToken);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<IndexHealthResponse>(response);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result = new IndexHealthResponse { IsValid = false, OriginalException = ex };
            }

            return result;
        }

        private bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public virtual async Task<ClusterStatsResponse> StatsAsync(string filterPath, CancellationToken cancellationToken) {
            return await StatsAsync(filterPath, null, cancellationToken);
        }

        public virtual async Task<ClusterStatsResponse> StatsAsync(string filterPath, string nodeId, CancellationToken cancellationToken)
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
