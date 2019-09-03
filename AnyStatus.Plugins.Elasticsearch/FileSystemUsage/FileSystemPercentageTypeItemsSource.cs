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
