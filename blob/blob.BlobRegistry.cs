using System;
using Microsoft.Win32;

namespace blob
{
	public class BlobRegistry
	{
		public static RegistryHive GetRegistrySoftwareClassesHive()
		{
			RegistryHive retval = RegistryHive.CurrentUser;

			try
			{
				RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\writetest");
				key.Close();
				Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Classes\writetest");
				retval = RegistryHive.LocalMachine;
				return retval;
			}
			catch (Exception)
			{
				RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes", true);
				retval = RegistryHive.CurrentUser;
				return retval;
			}
		}

		public static RegistryKey OpenRegistryHive(RegistryHive hive)
		{
			switch (hive)
			{
				case RegistryHive.ClassesRoot:
					return Registry.ClassesRoot;

				case RegistryHive.CurrentUser:
					return Registry.CurrentUser;

				case RegistryHive.LocalMachine:
					return Registry.LocalMachine;

				case RegistryHive.Users:
					return Registry.Users;

				case RegistryHive.PerformanceData:
					return Registry.PerformanceData;

				case RegistryHive.CurrentConfig:
					return Registry.CurrentConfig;

				case RegistryHive.DynData:
					return Registry.DynData;

				default:
					return null;
			}
		}

		public static void CreateKey(RegistryHive hive, string key)
		{
			using (RegistryKey created = BlobRegistry.OpenRegistryHive(hive).CreateSubKey(key))
			{
				created.Close();
			}
		}

		public static void SetValue(RegistryHive hive, string key, string name, object val)
		{
			using (RegistryKey created = BlobRegistry.OpenRegistryHive(hive).OpenSubKey(key, true))
			{
				created.SetValue(name, val);
				created.Flush();
			}
		}

		public static void CreateKeyWithValue(RegistryHive hive, string key, string name, object val)
		{
			CreateKey(hive,key);
			SetValue(hive,key,name,val);
		}

	}
}
