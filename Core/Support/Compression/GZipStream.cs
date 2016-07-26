// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GZipStream.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
// http://www.sharpgis.net/post/2011/08/28/GZIP-Compressed-Web-Requests-in-WP7-Take-2.aspx
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WP7Contrib.Communications.Compression
{
    using System;
    using System.IO;
    using System.Text;

    internal class GZipStream : Stream
    {
		internal static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		internal static Encoding Iso8859dash1 = Encoding.GetEncoding("iso-8859-1");

        public DateTime? LastModified;
		internal ZlibBaseStream BaseStream;

        bool disposed;
        bool firstReadDone;
		string fileName;
		string comment;


		#region Properties

		public int Crc32 { get; private set; }

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanRead
		{
			get
			{
				if (this.disposed)
					throw new ObjectDisposedException("GZipStream");
				else
					return this.BaseStream._stream.CanRead;
			}
		}

		public override bool CanWrite
		{
			get
			{
				if (this.disposed)
					throw new ObjectDisposedException("GZipStream");
				else
					return this.BaseStream._stream.CanWrite;
			}
		}

		public int BufferSize
		{
			get
			{
				return this.BaseStream._bufferSize;
			}
			set
			{
				if (this.disposed)
					throw new ObjectDisposedException("GZipStream");
				if (this.BaseStream._workingBuffer != null)
					throw new ZlibException("The working buffer is already set.");
				if (value < 128)
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer.", (object)value));
				this.BaseStream._bufferSize = value;
			}
		}

		public virtual long TotalIn
		{
			get { return this.BaseStream._z.TotalBytesIn; }
		}

		public virtual long TotalOut
		{
			get { return this.BaseStream._z.TotalBytesOut; }
		}

		public override long Length
		{
			get { return this.BaseStream.Length; }
		}

		public override long Position
		{
			get
			{
				if (this.BaseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
					return this.BaseStream._z.TotalBytesIn + (long)this.BaseStream._gzipHeaderByteCount;
				else
					return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                if (this.disposed)
                    throw new ObjectDisposedException("GZipStream");
                this.comment = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                if (this.disposed)
                    throw new ObjectDisposedException("GZipStream");
                this.fileName = value;
                if (this.fileName == null)
                    return;
                if (this.fileName.IndexOf("/") != -1)
                    this.fileName = this.fileName.Replace("/", "\\");
                if (this.fileName.EndsWith("\\"))
                    throw new Exception("Illegal filename");
                if (this.fileName.IndexOf("\\") == -1)
                    return;
                this.fileName = Path.GetFileName(this.fileName);
            }
        }

        public virtual FlushType FlushMode
        {
            get
            {
                return this.BaseStream._flushMode;
            }
            set
            {
                if (this.disposed)
                    throw new ObjectDisposedException("GZipStream");
                this.BaseStream._flushMode = value;
            }
        }

		#endregion Properties


		#region Constructors

        static GZipStream()
        {
        }

        public GZipStream(Stream stream)
        {
            this.BaseStream = new ZlibBaseStream(stream, ZlibStreamFlavor.GZIP, false);
        }

		#endregion Constructors


		#region Cleaning up

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (this.disposed)
                    return;
                if (disposing && this.BaseStream != null)
                {
                    this.BaseStream.Close();
                    this.Crc32 = this.BaseStream.Crc32;
                }
                this.disposed = true;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

		#endregion Cleaning up


        public override void Flush()
        {
            if (this.disposed)
                throw new ObjectDisposedException("GZipStream");
            this.BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.disposed)
                throw new ObjectDisposedException("GZipStream");
            int num = this.BaseStream.Read(buffer, offset, count);
            if (!this.firstReadDone)
            {
                this.firstReadDone = true;
                this.FileName = this.BaseStream._GzipFileName;
                this.Comment = this.BaseStream._GzipComment;
            }
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        int EmitHeader()
        {
            byte[] numArray1 = this.Comment == null ? (byte[])null : GZipStream.Iso8859dash1.GetBytes(this.Comment);
            byte[] numArray2 = this.FileName == null ? (byte[])null : GZipStream.Iso8859dash1.GetBytes(this.FileName);
            int num1 = this.Comment == null ? 0 : numArray1.Length + 1;
            int num2 = this.FileName == null ? 0 : numArray2.Length + 1;
            byte[] buffer = new byte[10 + num1 + num2];
            int num3 = 0;
            byte[] numArray3 = buffer;
            int index1 = num3;
            int num4 = 1;
            int num5 = index1 + num4;
            int num6 = 31;
            numArray3[index1] = (byte)num6;
            byte[] numArray4 = buffer;
            int index2 = num5;
            int num7 = 1;
            int num8 = index2 + num7;
            int num9 = 139;
            numArray4[index2] = (byte)num9;
            byte[] numArray5 = buffer;
            int index3 = num8;
            int num10 = 1;
            int num11 = index3 + num10;
            int num12 = 8;
            numArray5[index3] = (byte)num12;
            byte num13 = (byte)0;
            if (this.Comment != null)
                num13 ^= (byte)16;
            if (this.FileName != null)
                num13 ^= (byte)8;
            byte[] numArray6 = buffer;
            int index4 = num11;
            int num14 = 1;
            int destinationIndex1 = index4 + num14;
            int num15 = (int)num13;
            numArray6[index4] = (byte)num15;
            if (!this.LastModified.HasValue)
                this.LastModified = new DateTime?(DateTime.Now);
			Array.Copy((Array)BitConverter.GetBytes((int)(this.LastModified.Value - GZipStream.UnixEpoch).TotalSeconds), 0, (Array)buffer, destinationIndex1, 4);
            int num16 = destinationIndex1 + 4;
            byte[] numArray7 = buffer;
            int index5 = num16;
            int num17 = 1;
            int num18 = index5 + num17;
            int num19 = 0;
            numArray7[index5] = (byte)num19;
            byte[] numArray8 = buffer;
            int index6 = num18;
            int num20 = 1;
            int destinationIndex2 = index6 + num20;
            int num21 = (int)byte.MaxValue;
            numArray8[index6] = (byte)num21;
            if (num2 != 0)
            {
                Array.Copy((Array)numArray2, 0, (Array)buffer, destinationIndex2, num2 - 1);
                int num22 = destinationIndex2 + (num2 - 1);
                byte[] numArray9 = buffer;
                int index7 = num22;
                int num23 = 1;
                destinationIndex2 = index7 + num23;
                int num24 = 0;
                numArray9[index7] = (byte)num24;
            }
            if (num1 != 0)
            {
                Array.Copy((Array)numArray1, 0, (Array)buffer, destinationIndex2, num1 - 1);
                int num22 = destinationIndex2 + (num1 - 1);
                byte[] numArray9 = buffer;
                int index7 = num22;
                int num23 = 1;
                int num24 = index7 + num23;
                int num25 = 0;
                numArray9[index7] = (byte)num25;
            }
            this.BaseStream._stream.Write(buffer, 0, buffer.Length);
            return buffer.Length;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}