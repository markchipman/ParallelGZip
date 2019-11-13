using System.Collections;
using System.Collections.Generic;

namespace GZIpTest.Tests.TestData
{
    public sealed class TransformationTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // test data is a triple where the first and the second values are used to generate a test file (buffer size + repeat count)
            // the third argument is a block size that will be used during the compression phase.
            yield return new object[] {0, 0, 102}; // zero length file
            
            yield return new object[] {1, 1, 1024}; // 1 byte length file
            
            yield return new object[] {1024 * 1024, 1, 1024 * 1024}; // 1 Mb file contains exact one block
            
            yield return new object[] {1024, 4, 1024}; // 4Kb file contains exact four block, 1Kb - block size
            
            yield return new object[] {1568, 1, 1}; // >1Kb file, 1b - block size
            
            yield return new object[] { 1024*1024, 1024, 1024 }; // 1Gb file, 1Kb - block size
            
            yield return new object[] { 1024*1024, 1024 * 5, 1024 * 1024}; // 5Gb file, 1Mb - block size

            var bufferSize = DataGenerator.GenerateInt(1, 1024 * 1024);
            var repeatCount = DataGenerator.GenerateInt(1, 1024);
            var blockSize = DataGenerator.GenerateInt(10, 1024);
            yield return new object[] { bufferSize, repeatCount, blockSize }; // completely random file up to 1Gb
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}