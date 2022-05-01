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
			BlobCommandLine SwitchHelp = new BlobCommandLine("/","h",0,null);
			BlobCommandLine SwitchInstall = new BlobCommandLine("/","i",0,null);

			BlobCommandLine SwitchCreateSource = new BlobCommandLine("/","s",1,null);
			BlobCommandLine SwitchCreateMetaDataDirectoryName = new BlobCommandLine("/","m",1,null);
			BlobCommandLine SwitchCreateSplitVisibility = new BlobCommandLine("/","vis",1,null);
			BlobCommandLine SwitchCreateSplitDestination = new BlobCommandLine("/","dest",1,null);
			BlobCommandLine SwitchCreateSplitDestinationAppend = new BlobCommandLine("/","destapp",1,null);
			BlobCommandLine SwitchCreateDeleteSource = new BlobCommandLine("/","delsource",1,null);
			
			BlobCommandLine SwitchCreateType = new BlobCommandLine("/","type",1,null);
			BlobCommandLine SwitchCreateRequireAdmin = new BlobCommandLine("/","reqadmin",1,null);
			BlobCommandLine SwitchCreateFile = new BlobCommandLine("/","file",1,null);
			BlobCommandLine SwitchCreateArguments = new BlobCommandLine("/","args",1,null);
			BlobCommandLine SwitchCreateDeleteAfter = new BlobCommandLine("/","delrun",1,null);
			

			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(BlobException.BlobUnhandledException);
			
			MainLoadConfigFile();

			BlobCommand blobCommand = new BlobCommand("/");
			MainArgsShellCommands();

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
							blob.BlobInstall.Install(true);
							Console.WriteLine(@"Installing...");
						}

						if (!(Equals(switchHelp, null)))
						{
							DisplayHelp();
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
				blob.BlobInstall.Install(true);
			}
			blob.BlobWindowConsole.CloseConsole();
		}


		static void MainLoadConfigFile()
		{
			string LocalApplicationDataDirectoryApplication = BlobDirectory.CreateLocalApplicationDataDirectory("blob");
			//write default app config.
			configfile = System.IO.Path.Combine(LocalApplicationDataDirectoryApplication,configfile);
			if (System.IO.File.Exists(configfile))
			{
				try
				{
					blobConfig = (BlobConfig)blobConfig.ObjectFromDisk(configfile);
				}
				catch (Exception)
				{
					//XML Error cause of restructioning of xml
					System.IO.File.Delete(configfile);
					blobConfig.ObjectToDisk(configfile);
					blobConfig = (BlobConfig)blobConfig.ObjectFromDisk(configfile);
				}
			}
			else
			{
				blobConfig.ObjectToDisk(configfile);
				blobConfig = (BlobConfig)blobConfig.ObjectFromDisk(configfile);
			}
		}

		static void MainArgsShellCommands()
		{
			BlobCommandLine SwitchShellCreate = new BlobCommandLine("/","shell_create_blob",1,null);
			BlobCommandLine SwitchShellExtract = new BlobCommandLine("/","shell_extract",1,null);
			BlobCommandLine SwitchShellConfig = new BlobCommandLine("/","shell_config",1,null);

			if (SwitchShellCreate.isOk && SwitchShellCreate.SwitchIsExclusiveInCommandLine)
			{
				string absoluteDirectoryLocation = System.IO.Path.GetFullPath(SwitchShellCreate.ParameterFirst);
				
				string absoluteDirLocation = System.IO.Path.GetDirectoryName(absoluteDirectoryLocation);
				string lastpart = System.IO.Path.GetFileName(absoluteDirectoryLocation);
				
				string targetfilename = System.IO.Path.Combine(absoluteDirLocation, lastpart) + ".blob";

				if (blobConfig.mergeType == MergeType.AsBlobFile)
				{
					BlobMerge.Create(SwitchShellCreate.ParameterFirst, targetfilename, null);
				}
				else if (blobConfig.mergeType == MergeType.AsInvokerExe)
				{
					byte[] sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",true);
					BlobMerge.Create(SwitchShellCreate.ParameterFirst, targetfilename, sfxexe);
				}
				else if (blobConfig.mergeType == MergeType.AsAdminExe)
				{
					byte[] sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",false);
					BlobMerge.Create(SwitchShellCreate.ParameterFirst, targetfilename, sfxexe);
				}
			}

			if (SwitchShellExtract.isOk && SwitchShellExtract.SwitchIsExclusiveInCommandLine)
			{
				string absoluteFileLocation = System.IO.Path.GetFullPath(SwitchShellExtract.ParameterFirst);
				BlobSplit.Split(absoluteFileLocation);
			}
			if (SwitchShellConfig.isOk && SwitchShellConfig.SwitchIsExclusiveInCommandLine)
			{
				System.Windows.Forms.Application.Run(new blob.blobConfigForm());
			}
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
