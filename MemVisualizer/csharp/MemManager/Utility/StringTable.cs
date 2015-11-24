using System;
using System.Collections;

namespace MemManager
{
	namespace Utility
	{
		/// <summary>
		/// String table
		/// </summary>
		class StringTable
		{
			struct StringEntry
			{
				public int index;
			};

			public int Add(string str)
			{
				if (mStringsTable.ContainsKey(str))
				{
					return ((StringEntry)mStringsTable[str]).index;
				}

				StringEntry se;
				se.index = mStrings.Count;

				mStrings.Add(str);
				mStringsTable[str] = se;
				return se.index;
			}

			public void AddUnique(string str)
			{
				StringEntry se;
				se.index = mStrings.Count;

				mStrings.Add(str);
				mStringsTable[str] = se;
			}

			public int Count
			{
				get { return mStrings.Count; }
			}

			public string this[int index]
			{
				get 
				{
					return (string)(mStrings[index]);
				}
			}

			public void Clear()
			{
				mStrings.Clear();
				mStringsTable.Clear();
			}

			public void TrimToSize()
			{
				mStrings.TrimToSize();
			}

			ArrayList mStrings = new ArrayList(16384);
			Hashtable mStringsTable = new Hashtable(16384);
		};
	}
}