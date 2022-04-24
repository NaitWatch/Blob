using System;

namespace reqadmin
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(blob.BlobException.BlobUnhandledException);

			if (args.Length == 3)
			{
				string option = args[0];
				string filename = args[1];
				string arguments = args[2];

				System.Diagnostics.Process proc = new System.Diagnostics.Process();

				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.FileName = filename.Trim();
				proc.StartInfo.Arguments = arguments.Trim();
				proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(proc.StartInfo.FileName);

				if (option == "n")
				{
					proc.StartInfo.CreateNoWindow = true;
				}

				proc.Start();
				proc.WaitForExit();
			}
		}
	}
}
