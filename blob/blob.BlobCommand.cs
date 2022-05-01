using System;
using System.Diagnostics;
using System.Globalization;

namespace blob
{
	public class BlobCommand
	{
		private string _CommandSwitchCharacter = @"/";

		public string SetCommandSwitchCharacter
		{
			set { _CommandSwitchCharacter = value; }
		}

		private readonly string _Process = null;

		public string GetProcess
		{
			get { return _Process; }
		}

		private readonly string[] _CommandLineArgs = null;

		public string[] GetCommandLineArgs
		{
			get { return _CommandLineArgs; }
		}

		public BlobCommand()
		{
			_CommandLineArgs = System.Environment.GetCommandLineArgs();
		}

		public BlobCommand(string CommandCharacter)
		{
			_CommandLineArgs = System.Environment.GetCommandLineArgs();
			_CommandSwitchCharacter = CommandCharacter;
		}

		public bool CommandLineSwitchOneIsSet(params string[] characters)
		{
			for (int i = 0; i < characters.Length; i++)
			{
				bool isExisting = CommandLineSwitchExistingExclusiv(characters[i]);
				if (isExisting)
				{
					return true;
				}
			}
			return false;
		}


		public bool CommandLineSwitchExistingExclusiv(params string[] characters)
		{
			bool retval = false;
			int matchedCommandCharacterCount = 0;
			int commandCharacterCount = 0;
			for (int i = 0; i < _CommandLineArgs.Length; i++)
			{
				for (int j = 0; j < characters.Length; j++)
				{
					if (_CommandLineArgs[i].ToLower(CultureInfo.InvariantCulture) == _CommandSwitchCharacter + characters[j].ToLower(System.Globalization.CultureInfo.InvariantCulture))
					{
						matchedCommandCharacterCount++;
					}
				}

				if (_CommandLineArgs[i].ToLower(CultureInfo.InvariantCulture).StartsWith(_CommandSwitchCharacter))
				{
					commandCharacterCount++;
				}
			}

			if (matchedCommandCharacterCount == commandCharacterCount)
			{
				if (matchedCommandCharacterCount == characters.Length)
				{
					retval = true;
				}
			}

			return retval;
		}

		public bool CommandLineSwitchCombinationExisting(params string[] characters)
		{
			bool retval = false;
			int matchedCommandCharacterCount = 0;
			int commandCharacterCount = 0;
			for (int i = 0; i < _CommandLineArgs.Length; i++)
			{
				for (int j = 0; j < characters.Length; j++)
				{
					if (_CommandLineArgs[i].ToLower(CultureInfo.InvariantCulture) == _CommandSwitchCharacter + characters[j].ToLower(CultureInfo.InvariantCulture))
					{
						matchedCommandCharacterCount++;
					}
				}

				if (_CommandLineArgs[i].ToLower(CultureInfo.InvariantCulture).StartsWith(_CommandSwitchCharacter))
				{
					commandCharacterCount++;
				}
			}

			if (matchedCommandCharacterCount == characters.Length)
			{
				retval = true;
			}

			return retval;
		}

		public string[] Valid(string character, int parameterNeeded)
		{
			if (CommandLineSwitchExistingExclusiv(character))
			{
				string[] param = CommandLineSwitchGetValues(character);
				if (param.Length == parameterNeeded)
				{
					return param;
				}
				else
				{
					Console.WriteLine("/" + character + " switch has invalid amount of parameters. Found " + param.Length + " parameters, required " + parameterNeeded.ToString());
					return null;
				}
			}
			return null;
		}

		public int GetSwitchIndex(string Character)
		{
			for (int i = 0; i < _CommandLineArgs.Length; i++)
			{
				if (_CommandLineArgs[i].ToLower(CultureInfo.InvariantCulture) == _CommandSwitchCharacter + Character.ToLower())
				{
					return i;
				}
			}
			return -1;
		}

		public string CommandLineSwitchGetValueAfter(string Character, int num)
		{
			bool isSet = CommandLineSwitchCombinationExisting(Character);
			if (isSet)
			{
				int index = GetSwitchIndex(Character);
				int up = _CommandLineArgs.GetUpperBound(0);
				if (up >= index + num)
				{
					return _CommandLineArgs[index + num];
				}
				else
				{
					return null;
				}

			}
			return null;
		}

		public string[] CommandLineSwitchGetValues(string Character)
		{
			bool isSet = CommandLineSwitchCombinationExisting(Character);
			if (isSet)
			{
				Debug.WriteLine(Character);
				int index = GetSwitchIndex(Character);
				int up = _CommandLineArgs.GetUpperBound(0);
				int i = 0;
				for (i = index; i <= up; i++)
				{
					if (!(i == index))
					{
						if (_CommandLineArgs[i].StartsWith(_CommandSwitchCharacter))
						{
							break;
						}
					}
				}

				int range = (i - index) - 1;

				if (range == 0)
				{
					return new string[0];
				}

				string[] retval = new string[range];
				int retvalpoi = 0;
				for (int k = index + 1; k < index + 1 + range; k++)
				{
					retval[retvalpoi] = _CommandLineArgs[k];
					retvalpoi++;
				}
				return retval;

			}
			return new string[0];
		}

		public bool CommandLineHasArguments()
		{
			if (_CommandLineArgs.GetUpperBound(0) > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool CommandLineHasSwitches()
		{
			int switchCount = 0;
			for (int i = 0; i < _CommandLineArgs.Length; i++)
			{
				if (_CommandLineArgs[i].ToLower(CultureInfo.InvariantCulture).StartsWith(_CommandSwitchCharacter))
				{
					switchCount++;
				}
			}

			if (switchCount == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}


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
			get { return !subSwitchInParameter; }
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
				if (args[i] == switchFull)
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
