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
using AnyStatus.API;
using AnyStatus.API.Common.Services;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cat;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Shared;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using AnyStatus.Plugins.Elasticsearch.Index.DocumentCount;
using AnyStatus.Plugins.Elasticsearch.Index.IndexHealth;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Index.IndexCount
{
    public class IndexCountMetricQuery : IMetricQuery<IndexCountWidget>
    {
        /// <summary>
        /// Elasticsearch Helper to retrieve elastic client
        /// </summary>
        private readonly ElasticsearchHelper elasticsearchHelper;

        private readonly IUiAction uiAction;

        public IndexCountMetricQuery(IUiAction uiAction) : this(new ElasticsearchHelper()) {
            this.uiAction = uiAction;
        }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="elasticsearchHelper">Elasticsearch Helper class instance to use</param>
        internal IndexCountMetricQuery(ElasticsearchHelper elasticsearchHelper)
        {
            this.elasticsearchHelper = elasticsearchHelper;
        }

        public async Task Handle(MetricQueryRequest<IndexCountWidget> request, CancellationToken cancellationToken)
        {
            var indexCountWidget = request.DataContext;

            var client = elasticsearchHelper.GetElasticClient(indexCountWidget);

            var indexCountResponse = await client.IndexListAsync(cancellationToken);

            if (indexCountResponse.IsValid)
            {
                request.DataContext.Value = indexCountResponse.Indices.Length;

                if (uiAction!=null) //uiAction is null on unit tests...
                {
                    CollectionSynchronizer<IndexEntry, Item> synchronizer = null;
                    switch (indexCountWidget.IndexDetails)
                    {
                        case IndexDetail.None:
                            synchronizer = GetNoneSynchronizer(request);
                            break;
                        case IndexDetail.Health:
                            synchronizer = GetHealthSynchronizer(request);
                            break;
                        case IndexDetail.DocumentsCount:
                            synchronizer = GetDocumentsCountSynchronizer(request);
                            break;
                    }

                    uiAction.Invoke(() => synchronizer.Synchronize(indexCountResponse.Indices, request.DataContext.Items)); 
                }

                request.DataContext.State = State.Ok;
            }
            else
            {
                indexCountWidget.State = State.Invalid;
            }
        }

        private static CollectionSynchronizer<IndexEntry, Item> GetNoneSynchronizer(MetricQueryRequest<IndexCountWidget> request)
        {
            return new CollectionSynchronizer<IndexEntry, Item>
            {
                Compare = (indexEntry, item) => false,
                Remove = item => request.DataContext.Remove(item),
                Update = (indexEntry, item) => item.Name = indexEntry.Index,
                Add = indexEntry => { }
            };
        }

        private static CollectionSynchronizer<IndexEntry, Item> GetHealthSynchronizer(MetricQueryRequest<IndexCountWidget> request)
        {
            return new CollectionSynchronizer<IndexEntry, Item>
            {
                Compare = (indexEntry, item) => item is IndexHealthWidget indexHealthWidget && indexEntry.Uuid == indexHealthWidget.IndexUuid,
                Remove = item => request.DataContext.Remove(item),
                Update = (indexEntry, item) => ((IndexHealthWidget)item).IndexUuid = indexEntry.Uuid,
                Add = indexEntry => request.DataContext.Add(new IndexHealthWidget
                {
                    Name = indexEntry.Index,
                    IndexName = indexEntry.Index,
                    IndexUuid = indexEntry.Uuid,
                    NodeUris = request.DataContext.NodeUris,
                    UseBasicAuthentication = request.DataContext.UseBasicAuthentication,
                    Username = request.DataContext.Username,
                    Password = request.DataContext.Password,
                    TrustCertificate = request.DataContext.TrustCertificate,
                    State = indexEntry.Health == Health.Green ? State.Ok : indexEntry.Health == Health.Yellow ? State.PartiallySucceeded : State.Failed,
                    Interval = 0 //bypass scheduler
                })
            };
        }

        private static CollectionSynchronizer<IndexEntry, Item> GetDocumentsCountSynchronizer(MetricQueryRequest<IndexCountWidget> request)
        {
            return new CollectionSynchronizer<IndexEntry, Item>
            {
                Compare = (indexEntry, item) => item is DocumentCountWidget documentCountWidget && indexEntry.Uuid == documentCountWidget.IndexUuid,
                Remove = item => request.DataContext.Remove(item),
                Update = (indexEntry, item) => ((DocumentCountWidget)item).IndexUuid = indexEntry.Uuid,
                Add = indexEntry => request.DataContext.Add(new DocumentCountWidget
                {
                    Name = indexEntry.Index,
                    IndexName = indexEntry.Index,
                    IndexUuid = indexEntry.Uuid,
                    NodeUris = request.DataContext.NodeUris,
                    UseBasicAuthentication = request.DataContext.UseBasicAuthentication,
                    Username = request.DataContext.Username,
                    Password = request.DataContext.Password,
                    TrustCertificate = request.DataContext.TrustCertificate,
                    Interval = 0 //bypass scheduler
                })
            };
        }
    }
}
