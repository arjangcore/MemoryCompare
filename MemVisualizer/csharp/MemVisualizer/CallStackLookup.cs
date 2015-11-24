// Callstack lookup logic
//
// Spawns out to Symmogrifier to get symbols.
//
using System;

namespace MemVisualizer
{
	/// <summary>
	/// Summary description for CallStackLookup.
	/// </summary>
	public enum ExeType
	{
		SYMOGRIFIER = 0,
		ADDR2LINE	
	};
	public class CallStackLookup
	{
		//-------------------------------------------------------------------
		public CallStackLookup()
		{
		}

		//-------------------------------------------------------------------
		public void Clear()
		{
			mCache.Clear();
		}

		//-------------------------------------------------------------------
		public void Index(MemManager.Log.Log lg)
		{
			System.Collections.Hashtable cache = new System.Collections.Hashtable(4096);
			int processed = 0;

			System.Text.StringBuilder addresses = new System.Text.StringBuilder();		
			addresses.EnsureCapacity(32768);

			// Work out all unique addresses
			for (int i = 0; i < lg.Count; i++)
			{
				int index = lg[i].stackTraceString;
				if (index <= processed)
					continue;
				
				processed = index;
				string callstack = lg.GetString(index);
				string[] addrarray = callstack.Split(' ');
				foreach (string s in addrarray)
				{
					if (!cache.ContainsKey(s))
					{
						cache[s] = s;
						addresses.Append(s);
						addresses.Append(' ');

						// Fix issue with nasty command line length issue on win32.
						if (addresses.Length >= 32000)
						{
							Index(addresses.ToString());

							// Wipe addresses
							addresses = new System.Text.StringBuilder();
							addresses.EnsureCapacity(32768);
						}
					}
				}
            }

			// Index unique addresses
			if (addresses.Length > 0)
				Index(addresses.ToString());
		}

		//-------------------------------------------------------------------
		public void Index(string addresses)
		{
			string lookup = String.Empty;
			string[] addrarray = addresses.Split(' ');

			// Populate tmpcache
			foreach (string s in addrarray)
			{
				if (s.StartsWith("0x"))
				{
					long key = System.Convert.ToInt64(s.Replace("0x",""),16);
					if (!mCache.ContainsKey(key))
					{	
						lookup += s + " ";
					}
				}
			}

			// If cache does not have all entries look some up.
			if (lookup != String.Empty)
			{
				System.Diagnostics.Process process = new System.Diagnostics.Process();
				process.StartInfo.FileName = mPath;
				if (mExe == ExeType.ADDR2LINE)	//use addrline2 instead of symogrifier therefore different command line arguments passed in.
					process.StartInfo.Arguments = string.Format("-e {0} {1} ",mMap, lookup);
				else
					process.StartInfo.Arguments = string.Format("-db:{0} -addr:\"{1}\"", mMap, lookup);
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.Start();

				string[] output = process.StandardOutput.ReadToEnd().Split('\r');
				process.WaitForExit();
				process.Close();

				// Collapse output of empty lines..
				int oi = 0;
				for (int i = 0; i < output.Length; i++)
				{
					string v = output[i].Replace("\r","").Replace("\n","");
					if (v.Length <= 1)
						continue;

					output[oi++] = v;
				}

				// Parse the output back into the cache
				string[] input = lookup.Split(' ');
				if(mExe == ExeType.ADDR2LINE)
				{
						for (int i = 0; i < oi; i++)
						{
							string key = input[i];
							string val = output[i].Replace("__cdecl", "");
							string filteredkey = key.Replace("0x","");
							Int64 keyint = System.Convert.ToInt64(filteredkey,16);
							mCache[keyint] = val;	
						}
				}

				else
				{
					for (int i = 0; i < oi; i++)
					{
						string key = output[i];
						if (key.Length >= 2 && key[0] == '0' && key[1] == 'x')
						{
							string val = output[i+1].Replace("__cdecl", "");
							if (!val.StartsWith("0x"))
							{
								int j = i+1;
								string curval = null;
								while (!val.StartsWith("0x")&& j < oi)
								{
									curval += "\r\n"+val;
									j++;
									val = (string)output[j];
								
								}					

								// Cache full text entry
								string filteredkey = key.Replace("0x","");
								Int64 keyint = System.Convert.ToInt64(filteredkey,16);
								mCache[ keyint ] = curval;
								i = j-1;
							}
							else
							{
								// Cache address only
								string filteredkey = key.Replace("0x","");
								Int64 keyint = System.Convert.ToInt64(filteredkey,16);
								mCache[ keyint ] = key;
							}
						}
					}
				}
			}
		}

		//-------------------------------------------------------------------
		public String LookUp(string addresses)
		{
			Index(addresses);

			// Now return result from cache
			string result = String.Empty;
			string[] addrarray = addresses.Split(' ');
			foreach (string s in addrarray)
			{
				if (s.Length != 0)
				{
					string keyfiltered = s.Replace("0x","");
					Int64 key =  System.Convert.ToInt64(keyfiltered,16);
					if (mCache.ContainsKey(key))
						result += String.Format("{0}\r\n", mCache[key]);
					else
						result += String.Format("{0}\r\n", key);
				}				
			}
			return result;
		}

		//-------------------------------------------------------------------
		public void SetExePath(string p)
		{
			mPath = p;
		}

		//-------------------------------------------------------------------
		public void SetMapPath(string p)
		{
			mMap = p;
		}

		//-------------------------------------------------------------------
		public void SetExeType(MemVisualizer.ExeType p)
		{
			mExe = p;
		}

		//-------------------------------------------------------------------
		public MemVisualizer.ExeType GetExeType ()
		{
			return mExe;
		}

		//-------------------------------------------------------------------
		System.Collections.Hashtable mCache = new System.Collections.Hashtable(4096);
		string mMap;
		string mPath;
		ExeType mExe;
	}
}
