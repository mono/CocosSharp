// Copyright (c) 2006, ComponentAce
// http://www.componentace.com
// All rights reserved.

// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

// Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// Neither the name of ComponentAce nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

/*
Copyright (c) 2001 Lapo Luchini.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright 
notice, this list of conditions and the following disclaimer in 
the documentation and/or other materials provided with the distribution.

3. The names of the authors may not be used to endorse or promote products
derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS
OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
/*
* This program is based on zlib-1.1.3, so all credit should go authors
* Jean-loup Gailly(jloup@gzip.org) and Mark Adler(madler@alumni.caltech.edu)
* and contributors of zlib.
*/

using System.IO;

namespace Cocos2D.Compression.Zlib
{
    public class ZInputStream : BinaryReader
    {
        protected byte[] buf, buf1 = new byte[1];
        protected int bufsize = 512;
        protected bool compress;
        protected int flush;

        internal Stream in_Renamed;
        internal bool nomoreinput;
        protected ZStream z = new ZStream();

        public ZInputStream(Stream in_Renamed) : base(in_Renamed)
        {
            InitBlock();
            this.in_Renamed = in_Renamed;
            z.InflateInit();
            compress = false;
            z.next_in = buf;
            z.next_in_index = 0;
            z.avail_in = 0;
        }

        public ZInputStream(Stream in_Renamed, int level) : base(in_Renamed)
        {
            InitBlock();
            this.in_Renamed = in_Renamed;
            z.DeflateInit(level);
            compress = true;
            z.next_in = buf;
            z.next_in_index = 0;
            z.avail_in = 0;
        }

        public virtual int FlushMode
        {
            get { return (flush); }

            set { flush = value; }
        }

        /// <summary> Returns the total number of bytes input so far.</summary>
        public virtual long TotalIn
        {
            get { return z.total_in; }
        }

        /// <summary> Returns the total number of bytes output so far.</summary>
        public virtual long TotalOut
        {
            get { return z.total_out; }
        }

        internal void InitBlock()
        {
            flush = zlibConst.Z_NO_FLUSH;
            buf = new byte[bufsize];
        }

        /*public int available() throws IOException {
		return inf.finished() ? 0 : 1;
		}*/

        public override int Read()
        {
            if (Read(buf1, 0, 1) == - 1)
                return (- 1);
            return (buf1[0] & 0xFF);
        }

        public override int Read(byte[] b, int off, int len)
        {
            if (len == 0)
                return (0);
            int err;
            z.next_out = b;
            z.next_out_index = off;
            z.avail_out = len;
            do
            {
                if ((z.avail_in == 0) && (!nomoreinput))
                {
                    // if buffer is empty and more input is avaiable, refill it
                    z.next_in_index = 0;
                    z.avail_in = SupportClass.ReadInput(in_Renamed, buf, 0, bufsize);
                        //(bufsize<z.avail_out ? bufsize : z.avail_out));
                    if (z.avail_in == - 1)
                    {
                        z.avail_in = 0;
                        nomoreinput = true;
                    }
                }
                if (compress)
                    err = z.Deflate(flush);
                else
                    err = z.Inflate(flush);
                if (nomoreinput && (err == zlibConst.Z_BUF_ERROR))
                    return (- 1);
                if (err != zlibConst.Z_OK && err != zlibConst.Z_STREAM_END)
                    throw new ZStreamException((compress ? "de" : "in") + "flating: " + z.msg);
                if (nomoreinput && (z.avail_out == len))
                    return (0);
            } while (z.avail_out == len && err == zlibConst.Z_OK);
            //System.err.print("("+(len-z.avail_out)+")");
            return (len - z.avail_out);
        }

        public long Skip(long n)
        {
            int len = 512;
            if (n < len)
                len = (int) n;
            var tmp = new byte[len];
            return (SupportClass.ReadInput(BaseStream, tmp, 0, tmp.Length));
        }
#if NETFX_CORE
        public void Close()
#else
        public override void Close()
#endif
        {
            in_Renamed.Close();
        }
    }
}