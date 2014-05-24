// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZlibCodec.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
// http://www.sharpgis.net/post/2011/08/28/GZIP-Compressed-Web-Requests-in-WP7-Take-2.aspx
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WP7Contrib.Communications.Compression
{
    internal sealed class ZlibCodec
    {
        public int WindowBits = 15;
		public int NextIn;
		public int NextOut;
		public int AvailableBytesIn;
		public int AvailableBytesOut;

		public long TotalBytesIn;
		public long TotalBytesOut;

        public byte[] InputBuffer;
        public byte[] OutputBuffer;

        public string Message;
        
		internal InflateManager Istate;


		#region Properties

		public long Adler32 { get; internal set; }

		#endregion Properties


		#region Constructors
        
		public ZlibCodec()
        {
            if (this.InitializeInflate() != 0)
                throw new ZlibException("Cannot initialize for inflate.");
        }

		#endregion Constructors


        public int InitializeInflate()
        {
            return this.InitializeInflate(this.WindowBits);
        }

        public int InitializeInflate(bool expectRfc1950Header)
        {
            return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
        }

        public int InitializeInflate(int windowBits)
        {
            this.WindowBits = windowBits;
            return this.InitializeInflate(windowBits, true);
        }

        public int InitializeInflate(int windowBits, bool expectRfc1950Header)
        {
            this.WindowBits = windowBits;
            this.Istate = new InflateManager(expectRfc1950Header);
            return this.Istate.Initialize(this, windowBits);
        }

        public int Inflate(FlushType flush)
        {
            if (this.Istate == null)
                throw new ZlibException("No Inflate State!");
            else
                return this.Istate.Inflate(flush);
        }

        public int EndInflate()
        {
            if (this.Istate == null)
                throw new ZlibException("No Inflate State!");
            int num = this.Istate.End();
            this.Istate = (InflateManager)null;
            return num;
        }

        public int SyncInflate()
        {
            if (this.Istate == null)
                throw new ZlibException("No Inflate State!");
            else
                return this.Istate.Sync();
        }

        public int SetDictionary(byte[] dictionary)
        {
            if (this.Istate != null)
                return this.Istate.SetDictionary(dictionary);
            else
                throw new ZlibException("No Inflate state!");
        }
    }
}