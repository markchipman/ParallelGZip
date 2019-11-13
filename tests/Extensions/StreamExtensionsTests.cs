using System;
using System.IO;
using System.Linq;
using GZipTest.Extensions;
using GZIpTest.Tests.TestData;
using Xunit;

namespace GZIpTest.Tests.Extensions
{
    public class StreamExtensionsTests
    {
        [Theory]
        [ClassData(typeof(StreamBlocksTestData))]
        public void ReadToEndBlockByBlockTest(byte[] data, int blockSize, object[] expectedBlocks)
        {
            // Arrange
            using (var ms = new MemoryStream(data))
            {
                // Act
                var resultBlocks = ms.ReadToEndBlockByBlock(blockSize).ToArray();

                // Assert
                Assert.NotNull(resultBlocks);
                Assert.Equal(resultBlocks.Length, expectedBlocks.Length);
                
                for (var i = 0; i < resultBlocks.Length; i++)
                {
                    Assert.Equal(expectedBlocks[i], resultBlocks[i]);
                }
            }
        }

        [Fact]
        public void ReadBlockFailedTest()
        {
            // Arrange
            using (var ms = new MemoryStream())
            {
                // Act
                var (bytesRead, block) = ms.ReadBlock(10);

                // Assert
                Assert.Equal(0, bytesRead);
                Assert.Empty(block);
                
                Assert.Throws<ArgumentOutOfRangeException>(() => ms.ReadBlock(-1));
            }
        }
        
        [Theory]
        [ClassData(typeof(IntBytesTestData))]
        public void ReadIntTest(int expected, byte[] bytes)
        {
            // Arrange
            using (var ms = new MemoryStream(bytes))
            {
                // Act
                var result = ms.ReadInt();
            
                // Arrange
                Assert.Equal(expected, result);   
            }
        }

        [Fact]
        public void ReadIntFailed()
        {
            // Arrange
            using (var ms = new MemoryStream())
            {
                // Act
                var result = ms.ReadInt();
            
                // Arrange
                Assert.Equal(default(int?), result);   
            }
        }

        [Theory]
        [ClassData(typeof(IntBytesTestData))]
        public void WriteIntTest(int value, byte[] expected)
        {
            // Arrange
            using (var ms = new MemoryStream())
            {
                // Act
                ms.WriteInt(value);
            
                // Arrange
                var result = ms.ToArray();
                Assert.Equal(expected, result);   
            }
        }
    }
}