using System;
using System.Collections;
using System.Collections.Generic;
using GZipTest.GZip;

namespace GZIpTest.Tests.TestData
{
    public sealed class DataBlockTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new DataBlock(Array.Empty<byte>(), 0), // block 1
                new DataBlock(Array.Empty<byte>(), 1), // block 2
                -1 // compare result
            };

            var block = new DataBlock(Array.Empty<byte>(), DataGenerator.GenerateInt(0));
            yield return new object[]
            {
                block, // block 1
                block, // block 2
                0 // compare result
            };
            
            yield return new object[]
            {
                new DataBlock(Array.Empty<byte>(), 42), // block 1
                new DataBlock(Array.Empty<byte>(), 42), // block 2
                0 // compare result
            };
            
            yield return new object[]
            {
                new DataBlock(Array.Empty<byte>(), 42), // block 1
                null, // block 2
                -1 // compare result
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}