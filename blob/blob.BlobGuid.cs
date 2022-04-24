using System;

namespace blob
{
	public class BlobGuid
	{
		public enum GUID
		{
			METABLOCK,
			DATABLOCK,
		}

		public static Guid GetGuidConst(GUID guid)
		{
			switch (guid)
			{
				case GUID.METABLOCK:
					return new Guid("{41DFF925-BC8C-4d52-89B7-EAFAFA434975}");
				case GUID.DATABLOCK:
					return new Guid("{26DAB176-91E9-4f68-8D21-F1CBBEE940CD}");
				default:
					return new Guid("{00000000-0000-0000-0000-000000000000}");
			}
		}


		public static byte[] GetGuid(GUID guid)
		{
			return GetGuidConst(guid).ToByteArray();
		}
	}
}
