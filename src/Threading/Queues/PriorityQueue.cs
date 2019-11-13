using System;
using System.Collections.Generic;

namespace GZipTest.Threading.Queues
{
	/// <summary>
	/// The priority queue. Provide all typical queue operations, however pay attention to the items priorities.
	/// </summary>
	/// <typeparam name="T">The type parameter.</typeparam>
	/// <remarks>More info: https://en.wikipedia.org/wiki/Priority_queue</remarks>
	/// <remarks>Inspired by http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx</remarks>
	internal sealed class PriorityQueue<T> : IQueue<T> 
		where T : IComparable<T>
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

			var ci = _data.Count - 1; // child index; start at end
			while (ci > 0) 
			{
				var pi = (ci - 1) / 2; // parent index
				if (_data[ci] == null || _data[ci].CompareTo(_data[pi]) >= 0)
					break; // child item is larger than (or equal) parent so we're done
			
				T tmp = _data[ci];
				_data[ci] = _data[pi];
				_data[pi] = tmp;
				ci = pi;
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
			var li = _data.Count - 1; // last index (before removal)
			item = _data[0];   // fetch the front
			_data[0] = _data[li];
			_data.RemoveAt(li);

			--li; // last index (after removal)
			var pi = 0; // parent index. start at front of pq
			while (true) 
			{
				var ci = pi * 2 + 1; // left child index of parent
				if (ci > li)
					break;  // no children so done
			
				var rc = ci + 1;     // right child
				if (rc <= li && _data[rc]?.CompareTo(_data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
					ci = rc;
			
				if (_data[pi] != null && _data[pi].CompareTo(_data[ci]) <= 0)
					break; // parent is smaller than (or equal to) smallest child so done
			
				T tmp = _data[pi];
				_data[pi] = _data[ci];
				_data[ci] = tmp; // swap parent and child
				pi = ci;
			}

			return true;
		}

		/// <summary>
		/// Return the object from the beginning of the queue without removing it.
		/// </summary>
		/// <returns>The object</returns>
		public T Peek() => _data[0];

		/// <summary>
		/// Indicates whether the queue is empty.
		/// </summary>
		/// <returns>The empty boolean flag.</returns>
		public bool IsEmpty() => _data.Count == 0;
	}
}