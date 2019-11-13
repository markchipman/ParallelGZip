using GZIpTest.Tests.TestData;
using GZipTest.Threading.Queues;
using Xunit;

namespace GZIpTest.Tests.Threading
{
    public class PriorityQueueTest
    {
        [Theory]
        [ClassData(typeof(QueueTestData))]
        public void Test(string[] sequence, string[] expectedDequeueOrder)
        {
            // Arrange
            var queue = new PriorityQueue<string>();

            // Act
            foreach (var item in sequence)
            {
                queue.Enqueue(item);
            }

            // Assert
            Assert.False(queue.IsEmpty());
            foreach (var expected in expectedDequeueOrder)
            {
                Assert.Equal(expected, queue.Peek());
                var success = queue.TryDequeue(out var item);
                Assert.True(success);
                Assert.Equal(expected, item);
            }
            
            Assert.False(queue.TryDequeue(out _));
            Assert.True(queue.IsEmpty());
        }
    }
}