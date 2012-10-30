// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZlibBaseStream.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
// http://www.sharpgis.net/post/2011/08/28/GZIP-Compressed-Web-Requests-in-WP7-Take-2.aspx
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WP7Contrib.Communications.Compression
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class ZlibBaseStream : Stream
    {
        protected internal ZlibCodec _z = (ZlibCodec)null;
        protected internal ZlibBaseStream.StreamMode _streamMode = ZlibBaseStream.StreamMode.Undefined;
        protected internal int _bufferSize = 8192;
        protected internal byte[] _buf1 = new byte[1];
        private bool nomoreinput = false;
        protected internal FlushType _flushMode;
        protected internal ZlibStreamFlavor _flavor;
        protected internal bool _leaveOpen;
        protected internal byte[] _workingBuffer;
        protected internal Stream _stream;
        private Crc32 crc;
        protected internal string _GzipFileName;
        protected internal string _GzipComment;
        protected internal DateTime _GzipMtime;
        protected internal int _gzipHeaderByteCount;

        internal int Crc32
        {
            get
            {
                if (this.crc == null)
                    return 0;
                else
                    return this.crc.Crc32Result;
            }
        }

        ZlibCodec z
        {
            get
            {
                if (this._z == null)
                {
                    bool expectRfc1950Header = this._flavor == ZlibStreamFlavor.ZLIB;
                    this._z = new ZlibCodec();
                    this._z.InitializeInflate(expectRfc1950Header);
                }
                return this._z;
            }
        }

        byte[] workingBuffer
        {
            get
            {
                if (this._workingBuffer == null)
                    this._workingBuffer = new byte[this._bufferSize];
                return this._workingBuffer;
            }
        }

        public override bool CanRead
        {
            get
            {
                return this._stream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this._stream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this._stream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return this._stream.Length;
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ZlibBaseStream(Stream stream, ZlibStreamFlavor flavor, bool leaveOpen)
        {
            this._flushMode = FlushType.None;
            this._stream = stream;
            this._leaveOpen = leaveOpen;
            this._flavor = flavor;
            if (flavor != ZlibStreamFlavor.GZIP)
                return;
            this.crc = new Crc32();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.crc != null)
                this.crc.SlurpBlock(buffer, offset, count);
            if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
                this._streamMode = ZlibBaseStream.StreamMode.Writer;
            else if (this._streamMode != ZlibBaseStream.StreamMode.Writer)
                throw new ZlibException("Cannot Write after Reading.");
            if (count == 0)
                return;
            this.z.InputBuffer = buffer;
            this._z.NextIn = offset;
            this._z.AvailableBytesIn = count;
            bool flag;
            do
            {
                this._z.OutputBuffer = this.workingBuffer;
                this._z.NextOut = 0;
                this._z.AvailableBytesOut = this._workingBuffer.Length;
                int num = this._z.Inflate(this._flushMode);
                if (num != 0 && num != 1)
                    throw new ZlibException("inflating: " + this._z.Message);
                this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
                flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
                if (this._flavor == ZlibStreamFlavor.GZIP)
                    flag = this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0;
            }
            while (!flag);
        }

        private void finish()
        {
            if (this._z == null)
                return;
            if (this._streamMode == ZlibBaseStream.StreamMode.Writer)
            {
                bool flag;
                do
                {
                    this._z.OutputBuffer = this.workingBuffer;
                    this._z.NextOut = 0;
                    this._z.AvailableBytesOut = this._workingBuffer.Length;
                    int num = this._z.Inflate(FlushType.Finish);
                    if (num != 1 && num != 0)
                        throw new ZlibException("inflating: " + this._z.Message);
                    if (this._workingBuffer.Length - this._z.AvailableBytesOut > 0)
                        this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
                    flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
                    if (this._flavor == ZlibStreamFlavor.GZIP)
                        flag = this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0;
                }
                while (!flag);
                this.Flush();
                if (this._flavor == ZlibStreamFlavor.GZIP)
                    throw new ZlibException("Writing with decompression is not supported.");
            }
            else
            {
                if (this._streamMode != ZlibBaseStream.StreamMode.Reader || this._flavor != ZlibStreamFlavor.GZIP || this._z.TotalBytesOut == 0L)
                    return;
                byte[] numArray = new byte[8];
                if (this._z.AvailableBytesIn != 8)
                    throw new ZlibException(string.Format("Protocol error. AvailableBytesIn={0}, expected 8", (object)this._z.AvailableBytesIn));
                Array.Copy((Array)this._z.InputBuffer, this._z.NextIn, (Array)numArray, 0, numArray.Length);
                int num1 = BitConverter.ToInt32(numArray, 0);
                int crc32Result = this.crc.Crc32Result;
                int num2 = BitConverter.ToInt32(numArray, 4);
                int num3 = (int)(this._z.TotalBytesOut & (long)uint.MaxValue);
                if (crc32Result != num1)
                    throw new ZlibException(string.Format("Bad CRC32 in GZIP stream. (actual({0:X8})!=expected({1:X8}))", (object)crc32Result, (object)num1));
                if (num3 != num2)
                    throw new ZlibException(string.Format("Bad size in GZIP stream. (actual({0})!=expected({1}))", (object)num3, (object)num2));
            }
        }

        private void end()
        {
            if (this.z == null)
                return;
            this._z.EndInflate();
            this._z = (ZlibCodec)null;
        }

        public override void Close()
        {
            if (this._stream == null)
                return;
            try
            {
                this.finish();
            }
            finally
            {
                this.end();
                if (!this._leaveOpen)
                    this._stream.Close();
                this._stream = (Stream)null;
            }
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            this._stream.SetLength(value);
        }

        private string ReadZeroTerminatedString()
        {
            List<byte> list = new List<byte>();
            bool flag = false;
            while (this._stream.Read(this._buf1, 0, 1) == 1)
            {
                if ((int)this._buf1[0] == 0)
                    flag = true;
                else
                    list.Add(this._buf1[0]);
                if (flag)
                {
                    byte[] bytes = list.ToArray();
                    return GZipStream.iso8859dash1.GetString(bytes, 0, bytes.Length);
                }
            }
            throw new ZlibException("Unexpected EOF reading GZIP header.");
        }

        private int _ReadAndValidateGzipHeader()
        {
            int num1 = 0;
            byte[] buffer1 = new byte[10];
            int num2 = this._stream.Read(buffer1, 0, buffer1.Length);
            switch (num2)
            {
                case 0:
                    return 0;
                case 10:
                    if ((int)buffer1[0] != 31 || (int)buffer1[1] != 139 || (int)buffer1[2] != 8)
                        throw new ZlibException("Bad GZIP header.");

                    int num3 = BitConverter.ToInt32(buffer1, 4);
                    this._GzipMtime = GZipStream._unixEpoch.AddSeconds((double)num3);
                    int num4 = num1 + num2;
                    if (((int)buffer1[3] & 4) == 4)
                    {
                        int num5 = this._stream.Read(buffer1, 0, 2);
                        int num6 = num4 + num5;
                        short num7 = (short)((int)buffer1[0] + (int)buffer1[1] * 256);
                        byte[] buffer2 = new byte[(int)num7];
                        int num8 = this._stream.Read(buffer2, 0, buffer2.Length);
                        if (num8 != (int)num7)
                            throw new ZlibException("Unexpected end-of-file reading GZIP header.");
                        num4 = num6 + num8;
                    }
                    if (((int)buffer1[3] & 8) == 8)
                        this._GzipFileName = this.ReadZeroTerminatedString();
                    if (((int)buffer1[3] & 16) == 16)
                        this._GzipComment = this.ReadZeroTerminatedString();
                    if (((int)buffer1[3] & 2) == 2)
                        this.Read(this._buf1, 0, 1);
                    return num4;
                default:
                    throw new ZlibException("Not a valid GZIP stream.");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
            {
                if (!this._stream.CanRead)
                    throw new ZlibException("The stream is not readable.");
                this._streamMode = ZlibBaseStream.StreamMode.Reader;
                this.z.AvailableBytesIn = 0;
                if (this._flavor == ZlibStreamFlavor.GZIP)
                {
                    this._gzipHeaderByteCount = this._ReadAndValidateGzipHeader();
                    if (this._gzipHeaderByteCount == 0)
                        return 0;
                }
            }
            if (this._streamMode != ZlibBaseStream.StreamMode.Reader)
                throw new ZlibException("Cannot Read after Writing.");
            if (count == 0)
                return 0;
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (offset < buffer.GetLowerBound(0))
                throw new ArgumentOutOfRangeException("offset");
            if (offset + count > buffer.GetLength(0))
                throw new ArgumentOutOfRangeException("count");
            this._z.OutputBuffer = buffer;
            this._z.NextOut = offset;
            this._z.AvailableBytesOut = count;
            this._z.InputBuffer = this.workingBuffer;
            int num;
            do
            {
                if (this._z.AvailableBytesIn == 0 && !this.nomoreinput)
                {
                    this._z.NextIn = 0;
                    this._z.AvailableBytesIn = this._stream.Read(this._workingBuffer, 0, this._workingBuffer.Length);
                    if (this._z.AvailableBytesIn == 0)
                        this.nomoreinput = true;
                }
                num = this._z.Inflate(this._flushMode);
                if (this.nomoreinput && num == -5)
                    return 0;
                if (num != 0 && num != 1)
                    throw new ZlibException(string.Format("inflating:  rc={0}  msg={1}", (object)num, (object)this._z.Message));
            }
            while ((!this.nomoreinput && num != 1 || this._z.AvailableBytesOut != count) && (this._z.AvailableBytesOut > 0 && !this.nomoreinput && num == 0));
            if (this._z.AvailableBytesOut > 0)
            {
                if (num != 0 || this._z.AvailableBytesIn != 0)
                {}
                if (!this.nomoreinput)
                {}
            }
            int count1 = count - this._z.AvailableBytesOut;
            if (this.crc != null)
                this.crc.SlurpBlock(buffer, offset, count1);
            return count1;
        }

        internal enum StreamMode
        {
            Writer,
            Reader,
            Undefined,
        }
    }
}
