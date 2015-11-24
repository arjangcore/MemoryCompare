using System;
using System.IO;
using System.Collections;
using System.Xml;

namespace MemManager
{
	namespace Log
	{
		/// <summary>
		/// Log file reader for TCR format
		/// </summary>
		public class TCRLoader
		{
			class Header
			{
				public string title;
				public string version;
				public string description;
				public string platform;
				public string configuration;
				public string timestamp;

				public int mAddressIndex;
				public int mCategoryIndex;
				public int mAllocSizeIndex;
				public int mAlignmentIndex;
				public int mStackTraceIndex;
				public int mNumberIndex;
				public int mNameIndex;
			};
			Header mHeader = new Header();
			Log mLog;
            public uint GHeapMin;
            public uint GHeapMax;

			//-------------------------------------------------------------------------
			public TCRLoader()
			{
			}

			//-------------------------------------------------------------------------
			~TCRLoader()
			{
			}

			//--------------------------------------------------------------------------
			// Parse the XML header of the file to work out the format.
			void ParseXMLHeader(string xml)
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.LoadXml(xml);

				// Parse header
				XmlNodeList header = xDoc.GetElementsByTagName("Header");
				foreach (XmlNode n in header[0])
				{
					if (n.Name == "Title")
						mHeader.title = n.InnerXml;
					else if (n.Name == "Version")
						mHeader.version = n.InnerXml;
					else if (n.Name == "Description")
						mHeader.description = n.InnerXml;
					else if (n.Name == "Platform")
						mHeader.platform = n.InnerXml;
					else if (n.Name == "Configuration")
						mHeader.configuration = n.InnerXml;
					else if (n.Name == "Timestamp")
						mHeader.timestamp = n.InnerXml;
				}

				// Parse data fields
				mHeader.mAddressIndex = -1;
                mHeader.mCategoryIndex = -1;
				mHeader.mAllocSizeIndex = -1;
				mHeader.mAlignmentIndex = -1;
				mHeader.mStackTraceIndex = -1;
				mHeader.mNumberIndex = -1;
		//		mHeader.mTemporaryIndex = -1;
				mHeader.mNameIndex = -1;

				XmlNodeList dataFields = xDoc.GetElementsByTagName("DataFields");
				int i =0;
			
				foreach (XmlNode n in dataFields[0])
				{
					foreach (XmlNode m in n)
					{
						if (m.Name == "Name")
						{
							string name = m.InnerXml;
					
							mLog.AddFieldName(name);
							if (name == "Address")
								mHeader.mAddressIndex = i;
							else if (name == "Alignment")
								mHeader.mAlignmentIndex = i;
							else if (name == "Category")
                                mHeader.mCategoryIndex = i;
							else if (name == "AllocSize")
								mHeader.mAllocSizeIndex = i;
							else if (name == "Name")
								mHeader.mNameIndex = i;
							else if (name == "StackTrace")
								mHeader.mStackTraceIndex = i;
							else if (name == "Number")
								mHeader.mNumberIndex = i;
						}
					}
					i++;
				}

                // read in allocators and their ranges
				XmlNodeList allocators = xDoc.GetElementsByTagName("HeapRanges");
				if (allocators.Count != 0)
				{
					foreach (XmlNode n in allocators[0])
					{
						if (n.Name == "Heap")
						{
							LogEntryAllocators entry = new LogEntryAllocators();
							string tempUsedToConverToInt;
							foreach (XmlNode m in n)
							{
								if (m.Name == "Name")
								{	
									entry.mName = m.InnerXml;
								}
								else if (m.Name == "Start")
								{
									if (m.InnerText != string.Empty)
									{
										tempUsedToConverToInt = m.InnerText.Replace("0x","");
										entry.mStartAddress = UInt32.Parse(tempUsedToConverToInt,System.Globalization.NumberStyles.HexNumber);
									}
								}							
								else if (m.Name == "End")
								{
									if(m.InnerText != string.Empty)
									{
										tempUsedToConverToInt = m.InnerText.Replace("0x","");
										entry.mEndAddress = UInt32.Parse(tempUsedToConverToInt,System.Globalization.NumberStyles.HexNumber);
									}
								}
							}
							mLog.AddAllocatorEntry(entry);
						}
					}
				}
				
	            // read in categories names:
                XmlNodeList categories = xDoc.GetElementsByTagName("Categories");
                if (categories.Count != 0)
                {
                    foreach (XmlNode n in categories[0])
                    {
                        if (n.Name == "Name")
                        {
                            string catname = n.InnerXml;
                            mLog.AddCategory(catname);
                        }
                    }
                }

			}

			//-------------------------------------------------------------------------
			// Utility function to convert string to UInt
			static uint StringToUInt(string str)
			{
				if (str[0] <= ' ')
					str = str.Trim();
				int radix = 10;
				if (str.Length > 2 && str[0] == '0' && str[1] == 'x')
				{
					radix = 16;
				}
				Int64 i = System.Convert.ToInt64(str, radix);
				return (uint)(i);
			}

