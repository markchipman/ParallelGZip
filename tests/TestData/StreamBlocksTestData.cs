using System.Collections;
using System.Collections.Generic;

namespace GZIpTest.Tests.TestData
{
    
    public sealed class StreamBlocksTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new byte[] { 56, 34, 90, 1, 34, 253, 98 }, // all bytes
                2, // block size
                new object[] // block data
                {
                    new byte[] { 56, 34},
                    new byte[] { 90, 1 },
                    new byte[] { 34, 253},
                    new byte[] { 98}
                }
            };
            
            yield return new object[]
            {
                new byte[] { 56, 34, 90, 1, 34, 253, 98, 0, 42, 76, 254, 41, 1, 0, 87, 16 }, // all bytes
                8, // block size
                new object[] // block data
                {
                    new byte[] { 56, 34, 90, 1, 34, 253, 98, 0 },
                    new byte[] { 42, 76, 254, 41, 1, 0, 87, 16 }
                }
            };
            
            yield return new object[]
            {
                new byte[] { 56, 34, 90, 1, 34, 253, 98, 0, 42, 76, 254, 41, 1, 0, 87, 16 }, // all bytes
                100, // block size
                new object[] // block data
                {
                    new byte[] { 56, 34, 90, 1, 34, 253, 98, 0, 42, 76, 254, 41, 1, 0, 87, 16 }
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}