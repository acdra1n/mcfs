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

        private volatile byte[][] data_store;
        private volatile string[] file_refs;
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
        /// <param name="writeMode">Whether to write to the file. Set to false if you are creating a read stream.</param>
        /// <returns></returns>
        public MemoryStream CreateStream(string fileName, long expectedSize, bool writeMode = true)
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
            if(writeMode)
                data_store[fileIndex] = new byte[expectedSize];
            cache_track_totalLength += expectedSize;

            return new MemoryStream(data_store[fileIndex], true);
        }

        /// <summary>
        /// Adds capacity to a file cache store.
        /// </summary>
        /// <param name="index">The index of the store to expand.</param>
        /// <param name="sizeToAdd">The size to add.</param>
        public void ExpandStore(int index, long sizeToAdd)
        {
            if (sizeToAdd > (cache_max_limit - cache_track_totalLength))
                throw new Exception("No space left in memory.");

            byte[] oldStore = data_store[index];
            byte[] newStore = new byte[oldStore.Length + sizeToAdd];

            // Copy original bytes bytes
            for (long x = 0; x < oldStore.Length; x++)
                newStore[x] = oldStore[x];

            // Assign to cache store object.
            data_store[index] = newStore;
        }

        /// <summary>
        /// Removes a file from the cache.
        /// </summary>
        /// <param name="fileName">The file name to remove.</param>
        public void Unlink(string fileName)
        {
            int index = IndexOf(fileName);
            data_store[index] = null;
            file_refs[index] = null;
        }

        /// <summary>
        /// Moves a file reference.
        /// </summary>
        /// <param name="fileName">The original reference.</param>
        /// <param name="newRef">The target reference.</param>
        public void MoveFileRef(string fileName, string newRef)
        {
            int index = IndexOf(fileName);
            file_refs[index] = newRef;
        }

        //TODO add copy function
    }
}