			//-------------------------------------------------------------------------
			// Utility function to convert string to Int64
			static Int64 StringToInt64(string str)
			{
				if (str[0] <= ' ')
					str = str.Trim();
				int radix = 10;
				if (str.Length > 2 && str[0] == '0' && str[1] == 'x')
				{
					radix = 16;
				}
				Int64 i = Convert.ToInt64(str, radix);
				return i;
			}

			//-------------------------------------------------------------------------
			// Handle an allocation line
			void HandleAlloc(string str)
			{
				if (str.IndexOf("ALLOC,") == 0)
				{
                    LogEntry l = new LogEntry();
                    try
                    {
                        l.type = 'A';
                        string[] splitStr = str.Split(',');

                        l.address = 0;
                        if (mHeader.mAddressIndex != -1)
                        {
                            l.address = StringToUInt(splitStr[mHeader.mAddressIndex + 1]);
                        }
                        l.number = (Int32)(0);
                        if (mHeader.mNumberIndex != -1)
                        {
                            l.number = StringToUInt(splitStr[mHeader.mNumberIndex + 1]);
                        }
                        l.category = 0;
                        if (mHeader.mCategoryIndex != -1)
                        {
                            l.category = (byte)StringToUInt(splitStr[mHeader.mCategoryIndex + 1]);
                        }
                        l.allocSize = 0;
                        if (mHeader.mAllocSizeIndex != -1)
                        {
                            l.allocSize = StringToUInt(splitStr[mHeader.mAllocSizeIndex + 1]);
                        }
                        l.alignment = 0;
                        if (mHeader.mAlignmentIndex != -1)
                        {
                            l.alignment = StringToUInt(splitStr[mHeader.mAlignmentIndex + 1]);
                        }

                        //find the allocator for this alloc.
                        l.allocator = GetAllocatorIndex(l.address, l.address + l.allocSize);
                        GHeapMin = Math.Min(GHeapMin, l.address);
                        GHeapMax = Math.Max(GHeapMax, l.address + l.allocSize);

                        l.nameString = -1;
                        if (mHeader.mNameIndex != -1)
                        {
                            string name = splitStr[mHeader.mNameIndex + 1];
                            l.nameString = mLog.AddString(name);
                        }
                        l.stackTraceString = -1;
                        if (mHeader.mStackTraceIndex != -1)
                        {
                            l.stackTraceString = mLog.AddString(splitStr[mHeader.mStackTraceIndex + 1]);
                        }
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                       Console.WriteLine("IndexOutOFRangeException for %0x08x \n", l.address);
                    }
					mLog.Add(l);
				}
			}
            
            //find the index for the heap in which the memory is located
            public byte GetAllocatorIndex(uint StartAddress, uint EndAddress)
            {
                return mLog.GetAllocatorIndex(StartAddress, EndAddress);
            }

            public string GetAllocatorName(byte index)
            {
                return mLog.GetAllocator(index);
            }

			//-------------------------------------------------------------------------
			// Handle a free line
			void HandleFree(string str)
			{
				if (str.IndexOf("FREE,") == 0)
				{
					LogEntry l = new LogEntry();

					l.type = 'F';
					l.address = 0;
					l.number = (Int32)(0);
	//				l.reqSize = 0;
					l.allocSize = 0;
					l.alignment = 0;
					l.nameString = -1;
					l.stackTraceString = -1;
					l.category = 0;
                    l.allocator = 0;
					l.address = StringToUInt(str.Substring(5));
					mLog.Add(l);
				}
			}

			//-------------------------------------------------------------------------
			// Handle label creation
			void HandleLabel(string str)
			{
				if (str.IndexOf("LABEL,") == 0)
				{
					LogEntry l = new LogEntry();

					l.type = 'L';
					l.number = (Int32)(0);
				//	l.reqSize = 0;
					l.allocSize = 0;
					l.alignment = 0;
					l.nameString = -1;
					l.stackTraceString = -1;
					l.address = 0;
					l.category = 0;	// invalid
                    l.allocator = 0;

					string[] splitStr = str.Split(',');
					l.nameString = mLog.AddString(splitStr[1]);
					mLog.Add(l);

				}
			}

