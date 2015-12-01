using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace MemCompare
{ // test change for automatic build test
	// a second change to test.
	class Program
	{
		class Allocation : IComparable
		{
			public string name;
			public string pool;
			public int size;
			public int ptr;
		
			int IComparable.CompareTo(object x)
			{
				Allocation a = (Allocation)x;
				return a.size - size;
			}
		}

		class Category
		{
			public Category()
			{
				name = "";
				size = 0;
				count = 0;
			}
			public string name;
			public int size;
			public int count;
		}

		class TextFileInfo
		{
			public string filename;
			public List<Allocation> allocations = new List<Allocation>();
			public Dictionary<String, Category> categories = new Dictionary<String, Category>();
			public Dictionary<String, Category> blocks = new Dictionary<String, Category>();
		}

		TextFileInfo ReadTextFile(string filename)
		{
			Console.WriteLine("ReadTextFile " + filename);
			char[] seperatorTab = {'\t'};
			char[] seperatorColon = {':'};
			TextFileInfo fi = new TextFileInfo();
			fi.filename = filename;
			string currentPool = "";
			StreamReader re = File.OpenText(filename);
			string input = null;
			while ((input = re.ReadLine()) != null)
			{
				input.Trim();
				if (input.StartsWith("MemPool:"))
					currentPool = input.Substring(8);
				else
				{
					string[] line = input.Split(seperatorTab);
					int size = int.Parse(line[2]);
					Allocation a = new Allocation();
					a.name = line[0];
					a.ptr = Int32.Parse(line[1], System.Globalization.NumberStyles.AllowHexSpecifier); 
					a.pool = currentPool;
					a.size = size;
					fi.allocations.Add(a);

					// Handle blocks (identically named allocations)
					Category c;
					if (fi.blocks.ContainsKey(line[0]))
						c = fi.blocks[line[0]];
					else
						c = new Category();
					c.name = line[0];
					c.size += size;
					++c.count;

					fi.blocks[line[0]] = c;

					// Handle categories
					string catString = "NO CATEGORY";
					if (line[0].Contains(":"))
						catString = line[0].Split(seperatorColon)[0];

					if (fi.categories.ContainsKey(catString))
						c = fi.categories[catString];
					else
						c = new Category();
					c.name = catString;
					c.size += size;
					++c.count;

					fi.categories[catString] = c;
				}
			}
			re.Close();
			return fi;
		}

		
		void Run(string[] args)
		{
			int lastAllocation = 0;
			if (args.Length == 0)
			{
				Console.WriteLine("usage: memcompare input1.txt [input2.txt]");
				return;
			}

			TextFileInfo f1 = null;
			TextFileInfo f2 = null;

			f1 = ReadTextFile(args[0]);
			if (args.Length > 1)
				f2 = ReadTextFile(args[1]);

			Console.WriteLine("CATEGORIES       SIZE      COUNT");

			string[] allKeys = new string[f1.categories.Keys.Count];
			f1.categories.Keys.CopyTo(allKeys, 0);
			Array.Sort(allKeys);
			foreach (string cName in allKeys)
			{
				Category c = f1.categories[cName];
				Console.WriteLine(String.Format("{0,-15} {1, 8} {2,5}", c.name, c.size, c.count));
			}

			Console.WriteLine("\n\nBLOCKS >20k                        SIZE  COUNT");
			allKeys = new string[f1.blocks.Keys.Count];
			f1.blocks.Keys.CopyTo(allKeys, 0);
			Array.Sort(allKeys);
			foreach (string cName in allKeys)
			{
				Category c = f1.blocks[cName];
				if (c.size > 20 * 1024)
					Console.WriteLine(String.Format("{0,-30} {1, 8}  {2,5}", c.name, c.size, c.count));
			}

			int totalAllocations = 0;
			Console.WriteLine("\n\nALLOCATIONS >20k                   SIZE");
			ArrayList topAllocations = new ArrayList();
			foreach (Allocation a in f1.allocations)
			{
				if (a.size > 20 * 1024)
					topAllocations.Add(a);

				totalAllocations += a.size;
			}

			topAllocations.Sort();
					
			foreach (Allocation a in topAllocations)
			{
				Console.WriteLine(String.Format("{0,-30} {1, 8}", a.name, a.size));
			}

			Console.WriteLine("\n\nTOTAL ALLOCATIONS : " + totalAllocations);
			Console.WriteLine("\nFINAL ALLOCATION : " + lastAllocation);
		}

		static void Main(string[] args)
		{
			Program app = new Program();
			app.Run(args);
		}
	}
}
