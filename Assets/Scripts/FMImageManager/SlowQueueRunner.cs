// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;

namespace FMImageManager
{
	internal class SlowQueueRunner : IQueueRunner
	{
		public void Enqueue(CacheTask task)
		{
			this.queue.Enqueue(task);
		}

		public IEnumerator QueueProcessor()
		{
			for (;;)
			{
				if (this.queue.Count > 0)
				{
					CacheTask task = this.queue.Dequeue();
					yield return task.Run();
				}
				else
				{
					yield return 0;
				}
			}
		}

		private Queue<CacheTask> queue = new Queue<CacheTask>();
	}
}
