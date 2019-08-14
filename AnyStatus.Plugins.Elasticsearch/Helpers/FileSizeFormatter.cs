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
using System;

namespace AnyStatus.Plugins.Elasticsearch.Helpers
{
    //Please refer for the original implementation : https://www.c-sharpcorner.com/article/csharp-convert-bytes-to-kb-mb-gb/
    public static class FileSizeFormatter
    {
        // Load all suffixes in an array  
        static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public static string FormatSize(double bytes)
        {
            int counter = 0;
            double number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }
            return string.Format("{0:n1} {1}", number, suffixes[counter]);
        }
    }
}
