using System;

namespace blob
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class BlobObject
	{
		public static bool IsSet(object @object)
		{
			if (!(Equals(@object, null)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
