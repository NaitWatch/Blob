using System;
using System.IO;

namespace blob
{

	public class BlobDirectory
	{
		public static void CreateDirectoryIfNotExists(string directory)
		{
			if (!(Directory.Exists(directory)))
			{
				Directory.CreateDirectory(directory);
			}
		}

		public static string CreateLocalApplicationDataDirectory(string Appdir)
		{
			string LocalApplicationDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string LocalApplicationDataDirectoryApplication = Path.Combine(LocalApplicationDataDirectory, Appdir);
			CreateDirectoryIfNotExists(LocalApplicationDataDirectoryApplication);
			return LocalApplicationDataDirectoryApplication;
		}

		public static void CreateDirectoryForFile(string location)
		{
			string directory = Path.GetDirectoryName(location);
			CreateDirectoryIfNotExists(directory);
		}

		public static string[] GetDirs(string directory)
		{
			string[] subdir = Directory.GetDirectories(directory);

			foreach (string item in subdir)
			{
				string[] re = GetDirs(item);
				subdir = BlobArray.Add(subdir, re);
			}
			return subdir;
		}

		public static string GetTempFileName()
		{
			return RandomString(8);
		}
		
		public static string RandomString(int len)
		{
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			string stringChars = "";
			System.Random random = new System.Random();

			for (int i = 0; i < len; i++)
			{
				stringChars = stringChars + chars[random.Next(chars.Length)];
			}

			return stringChars ;
		}


		public static string[] GetFiles(string directory)
		{
			string[] dirs = GetDirs(directory);
			string[] files = new string[0];
			string[] fi = Directory.GetFiles(directory, "*");
			files = BlobArray.Add(files, fi);
			foreach (string item in dirs)
			{
				fi = Directory.GetFiles(item, "*");
				files = BlobArray.Add(files, fi);
			}
			return files;
		}

		public static string[] GetFilesRelative(string directory)
		{
			string[] files = GetFiles(directory);
			string[] retval = BlobArray.RemoveFromStart(files, directory + "\\");
			return retval;
		}



	}
}
