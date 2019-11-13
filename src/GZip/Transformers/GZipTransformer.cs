using System.Collections.Generic;
using System.IO;
using GZipTest.Threading;
using GZipTest.Threading.Queues;
using GZipTest.Utilities;

namespace GZipTest.GZip.Transformers
{
    /// <summary>
    /// Defines a template for Gzip transformation.
    /// </summary>
    /// <remarks>
    /// Actually, the class is an implementation of the Template method design pattern.
    /// See also: https://en.wikipedia.org/wiki/Template_method_pattern</remarks>
    internal abstract class GZipTransformer
    {
        private readonly Stream _source;
        private readonly Stream _destination;
        
        protected GZipTransformer(Stream source, Stream destination)
        {
            _source = source;
            _destination = destination;
        }

        /// <summary>
        /// Run the transformation process.
        /// </summary>
        public void Run()
        {
            // create an output writer as a single thread worker.
            var writeQueue = CreateWriteSynchronizationQueue();
            using (var outputWriter = new MultiThreadsWorker<DataBlock>(1, writeQueue, IsWritable, block => WriteToDestination(block, _destination)))
            {
                void TransformAndWrite(DataBlock block)
                {
                    block = Transform(block);
                    outputWriter.DoWork(block);
                }

                // create a parallel transformer
                var compressionThreadsCount = EnvironmentUtilities.GetOptimalThreadsCount();
                var readQueue = CreateReadSynchronizationQueue();
                using (var transformer = new MultiThreadsWorker<DataBlock>(compressionThreadsCount, readQueue, TransformAndWrite))
                {
                    foreach (var block in ReadSource(_source))
                    {
                        transformer.DoWork(block);
                    }
                }
            }
        }

        /// <summary>
        /// Define a way how to read a source stream.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <returns>A sequence of data blocks</returns>
        protected abstract IEnumerable<DataBlock> ReadSource(Stream source);

        /// <summary>
        /// Perform a data block transformation. 
        /// </summary>
        /// <param name="block">The original data block</param>
        /// <returns>The transformed data block.</returns>
        protected abstract DataBlock Transform(DataBlock block);
        
        /// <summary>
        /// Create a queue that's used for storing the parsed source data.
        /// </summary>
        /// <returns></returns>
        protected virtual IQueue<DataBlock> CreateReadSynchronizationQueue()
        {
            return new QueueAdapter<DataBlock>(new Queue<DataBlock>());
        }

        /// <summary>
        /// Create a queue that's used for storing the transformed data.
        /// </summary>
        /// <returns></returns>
        protected virtual IQueue<DataBlock> CreateWriteSynchronizationQueue()
        {
            return new QueueAdapter<DataBlock>(new Queue<DataBlock>());
        }

        /// <summary>
        /// Perform a write operation to the destination stream.
        /// </summary>
        /// <param name="block">The desired data block pending the writing.</param>
        /// <param name="destination">The destination stream.</param>
        protected abstract void WriteToDestination(DataBlock block, Stream destination);

        /// <summary>
        /// Check whether a worker thread can extract and write the specified data block.
        /// </summary>
        /// <param name="block">The desired data block pending the writing.</param>
        /// <returns></returns>
        protected abstract bool IsWritable(DataBlock block);
    }
}