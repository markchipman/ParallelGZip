using System;

namespace GZipTest.Utilities
{
    /// <summary>
    /// Defines extra methods to work with the system environment.
    /// </summary>
    internal static class EnvironmentUtilities
    {
        /// <summary>
        /// Get the maximum meaningful available threads for the transform operation.
        /// </summary>
        /// <returns>The optimal amount of available thread.</returns>
        public static int GetOptimalThreadsCount()
        {
            // (the count of processor) - 2, because we need to keep two threads for reading and writing operations.
            // if the total processor count is less than what we need, just use one simple thread as a fallback.
            return Math.Max(Environment.ProcessorCount - 2, 1);
        } 
        
        /// <summary>
        /// Exit with error code and write the diagnostic message to the console.
        /// </summary>
        /// <param name="message">The diagnostic message</param>
        public static void ExitWithError(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(1);
        }
    }
}