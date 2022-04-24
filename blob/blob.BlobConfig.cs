using System;
using System.IO;
using System.Xml.Serialization;

namespace blob
{
	public class BlobConfig : BlobSerializer
	{
		public MergeType mergeType = MergeType.AsBlobFile;
		public MergeVisibility mergeVisibility = MergeVisibility.Console;

		public SplitVisibility splitVisibility = SplitVisibility.Console;
		public string splitDestination = @".";
		public SplitDestinationAppend splitDestinationAppend = SplitDestinationAppend.CurrentFilename;
		public bool splitDeleteAfter = false;

		public StartUpType startupType = StartUpType.None;
		
		public string startUpFile = @"";
		public string startUpArguments = @"";
		public bool startUpRequireAdmin = false;
		public bool startUpDeleteAfter = false;
		
	}

	public enum MergeType : int
	{
		AsBlobFile = 0,
		AsInvokerExe = 1,
		AsAdminExe = 2,
	}

	public enum MergeVisibility: int
	{
		Console = 0,
		None = 1,
	}

	public enum SplitVisibility : int
	{
		Console = 0,
		None = 1,
	}

	public enum SplitDestinationAppend : int
	{
		OrginalFolderName = 0,
		CurrentFilename = 1,
		None = 2,
	}

	public enum StartUpType : int
	{
		None = 0,
		ProcessNormal = 1,
		ProcessNoWindow = 2,
	}
}


