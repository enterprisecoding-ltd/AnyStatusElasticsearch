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
using AnyStatus.API;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Index.IndexHealth
{
    public class IndexHealthCheck : ICheckHealth<IndexHealthWidget>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        public IndexHealthCheck() : this(new ElasticsearchHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal IndexHealthCheck(ElasticsearchHelper elasticsearchHelper)
        {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(HealthCheckRequest<IndexHealthWidget> request, CancellationToken cancellationToken)
        {
            var indexHealthWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(indexHealthWidget);

            var indexHealthResponse = await client.IndexHealthAsync(indexHealthWidget.IndexName, cancellationToken);

            if (indexHealthResponse.IsValid)
            {
                switch (indexHealthResponse.Status)
                {
                    case ElasticsearchClient.Objects.Shared.Health.Green:
                        indexHealthWidget.State = State.Ok;
                        break;
                    case ElasticsearchClient.Objects.Shared.Health.Yellow:
                        indexHealthWidget.State = State.PartiallySucceeded;
                        break;
                    default:
                        indexHealthWidget.State = State.Failed;
                        break;
                }
            }
            else
            {
                indexHealthWidget.State = State.Invalid;
            }
        }
    }
}