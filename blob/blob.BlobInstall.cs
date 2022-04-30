using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace blob
{
	public class BlobInstall
	{
		//TODO: Rework added ugly enviroment path and InstallToAppdir, check if started from cmd with path
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessageTimeout(
			IntPtr hWnd,
			uint Msg,
			UIntPtr wParam,
			string lParam,
			uint fuFlags,
			uint uTimeout,
			out UIntPtr lpdwResult);

		//TODO: Rework added ugly enviroment path and InstallToAppdir, check if started from cmd with path
		public static void Install(bool showform)
		{
			string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string TargetDirectory = Path.Combine(LocalAppData, "blob");
			string TargetFile = Path.Combine(TargetDirectory,Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location));

			if (TargetFile == System.Reflection.Assembly.GetEntryAssembly().Location)
			{
				return;
			}

			string application = InstallToAppDir("blob");

			ShellRegistry(application);
			if (showform)
			{
				System.Windows.Forms.Application.Run(new blob.blobInstallForm());
			}

			string applicationDir = System.IO.Path.GetDirectoryName(application);
			string[] env = blob.BlobRegistry.ReadEnvironment();
			bool appIsPath = false;
			for (int i = 0;i < env.Length;i++)
			{
				if (applicationDir.ToLower(System.Globalization.CultureInfo.InvariantCulture) == env[i].ToLower(System.Globalization.CultureInfo.InvariantCulture))
				{
					appIsPath = true;
					break;
				}
			}

			if (!appIsPath)
			{
				blob.BlobRegistry.AppendEnvironment(applicationDir);

				UIntPtr RESULT;
				SendMessageTimeout(new IntPtr(0xffff), 0x001A, UIntPtr.Zero, "Environment", 0x2, 15000, out RESULT);
			}

		}

		//TODO: Rework added ugly enviroment path and InstallToAppdir, check if started from cmd with path
		private static string InstallToAppDir(string AppDir)
		{
			string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string TargetDirectory = Path.Combine(LocalAppData, AppDir);
			BlobDirectory.CreateDirectoryIfNotExists(TargetDirectory);
			string TargetFile = Path.Combine(TargetDirectory,Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location));
			System.IO.File.Copy(Assembly.GetEntryAssembly().Location,TargetFile,true);
			if (System.IO.File.Exists(Program.configfile))
			{
				System.IO.File.Delete(Program.configfile);
			}
			return TargetFile;
		}

		private static void ShellRegistry(string appLocation)
		{
			RegisterExtensions(".blo", "blobfile");
			RegisterExtensions(".blob", "blobfile");
			RegisterExtensionCommandIconArgument("blobfile", appLocation , "/shell_extract");
			RegisterShellDirectoryEntry("blobshellcreate", appLocation ,"/shell_create_blob", "Blob create");
			RegisterShellDirectoryEntry("blobshellconfig", appLocation, "/shell_config", "Blob config");
		}

		public static void RegisterExtensions(string fileExtension, string regEntryLocation)
		{
			RegistryHive hive = BlobRegistry.GetRegistrySoftwareClassesHive();
			BlobRegistry.CreateKeyWithValue(hive,@"SOFTWARE\Classes\" + fileExtension, null ,regEntryLocation);
		}

		private static void RegisterExtensionCommandIconArgument(string fileExtension, string location,string argument)
		{
			RegistryHive hive = BlobRegistry.GetRegistrySoftwareClassesHive();
			string key = @"SOFTWARE\Classes\" + fileExtension;
			string openCommand = @"""" + location + @""" "+argument+@" ""%1""";
			BlobRegistry.CreateKeyWithValue(hive, key + @"\shell\open\command",null,openCommand);
			string defaultIcon = @"" + location + @",0";
			BlobRegistry.CreateKeyWithValue(hive, key + @"\DefaultIcon", null, defaultIcon);
		}

		private static void RegisterShellDirectoryEntry(string entry, string location, string argument, string description)
		{
			RegistryHive hive = BlobRegistry.GetRegistrySoftwareClassesHive();
			string key = @"SOFTWARE\Classes\Directory\shell\" + entry;
			string command = @"""" + location + @"""" + @" """ + argument + @""" " + @"""" + "%1" + @"""";
			string icon = @"" + location + @",0";
			BlobRegistry.CreateKeyWithValue(hive, key + @"\command", null, command);
			BlobRegistry.CreateKeyWithValue(hive, key , "Icon", icon);
			BlobRegistry.CreateKeyWithValue(hive, key , null, description);
		}

	}
}
