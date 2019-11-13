using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using GZipTest.Extensions;

namespace GZipTest.GZip.Transformers
{
    /// <summary>
    /// Defines a GZip parallel compression algorithm.
    /// </summary>
    internal sealed class GZipCompressor : GZipTransformer
    {
        private readonly int _readBlockSize;
        
        public GZipCompressor(Stream source, Stream destination, int readBlockSize)
            : base(source, destination)
        {
            _readBlockSize = readBlockSize;
        }

        /// <inheritdoc/>
        protected override IEnumerable<DataBlock> ReadSource(Stream source)
        {
            return source.ReadToEndBlockByBlock(_readBlockSize)
                .Select((data, index) => new DataBlock(data, index));
        }

        /// <inheritdoc/>
        protected override DataBlock Transform(DataBlock block)
        {
            using (var ms = new MemoryStream())
            {
                using (var gz = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gz.Write(block.Data);
                }
                
                block.Data = ms.ToArray();
            }
            
            return block;
        }

        /// <inheritdoc/>
        protected override bool IsWritable(DataBlock block)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void WriteToDestination(DataBlock block, Stream destination)
        {
            //Write the block position
            destination.WriteInt(block.Position);
            
            // Write the block length.
            destination.WriteInt(block.Data.Length);

            // Write the compressed data.
            destination.Write(block.Data);
        }
    }
}