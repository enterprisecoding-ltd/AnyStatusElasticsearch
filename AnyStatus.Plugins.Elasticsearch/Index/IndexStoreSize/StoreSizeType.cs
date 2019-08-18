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
namespace AnyStatus.Plugins.Elasticsearch.Index.IndexStoreSize
{
    public enum StoreSizeType
    {
        /// <summary>
        /// Indicates calculations should be done against primary store
        /// </summary>
        Primary,

        /// <summary>
        /// Indicates calculations should be done against primary and secondary stores
        /// </summary>
        Total
    }
}
