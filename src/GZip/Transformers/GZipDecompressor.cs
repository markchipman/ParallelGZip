using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using GZipTest.Extensions;
using GZipTest.Threading.Queues;

namespace GZipTest.GZip.Transformers
{
    /// <summary>
    /// Defines a Gzip parallel decompression algorithm.
    /// </summary>
    internal sealed class GZipDecompressor : GZipTransformer
    {
        private int _currentWritingPosition;
        
        public GZipDecompressor(Stream source, Stream destination) 
            : base(source, destination)
        {
        }

        /// <inheritdoc/>
        protected override IQueue<DataBlock> CreateWriteSynchronizationQueue()
        {
            return new PriorityQueue<DataBlock>();
        }

        /// <inheritdoc/>
        protected override IEnumerable<DataBlock> ReadSource(Stream source)
        {
            var bytesRead = int.MaxValue;
            while (bytesRead > 0)
            {
                // read a header
                var position = source.ReadInt();
                var length = source.ReadInt();
                
                if (length.GetValueOrDefault() > 0)
                {
                    // read a data
                    var (_, blockData) = source.ReadBlock(length.GetValueOrDefault());
                    yield return new DataBlock(blockData, position.GetValueOrDefault());
                }

                bytesRead = length.GetValueOrDefault();
            }
        }

        /// <inheritdoc/>
        protected override DataBlock Transform(DataBlock block)
        {
            using (var compressedStream = new MemoryStream(block.Data))
            {
                using (var gz = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var decompressedStream = new MemoryStream())
                    {
                        gz.CopyTo(decompressedStream);
                        block.Data = decompressedStream.ToArray();
                    }
                }
            }
            
            return block;
        }

        /// <inheritdoc/>
        protected override bool IsWritable(DataBlock block)
        {
            return block == null || block.Position == _currentWritingPosition;
        }

        /// <inheritdoc/>
        protected override void WriteToDestination(DataBlock block, Stream destination)
        {
            destination.Write(block.Data);
            _currentWritingPosition++;
        }
    }
}