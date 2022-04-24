using System;
using System.IO;
using System.Xml.Serialization;

namespace blob
{
	public class BlobSerializer
	{
		public void ObjectToDisk(string filename)
		{
			string content = ObjectToXmlString();
			using (System.IO.StreamWriter streamWriter = System.IO.File.CreateText(filename))
			{
				streamWriter.Write(content);
				streamWriter.Close();
			}
		}

		public object ObjectFromDisk(string filename)
		{
			object content = null;
			using (System.IO.StreamReader streamReader = System.IO.File.OpenText(filename))
			{
				content = ObjectFromXmlString(streamReader.ReadToEnd());
				streamReader.Close();
			}
			return content;
		}

		public string ObjectToXmlString()
		{
			byte[] retval = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (System.IO.TextWriter writer = new System.IO.StreamWriter(memoryStream,System.Text.Encoding.UTF8))
				{
					XmlSerializer serializer = new XmlSerializer(this.GetType());
					serializer.Serialize(writer, this);
					writer.Close();
				}
				retval = memoryStream.ToArray();
			}
			return System.Text.Encoding.UTF8.GetString(retval);
		}

		public object ObjectFromXmlString(string @string)
		{
			byte[] StringAsBytes = System.Text.Encoding.UTF8.GetBytes(@string);
			object retval = null;
			using (MemoryStream memoryStream = new MemoryStream(StringAsBytes))
			{
				using (System.IO.TextReader reader = new StreamReader(memoryStream,System.Text.Encoding.UTF8))
				{
					XmlSerializer serializer = new XmlSerializer(this.GetType());
					retval = serializer.Deserialize(reader);
					reader.Close();
				}
			}
			return retval;
		}
	}

}
