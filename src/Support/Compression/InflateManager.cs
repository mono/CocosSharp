// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InflateManager.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
// http://www.sharpgis.net/post/2011/08/28/GZIP-Compressed-Web-Requests-in-WP7-Take-2.aspx
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WP7Contrib.Communications.Compression
{
    internal sealed class InflateManager
    {
		static byte[] Mark = { (byte) 0, (byte) 0, byte.MaxValue, byte.MaxValue };
		const int PRESET_DICT = 32;
		const int Z_DEFLATED = 8;
		const int METHOD = 0;
		const int FLAG = 1;
		const int DICT4 = 2;
		const int DICT3 = 3;
		const int DICT2 = 4;
		const int DICT1 = 5;
		const int DICT0 = 6;
		const int BLOCKS = 7;
		const int CHECK4 = 8;
		const int CHECK3 = 9;
		const int CHECK2 = 10;
		const int CHECK1 = 11;
		const int DONE = 12;
		const int BAD = 13;

		internal int Mode;
		internal int Method;
		internal int Marker;
		internal int Wbits;
		internal ZlibCodec Codec;
		internal InflateBlocks Blocks;
		internal long Need;
		internal long[] Was = new long[1];


		#region Properties

		internal bool HandleRfc1950HeaderBytes { get; set; }

		#endregion Properties


		#region Constructors

        static InflateManager()
        {
        }

		public InflateManager() : this(true)
        {
        }

		public InflateManager(bool expectRfc1950HeaderBytes=true)
        {
			HandleRfc1950HeaderBytes = expectRfc1950HeaderBytes;
        }

		#endregion Constructors


        internal int Reset()
        {
            this.Codec.TotalBytesIn = this.Codec.TotalBytesOut = 0L;
            this.Codec.Message = (string)null;
            this.Mode = this.HandleRfc1950HeaderBytes ? 0 : 7;
            this.Blocks.Reset((long[])null);
            return 0;
        }

        internal int End()
        {
            if (this.Blocks != null)
                this.Blocks.Free();
            this.Blocks = (InflateBlocks)null;
            return 0;
        }

        internal int Initialize(ZlibCodec codec, int w)
        {
            this.Codec = codec;
            this.Codec.Message = (string)null;
            this.Blocks = (InflateBlocks)null;
            if (w < 8 || w > 15)
            {
                this.End();
                throw new ZlibException("Bad window size.");
            }
            else
            {
                this.Wbits = w;
                this.Blocks = new InflateBlocks(codec, this.HandleRfc1950HeaderBytes ? (object)this : (object)(InflateManager)null, 1 << w);
                this.Reset();
                return 0;
            }
        }

        internal int Inflate(FlushType flush)
        {
            int num1 = (int)flush;
            if (this.Codec.InputBuffer == null)
                throw new ZlibException("InputBuffer is null. ");
            int num2 = num1 == 4 ? -5 : 0;
            int r = -5;
            while (true)
            {
                switch (this.Mode)
                {
                    case 0:
                        if (this.Codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this.Codec.AvailableBytesIn;
                            ++this.Codec.TotalBytesIn;
                            InflateManager inflateManager = this;
                            byte[] numArray = this.Codec.InputBuffer;
                            int index = this.Codec.NextIn++;
                            int num3;
                            int num4 = num3 = (int)numArray[index];
                            inflateManager.Method = num3;
                            if ((num4 & 15) != 8)
                            {
                                this.Mode = 13;
                                this.Codec.Message = string.Format("unknown compression Method (0x{0:X2})", (object)this.Method);
                                this.Marker = 5;
                                break;
                            }
                            else if ((this.Method >> 4) + 8 > this.Wbits)
                            {
                                this.Mode = 13;
                                this.Codec.Message = string.Format("invalid window size ({0})", (object)((this.Method >> 4) + 8));
                                this.Marker = 5;
                                break;
                            }
                            else
                            {
                                this.Mode = 1;
                                goto case 1;
                            }
                        }
                        else
                            goto label_4;
                    case 1:
                        if (this.Codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this.Codec.AvailableBytesIn;
                            ++this.Codec.TotalBytesIn;
                            int num3 = (int)this.Codec.InputBuffer[this.Codec.NextIn++] & (int)byte.MaxValue;
                            if (((this.Method << 8) + num3) % 31 != 0)
                            {
                                this.Mode = 13;
                                this.Codec.Message = "incorrect header check";
                                this.Marker = 5;
                                break;
                            }
                            else if ((num3 & 32) == 0)
                            {
                                this.Mode = 7;
                                break;
                            }
                            else
                                goto label_16;
                        }
                        else
                            goto label_11;
                    case 2:
                        goto label_17;
                    case 3:
                        goto label_20;
                    case 4:
                        goto label_23;
                    case 5:
                        goto label_26;
                    case 6:
                        goto label_29;
                    case 7:
                        r = this.Blocks.Process(r);
                        if (r == -3)
                        {
                            this.Mode = 13;
                            this.Marker = 0;
                            break;
                        }
                        else
                        {
                            if (r == 0)
                                r = num2;
                            if (r == 1)
                            {
                                r = num2;
                                this.Blocks.Reset(this.Was);
                                if (!this.HandleRfc1950HeaderBytes)
                                {
                                    this.Mode = 12;
                                    break;
                                }
                                else
                                {
                                    this.Mode = 8;
                                    goto case 8;
                                }
                            }
                            else
                                goto label_35;
                        }
                    case 8:
                        if (this.Codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this.Codec.AvailableBytesIn;
                            ++this.Codec.TotalBytesIn;
                            this.Need = (long)(((int)this.Codec.InputBuffer[this.Codec.NextIn++] & (int)byte.MaxValue) << 24 & -16777216);
                            this.Mode = 9;
                            goto case 9;
                        }
                        else
                            goto label_40;
                    case 9:
                        if (this.Codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this.Codec.AvailableBytesIn;
                            ++this.Codec.TotalBytesIn;
                            this.Need += (long)(((int)this.Codec.InputBuffer[this.Codec.NextIn++] & (int)byte.MaxValue) << 16) & 16711680L;
                            this.Mode = 10;
                            goto case 10;
                        }
                        else
                            goto label_43;
                    case 10:
                        if (this.Codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this.Codec.AvailableBytesIn;
                            ++this.Codec.TotalBytesIn;
                            this.Need += (long)(((int)this.Codec.InputBuffer[this.Codec.NextIn++] & (int)byte.MaxValue) << 8) & 65280L;
                            this.Mode = 11;
                            goto case 11;
                        }
                        else
                            goto label_46;
                    case 11:
                        if (this.Codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this.Codec.AvailableBytesIn;
                            ++this.Codec.TotalBytesIn;
                            this.Need += (long)this.Codec.InputBuffer[this.Codec.NextIn++] & (long)byte.MaxValue;
                            if ((int)this.Was[0] != (int)this.Need)
                            {
                                this.Mode = 13;
                                this.Codec.Message = "incorrect data check";
                                this.Marker = 5;
                                break;
                            }
                            else
                                goto label_52;
                        }
                        else
                            goto label_49;
                    case 12:
                        goto label_53;
                    case 13:
                        goto label_54;
                    default:
                        goto label_55;
                }
            }
            label_4:
            return r;
            label_11:
            return r;
            label_16:
            this.Mode = 2;
            label_17:
            if (this.Codec.AvailableBytesIn == 0)
                return r;
            r = num2;
            --this.Codec.AvailableBytesIn;
            ++this.Codec.TotalBytesIn;
            this.Need = (long)(((int)this.Codec.InputBuffer[this.Codec.NextIn++] & (int)byte.MaxValue) << 24 & -16777216);
            this.Mode = 3;
            label_20:
            if (this.Codec.AvailableBytesIn == 0)
                return r;
            r = num2;
            --this.Codec.AvailableBytesIn;
            ++this.Codec.TotalBytesIn;
            this.Need += (long)(((int)this.Codec.InputBuffer[this.Codec.NextIn++] & (int)byte.MaxValue) << 16) & 16711680L;
            this.Mode = 4;
            label_23:
            if (this.Codec.AvailableBytesIn == 0)
                return r;
            r = num2;
            --this.Codec.AvailableBytesIn;
            ++this.Codec.TotalBytesIn;
            this.Need += (long)(((int)this.Codec.InputBuffer[this.Codec.NextIn++] & (int)byte.MaxValue) << 8) & 65280L;
            this.Mode = 5;
            label_26:
            if (this.Codec.AvailableBytesIn == 0)
                return r;
            --this.Codec.AvailableBytesIn;
            ++this.Codec.TotalBytesIn;
            this.Need += (long)this.Codec.InputBuffer[this.Codec.NextIn++] & (long)byte.MaxValue;
            this.Codec.Adler32 = this.Need;
            this.Mode = 6;
            return 2;
            label_29:
            this.Mode = 13;
            this.Codec.Message = "Need dictionary";
            this.Marker = 0;
            return -2;
            label_35:
            return r;
            label_40:
            return r;
            label_43:
            return r;
            label_46:
            return r;
            label_49:
            return r;
            label_52:
            this.Mode = 12;
            label_53:
            return 1;
            label_54:
            throw new ZlibException(string.Format("Bad state ({0})", (object)this.Codec.Message));
            label_55:
            throw new ZlibException("Stream error.");
        }

        internal int SetDictionary(byte[] dictionary)
        {
            int start = 0;
            int n = dictionary.Length;
            if (this.Mode != 6)
                throw new ZlibException("Stream error.");
            if (Adler.Adler32(1L, dictionary, 0, dictionary.Length) != this.Codec.Adler32)
                return -3;
            this.Codec.Adler32 = Adler.Adler32(0L, (byte[])null, 0, 0);
            if (n >= 1 << this.Wbits)
            {
                n = (1 << this.Wbits) - 1;
                start = dictionary.Length - n;
            }
            this.Blocks.SetDictionary(dictionary, start, n);
            this.Mode = 7;
            return 0;
        }

        internal int Sync()
        {
            if (this.Mode != 13)
            {
                this.Mode = 13;
                this.Marker = 0;
            }
            int num1;
            if ((num1 = this.Codec.AvailableBytesIn) == 0)
                return -5;
            int index1 = this.Codec.NextIn;
            int index2;
            for (index2 = this.Marker; num1 != 0 && index2 < 4; --num1)
            {
                if ((int)this.Codec.InputBuffer[index1] == (int)InflateManager.Mark[index2])
                    ++index2;
                else
                    index2 = (int)this.Codec.InputBuffer[index1] == 0 ? 4 - index2 : 0;
                ++index1;
            }
            this.Codec.TotalBytesIn += (long)(index1 - this.Codec.NextIn);
            this.Codec.NextIn = index1;
            this.Codec.AvailableBytesIn = num1;
            this.Marker = index2;
            if (index2 != 4)
                return -3;
            long num2 = this.Codec.TotalBytesIn;
            long num3 = this.Codec.TotalBytesOut;
            this.Reset();
            this.Codec.TotalBytesIn = num2;
            this.Codec.TotalBytesOut = num3;
            this.Mode = 7;
            return 0;
        }

        internal int SyncPoint(ZlibCodec z)
        {
            return this.Blocks.SyncPoint();
        }
    }
}