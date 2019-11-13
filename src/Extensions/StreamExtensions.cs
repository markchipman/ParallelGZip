using System;
using System.Collections.Generic;
using System.IO;
using GZipTest.Utilities;

namespace GZipTest.Extensions
{
    /// <summary>
    /// Provide extensions to work with streams.
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// Read a sequence of block byte data from the current stream block by block.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="blockSize">The data block size.</param>
        /// <returns>The sequence of block data.</returns>
        public static IEnumerable<byte[]> ReadToEndBlockByBlock(this Stream source, int blockSize)
        {
            int bytesRead;
            do
            {
                var (blockLength, blockData) = ReadBlock(source, blockSize);
                if (blockLength > 0)
                    yield return blockData;

                bytesRead = blockLength;
            } while (bytesRead > 0);
        }

        /// <summary>
        /// Read a sequence of bytes from the current stream.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="blockSize">The data block size.</param>
        /// <returns>A tuple containing the total count of read bytes + the read sequence of bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The block size can't be equal or less than zero.</exception>
        public static (int, byte[]) ReadBlock(this Stream source, int blockSize)
        {
            if (blockSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(blockSize), "The block size can't be equal or less than zero.");
            
            Span<byte> buffer = new byte[blockSize];
            var bytesRead = source.Read(buffer);
            if (bytesRead > 0)
            {
                if (bytesRead < blockSize)
                    buffer = buffer.Slice(0, bytesRead);
            }
            else
            {
                buffer = Span<byte>.Empty;
            }

            return (bytesRead, buffer.ToArray());
        }

        public static int? ReadInt(this Stream stream)
        {
            Span<byte> block = new byte[sizeof(int)];
            var bytesRead = stream.Read(block);
            return bytesRead < sizeof(int) ? default(int?) : BytesUtilities.GetInt(block.ToArray());
        }

        public static void WriteInt(this Stream stream, int value)
        {
            var bytes = BytesUtilities.GetBytes(value);
            stream.Write(bytes);
        }
    }
}