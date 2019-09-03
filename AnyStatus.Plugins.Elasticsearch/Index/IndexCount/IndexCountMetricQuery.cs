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
using AnyStatus.API.Common.Services;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Cat;
using AnyStatus.Plugins.Elasticsearch.ElasticsearchClient.Objects.Shared;
using AnyStatus.Plugins.Elasticsearch.Helpers;
using AnyStatus.Plugins.Elasticsearch.Index.IndexDeletedDocumentCount;
using AnyStatus.Plugins.Elasticsearch.Index.IndexDocumentCount;
using AnyStatus.Plugins.Elasticsearch.Index.IndexHealth;
using AnyStatus.Plugins.Elasticsearch.Index.IndexStoreSize;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Elasticsearch.Index.IndexCount
{
    public class IndexCountMetricQuery : IRequestHandler<MetricQueryRequest<IndexCountWidget>>
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
                        case IndexDetail.DeletedDocumentsCount:
                            synchronizer = GetDeletedDocumentsCountSynchronizer(request);
                            break;
                        case IndexDetail.TotalStoreSize:
                            synchronizer = GetTotalStoreSizeSynchronizer(request);
                            break;
                        case IndexDetail.PrimaryStoreSize:
                            synchronizer = GetPrimaryStoreSizeSynchronizer(request);
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
                Compare = (indexEntry, item) => item is IndexDocumentCountWidget documentCountWidget && indexEntry.Uuid == documentCountWidget.IndexUuid,
                Remove = item => request.DataContext.Remove(item),
                Update = (indexEntry, item) => ((IndexDocumentCountWidget)item).IndexUuid = indexEntry.Uuid,
                Add = indexEntry => request.DataContext.Add(new IndexDocumentCountWidget
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

        private static CollectionSynchronizer<IndexEntry, Item> GetDeletedDocumentsCountSynchronizer(MetricQueryRequest<IndexCountWidget> request)
        {
            return new CollectionSynchronizer<IndexEntry, Item>
            {
                Compare = (indexEntry, item) => item is IndexDeletedDocumentCountWidget deletedDocumentCountWidget && indexEntry.Uuid == deletedDocumentCountWidget.IndexUuid,
                Remove = item => request.DataContext.Remove(item),
                Update = (indexEntry, item) => ((IndexDeletedDocumentCountWidget)item).IndexUuid = indexEntry.Uuid,
                Add = indexEntry => request.DataContext.Add(new IndexDeletedDocumentCountWidget
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

        private static CollectionSynchronizer<IndexEntry, Item> GetTotalStoreSizeSynchronizer(MetricQueryRequest<IndexCountWidget> request)
        {
            return new CollectionSynchronizer<IndexEntry, Item>
            {
                Compare = (indexEntry, item) => item is IndexStoreSizeWidget storeSizeWidget && indexEntry.Uuid == storeSizeWidget.IndexUuid && storeSizeWidget.SizeType == StoreSizeType.Total,
                Remove = item => request.DataContext.Remove(item),
                Update = (indexEntry, item) => ((IndexStoreSizeWidget)item).IndexUuid = indexEntry.Uuid,
                Add = indexEntry => request.DataContext.Add(new IndexStoreSizeWidget
                {
                    Name = indexEntry.Index,
                    IndexName = indexEntry.Index,
                    SizeType = StoreSizeType.Total,
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

        private static CollectionSynchronizer<IndexEntry, Item> GetPrimaryStoreSizeSynchronizer(MetricQueryRequest<IndexCountWidget> request)
        {
            return new CollectionSynchronizer<IndexEntry, Item>
            {
                Compare = (indexEntry, item) => item is IndexStoreSizeWidget storeSizeWidget && indexEntry.Uuid == storeSizeWidget.IndexUuid && storeSizeWidget.SizeType == StoreSizeType.Primary,
                Remove = item => request.DataContext.Remove(item),
                Update = (indexEntry, item) => ((IndexStoreSizeWidget)item).IndexUuid = indexEntry.Uuid,
                Add = indexEntry => request.DataContext.Add(new IndexStoreSizeWidget
                {
                    Name = indexEntry.Index,
                    IndexName = indexEntry.Index,
                    SizeType = StoreSizeType.Primary,
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
