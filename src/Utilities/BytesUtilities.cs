using System;

namespace GZipTest.Utilities
{
    /// <summary>
    /// Defines methods for converting array of bytes to the base data types and vice versa.
    /// </summary>
    internal static class BytesUtilities
    {
        /// <summary>
        /// Convert the specified <see cref="Int32"/> value into the corresponding array of bytes representation.
        /// </summary>
        /// <param name="value">The <see cref="Int32"/> value for converting.</param>
        /// <returns>The array of bytes which represent the specified value.</returns>
        public static byte[] GetBytes(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }

        /// <summary>
        /// Convert the array of bytes to the corresponding <see cref="Int32"/> value.
        /// </summary>
        /// <param name="value">The array of bytes for converting.</param>
        /// <returns>The array of bytes.</returns>
        public static int GetInt(byte[] value)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            
            return BitConverter.ToInt32(value);
        }
    }
}