using System;


namespace blob
{

	public class Program
	{
		public static BlobConfig blobConfig = new BlobConfig();
		public static string configfile = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string LocalApplicationDataDirectoryApplication = BlobDirectory.CreateLocalApplicationDataDirectory("blob");
			//write default app config.
			configfile = System.IO.Path.Combine(LocalApplicationDataDirectoryApplication,"config.xml");
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
					bool HasSpecificSwitches = blobCommand.CommandLineSwitchOneIsSet("h","i","shell_create_blob", "shell_extract","shell_config");
					if (HasSpecificSwitches)
					{
						string[] switchParam = null;
                     

						switchParam = blobCommand.Valid("i", 0);
						if (!(Equals(switchParam, null)))
						{
							Console.WriteLine(@"Installing...");
							blob.BlobInstall.Install();
						}

						switchParam = blobCommand.Valid("h", 0);
						if (!(Equals(switchParam, null)))
						{
							DisplayHelp();
							//Console.ReadLine();
						}

						switchParam = blobCommand.Valid("shell_create_blob", 1);
						if (!(Equals(switchParam, null)))
						{
							string absoluteDirectoryLocation = System.IO.Path.GetFullPath(switchParam[0]);
							string absoluteDirLocation = System.IO.Path.GetDirectoryName(absoluteDirectoryLocation);
							string lastpart = System.IO.Path.GetFileName(absoluteDirectoryLocation);
							string targetfilename = System.IO.Path.Combine(absoluteDirLocation, lastpart) + ".blob";

							if (blobConfig.mergeType == MergeType.AsBlobFile)
							{
								BlobMerge.Create(switchParam[0], targetfilename, null);
							}
							else if (blobConfig.mergeType == MergeType.AsInvokerExe)
							{
								byte[] sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",true);
								BlobMerge.Create(switchParam[0], targetfilename, sfxexe);
							}
							else if (blobConfig.mergeType == MergeType.AsAdminExe)
							{
								byte[] sfxexe = BlobReflection.GetEmbeddedNetExeFromResource("blobstuba.exe","blobstub.app",false);
								BlobMerge.Create(switchParam[0], targetfilename, sfxexe);
							}
							
						}

						switchParam = blobCommand.Valid("shell_extract", 1);
						if (!(Equals(switchParam, null)))
						{
							string filename = switchParam[0];
							string absoluteFileLocation = System.IO.Path.GetFullPath(filename);
							BlobSplit.Split(absoluteFileLocation);
						}

						switchParam = blobCommand.Valid("shell_config", 1);
						if (!(Equals(switchParam, null)))
						{
							System.Windows.Forms.Application.Run(new blob.blobconfigForm());
						}
					}
					else
					{
						Console.WriteLine("Invalid or multiple exclusive switches detected.");
						//Console.WriteLine(Environment.CommandLine);
						//DisplayHelp();
						//Console.ReadLine();
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
