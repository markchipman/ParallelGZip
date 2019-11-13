using System.Collections.Generic;

namespace GZipTest.Threading.Queues
{
    /// <summary>
    /// The simple adapter for <see cref="Queue{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type parameter</typeparam>
    internal sealed class QueueAdapter<T> : IQueue<T>
    {
        private readonly Queue<T> _queue;

        public QueueAdapter(Queue<T> queue)
        {
            _queue = queue;
        }
        
        public void Enqueue(T item) => _queue.Enqueue(item);

        public bool TryDequeue(out T item) => _queue.TryDequeue(out item);

        public T Peek() => _queue.Peek();

        public bool IsEmpty() => _queue.Count == 0;
    }
}