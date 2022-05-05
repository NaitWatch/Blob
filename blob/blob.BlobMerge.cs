using System;

namespace blob
{

	public class BlobMerge
	{
		public static void CreateUserTempInstaller(string sourceDirectory,string destinationFile,string filename,string arguments)
		{
			CreateNew(sourceDirectory,destinationFile,MergeType.AsInvokerExe,MergeVisibility.Console,SplitVisibility.None,"%TEMP%",SplitDestinationAppend.CurrentFilename,false,StartUpType.ProcessNoWindow,filename,arguments,false,true);
		}

		public static void CreateFromConfig(string sourceDirectory,string destinationFile)
		{
			CreateNew(sourceDirectory,destinationFile,Program.blobConfig.mergeType,Program.blobConfig.mergeVisibility,Program.blobConfig.splitVisibility,Program.blobConfig.splitDestination,Program.blobConfig.splitDestinationAppend,Program.blobConfig.splitDeleteAfter,Program.blobConfig.startupType,Program.blobConfig.startUpFile,Program.blobConfig.startUpArguments,Program.blobConfig.startUpRequireAdmin,Program.blobConfig.startUpDeleteAfter);
		}

		public static void CreateNew(
			string sourceDirectory,
			string destinationFile,
			MergeType mergeType,
			MergeVisibility mergeVisibility,

			SplitVisibility splitVisibility,
			string splitDestination,
			SplitDestinationAppend splitDestinationAppend,
			bool splitDeleteAfter,
			StartUpType startupType,
			string startUpFile,
			string startUpArguments,
			bool startUpRequireAdmin,
			bool startUpDeleteAfter
			)
		{
			if (mergeVisibility == MergeVisibility.Console)
			{
				BlobWindowConsole.InitializeNew();
			}

			byte[] sfxexe = null;
			if (mergeType == MergeType.AsBlobFile)
			{
				sfxexe = null;
			}
			else if (mergeType == MergeType.AsInvokerExe)
			{
				sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",true);
			}
			else if (mergeType == MergeType.AsAdminExe)
			{
				sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",false);
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
			fileStream.Write((int)splitVisibility);
			fileStream.Write(splitDestination, System.Text.Encoding.Unicode, true);
			fileStream.Write((int)splitDestinationAppend);
			fileStream.Write(((bool)splitDeleteAfter));
			
			fileStream.Write((int)startupType);
			fileStream.Write((bool)startUpRequireAdmin);
			fileStream.Write(startUpFile, System.Text.Encoding.Unicode, true);
			fileStream.Write(startUpArguments, System.Text.Encoding.Unicode, true);
			fileStream.Write((bool)startUpDeleteAfter);

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

		/// <summary>
		/// If sfexe is given add binaray block at start
		/// </summary>
		/// <param name="sourceDirectory"></param>
		/// <param name="destinationFile"></param>
		/// <param name="sfxexe"></param>
		/// <param name="postlaunch"></param>
		public static void CreateOld(string sourceDirectory, string destinationFile)
		{
			byte[] sfxexe = null;
			if (Program.blobConfig.mergeType == MergeType.AsBlobFile)
			{
				sfxexe = null;
			}
			else if (Program.blobConfig.mergeType == MergeType.AsInvokerExe)
			{
				sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",true);
			}
			else if (Program.blobConfig.mergeType == MergeType.AsAdminExe)
			{
				sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",false);
			}

			if (Program.blobConfig.mergeVisibility == MergeVisibility.Console)
			{
				BlobWindowConsole.InitializeNew();
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
