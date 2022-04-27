using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace blob
{
	public class BlobInstall
	{
		public static void Install()
		{
			string application = InstallToAppDir("blob");
			ShellRegistry(application);
		}

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
