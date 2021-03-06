using System;
using System.Runtime.InteropServices;
using System.IO;

namespace blob
{

	//
	public class BlobWindowConsole
	{
		const uint WM_CHAR = 0x0102;
		const int VK_ENTER = 0x0D;

		[DllImport("kernel32.dll",SetLastError=true)]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll",SetLastError=true)]
		static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll",SetLastError=true)]
		[return: MarshalAs(UnmanagedType.U1)]
		private static extern bool AllocConsole();

		[DllImport("kernel32.dll",SetLastError=true)]
		[return: MarshalAs(UnmanagedType.U1)]
		private static extern bool AttachConsole(int dwProcessId);

		[DllImport("kernel32.dll", SetLastError=true, ExactSpelling=true)]
		private static extern bool FreeConsole();


		[DllImport("kernel32.dll",SetLastError=true)]
		private static extern IntPtr CreateFile(
			string lpFileName,
			uint dwDesiredAccess,
			uint dwShareMode,
			IntPtr lpSecurityAttributes,
			uint dwCreationDisposition,
			uint dwFlagsAndAttributes,
			IntPtr hTemplateFile
			);

		private const uint ATTACH_PARRENT = 0xFFFFFFFF;
		private const int ATTACH_PARRENT_DWORD = -1;
		private const uint GENERIC_WRITE = 0x40000000;
		private const uint FILE_SHARE_WRITE = 0x00000002;
		private const uint OPEN_EXISTING = 0x00000003;
		private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

		public static void InitializeNew()
		{
			IntPtr con = GetConsoleWindow();
			bool ConsoleNotExists = (con == IntPtr.Zero);
			if (ConsoleNotExists)
			{
				bool AttachSuccess = AttachConsole(ATTACH_PARRENT_DWORD);
				if (AttachSuccess)
				{
					InitializeOutStream();
				}
				else
				{
					bool AllocSuccess = AllocConsole();
					if (AllocSuccess)
					{
						InitializeOutStream();
					}
				}
			}


		}

		public static void CloseConsole()
		{
			IntPtr cw = GetConsoleWindow();
			if (cw != IntPtr.Zero)
			{
				SendMessage(cw, WM_CHAR, (IntPtr)VK_ENTER, IntPtr.Zero);
				FreeConsole();
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
