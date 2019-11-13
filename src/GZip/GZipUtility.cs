using System;
using System.IO;
using GZipTest.GZip.Transformers;

namespace GZipTest.GZip
{
    /// <summary>
    /// Provide methods for parallel GZip compression / decompression
    /// </summary>
    public static class GZipUtility
    {
        private const int DefaultBlockSize = 1024 * 1024; // 1 Mb
        
        /// <summary>
        /// Compress the source stream and write it to the destination one using parallel Gzip algorithm.
        /// </summary>
        /// <param name="source">The source stream to compress.</param>
        /// <param name="destination">The destination stream.</param>
        /// <param name="readBlockSize">The block size for read operation. A default value is 1 MB.</param>
        /// <exception cref="ArgumentNullException">Either source or destination stream is null.</exception>
        public static void Compress(Stream source, Stream destination, int readBlockSize = DefaultBlockSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Source stream is null");

            if (destination == null)
                throw new ArgumentNullException(nameof(destination), "Destination stream is null");

            var compressor = new GZipCompressor(source, destination, readBlockSize);
            compressor.Run();
        }

        /// <summary>
        /// Decompress the source stream and write it to the destination one using parallel Gzip algorithm.
        /// </summary>
        /// <param name="source">The source stream to decompress.</param>
        /// <param name="destination">The destination stream.</param>
        /// <exception cref="ArgumentNullException">Either source or destination stream is null.</exception>
        public static void Decompress(Stream source, Stream destination)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Source stream is null");

            if (destination == null)
                throw new ArgumentNullException(nameof(destination), "Destination stream is null");
            
            var decompressor = new GZipDecompressor(source, destination);
            decompressor.Run();
        }
    }
}