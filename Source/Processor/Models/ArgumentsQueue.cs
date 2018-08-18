﻿using System.Collections.Generic;

namespace R5.RunInfoBuilder.Processor.Models
{
    internal class ArgumentsQueue
    {
		private Queue<string> _queue { get; }

		internal ArgumentsQueue(string[] args)
		{
			_queue = new Queue<string>(args);
		}

		internal bool HasNext() => _queue.Count > 0;

		internal string Peek() => _queue.Peek();

		internal string Dequeue() => _queue.Dequeue();
    }
}