using System;

namespace blob
{
	public class BlobSplit
	{
		public static void Split(string blob)
		{
			BlobFileStream fileStream = new BlobFileStream(blob, BlobFileStream.Type.Reader);

			//Seek the metadata guid from the start
			byte[] GUIDBYTEARRAY_META = BlobGuid.GetGuid(BlobGuid.GUID.METABLOCK);
			long pos = fileStream.Seek(GUIDBYTEARRAY_META);

			if (pos == -1)
			{
				//Console.WriteLine("MetaData not found");
				return;
			}

			fileStream.Position = pos;
			fileStream.ReadGuid();

			string MetaDataDirectoryName = fileStream.ReadString(System.Text.Encoding.Unicode);
			SplitVisibility splitVisibility = (SplitVisibility)fileStream.ReadInt();
			string splitDestination = fileStream.ReadString(System.Text.Encoding.Unicode);
			SplitDestinationAppend splitDestinationAppend = (SplitDestinationAppend)fileStream.ReadInt();
			bool splitDeleteAfter = (bool)fileStream.ReadBool();

			StartUpType startUpType = (StartUpType)fileStream.ReadInt();
			bool startUpRequireAdmin = (bool)fileStream.ReadBool();
			string startUpFile = fileStream.ReadString(System.Text.Encoding.Unicode);
			string startUpArguments = fileStream.ReadString(System.Text.Encoding.Unicode);
			bool startUpDeleteAfter = (bool)fileStream.ReadBool();

			if (splitVisibility == SplitVisibility.Console)
			{
				BlobWindowConsole.InitializeNew();
			}

			Console.WriteLine("-------------- Embedded Params --------------");
			Console.WriteLine("MetaDataDirectoryName: " + MetaDataDirectoryName);
			Console.WriteLine("deBlobVisibility: " + splitVisibility.ToString());
			Console.WriteLine("targetDirectory: " + splitDestination);
			Console.WriteLine("targetDirectoryAppend: " + splitDestinationAppend.ToString());
			Console.WriteLine("DeleteSource: " + splitDeleteAfter.ToString());

			Console.WriteLine("postExtractStartupType: " + startUpType.ToString());
			Console.WriteLine("ReqAdmin: " + startUpRequireAdmin.ToString());
			Console.WriteLine("Filename: " + startUpFile);
			Console.WriteLine("Arguments: " + startUpArguments);
			Console.WriteLine("DeleteTarget: " + startUpDeleteAfter.ToString());


			Console.WriteLine("-------------- Calculated Params --------------");
			
			Console.WriteLine("Embedded targetDirectory exp: " + System.Environment.ExpandEnvironmentVariables(splitDestination));
			splitDestination = System.IO.Path.GetFullPath(System.Environment.ExpandEnvironmentVariables(splitDestination));
			Console.WriteLine("Embedded targetDirectory expfull: " + splitDestination);

			string targetdir = splitDestination;

			switch (splitDestinationAppend)
			{
				case SplitDestinationAppend.OrginalFolderName:
					targetdir = System.IO.Path.Combine(splitDestination, MetaDataDirectoryName);
					break;
				case SplitDestinationAppend.CurrentFilename:
					targetdir = System.IO.Path.Combine(splitDestination, System.IO.Path.GetFileNameWithoutExtension(blob));
					break;
				case SplitDestinationAppend.None:
					targetdir = targetdir;
					break;
			}

			Console.WriteLine("Fulltarget: " + targetdir);

			try
			{
				BlobDirectory.CreateDirectoryIfNotExists(targetdir);
			}
			catch (Exception)
			{
				targetdir = System.IO.Path.Combine(@"C:\temp", MetaDataDirectoryName);
				BlobDirectory.CreateDirectoryIfNotExists(targetdir);
			}
			Console.WriteLine("Choosen target: " + targetdir);
   
			byte[] GUIDBYTEARRAY_DATA = BlobGuid.GetGuid(BlobGuid.GUID.DATABLOCK);
			pos = fileStream.Seek(GUIDBYTEARRAY_DATA);
			if (pos == -1)
			{
				Console.WriteLine("Blob not found");
				return;
			}
			fileStream.Position = pos;
			fileStream.ReadGuid();

			do
			{
				string storedFileName = fileStream.ReadString(System.Text.Encoding.Unicode);
				Console.Write(storedFileName + " ");
				string filef = System.IO.Path.Combine(targetdir, storedFileName);
				BlobDirectory.CreateDirectoryForFile(filef);
				fileStream.ReadBytesToFile(filef);
				Console.WriteLine(" Done.");
			} while (fileStream.Position != fileStream.Length);

			fileStream.Close();

			if (splitDeleteAfter)
			{
				BlobProcess.Delete(blob,System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
			}

			if (startUpType != StartUpType.None)
			{
				string TargetFile = System.IO.Path.Combine(targetdir,startUpFile);
				if (startUpType == StartUpType.ProcessNormal)
				{
					BlobProcess.launch(false,startUpRequireAdmin,TargetFile,startUpArguments);
				}
				if (startUpType == StartUpType.ProcessNoWindow)
				{
					BlobProcess.launch(true,startUpRequireAdmin,TargetFile,startUpArguments);
				}

				if (startUpDeleteAfter)
				{
					BlobProcess.Delete(targetdir,"-1");
				}
			}


		}

	}
}
