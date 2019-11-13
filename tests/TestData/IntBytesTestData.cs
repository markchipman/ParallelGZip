using System.Collections;
using System.Collections.Generic;

namespace GZIpTest.Tests.TestData
{
    public sealed class IntBytesTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // for each test case: Int32 value +  its representation as array of bytes
            yield return new object[] { 1, new byte[] { 0, 0, 0, 1} };
            yield return new object[] { 0, new byte[] { 0, 0, 0, 0 } };
            yield return new object[] { -1, new byte[] { 255, 255, 255, 255 } };
            yield return new object[] { int.MinValue, new byte[] { 128, 0, 0, 0 } };
            yield return new object[] { 42, new byte[] { 0, 0, 0, 42 }};
            yield return new object[] { int.MaxValue, new byte[] { 127, 255, 255, 255 } };
            yield return new object[] { 129247584, new byte[] {7, 180, 41, 96 } };
            yield return new object[] { -15696523, new byte[] { 255, 16, 125, 117 }};
            yield return new object[] {-376, new byte[] {255, 255, 254, 136}};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}