using System;
using GZipTest.GZip;
using GZIpTest.Tests.TestData;
using Xunit;

namespace GZIpTest.Tests.GZip
{
    public class DataBlockTests
    {
        [Theory]
        [ClassData(typeof(DataBlockTestData))]
        internal void CompareToTest(DataBlock first, DataBlock second, int compareResult)
        {
            // Arrange
            // Act
            var result = first.CompareTo(second);
            
            // Assert
            Assert.Equal(compareResult, result);
        }

        [Fact]
        public void CreationFailedTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new DataBlock(Array.Empty<byte>(), -1));
            Assert.Throws<ArgumentNullException>(() => new DataBlock(null, DataGenerator.GenerateInt(0)));
        }
    }
}