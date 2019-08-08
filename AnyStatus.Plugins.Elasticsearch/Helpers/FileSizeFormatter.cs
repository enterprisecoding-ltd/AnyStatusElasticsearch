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
