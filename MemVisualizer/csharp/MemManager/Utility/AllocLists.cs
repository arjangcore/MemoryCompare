using System;
using System.Collections;

namespace MemManager
{
	namespace Utility
	{
		/// <summary>
		/// Fast allocation lookup table
		/// </summary>
		class AllocLists
		{
			static int kSize = 17389;
			static uint kDivider = 64;
			ArrayList activeAlloc = new ArrayList(kSize);
	
			struct AddressIndex
			{
				public uint address;
				public int index;
			};

			public AllocLists()
			{
				for (int i = 0; i < kSize; i++)
					activeAlloc.Add( new ArrayList(16) );
			}

			public void Allocate(uint address, int index)
			{
				uint hash = (address / kDivider) % (uint)(kSize);
				ArrayList lst = (ArrayList)(activeAlloc[(int)(hash)]);

				AddressIndex ai;
				ai.address = address;
				ai.index = index;

				lst.Add(ai);
			}

			public int Free(uint address)
			{
				uint hash = (address / kDivider) % (uint)(kSize);
				ArrayList lst = (ArrayList)(activeAlloc[(int)(hash)]);

				for (int j = 0; j < lst.Count; j++)
				{
					AddressIndex logentry = (AddressIndex)(lst[j]);
					if (logentry.address == address)
					{
						lst.RemoveAt(j);
						return logentry.index;
					}
				}
//discard, as can be due to incomplete log				throw new Exception("Free of item that is not in allocation list!");
				return 0;
			}

			public ArrayList GetArray()
			{
				ArrayList l = new ArrayList(kSize);
				for (int i = 0; i < kSize; i++)
				{
					ArrayList lst = (ArrayList)(activeAlloc[i]);
					if (lst.Count > 0)
					{
						for (int j = 0; j < lst.Count; j++)
						{
							AddressIndex logentry = (AddressIndex)(lst[j]);
							l.Add( logentry.index );
						}
					}
				}
				l.TrimToSize();
				return l;
			}
		}
	}
}
