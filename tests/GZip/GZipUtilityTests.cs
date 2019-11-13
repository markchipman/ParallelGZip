using System;
using System.ComponentModel;
using System.IO;
using GZipTest.GZip;
using GZIpTest.Tests.TestData;
using Xunit;

namespace GZIpTest.Tests.GZip
{
    public class GZipUtilityTests
    {
        [Fact]
        public void FailedArgumentsTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => GZipUtility.Compress(null, Stream.Null));
            Assert.Throws<ArgumentNullException>(() => GZipUtility.Compress(Stream.Null, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => GZipUtility.Compress(Stream.Null, Stream.Null, -1));

            Assert.Throws<ArgumentNullException>(() => GZipUtility.Decompress(null, Stream.Null));
            Assert.Throws<ArgumentNullException>(() => GZipUtility.Decompress(Stream.Null, null));
        }

        [Theory]
        [ClassData(typeof(TransformationTestData))]
        [Description("These tests might take a lot of time on low-resource PC.")]
        public void TransformationTest(int bufferSize, int repeatCount, int blockSize)
        {
            // Arrange
            var origFile = DataGenerator.GenerateRandomFile(bufferSize, repeatCount);
            var gzFile = Path.GetTempFileName();
            var decompressedFile = Path.GetTempFileName();

            // Act

            // compress an original file
            using (var source = new FileStream(origFile, FileMode.Open))
            {
                using (var destination = new FileStream(gzFile, FileMode.Create))
                {
                    GZipUtility.Compress(source, destination, blockSize);
                }
            }

            // decompress the destination file
            using (var source = new FileStream(gzFile, FileMode.Open))
            {
                using (var destination = new FileStream(decompressedFile, FileMode.Create))
                {
                    GZipUtility.Decompress(source, destination);
                }
            }

            // Assert
            // as result, the both original and decompressed file have to be completely identical. 
            var origFileInfo = new FileInfo(origFile);
            var decompressedFileInfo = new FileInfo(decompressedFile);

            Assert.Equal(origFileInfo.Length, decompressedFileInfo.Length);

            using (var source = new FileStream(origFile, FileMode.Open))
            {
                using (var destination = new FileStream(decompressedFile, FileMode.Open))
                {
                    Assert.Equal(source.ReadByte(), destination.ReadByte());
                }
            }

            File.Delete(origFile);
            File.Delete(gzFile);
            File.Delete(decompressedFile);
        }
    }
}