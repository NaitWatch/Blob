using System;
using System.Diagnostics;
using System.Globalization;

namespace blob
{
	public class BlobCommandLine
	{
		#region generic properties

		private readonly string switchSymbol;

		public string GetSwitchSymbol
		{
			get { return switchSymbol; }
		}

		private readonly string switchCharacter;

		public string GetSwitchCharacter
		{
			get { return switchCharacter; }
		}

		private readonly string switchFull;

		public string GetSwitchFull
		{
			get { return switchFull; }
		}

		private readonly int switchMinimumRequiredParameter;

		public int GetSwitchMinimumRequiredParameter
		{
			get { return switchMinimumRequiredParameter; }
		}


		private bool commandLineHasArgs;

		public bool CommandLineHasArgs
		{
			get { return commandLineHasArgs; }
		}

		private bool switchIsExclusiveInCommandLine;
		public bool SwitchIsExclusiveInCommandLine
		{
			get { return switchIsExclusiveInCommandLine; }
		}

		private string[] switchIsExclusiveCommands;

		public string[] SwitchIsExclusiveCommands
		{
			get { return switchIsExclusiveCommands; }
		}

		private bool subSwitchInParameter;

		public bool SubSwitchInParameter
		{
			get { return subSwitchInParameter; }
		}

		public bool isOk
		{
			get { return (!subSwitchInParameter) && switchExists && switchHasMinimumRequiredParameter; }
		}

		private bool switchExists;

		public bool SwitchExists
		{
			get { return switchExists; }
		}

		private int switchExistsPosition;

		public int SwitchExistsPosition
		{
			get { return switchExistsPosition; }
		}

		private bool switchHasMinimumRequiredParameter;

		public bool SwitchHasMinimumRequiredParameter
		{
			get { return switchHasMinimumRequiredParameter; }
		}

		private int switchParameterFristPosition;

		public int SwitchParameterFristPosition
		{
			get { return switchParameterFristPosition; }
		}

		private int switchParameterLastPosition;

		public int SwitchParameterLastPosition
		{
			get { return switchParameterLastPosition; }
		}


		private string[] switchParameters = new string[0];

		public string[] GetSwitchParameters
		{
			get { return switchParameters; }
		}

		#endregion

		public string ParameterFirst
		{
			get 
			{ 
				if (switchParameters.Length > 0)
				{
					return switchParameters[0];
				}
				else
				{
					return null;
				}
                
			}
		}
		public string ParameterSecond
		{
			get 
			{ 
				if (switchParameters.Length > 1)
				{
					return switchParameters[1];
				}
				else
				{
					return null;
				}
                
			}
		}

		public string ParameterThird
		{
			get 
			{ 
				if (switchParameters.Length > 2)
				{
					return switchParameters[2];
				}
				else
				{
					return null;
				}
                
			}
		}

		private string[] args = null;

		public BlobCommandLine(string SwitchSymbol,string SwitchCharacter,int MinimumRequiredParameter,string[] overrideCommandLineArgs)
		{
			switchSymbol = SwitchSymbol;
			switchCharacter = SwitchCharacter;
			switchFull = SwitchSymbol + SwitchCharacter;

			switchMinimumRequiredParameter = MinimumRequiredParameter;

			args = System.Environment.GetCommandLineArgs();

			if (!(Equals(overrideCommandLineArgs, null)))
			{
				args = overrideCommandLineArgs;
			}

			if (args.Length > 1)
			{
				commandLineHasArgs = true;
			}

			if (commandLineHasArgs)
			{
				FindPresents();
				SwitchParametersPositions();
				PharseSwitchParameters();
				SwitchParameterHasNoSymbol();
				IsCommandLineExclusive();
			}
		}

		
		private void FindPresents()
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].ToLower(CultureInfo.InvariantCulture) == switchFull.ToLower(CultureInfo.InvariantCulture))
				{
					switchExists = true;
					switchExistsPosition = i;
					break;
				}
			}
		}

		private void SwitchParametersPositions()
		{
			//If switch exists
			if (switchExists)
			{
				//if param could be inside commandline
				if (switchExistsPosition + switchMinimumRequiredParameter < args.Length)
				{
					switchHasMinimumRequiredParameter = true;

					switchParameterFristPosition = switchExistsPosition;
					switchParameterLastPosition = switchExistsPosition + switchMinimumRequiredParameter;
				}
				else
                {
					switchHasMinimumRequiredParameter = false;
				}
			}
		}

		private void PharseSwitchParameters()
        {
			if (switchExists)
			{
				if (switchHasMinimumRequiredParameter)
                {

						string[] result = new string[switchParameterLastPosition - switchParameterFristPosition];
						for (int i = switchParameterFristPosition; i < switchParameterLastPosition; i++)
						{
							result[i - switchParameterFristPosition] = args[i+1];
							
						}
						switchParameters = result;
					
                }
			}
		}

		private void SwitchParameterHasNoSymbol()
		{
			bool symbolIsInParameter = false;
			for (int i = switchExistsPosition; i < switchParameterLastPosition; i++)
			{
				if (args[i+1].StartsWith(switchSymbol))
                {
					symbolIsInParameter = true;
				}
			}
			subSwitchInParameter = symbolIsInParameter;
		}

		private void IsCommandLineExclusive()
        {
			string[] argscopy = new string[args.Length];
			args.CopyTo(argscopy, 0);
			argscopy[0] = null;
			for (int i = switchExistsPosition; i <= switchParameterLastPosition; i++)
			{
				argscopy[i] = null;
			}

			bool exclusive = true;
			string[] exclusiveCommands = new string[0];

            for (int i = 0; i < argscopy.Length; i++)
            {
				if (!(Equals(argscopy[i], null)))
				{
					exclusive = false;

					string[] temp = new string[exclusiveCommands.Length + 1];
					exclusiveCommands.CopyTo(temp, 0);
					temp[exclusiveCommands.Length] = argscopy[i];
					exclusiveCommands = temp;
				}
			}

			switchIsExclusiveInCommandLine = exclusive;
			switchIsExclusiveCommands = exclusiveCommands;

		}

	}
}