			//-------------------------------------------------------------------------
			void PopulateFreeInformation()
			{
                uint rangeStart = 0;
                uint rangeEnd = 0;
                if(mLog.Count > 0)
                {
                    rangeEnd = rangeStart = mLog[0].address;
                }
				Utility.AllocLists al = new Utility.AllocLists();
				for (int i = 0; i < mLog.Count; i++)
				{
					LogEntry l = mLog[i];
					if (l.type == 'A')
					{
                        rangeStart = l.address < rangeStart ? l.address : rangeStart;
                        rangeEnd = l.address > rangeEnd ? l.address : rangeEnd;
						al.Allocate(l.address, i);
					}
					else if (l.type == 'F')
					{
						int index = al.Free(l.address);
						LogEntry logentry = mLog[ index ];
						l.alignment = logentry.alignment;
						l.allocSize = logentry.allocSize;
						l.nameString = logentry.nameString;
						l.number = logentry.number;
					//	l.reqSize = logentry.reqSize;
						l.category = logentry.category;
                        l.allocator = logentry.allocator;
					}
				}

                // this is a fix for cases when we don't know the range for heap. Assumption here is that there is only one heap (i.e. Allocator)
				foreach (LogEntryAllocators e in mLog.GetAllocatorList())
				{
                    if(e.mStartAddress == 0 && e.mEndAddress == 0)
                    {
                        e.mStartAddress = rangeStart;
                        e.mEndAddress = rangeEnd;
                    }
				}
			}

			//-------------------------------------------------------------------------
			// Load and parse a log file
			public Log Load(string logname)
			{
				string line = String.Empty;
				DateTime start = DateTime.Now;
				
				// Load line by line to get XML header
				FileStream fs = File.Open(logname,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
				StreamReader stream = new StreamReader(fs);
				string xml = String.Empty;
				string format = String.Empty;
				for (;;)
				{
					line = stream.ReadLine();
					if (line == String.Empty || line == null)
						return null;

					if (line.IndexOf("<MetricsMemoryLog>") != -1)
					{
						format = "MetricsMemoryLog";
					}
					else if (line.IndexOf("<MetricsHeapDump>") != -1)
					{
						format = "MetricsHeapDump";
					}
					if (line.IndexOf("![CDATA[") != -1 || line.IndexOf("<Data>") != -1)
					{
						xml += "</" + format + ">";
						break;
					}

					xml += line;
				}

				mLog = new Log(format);


				if (format == String.Empty)
					return null;		// Didn't recognise format.
			
				// Parse log header as XML
				ParseXMLHeader(xml);

				// Skip down into CDATA Section
				if (line.IndexOf("<Data>") != -1)
				{
					while (line.IndexOf("![CDATA[") == -1)
					{
						line = stream.ReadLine();
					}
				}

				// Now read each string and parse it
                GHeapMin = uint.MaxValue;
                GHeapMax = uint.MinValue;
				try
				{
					if (format == "MetricsMemoryLog")
					{
						for (;;)
						{
							line = stream.ReadLine();
							if (line == null || line.Length == 0)
								break;
							if (line[0] == 'A')			// Alloc
							{
								HandleAlloc(line);
							}
							else if (line[0] == 'F')	// Free
							{
								HandleFree(line);
							}
							else if (line[0] == 'L')	// Label
							{
								HandleLabel(line);
							}
							else
							{
								line = line.Trim();
								if (line.IndexOf("]]>") != -1)
									break;				// Input finished!
							}
						}
					}
					else
					{
						for (;;)
						{
							line = stream.ReadLine();
							if (line == null || line.Length == 0)
								break;
							if (line.IndexOf("]]>") != -1)
								break;				// Input finished!
							HandleAlloc("ALLOC," + line);
						}
					}
					
	                // try read the full heap range
                    for (; ; )
                    {
                        line = stream.ReadLine();
                        if (line == null || line.Length == 0)
                            break;
                        if (line.IndexOf("<FullHeapRange>") != -1)
                        {
                            line = stream.ReadLine();
                            string[] splitStr = line.Split(',');

                            uint minHeapRange = StringToUInt(splitStr[0]);
                            uint maxHeapRange = StringToUInt(splitStr[1]);

                            break;
                        }
                    }

				} 
				catch (System.Exception e)
				{
					System.Console.WriteLine(e.ToString());
				}

				stream.Close();
				fs.Close();
				DateTime end = DateTime.Now;
				Console.WriteLine("Parsed {1} in {0} seconds", end-start, logname);

                // update the allocator range if it is the only one, so it is the global allocator
                if (GetAllocatorList().Count == 1)
                {
                    LogEntryAllocators alloc = (LogEntryAllocators)GetAllocatorList()[0];
                    alloc.mStartAddress = Math.Min(alloc.mStartAddress, GHeapMin);
                    alloc.mEndAddress = Math.Max(alloc.mEndAddress, GHeapMax);
                }

				// Do pass to flesh out Free information.
				start = DateTime.Now;
				try
				{
					PopulateFreeInformation();	// Its possible this will thro if incomplete log file is read
				}
				catch (System.Exception e)
				{
					System.Console.WriteLine(e.ToString());
				}

				end = DateTime.Now;
				Console.WriteLine("Populated Free items in {0} seconds", end-start);
				return mLog;
			}
			
			public ArrayList GetAllocatorList()
			{
				return mLog.GetAllocatorList();
			}

		}

	}
}
