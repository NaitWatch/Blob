using System;
using System.Runtime.InteropServices;
using System.IO;

namespace blobdel
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				return;
			}

			FileAttributes attr = FileAttributes.Offline;
			try
			{
				// get the file attributes for file or directory
				attr = File.GetAttributes(args[0]);
			}
			catch(Exception)
			{
				return;
			}

			bool pidexists = true;

			do
			{
				try
				{
					System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById(System.Convert.ToInt32(args[1]));
					System.Threading.Thread.Sleep(1000);
				}
				catch
				{
					pidexists = false;
				}

			} while (pidexists);


			//detect whether its a directory or file
			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
			{
				if (System.IO.Directory.Exists(args[0]))
				{	
					try
					{
						System.IO.Directory.Delete(args[0],true);
					}
					catch(Exception)
					{
					}
				}
			}
			else
			{
				if ((attr & FileAttributes.Directory) != FileAttributes.ReadOnly)
				{
					if (System.IO.File.Exists(args[0]))
					{
						try
						{
							System.IO.File.Delete(args[0]);
						}
						catch(Exception)
						{
						}
					}
				}
			}
		}
	}
}
