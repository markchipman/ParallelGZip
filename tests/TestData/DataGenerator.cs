using System;
using System.IO;
using System.Linq;

namespace GZIpTest.Tests.TestData
{
    public static class DataGenerator
    {
        private static readonly Random _random = new Random();
        
        private static string GenerateString()
        {
            var count = _random.Next(1, 15); // random string length
            return new string(Enumerable.Range(0, count).Select(x => (char)_random.Next(0, 255)).ToArray());
        }

        public static string[] GenerateStringArray()
        {
            var arrayCount = _random.Next(1, 15); // random length
            return Enumerable.Range(0, arrayCount).Select(x => GenerateString()).ToArray();
        }

        public static int GenerateInt(int min = int.MinValue, int max = int.MaxValue)
        {
            return _random.Next(min, max);
        }

        public static byte[] GenerateByteArray(int size)
        {
            return Enumerable.Range(0, size).Select(x => (byte)_random.Next(0, 256)).ToArray();
        }

        public static string GenerateRandomFile(int bufferSize, int repeatCount)
        {
            var path = Path.GetTempFileName();
            using (var fs = new FileStream(path, FileMode.Create))
            {
                for (var i = 0; i < repeatCount; i++)
                {
                    fs.Write(GenerateByteArray(bufferSize));
                    fs.Flush();
                }
            }

            return path;
        }
    }
}