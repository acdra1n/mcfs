using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MCFS.Caching
{
    public class MCFSCacheManager
    {
        const int CACHE_MAX = 1024; // max 1k cached entries

        private int cache_max_limit;
        private long cache_track_totalLength;

        public volatile byte[][] data_store;
        public volatile string[] file_refs;
        private int current_dsIndex = 0;

        public MCFSCacheManager(int sizeMax)
        {
            data_store = new byte[CACHE_MAX][];
            file_refs = new string[CACHE_MAX];
            cache_track_totalLength = 0;
            cache_max_limit = sizeMax;
        }

        /// <summary>
        /// Returns the index of a file name.
        /// </summary>
        /// <param name="fileName">The file name to resolve the index for.</param>
        /// <returns></returns>
        public int IndexOf(string fileName)
        {
            for (var x = 0; x < file_refs.Length; x++)
                if (file_refs[x] == fileName)
                    return x;
            return -1;
        }

        /// <summary>
        /// Check if the file is cached.
        /// </summary>
        /// <param name="fileName">The file name to check cache existence for.</param>
        /// <returns></returns>
        public bool IsCached(string fileName)
        {
            return IndexOf(fileName) != -1;
        }

        /// <summary>
        /// Creates a write stream for the specified file name.
        /// </summary>
        /// <param name="fileName">The file name to create a write stream for.</param>
        /// <param name="expectedSize">The expected size of the file to write.</param>
        /// <returns></returns>
        public MemoryStream CreateWriteStream(string fileName, long expectedSize)
        {
            if (expectedSize > (cache_max_limit - cache_track_totalLength))
                throw new Exception("No space left in memory.");

            int fileIndex = IndexOf(fileName);

            if(fileIndex == -1)
            {
                if (current_dsIndex > CACHE_MAX)
                    throw new Exception("Cache full.");
                fileIndex = current_dsIndex++;
            }

            file_refs[fileIndex] = fileName;
            data_store[fileIndex] = new byte[expectedSize];
            cache_track_totalLength += expectedSize;

            return new MemoryStream(data_store[fileIndex], true);
        }
    }
}
