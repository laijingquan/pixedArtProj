// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMImageManager
{
	internal class FastQueueRunner : IQueueRunner
	{
		public FastQueueRunner(Func<IEnumerator, Coroutine> coroutineProxy)
		{
			this.coroutineProxy = coroutineProxy;
		}

		public void Enqueue(CacheTask task)
		{
			this.queue.Enqueue(task);
		}

		public IEnumerator QueueProcessor()
		{
			while(true)
			{
				for (int i = 0; i < 3; i++)
				{
					if (!this.Dequeue())
					{
						break;
					}
				}
				yield return 0;
			}
		}

		private bool Dequeue()
		{
			if (this.queue.Count > 0)
			{
				CacheTask cacheTask = this.queue.Dequeue();
				this.coroutineProxy(cacheTask.Run());
				return true;
			}
			return false;
		}

		private const int TASKS_PER_FRAME = 3;

		private Queue<CacheTask> queue = new Queue<CacheTask>();

		private Func<IEnumerator, Coroutine> coroutineProxy;
	}
}
