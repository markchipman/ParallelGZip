using System;

namespace GZipTest.GZip
{
    /// <summary>
    /// Defines a block of bytes together with their position in a file.
    /// </summary>
    internal sealed class DataBlock : IComparable<DataBlock>
    {
        /// <summary>
        /// Indicates the block position in a file.
        /// </summary>
        public int Position { get; }
        
        /// <summary>
        /// The block data.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Create new data block
        /// </summary>
        /// <param name="data">The block data.</param>
        /// <param name="position">The block position in a file.</param>
        public DataBlock(byte[] data, int position)
        {
            Data = data;
            Position = position;
        }

        /// <summary>
        /// Defines a generalized comparison method for <see cref="DataBlock"/> class.
        /// </summary>
        /// <param name="other">The <see cref="DataBlock"/> instance to compare.</param>
        /// <returns></returns>
        public int CompareTo(DataBlock other)
        {
            if (ReferenceEquals(this, other)) 
                return 0;
            
            if (ReferenceEquals(null, other)) 
                return -1;
            
            return Position.CompareTo(other.Position);
        }
    }
}