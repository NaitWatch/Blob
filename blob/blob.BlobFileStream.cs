using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace blob
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class BlobFileStream : IDisposable
	{
		public enum Type
		{
			Writer,
			Reader,
		}

		[DllImport("kernel32", SetLastError = true)]
		private static extern bool FlushFileBuffers(IntPtr handle);

		private FileStream fileStream;

		public BlobFileStream(string location, Type type)
		{
			if (type == Type.Writer)
			{
				fileStream = new FileStream(location, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
			}
			else if (type == Type.Reader)
			{
				fileStream = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
		}

		public long Position
		{
			get { return fileStream.Position; }
			set { fileStream.Position = value; }
		}

		public long Length
		{
			get { return fileStream.Length; }
		}

		public Guid ReadGuid()
		{
			byte[] bytes = new byte[16];
			fileStream.Read(bytes, 0, bytes.Length);
			return new Guid(bytes);
		}

		public int ReadInt()
		{
			byte[] bytes = new byte[4];
			fileStream.Read(bytes, 0, bytes.Length);
			return System.BitConverter.ToInt32(bytes, 0);
		}

		public bool ReadBool()
		{
			byte[] bytes = new byte[1];
			fileStream.Read(bytes, 0, bytes.Length);
			return System.BitConverter.ToBoolean(bytes, 0);
		}

		public string ReadString(int len, Encoding encoding)
		{
			byte[] bytes = new byte[len];
			fileStream.Read(bytes, 0, bytes.Length);
			string na = encoding.GetString(bytes);
			return na;
		}

		public string ReadString(Encoding encoding)
		{
			int len = ReadInt();
			return ReadString(len, encoding);
		}

		public byte[] ReadBytes(int len)
		{
			byte[] bytes = new byte[len];
			fileStream.Read(bytes, 0, bytes.Length);
			return bytes;
		}

		public byte[] ReadBytes()
		{
			int len = ReadInt();
			return ReadBytes(len);
		}

		public void ReadBytesToFile(string file)
		{
			int len = ReadInt();
			long pos = this.fileStream.Position;
			int buffersize = 1024*10*10;

			using(FileStream destStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
			{
				byte[] buffer = new byte[buffersize];
				int readed = 0;
				do
				{
					int read = this.fileStream.Read(buffer,0,buffer.Length);
					destStream.Write(buffer,0,read);
					destStream.Flush();
					readed =  readed + read;
				}while(len > readed);
				this.fileStream.Position = pos + len;
				destStream.SetLength(len);
				destStream.Close();
			}

		}

		public void Write(byte[] bytes, bool withLen)
		{
			if (withLen)
			{
				Write(bytes.Length);
				Write(bytes, 0, bytes.Length);
			}
			else
			{
				Write(bytes, 0, bytes.Length);
			}
		}

		public void WriteFromReadStream(string filename, bool withLen)
		{
			if (withLen)
			{
				System.IO.FileInfo nfo = new FileInfo(filename);
				Write((int)nfo.Length);
				WriteFromReadStream(filename);
			}
			else
			{
				WriteFromReadStream(filename);
			}
		}

		public void WriteFromReadStream(string filename)
		{
			FileStream sourceStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			int buffersize = 1024*10*10;
			byte[] buffer = new byte[buffersize];
			do
			{
				int read = sourceStream.Read(buffer,0,buffer.Length);
				this.Write(buffer,0,read);
				
			}while(sourceStream.Position != sourceStream.Length);
		}

		/// <summary>
		/// Write a string as bytearray to the stream
		/// </summary>
		/// <param name="string"></param>
		/// <param name="encoding"></param>
		/// <param name="withlen"></param>
		public void Write(string @string, Encoding encoding, bool withlen)
		{
			byte[] bytes = encoding.GetBytes(@string);
			if (withlen)
			{
				Write(bytes.Length);
				Write(bytes);
			}
			else
			{
				Write(bytes);
			}
		}

		public void Write(int integer)
		{
			byte[] bytes = System.BitConverter.GetBytes(integer);
			Write(bytes, 0, bytes.Length);
		}

		public void Write(bool boolean)
		{
			byte[] bytes = System.BitConverter.GetBytes(boolean);
			Write(bytes, 0, bytes.Length);
		}

		public void Write(byte[] bytes)
		{
			Write(bytes, 0, bytes.Length);
		}

		public void Write(byte[] bytes, int offset, int count)
		{
			fileStream.Write(bytes, offset, count);
			fileStream.Flush();
		}

		public void SeekEnd()
		{
			fileStream.Position = fileStream.Length;
		}

		public long Seek(byte[] vs)
		{
			byte[] buffer = new byte[vs.Length * 2];
			for (long i = 0; i < fileStream.Length; i = i + (vs.Length))
			{
				fileStream.Position = i;
				fileStream.Read(buffer, 0, buffer.Length);

				long pos = IsIn(buffer, vs);
				if (pos != -1)
				{
					return fileStream.Position - buffer.Length + pos;
				}
			}
			return -1;
		}

		public long IsIn(byte[] buffer, byte[] search)
		{
			for (int j = 0; j < search.Length; j++)
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

		public void Flush()
		{
			fileStream.Flush();
			FlushFileBuffers(fileStream.Handle);
		}

		public void Close()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			fileStream.Close();
		}
	}

}
