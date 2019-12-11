using System;
using System.Collections.Generic;
using System.Linq;
using GZIpTest.Tests.TestData;
using GZipTest.Threading;
using GZipTest.Threading.Queues;
using Xunit;

namespace GZIpTest.Tests.Threading
{
    public class MultiThreadsWorkerTests
    {
        [Fact]
        public void DoWorkTest()
        {
            // Arrange
            var workerCount = DataGenerator.GenerateInt(1, 5);
            var items = DataGenerator.GenerateStringArray();
            var queue = new QueueAdapter<string>(new Queue<string>(items.Length));

            var result = new List<string>(items.Length);
            void DequeAction(string item)
            {
                Assert.Contains(item, items);
                result.Add(item);
            }

            using (var worker = new MultiThreadsWorker<string>(workerCount, queue, DequeAction, 50))
            {
                // Act
                foreach (var item in items)
                {
                    worker.DoWork(item);
                }
            
                worker.Wait();
            }
            
            Assert.Equal(items.OrderBy(x => x), result.OrderBy(x => x));
            Assert.True(queue.IsEmpty());
        }

        [Fact]
        public void CreationFailedTest()
        {
            // Arrange
            var queue = new QueueAdapter<string>(new Queue<string>());

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new MultiThreadsWorker<string>(0, queue, x => { }, 50));
            Assert.Throws<ArgumentNullException>(() => new MultiThreadsWorker<string>(1, null, x => { }, 50));
            Assert.Throws<ArgumentNullException>(() => new MultiThreadsWorker<string>(1, queue, null, 50));
            Assert.Throws<ArgumentNullException>(() => new MultiThreadsWorker<string>(1, queue, null, x => { }, 50));
        }

        [Fact]
        public void DoWorkFailedTest()
        {
            // Arrange
            void TestAction()
            {
                var queue = new QueueAdapter<string>(new Queue<string>());
                
                using (var worker = new MultiThreadsWorker<string>(2, queue, x => throw new Exception(), 50))
                {
                    worker.DoWork(string.Empty);
                }
            }
            
            // Act
            // Assert
            Assert.Throws<AggregateException>(TestAction);
        }
    }
}