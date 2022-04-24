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

}
