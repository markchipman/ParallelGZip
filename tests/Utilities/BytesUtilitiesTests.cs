using GZIpTest.Tests.TestData;
using GZipTest.Utilities;
using Xunit;

namespace GZIpTest.Tests.Utilities
{
    public class BytesUtilitiesTests
    {
        [Theory]
        [ClassData(typeof(IntBytesTestData))]
        public void GetBytesTest(int value, byte[] expected)
        {
            // Arrange
            // Act
            var result = BytesUtilities.GetBytes(value);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [ClassData(typeof(IntBytesTestData))]
        public void GetIntTest(int expected, byte[] bytes)
        {
            // Arrange
            // Act
            var result = BytesUtilities.GetInt(bytes);
            
            // Assert
            Assert.Equal(expected, result);
        }
        
        
    }
}