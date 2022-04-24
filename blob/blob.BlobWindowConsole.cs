using System;
using System.Runtime.InteropServices;
using System.IO;

namespace blob
{
	public class BlobWindowConsole
	{

		[DllImport("kernel32.dll")]
		private static extern int AllocConsole();

		[DllImport("kernel32.dll")]
		private static extern IntPtr CreateFile(
			string lpFileName,
			uint dwDesiredAccess,
			uint dwShareMode,
			IntPtr lpSecurityAttributes,
			uint dwCreationDisposition,
			uint dwFlagsAndAttributes,
			IntPtr hTemplateFile
			);

		private const uint GENERIC_WRITE = 0x40000000;
		private const uint FILE_SHARE_WRITE = 0x00000002;
		private const uint OPEN_EXISTING = 0x00000003;
		private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

		public static void Initialize()
		{
			if (AllocConsole() != 0)
			{
				InitializeOutStream();
			}
		}

		private static void InitializeOutStream()
		{
			FileStream fileStream = CreateFileStream("CONOUT$", GENERIC_WRITE, FILE_SHARE_WRITE, FileAccess.Write);
			if (fileStream != null)
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.AutoFlush = true;
				Console.SetOut(streamWriter);
				Console.SetError(streamWriter);
			}
		}

		private static FileStream CreateFileStream(string name, uint dwDesiredAccess, uint dwShareMode, FileAccess fileAccess)
		{
			IntPtr handle = CreateFile(name, dwDesiredAccess, dwShareMode, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
			if (handle != IntPtr.Zero)
			{
				FileStream fileStream = new FileStream(handle, fileAccess);
				return fileStream;
			}
			return null;
		}


	}
}
