namespace GZipTest.Threading.Queues
{
    /// <summary>
    /// Define a common queue behavior (First-in, first-out collection of object).
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    internal interface IQueue<T>
    {
        /// <summary>
        /// Add an <see cref="T"/> object to the queue.
        /// </summary>
        /// <param name="item"></param>
        void Enqueue(T item);

        /// <summary>
        /// Try to extract the top item from the queue.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Return the operation success flag.</returns>
        bool TryDequeue(out T item);

        /// <summary>
        /// Return the object from the beginning of the queue without removing it.
        /// </summary>
        /// <returns>The first object from the queue</returns>
        T Peek();

        /// <summary>
        /// Indicates whether the queue is empty.
        /// </summary>
        /// <returns>The empty boolean flag.</returns>
        bool IsEmpty();
    }
}