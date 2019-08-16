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
using AnyStatus.Plugins.Elasticsearch.Helpers;
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

        public IndexCountMetricQuery() : this(new ElasticsearchHelper()) { }

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
                request.DataContext.State = State.Ok;
            }
            else
            {
                indexCountWidget.State = State.Invalid;
            }
        }
    }
}
