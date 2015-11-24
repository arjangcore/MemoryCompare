using System;
using System.Collections;

namespace MemManager
{
	namespace Log
	{
		public class SnapShot
		{
			static System.Threading.Mutex mAllocListsMutex = new System.Threading.Mutex();
			static Utility.AllocLists mAllocLists;
			static int mAllocListsLastIndex;

			ArrayList mArray;		
			int mIndex;
			bool mSorted = false;

			public SnapShot(Log log, int index)
			{
				mIndex = index;
				Utility.AllocLists li = new Utility.AllocLists();
				
				// Play thru allocations till index reached.
				for (int i = 0; i < index; i++)
				{
					LogEntry logentry = log[i];
					if (logentry.type == 'A')
						li.Allocate(logentry.address, i);
					else if (logentry.type == 'F')
						li.Free(logentry.address);
				}

				// Now store as an array
				mArray = li.GetArray();
				mSorted = false;
				mAllocListsMutex.WaitOne();
				mAllocListsLastIndex = index;
				mAllocLists = li;
				mAllocListsMutex.ReleaseMutex();
			}

			public SnapShot(Log log, int index, SnapShot prev)
			{
				if (prev.mIndex > index)
					throw new Exception("Snapshot passed needs to be behind current one");

				mIndex = index;
				Utility.AllocLists li;

				mAllocListsMutex.WaitOne();
				if (mAllocListsLastIndex == prev.mIndex)
				{
					li = mAllocLists;
					mAllocListsMutex.ReleaseMutex();
				}
				else
				{
					mAllocListsMutex.ReleaseMutex();

					// Populate snapshot with previous snapshot
					li = new Utility.AllocLists();
					for (int i = 0; i < prev.mArray.Count; i++)
					{
						int idx = (int)(prev.mArray[i]);
						uint address = log[idx].address;
						li.Allocate( address, idx );
					}
				}

				// Play through allocations till index reached.
				for (int i = prev.mIndex; i < index; i++)
				{
					LogEntry logentry = log[i];
					if (logentry.type == 'A')
						li.Allocate(logentry.address, i);
					else if (logentry.type == 'F')
						li.Free(logentry.address);
				}

				// Now store as an array
				mArray = li.GetArray();
				mSorted = false;

				mAllocListsMutex.WaitOne();
				mAllocListsLastIndex = index;
				mAllocLists = li;
				mAllocListsMutex.ReleaseMutex();
			}

			class CompareAddresses : IComparer
			{
				int System.Collections.IComparer.Compare(object x, object y)
				{
					LogEntry a = (LogEntry)x;
					LogEntry b = (LogEntry)y;
					return ((int)a.address - (int)b.address);
				}
			};

			public void Sort()
			{
				if (mSorted)
					return;
				mArray.Sort( new CompareAddresses() );
				mSorted = true;
			}

			public int this[int index]
			{
				get { return (int)mArray[index]; }
			}

			public int Count
			{
				get { return mArray.Count; }
			}
		}
	}
}

