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

		//TODO: NotWorking win10
		public static RegistryHive GetRegistrySoftwareClassesHive2()
		{
			try
			{
				RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\test");
				key.Close();
				Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\test");
				return RegistryHive.LocalMachine;
			}
			catch (Exception)
			{
				return RegistryHive.CurrentUser;
			}
		}

		public static string[] ReadEnvironment()
		{
			Microsoft.Win32.RegistryHive hive = GetRegistrySoftwareClassesHive();
			string[] retval = null;
			if (hive == Microsoft.Win32.RegistryHive.CurrentUser)
			{
				RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Environment");
				string path = (string)key.GetValue("Path","");
				retval = path.Split(';');
				key.Close();
			}
			if (hive  == Microsoft.Win32.RegistryHive.LocalMachine)
			{
				RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment");
				string path =  (string)key.GetValue("Path","");
				retval = path.Split(';');
				key.Close();
			}
			return retval;
		}

		public static void AppendEnvironment(string add)
		{
			Microsoft.Win32.RegistryHive hive = GetRegistrySoftwareClassesHive();
			string[] retval = null;
			if (hive == Microsoft.Win32.RegistryHive.CurrentUser)
			{
				RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Environment");
				string path = (string)key.GetValue("Path","");
				path = path + ";" + add;
				retval = path.Split(';');
				string towrite = String.Join(";",retval);
				key.SetValue("Path",(string)towrite);
				key.Close();
			}
			if (hive  == Microsoft.Win32.RegistryHive.LocalMachine)
			{
				RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment");
				string path =  (string)key.GetValue("Path","");
				path = path + ";" + add;
				retval = path.Split(';');
				string towrite = String.Join(";",retval);
				key.SetValue("Path",(string)towrite);
				key.Close();
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
