using System;
using System.IO;
using System.Reflection;

namespace blob
{

	public class BlobReflection
	{
		public static byte[] GetEmbeddedNetExeFromResource(string resourceName,string assemblyIdentityName,bool changeManifestToInvoker)
		{
			byte[] exe = GetManifestResourceByName(resourceName);
			if(changeManifestToInvoker)
			{
				exe = ChangeManifestToInvoker(exe,assemblyIdentityName);
			}
			return exe;
		}

		public static string GetEntryAssemblyVersion()
		{
			Version version = Assembly.GetEntryAssembly().GetName().Version;
			return version.ToString();
		}

		

		public static byte[] GetManifestResourceByName(string resourceName)
		{
			Assembly assembly = Assembly.GetEntryAssembly();
			string[] names = assembly.GetManifestResourceNames();

			int founIndex = -1;
			for (int index = 0;index < names.Length; index++)
			{
				int contains = names[index].IndexOf(resourceName);
				if (contains != -1)
				{
					founIndex = index;
					break;
				}
			}

			byte[] retval = null;
			if (founIndex != -1)
			{
				using(Stream stream = assembly.GetManifestResourceStream(names[founIndex]))
				{
					retval = new byte[stream.Length];
					stream.Read(retval, 0, retval.Length);
					stream.Close();
				}
			}
			return retval;
		}

		public static byte[] ChangeManifestToInvoker(byte[] exe,string assemblyIdentityName)
		{
			byte[] name = System.Text.Encoding.ASCII.GetBytes(@"name="""+assemblyIdentityName+@""">");
			long namepos = BlobArray.Position(exe,name);

			if (namepos != -1)
			{

				byte[] exlevelfind = System.Text.Encoding.ASCII.GetBytes(@"<requestedExecutionLevel level=""requireAdministrator"" uiAccess=""false"">");
				byte[] exlevel =     System.Text.Encoding.ASCII.GetBytes(@"<requestedExecutionLevel level=""asInvoker""            uiAccess=""false"">");
				long pos = BlobArray.Position(exe,exlevelfind,(int)namepos);
				if (pos != -1)
				{
					Array.Copy(exlevel,0,exe,(int)pos,exlevel.Length);
				}
			}
			return exe;
		}
	}
}
