using System;

namespace blob
{

	public class BlobArray
	{

		public static string[] Add(string[] array1, string[] array2)
		{
			int len = array1.Length + array2.Length;
			string[] retval = new string[len];
			Array.Copy(array1, 0, retval, 0, array1.Length);
			Array.Copy(array2, 0, retval, array1.Length, array2.Length);
			return retval;
		}

		public static long Position(byte[] buffer, byte[] search)
		{
			return Position(buffer,search,0);
		}


		public static long Position(byte[] buffer, byte[] search,int start)
		{
			if (start >= buffer.Length)
			{
				return -1;
			}

			for (int j = start; j < buffer.Length; j++)
			{
				int cmp = 0;
				for (int k = 0; k < search.Length; k++)
				{
					byte bu = buffer[j + k];
					byte se = search[k];
					if (bu == se)
					{
						cmp++;
					}
					else
					{
						break;
					}
				}

				if (cmp == search.Length)
				{
					return j;
				}
			}
			return -1;
		}


		public static string[] RemoveFromStart(string[] stringArray, string remove)
		{
			string[] retval = new string[stringArray.Length];

			for (int i = 0; i < stringArray.Length; i++)
			{
				if (stringArray[i].StartsWith(remove))
				{
					retval[i] = stringArray[i].Substring(remove.Length);

				}
				else
				{
					retval[i] = stringArray[i];
				}
			}
			return retval;
		}
	}
}
