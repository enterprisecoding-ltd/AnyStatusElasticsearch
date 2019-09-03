/*
Anystatus Elasticsearch plugin
Copyright 2019 Fatih Boy

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */
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
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cat;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cluster;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Health;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Index;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Stats;
using AnyStatus.Plugins.Elasticsearch.Shared;

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

        public ElasticsearchSimpleClient(IElasticsearchWidget elasticsearchWidget)
        {
            httpClients = new List<HttpClient>(elasticsearchWidget.NodeUris.Count());
            foreach (var uri in elasticsearchWidget.NodeUris)
            {
                var httpClient = new HttpClient { BaseAddress = new Uri(uri) };
                if (elasticsearchWidget.UseBasicAuthentication 
                    && !string.IsNullOrWhiteSpace(elasticsearchWidget.Username) 
                    && !string.IsNullOrWhiteSpace(elasticsearchWidget.Password))
                {
                    var authenticationHeaderBytes = Encoding.UTF8.GetBytes($"{elasticsearchWidget.Username}:{elasticsearchWidget.Password}");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationHeaderBytes));
                }

                httpClients.Add(httpClient);
            }

            if (elasticsearchWidget.TrustCertificate)
            {
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidation;
            }
        }

        /// <summary>
        /// Retrieves cluster heaşth status
        /// </summary>
        /// <param name="cancellationToken">Token to cancel Elasticsearch request</param>
        /// <returns>Cluster health status</returns>
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

        public virtual async Task<IndexListResponse> IndexListAsync(CancellationToken cancellationToken)
        {
            IndexListResponse result;
            try
            {
                var responseMessage = await GetAsync($"/_cat/indices?format=json", null, cancellationToken);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = new IndexListResponse
                {
                    Indices = JsonConvert.DeserializeObject<IndexEntry[]>(response),
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                result = new IndexListResponse { IsValid = false, OriginalException = ex };
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
        public virtual async Task<IndicesStatsResponse> IndexStatsAsync(string indexName, string filterPath, CancellationToken cancellationToken)
        {
            IndicesStatsResponse result;
            try
            {
                HttpResponseMessage  responseMessage = await GetAsync($"/{indexName}/_stats", filterPath, cancellationToken);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<IndicesStatsResponse>(response);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result = new IndicesStatsResponse { IsValid = false, OriginalException = ex };
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
