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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Elasticsearch.FileSystemUsage
{
    /// <summary>
    /// Provides human readable entries for <see cref="FileSystemPercentageType"/>
    /// </summary>
    public class FileSystemPercentageTypeItemsSource : IItemsSource
    {
        /// <summary>
        /// Maps <see cref="FileSystemPercentageType"/> entries wit human redable texts
        /// </summary>
        /// <returns>Returns mapped entries</returns>
        public ItemCollection GetValues()
        {
            ItemCollection values = new ItemCollection
            {
                { FileSystemPercentageType.PercentageUsed, "Percentage Used" },
                { FileSystemPercentageType.PercentageRemaining, "Percentage Remaining" },
            };

            return values;
        }
    }
}
