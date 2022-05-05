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
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(BlobException.BlobUnhandledException);
			
			MainLoadConfigFile();

			//If nothing was done all returned false
			if (!(
				MainArgsShellCommands() ||
				MainArgsInstall() ||
				MainArgsHelp() ||
				MainArgsCreateInterfaceParam()
				))
			{
				DisplayHelp();
			}

			/*
			TimeSpan xx = TimeSpan.FromSeconds(3);
			DateTime dd = System.DateTime.Now + xx;
			do
			{
				System.Threading.Thread.Sleep(50);
			} while(dd > DateTime.Now);
			*/
			
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


		static bool MainArgsShellCommands()
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

				BlobMerge.CreateFromConfig(SwitchShellCreate.ParameterFirst, targetfilename);
				return true;
			}

			if (SwitchShellExtract.isOk && SwitchShellExtract.SwitchIsExclusiveInCommandLine)
			{
				string absoluteFileLocation = System.IO.Path.GetFullPath(SwitchShellExtract.ParameterFirst);
				BlobSplit.Split(absoluteFileLocation);
				return true;
			}
			if (SwitchShellConfig.isOk && SwitchShellConfig.SwitchIsExclusiveInCommandLine)
			{
				System.Windows.Forms.Application.Run(new blob.blobConfigForm());
				return true;
			}

			return false;
		}

		
		static bool MainArgsInstall()
		{
			BlobCommandLine SwitchInstall = new BlobCommandLine("/","i",0,null);
			if (SwitchInstall.isOk && SwitchInstall.SwitchIsExclusiveInCommandLine)
			{
				blob.BlobInstall.Install(true);
				return true;
			}

			BlobCommandLine SwitchInstallSilent = new BlobCommandLine("/","s",0,null);
			if (SwitchInstallSilent.isOk && SwitchInstallSilent.SwitchIsExclusiveInCommandLine)
			{
				blob.BlobInstall.Install(false);
				return true;
			}

			return false;
		}


		static bool MainArgsHelp()
		{
			BlobCommandLine SwitchHelp = new BlobCommandLine("/","h",0,null);
			//BlobCommandLine SwitchVersion = new BlobCommandLine("/","v",0,null);
			if (SwitchHelp.isOk && SwitchHelp.SwitchIsExclusiveInCommandLine)
			{
				DisplayHelp();
				return true;
			}
			/*
			if (SwitchVersion.isOk && SwitchVersion.SwitchIsExclusiveInCommandLine)
			{
				System.Windows.Forms.MessageBox.Show("v"+blob.BlobReflection.GetEntryAssemblyVersion());
				return true;
			}
			*/
			return false;
		}
		

		static bool MainArgsCreateInterfaceParam()
		{
			BlobCommandLine SwitchSource = new BlobCommandLine("/","s",1,null);
			BlobCommandLine SwitchDestination = new BlobCommandLine("/","d",1,null);
			BlobCommandLine SwitchFilename = new BlobCommandLine("/","f",1,null);
			BlobCommandLine SwitchArguments = new BlobCommandLine("/","a",1,null);
			if (SwitchSource.isOk && SwitchDestination.isOk && !SwitchFilename.isOk && !SwitchArguments.isOk)
			{
				BlobMerge.CreateFromConfig(SwitchSource.ParameterFirst, SwitchDestination.ParameterFirst);
				return true;
			}
			if (SwitchSource.isOk && SwitchDestination.isOk && SwitchFilename.isOk && SwitchArguments.SwitchHasMinimumRequiredParameter)
			{
				BlobMerge.CreateUserTempInstaller(SwitchSource.ParameterFirst, SwitchDestination.ParameterFirst,SwitchFilename.ParameterFirst,SwitchArguments.ParameterFirst);
				return true;
			}

			return false;
		}

		static void DisplayHelp()
		{
			blob.BlobWindowConsole.InitializeNew();
			Console.WriteLine();
			Console.WriteLine("Version: "+blob.BlobReflection.GetEntryAssemblyVersion());
			Console.WriteLine();
			
			Console.WriteLine(@"Example of usage:");
			Console.WriteLine();
			Console.WriteLine(@"blob /s ""C:\temp"" /d ""C:\temp\temp.blob""");
			Console.WriteLine(@"Creates a blob with config parameters");
			Console.WriteLine();
			Console.WriteLine(@"blob /s ""C:\base\github.com\NaitWatch\blob\blob\bin\Release"" /d ""C:\temp\blob_setup.blob"" /f ""blob.exe"" /a ""/i"" ");
			Console.WriteLine(@"Creates a user installer (mini setup)");
			Console.WriteLine();
			
			Console.WriteLine(@"Options (single):");
			Console.WriteLine(@"/i ".PadRight(10,' ')+"Installs the blob.");
			Console.WriteLine(@"/s ".PadRight(10,' ')+"Installs the blob silent.");
			Console.WriteLine(@"/h ".PadRight(10,' ')+"Displays the help");

			Console.WriteLine();
			Console.WriteLine(@"Options:");
			Console.WriteLine(@"/s".PadRight(10,' ')+"Source directory");
			Console.WriteLine(@"/d".PadRight(10,' ')+"Destination file, needs always end with .blob");
			Console.WriteLine(@"/f".PadRight(10,' ')+"Filename that will be exectuted after extract.");
			Console.WriteLine(@"/a".PadRight(10,' ')+"Argument that will be exectuted after extract.");
			

		}
	}
}
