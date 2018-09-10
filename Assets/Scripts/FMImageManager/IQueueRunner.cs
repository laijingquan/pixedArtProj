// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;

namespace FMImageManager
{
	internal interface IQueueRunner
	{
		void Enqueue(CacheTask task);

		IEnumerator QueueProcessor();
	}
}
