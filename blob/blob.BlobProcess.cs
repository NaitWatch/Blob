using System;
using System.Diagnostics;

namespace blob
{
	/// <summary>
	/// Summary description for blob.
	/// </summary>
	public class BlobProcess
	{

		public static void launch(bool nowindow,bool admin,string file,string args)
		{
			
			byte[] reqadminexe = BlobReflection.GetEmbeddedNetExeFromResource("reqadmin.exe","reqadmin.app",!admin);

			string temppath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetTempFileName()));
			BlobDirectory.CreateDirectoryIfNotExists(temppath);
			string writeloc = System.IO.Path.Combine(temppath,"reqadmin.exe");

			using (BlobFileStream stream = new BlobFileStream(writeloc,blob.BlobFileStream.Type.Writer))
			{
				stream.Write(reqadminexe);
			}

			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo.FileName = writeloc;
			if (nowindow)
			{
				proc.StartInfo.Arguments = @" ""n"" """+file+@""" """+args+@"""  ";
			}
			else
			{
				proc.StartInfo.Arguments = @" """" """+file+@""" """+args+@"""  ";
			}

			proc.Start();
			int pid = proc.Id;
			proc.WaitForExit();
			try
			{
				System.Diagnostics.Process procx = System.Diagnostics.Process.GetProcessById(pid);
				procx.Kill();
			}
			catch(Exception)
			{
			}

			try
			{
				System.IO.Directory.Delete(temppath,true);
			}
			catch(Exception)
			{
			}

		}

		public static byte[] ChangeManifestToInvoker(byte[] exe)
		{
			byte[] exlevelfind = System.Text.Encoding.ASCII.GetBytes(@"<requestedExecutionLevel level=""requireAdministrator"" uiAccess=""false"">");
			byte[] exlevel =     System.Text.Encoding.ASCII.GetBytes(@"<requestedExecutionLevel level=""asInvoker""            uiAccess=""false"">");
			long pos = BlobArray.Position(exe,exlevelfind);
			if (pos != -1)
			{
				Array.Copy(exlevel,0,exe,(int)pos,exlevel.Length);
			}
			return exe;
		}

		public static void Delete(string arg,string pid)
		{
			byte[] deleteexe = BlobReflection.GetManifestResourceByName("blobdel.exe");
			string loc = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "blobdel.exe");
			using (BlobFileStream stream = new BlobFileStream(loc,blob.BlobFileStream.Type.Writer))
			{
				stream.Write(deleteexe);
			}
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo.FileName = loc;
			proc.StartInfo.Arguments = @" """+arg+@""" """+pid+@""" ";
			proc.Start();
		}

	}

	public class vb
	{

		private static string FindIndexedProcessName(int pid) 
		{
			string processName = Process.GetProcessById(pid).ProcessName;
			Process[] processesByName = Process.GetProcessesByName(processName);
			string processIndexdName = null;

			for (int index = 0; index < processesByName.Length; index++) 
			{
				processIndexdName = index == 0 ? processName : processName + "#" + index;
				PerformanceCounter processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
				if ((int) processId.NextValue() == pid) 
				{
					return processIndexdName;
				}
			}

			return processIndexdName;
		}

		private static System.Diagnostics.Process FindPidFromIndexedProcessName(string indexedProcessName) 
		{
			System.Diagnostics.PerformanceCounter parentId = new System.Diagnostics.PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
			return Process.GetProcessById((int) parentId.NextValue());
		}
		/*
				public static Process Parent(this Process process) 
				{
					return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
				}
				*/

		public static int Parentxxx() 
		{
			return FindPidFromIndexedProcessName(FindIndexedProcessName(System.Diagnostics.Process.GetCurrentProcess().Id)).Id;
		}
	

	}
}
