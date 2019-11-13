using System;
using System.Collections.Generic;
using System.Threading;
using GZipTest.Threading.Queues;

namespace GZipTest.Threading
{
    /// <summary>
    /// Provide methods to work with a multi-threads processing.
    /// Make a deal with the thread synchronization and internal threads lifecycle. 
    /// </summary>
    /// <typeparam name="T">The input type for the task queue.</typeparam>
    /// <remarks>
    /// Actually, this class is an implementation of the producer consumer design pattern.
    /// See also: https://en.wikipedia.org/wiki/Producer%E2%80%93consumer_problem   
    /// </remarks>
    internal sealed class MultiThreadsWorker<T> : IDisposable 
        where T : class
    {
        private readonly List<Thread> _workers;
        private readonly IQueue<T> _taskQueue;
        private readonly Action<T> _dequeueAction;
        private readonly Func<T, bool> _dequeueRule;
        
        private readonly object _locker = new object();
        
        private bool _allDone;
        private Exception _exception;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiThreadsWorker{T}"/> class.
        /// </summary>
        /// <param name="workerCount">The worker count.</param>
        /// <param name="taskQueue">The task queue being shared among threads. It's used to keep producer's results.</param>
        /// <param name="dequeueAction">The dequeue action which will be executed when the thread dequeues an item from the task queue.</param>
        /// <exception cref="ArgumentNullException">One of the arguments is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The worker count must be greater than zero.</exception>
        public MultiThreadsWorker(
            int workerCount,
            IQueue<T> taskQueue,
            Action<T> dequeueAction)
            : this(workerCount, taskQueue, item => true, dequeueAction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiThreadsWorker{T}"/> class.
        /// </summary>
        /// <param name="workerCount">The worker count.</param>
        /// <param name="taskQueue">The task queue being shared among threads. It's used to keep producer's results.</param>
        /// <param name="dequeueRule">The rule defining strategy how threads dequeue items from the queue.
        /// It allows to configure the dequeue algorithm for a non standard queue implementation.
        /// Have to be always 'TRUE result delegate' for the usual queue algorithm.</param>
        /// <param name="dequeueAction">The dequeue action which will be executed when threads dequeue an item from the task queue.</param>
        /// <exception cref="ArgumentNullException">One of the arguments is actually null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The worker count must be greater than zero.</exception>
        public MultiThreadsWorker(
            int workerCount,
            IQueue<T> taskQueue,
            Func<T, bool> dequeueRule,
            Action<T> dequeueAction)
        {
            _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
            _dequeueAction = dequeueAction ?? throw new ArgumentNullException(nameof(dequeueAction));
            _dequeueRule = dequeueRule ?? throw new ArgumentNullException(nameof(dequeueRule));
            
            if (workerCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(workerCount), "Value must be greater than zero.");
            
            _workers = new List<Thread>(workerCount);
            
            // Create and start a separate thread for each worker
            CreateThreads(workerCount);
        }

        private void CreateThreads(int workerCount)
        {
            for (var i = 0; i < workerCount; i++)
            {
                var t = new Thread(Consume);
                _workers.Add(t);
                t.Start();
            }
        }

        /// <summary>
        /// Run the specified action on passed data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void DoWork(T data)
        {
            Monitor.Enter(_locker);

            _taskQueue.Enqueue(data);
            Monitor.PulseAll(_locker);
            Monitor.Exit(_locker);
        }

        /// <summary>
        /// Wait until all planned operations are completed.
        /// </summary>
        public void Wait()
        {
            // exit, if we are already done (waited all operation)
            if (_allDone)
                return;
            
            _allDone = true;
            
            // Enqueue one default task per worker to make each exit.
            _workers.ForEach(thread => DoWork(default));
            _workers.ForEach(thread => thread.Join());
            
            if (_exception != null)
                throw new AggregateException("Aggregate exception is occured", _exception);

        }
        
        /// <summary>
        /// Consumes this instance.
        /// </summary>
        private void Consume()
        {
            try
            {
                while (true)
                {
                    T item = default;
                    Monitor.Enter(_locker);

                    while (_taskQueue.IsEmpty())
                        Monitor.Wait(_locker);

                    var isDequeued = false;
                    if (_dequeueRule(_taskQueue.Peek()))
                        isDequeued = _taskQueue.TryDequeue(out item);

                    Monitor.Exit(_locker);

                    if (isDequeued)
                    {
                        // a default special case, it's needed to mark the current thread as completed
                        if (Equals(item, default(T)))
                        {
                            return;
                        }

                        // run an actual method
                        _dequeueAction(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            finally
            {
                if (Monitor.IsEntered(_locker))
                    Monitor.Exit(_locker);
            }
        }

        /// <summary>
        /// Dispose all internal resources.
        /// Wait until all planned operations are completed then stop all running threads.
        /// </summary>
        public void Dispose()
        {
            Wait();
            _workers.Clear();
        }
    }
}