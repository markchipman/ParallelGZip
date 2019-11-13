using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GZIpTest.Tests.TestData
{
    public sealed class QueueTestData : IEnumerable<object[]>
    {
        
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new[] { "1", string.Empty, "cat", "dog", "1", "AA", "AAA", string.Empty }, // enqueue order
                new[] { string.Empty, string.Empty, "1", "1", "AA", "AAA", "cat", "dog" } // dequeue order
            };
            
            // generate random array of string
            var randomArrayStr = DataGenerator.GenerateStringArray();
            var sortArray = randomArrayStr.OrderBy(x => x).ToArray();

            yield return new object[]
            {
                randomArrayStr, // enqueue order
                sortArray       // dequeue order
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}