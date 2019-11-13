using System;
using System.IO;
using System.Security;
using GZipTest.GZip;
using GZipTest.Utilities;

namespace GZipTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 3)
                EnvironmentUtilities.ExitWithError("The required arguments are missing.");

            var command = args[0]; 
            var sourceFile = args[1];
            var destinationFile = args[2];

            try
            {
                using (var source = new FileStream(sourceFile, FileMode.Open))
                {
                    using (var destination = new FileStream(destinationFile, FileMode.Create))
                    {
                        switch (command.ToUpperInvariant())
                        {
                            case "COMPRESS":
                                GZipUtility.Compress(source, destination);
                                break;
                            case "DECOMPRESS":
                                GZipUtility.Decompress(source, destination);
                                break;
                            default:
                                throw new ArgumentException("The specified command is invalid.");
                        }
                    }
                }
            }
            catch (NotSupportedException)
            {
                EnvironmentUtilities.ExitWithError("Path refers to the non-file device.");
            }
            catch (SecurityException)
            {
                EnvironmentUtilities.ExitWithError("The application doesn't have enough permission to complete the current operation.");
            }
            catch (FileNotFoundException)
            {
                EnvironmentUtilities.ExitWithError("The source file cannot be found.");
            }
            catch (DirectoryNotFoundException)
            {
                EnvironmentUtilities.ExitWithError("The specified path is invalid such as being on unmapped drive or includes non-existing sub path.");
            }
            catch (PathTooLongException)
            {
                EnvironmentUtilities.ExitWithError("The specified path, file name or both exceed the system defined maximum length.");
            }
            catch (AggregateException ex) when (ex.InnerException != null)
            {
                EnvironmentUtilities.ExitWithError(ex.InnerException.Message);
            }
            catch (OutOfMemoryException)
            {
                EnvironmentUtilities.ExitWithError("Not enough memory to complete an operation.");
            }
            catch (Exception ex)
            {
                EnvironmentUtilities.ExitWithError(ex.Message);
            }
        }
    }
}