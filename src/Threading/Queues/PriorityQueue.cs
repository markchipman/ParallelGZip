using System;
using System.Collections.Generic;

namespace GZipTest.Threading.Queues
{
	/// <summary>
	/// The priority queue. Provide all typical queue operations, however pay attentions to the items priorities.
	/// </summary>
	/// <typeparam name="T">The type parameter.</typeparam>
	/// <remarks>More info: https://en.wikipedia.org/wiki/Priority_queue</remarks>
	/// <remarks>Inspired by http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx</remarks>
	internal sealed class PriorityQueue<T> : IQueue<T> 
		where T : class, IComparable<T>
	{
		private readonly List<T> _data;

		public PriorityQueue() 
		{
			_data = new List<T>();
		}

		/// <summary>
		/// Add an <see cref="T"/> object to the queue.
		/// </summary>
		/// <param name="item"></param>
		public void Enqueue(T item) 
		{
			_data.Add(item);

			var childIndex = _data.Count - 1; // child index; start at end
			while (childIndex > 0) 
			{
				var parentIndex = (childIndex - 1) / 2; // parent index
				if (_data[childIndex] == null || _data[childIndex].CompareTo(_data[parentIndex]) >= 0)
					break; // child item is larger than (or equal) parent so we're done
			
				var tmp = _data[childIndex];
				_data[childIndex] = _data[parentIndex];
				_data[parentIndex] = tmp;
				childIndex = parentIndex;
			}
		}

		/// <summary>
		/// Try to extract the top priority item from the queue.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>Return the operation success flag.</returns>
		public bool TryDequeue(out T item)
		{
			if (IsEmpty())
			{
				item = default;
				return false;
			}
		
			// assumes pq is not empty; up to calling code
			var lastIndex = _data.Count - 1; // last index (before removal)
			item = _data[0];   // fetch the front
			_data[0] = _data[lastIndex];
			_data.RemoveAt(lastIndex);

			--lastIndex; // last index (after removal)
			var parentIndex = 0; // parent index. start at front of pq
			while (true) 
			{
				var childIndex = parentIndex * 2 + 1; // left child index of parent
				if (childIndex > lastIndex)
					break;  // no children so done
			
				var rightChildIndex = childIndex + 1;     // right child
				if (rightChildIndex <= lastIndex && _data[rightChildIndex]?.CompareTo(_data[childIndex]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
					childIndex = rightChildIndex;
			
				if (_data[parentIndex] != null && _data[parentIndex].CompareTo(_data[childIndex]) <= 0)
					break; // parent is smaller than (or equal to) smallest child so done
			
				var tmp = _data[parentIndex];
				_data[parentIndex] = _data[childIndex];
				_data[childIndex] = tmp; // swap parent and child
				parentIndex = childIndex;
			}

			return true;
		}

		/// <summary>
		/// Return the object from the beginning of the queue without removing it.
		/// </summary>
		/// <returns>The object with the highest priority.</returns>
		public T Peek() => _data[0];

		/// <summary>
		/// Indicates whether the queue is empty.
		/// </summary>
		/// <returns>The empty boolean flag.</returns>
		public bool IsEmpty() => _data.Count == 0;
	}
}