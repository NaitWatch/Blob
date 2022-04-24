using System;

namespace blobstub
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
			string loc = System.Reflection.Assembly.GetEntryAssembly().Location;
			blob.BlobSplit.Split(loc);
		}
	}
}
