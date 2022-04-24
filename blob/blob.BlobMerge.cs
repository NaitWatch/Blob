using System;

namespace blob
{

	public class BlobMerge
	{

		/// <summary>
		/// If sfexe is given add binaray block at start
		/// </summary>
		/// <param name="sourceDirectory"></param>
		/// <param name="destinationFile"></param>
		/// <param name="sfxexe"></param>
		/// <param name="postlaunch"></param>
		public static void Create(string sourceDirectory, string destinationFile, byte[] sfxexe)
		{
			
			if (Program.blobConfig.mergeVisibility == MergeVisibility.Console)
			{
				BlobWindowConsole.Initialize();
			}

			//ensure that the directory for the destination .blob exists
			string fileLocationDirectory = System.IO.Path.GetDirectoryName(destinationFile);
			BlobDirectory.CreateDirectoryIfNotExists(fileLocationDirectory);

			string MetaDataDirectoryName = System.IO.Path.GetFileNameWithoutExtension(destinationFile);

			//if sfxexe is given write SFX exe first
			if (!Object.Equals(sfxexe, null))
			{
				string tdir = System.IO.Path.GetDirectoryName(destinationFile);
				string tfile = System.IO.Path.GetFileNameWithoutExtension(destinationFile);
				destinationFile = System.IO.Path.Combine(tdir,tfile) + ".exe";
			}
			BlobFileStream fileStream = new BlobFileStream(destinationFile, BlobFileStream.Type.Writer);
			if (!Object.Equals(sfxexe, null))
			{
				fileStream.Write(sfxexe);
			}

			//Writes blob meta data at start. Writes the target blob filename, cause orginal file can be accidently changed
			//e.g. writing on a cd drive iso that does support characters...
			
			byte[] GUIDBYTEARRAY_META = BlobGuid.GetGuid(BlobGuid.GUID.METABLOCK);
			fileStream.Write(GUIDBYTEARRAY_META);

			fileStream.Write(MetaDataDirectoryName, System.Text.Encoding.Unicode, true);
			fileStream.Write((int)Program.blobConfig.splitVisibility);
			fileStream.Write(Program.blobConfig.splitDestination, System.Text.Encoding.Unicode, true);
			fileStream.Write((int)Program.blobConfig.splitDestinationAppend);
			fileStream.Write(((bool)Program.blobConfig.splitDeleteAfter));
			
			fileStream.Write((int)Program.blobConfig.startupType);
			fileStream.Write((bool)Program.blobConfig.startUpRequireAdmin);
			fileStream.Write(Program.blobConfig.startUpFile, System.Text.Encoding.Unicode, true);
			fileStream.Write(Program.blobConfig.startUpArguments, System.Text.Encoding.Unicode, true);
			fileStream.Write((bool)Program.blobConfig.startUpDeleteAfter);

			//Write all other files GUID,ID,Len,Filename,Len,Data,Len,Filename,Len,Data
			byte[] GUIDBYTEARRAY_DATA = BlobGuid.GetGuid(BlobGuid.GUID.DATABLOCK);
			fileStream.Write(GUIDBYTEARRAY_DATA);
			
			string[] files = BlobDirectory.GetFilesRelative(sourceDirectory);

			for (int i = 0; i < files.Length; i++)
			{
				Console.Write(files[i] + " ");

				string fullloc = System.IO.Path.Combine(sourceDirectory, files[i]);
				fileStream.Write(files[i], System.Text.Encoding.Unicode, true);
				fileStream.WriteFromReadStream(fullloc,true);
				fileStream.Flush();

				Console.WriteLine(" Done.");
			}


		}
    
	
	
		
	
	}
}
