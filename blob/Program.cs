using System;


namespace blob
{

	public class Program
	{
		public static BlobConfig blobConfig = new BlobConfig();
		public static string configfile = "config.xml";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Commander SwitchShellCreate = new Commander("/","shell_create_blob",1);
			Commander SwitchShellExtract = new Commander("/","shell_extract",1);
			Commander SwitchShellConfig = new Commander("/","shell_config",1);
			Commander SwitchHelp = new Commander("/","h",0);
			Commander SwitchInstall = new Commander("/","i",0);

			Commander SwitchCreateSource = new Commander("/","s",1);
			Commander SwitchCreateMetaDataDirectoryName = new Commander("/","m",1);
			Commander SwitchCreateSplitVisibility = new Commander("/","vis",1);
			Commander SwitchCreateSplitDestination = new Commander("/","dest",1);
			Commander SwitchCreateSplitDestinationAppend = new Commander("/","destapp",1);
			Commander SwitchCreateDeleteSource = new Commander("/","delsource",1);
			
			Commander SwitchCreateType = new Commander("/","type",1);
			Commander SwitchCreateRequireAdmin = new Commander("/","reqadmin",1);
			Commander SwitchCreateFile = new Commander("/","file",1);
			Commander SwitchCreateArguments = new Commander("/","args",1);
			Commander SwitchCreateDeleteAfter = new Commander("/","delrun",1);
			

			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(BlobException.BlobUnhandledException);
			string LocalApplicationDataDirectoryApplication = BlobDirectory.CreateLocalApplicationDataDirectory("blob");
			//write default app config.
			configfile = System.IO.Path.Combine(LocalApplicationDataDirectoryApplication,"configfile");
			if (System.IO.File.Exists(configfile))
			{
				blobConfig = (BlobConfig)blobConfig.ObjectFromDisk(configfile);
			}
			else
			{
				blobConfig.ObjectToDisk(configfile);
				blobConfig = (BlobConfig)blobConfig.ObjectFromDisk(configfile);
			}

			BlobCommand blobCommand = new BlobCommand("/");

			bool HasArguments = blobCommand.CommandLineHasArguments();
			if (HasArguments)
			{
				bool HasSwitches = blobCommand.CommandLineHasSwitches();
				if (HasSwitches)
				{
					bool HasSpecificSwitches = blobCommand.CommandLineSwitchOneIsSet("s","d","t","h","i","shell_create_blob", "shell_extract","shell_config");
					if (HasSpecificSwitches)
					{
						string[] switchSource = blobCommand.Valid("s", 1);
						string[] switchDestination = blobCommand.Valid("d", 1);
						string[] switchType = blobCommand.Valid("t", 1);

						if (BlobObject.IsSet(switchSource) && BlobObject.IsSet(switchDestination) && BlobObject.IsSet(switchType))
						{
							//TODO: SET ENVIROMENT PATH TO FIND BLOB
							//ALL PARAMETER MUST BE SETABLE
							switchSource[0] = System.IO.Path.GetFullPath(switchSource[0]);

							if (switchType[0] == "b")
							{
								BlobMerge.Create(switchSource[0], switchDestination[0], null);
							}
							else if (switchType[0] == "i")
							{
								byte[] sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",true);
								BlobMerge.Create(switchSource[0], switchDestination[0], sfxexe);
							}
							else if (switchType[0] == "a")
							{
								byte[] sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",false);
								BlobMerge.Create(switchSource[0], switchDestination[0], sfxexe);
							}
						}

						string[] switchInstall = blobCommand.Valid("i", 0);
						string[] switchHelp = blobCommand.Valid("h", 0);
						string[] switchShellCreateBlob = blobCommand.Valid("shell_create_blob", 1);
						string[] switchShellExtract = blobCommand.Valid("shell_extract", 1);
						string[] switchShellConfig = blobCommand.Valid("shell_config", 1);

						if (!(Equals(switchInstall, null)))
						{
							blob.BlobInstall.Install();
							Console.WriteLine(@"Installing...");
						}

						if (!(Equals(switchHelp, null)))
						{
							DisplayHelp();
						}

						if (!(Equals(switchShellCreateBlob, null)))
						{
							string absoluteDirectoryLocation = System.IO.Path.GetFullPath(switchShellCreateBlob[0]);
							string absoluteDirLocation = System.IO.Path.GetDirectoryName(absoluteDirectoryLocation);
							string lastpart = System.IO.Path.GetFileName(absoluteDirectoryLocation);
							string targetfilename = System.IO.Path.Combine(absoluteDirLocation, lastpart) + ".blob";

							if (blobConfig.mergeType == MergeType.AsBlobFile)
							{
								BlobMerge.Create(switchShellCreateBlob[0], targetfilename, null);
							}
							else if (blobConfig.mergeType == MergeType.AsInvokerExe)
							{
								byte[] sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",true);
								BlobMerge.Create(switchShellCreateBlob[0], targetfilename, sfxexe);
							}
							else if (blobConfig.mergeType == MergeType.AsAdminExe)
							{
								byte[] sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",false);
								BlobMerge.Create(switchShellCreateBlob[0], targetfilename, sfxexe);
							}
						}

						if (!(Equals(switchShellExtract, null)))
						{
							string filename = switchShellExtract[0];
							string absoluteFileLocation = System.IO.Path.GetFullPath(filename);
							BlobSplit.Split(absoluteFileLocation);
						}

						if (!(Equals(switchShellConfig, null)))
						{
							System.Windows.Forms.Application.Run(new blob.blobconfigForm());
						}
					}
					else
					{
						Console.WriteLine("Invalid or multiple exclusive switches detected.");
					}
				}
				else
				{
					Console.WriteLine("No switches detected.");
				}
			}
			else
			{
				Console.WriteLine("No arguments detected.");
				Console.WriteLine(@"Installing...");
				blob.BlobInstall.Install();
			}
			blob.BlobWindowConsole.CloseConsole();
		}

		private static void DisplayHelp()
		{
			Console.WriteLine(@"Sample usage:");
			Console.WriteLine(@"Create a self extracting blob file from a directory: blob.exe /s ""C:\user\Download"" ""C:\temp\Download.blob""");
			Console.WriteLine(@"Create a blob file from a directory: blob.exe /c ""C:\user\Download"" ""C:\temp\Download.blob""");
			Console.WriteLine(@"Extract files to a directory: blob.exe /d ""C:\temp\Download.blob"" ""C:\temp\foo""");
		}
	}
}
