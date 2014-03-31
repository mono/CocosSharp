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
        private static byte[] mark = new byte[4]
                                         {
                                             (byte) 0,
                                             (byte) 0,
                                             byte.MaxValue,
                                             byte.MaxValue
                                         };
        internal long[] was = new long[1];
        private bool _handleRfc1950HeaderBytes = true;
        private const int PRESET_DICT = 32;
        private const int Z_DEFLATED = 8;
        private const int METHOD = 0;
        private const int FLAG = 1;
        private const int DICT4 = 2;
        private const int DICT3 = 3;
        private const int DICT2 = 4;
        private const int DICT1 = 5;
        private const int DICT0 = 6;
        private const int BLOCKS = 7;
        private const int CHECK4 = 8;
        private const int CHECK3 = 9;
        private const int CHECK2 = 10;
        private const int CHECK1 = 11;
        private const int DONE = 12;
        private const int BAD = 13;
        internal int mode;
        internal ZlibCodec _codec;
        internal int method;
        internal long need;
        internal int marker;
        internal int wbits;
        internal InflateBlocks blocks;

        internal bool HandleRfc1950HeaderBytes
        {
            get
            {
                return this._handleRfc1950HeaderBytes;
            }
            set
            {
                this._handleRfc1950HeaderBytes = value;
            }
        }

        static InflateManager()
        {
        }

        public InflateManager()
        {
        }

        public InflateManager(bool expectRfc1950HeaderBytes)
        {
            this._handleRfc1950HeaderBytes = expectRfc1950HeaderBytes;
        }

        internal int Reset()
        {
            this._codec.TotalBytesIn = this._codec.TotalBytesOut = 0L;
            this._codec.Message = (string)null;
            this.mode = this.HandleRfc1950HeaderBytes ? 0 : 7;
            this.blocks.Reset((long[])null);
            return 0;
        }

        internal int End()
        {
            if (this.blocks != null)
                this.blocks.Free();
            this.blocks = (InflateBlocks)null;
            return 0;
        }

        internal int Initialize(ZlibCodec codec, int w)
        {
            this._codec = codec;
            this._codec.Message = (string)null;
            this.blocks = (InflateBlocks)null;
            if (w < 8 || w > 15)
            {
                this.End();
                throw new ZlibException("Bad window size.");
            }
            else
            {
                this.wbits = w;
                this.blocks = new InflateBlocks(codec, this.HandleRfc1950HeaderBytes ? (object)this : (object)(InflateManager)null, 1 << w);
                this.Reset();
                return 0;
            }
        }

        internal int Inflate(FlushType flush)
        {
            int num1 = (int)flush;
            if (this._codec.InputBuffer == null)
                throw new ZlibException("InputBuffer is null. ");
            int num2 = num1 == 4 ? -5 : 0;
            int r = -5;
            while (true)
            {
                switch (this.mode)
                {
                    case 0:
                        if (this._codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this._codec.AvailableBytesIn;
                            ++this._codec.TotalBytesIn;
                            InflateManager inflateManager = this;
                            byte[] numArray = this._codec.InputBuffer;
                            int index = this._codec.NextIn++;
                            int num3;
                            int num4 = num3 = (int)numArray[index];
                            inflateManager.method = num3;
                            if ((num4 & 15) != 8)
                            {
                                this.mode = 13;
                                this._codec.Message = string.Format("unknown compression method (0x{0:X2})", (object)this.method);
                                this.marker = 5;
                                break;
                            }
                            else if ((this.method >> 4) + 8 > this.wbits)
                            {
                                this.mode = 13;
                                this._codec.Message = string.Format("invalid window size ({0})", (object)((this.method >> 4) + 8));
                                this.marker = 5;
                                break;
                            }
                            else
                            {
                                this.mode = 1;
                                goto case 1;
                            }
                        }
                        else
                            goto label_4;
                    case 1:
                        if (this._codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this._codec.AvailableBytesIn;
                            ++this._codec.TotalBytesIn;
                            int num3 = (int)this._codec.InputBuffer[this._codec.NextIn++] & (int)byte.MaxValue;
                            if (((this.method << 8) + num3) % 31 != 0)
                            {
                                this.mode = 13;
                                this._codec.Message = "incorrect header check";
                                this.marker = 5;
                                break;
                            }
                            else if ((num3 & 32) == 0)
                            {
                                this.mode = 7;
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
                        r = this.blocks.Process(r);
                        if (r == -3)
                        {
                            this.mode = 13;
                            this.marker = 0;
                            break;
                        }
                        else
                        {
                            if (r == 0)
                                r = num2;
                            if (r == 1)
                            {
                                r = num2;
                                this.blocks.Reset(this.was);
                                if (!this.HandleRfc1950HeaderBytes)
                                {
                                    this.mode = 12;
                                    break;
                                }
                                else
                                {
                                    this.mode = 8;
                                    goto case 8;
                                }
                            }
                            else
                                goto label_35;
                        }
                    case 8:
                        if (this._codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this._codec.AvailableBytesIn;
                            ++this._codec.TotalBytesIn;
                            this.need = (long)(((int)this._codec.InputBuffer[this._codec.NextIn++] & (int)byte.MaxValue) << 24 & -16777216);
                            this.mode = 9;
                            goto case 9;
                        }
                        else
                            goto label_40;
                    case 9:
                        if (this._codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this._codec.AvailableBytesIn;
                            ++this._codec.TotalBytesIn;
                            this.need += (long)(((int)this._codec.InputBuffer[this._codec.NextIn++] & (int)byte.MaxValue) << 16) & 16711680L;
                            this.mode = 10;
                            goto case 10;
                        }
                        else
                            goto label_43;
                    case 10:
                        if (this._codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this._codec.AvailableBytesIn;
                            ++this._codec.TotalBytesIn;
                            this.need += (long)(((int)this._codec.InputBuffer[this._codec.NextIn++] & (int)byte.MaxValue) << 8) & 65280L;
                            this.mode = 11;
                            goto case 11;
                        }
                        else
                            goto label_46;
                    case 11:
                        if (this._codec.AvailableBytesIn != 0)
                        {
                            r = num2;
                            --this._codec.AvailableBytesIn;
                            ++this._codec.TotalBytesIn;
                            this.need += (long)this._codec.InputBuffer[this._codec.NextIn++] & (long)byte.MaxValue;
                            if ((int)this.was[0] != (int)this.need)
                            {
                                this.mode = 13;
                                this._codec.Message = "incorrect data check";
                                this.marker = 5;
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
            this.mode = 2;
            label_17:
            if (this._codec.AvailableBytesIn == 0)
                return r;
            r = num2;
            --this._codec.AvailableBytesIn;
            ++this._codec.TotalBytesIn;
            this.need = (long)(((int)this._codec.InputBuffer[this._codec.NextIn++] & (int)byte.MaxValue) << 24 & -16777216);
            this.mode = 3;
            label_20:
            if (this._codec.AvailableBytesIn == 0)
                return r;
            r = num2;
            --this._codec.AvailableBytesIn;
            ++this._codec.TotalBytesIn;
            this.need += (long)(((int)this._codec.InputBuffer[this._codec.NextIn++] & (int)byte.MaxValue) << 16) & 16711680L;
            this.mode = 4;
            label_23:
            if (this._codec.AvailableBytesIn == 0)
                return r;
            r = num2;
            --this._codec.AvailableBytesIn;
            ++this._codec.TotalBytesIn;
            this.need += (long)(((int)this._codec.InputBuffer[this._codec.NextIn++] & (int)byte.MaxValue) << 8) & 65280L;
            this.mode = 5;
            label_26:
            if (this._codec.AvailableBytesIn == 0)
                return r;
            --this._codec.AvailableBytesIn;
            ++this._codec.TotalBytesIn;
            this.need += (long)this._codec.InputBuffer[this._codec.NextIn++] & (long)byte.MaxValue;
            this._codec._Adler32 = this.need;
            this.mode = 6;
            return 2;
            label_29:
            this.mode = 13;
            this._codec.Message = "need dictionary";
            this.marker = 0;
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
            this.mode = 12;
            label_53:
            return 1;
            label_54:
            throw new ZlibException(string.Format("Bad state ({0})", (object)this._codec.Message));
            label_55:
            throw new ZlibException("Stream error.");
        }

        internal int SetDictionary(byte[] dictionary)
        {
            int start = 0;
            int n = dictionary.Length;
            if (this.mode != 6)
                throw new ZlibException("Stream error.");
            if (Adler.Adler32(1L, dictionary, 0, dictionary.Length) != this._codec._Adler32)
                return -3;
            this._codec._Adler32 = Adler.Adler32(0L, (byte[])null, 0, 0);
            if (n >= 1 << this.wbits)
            {
                n = (1 << this.wbits) - 1;
                start = dictionary.Length - n;
            }
            this.blocks.SetDictionary(dictionary, start, n);
            this.mode = 7;
            return 0;
        }

        internal int Sync()
        {
            if (this.mode != 13)
            {
                this.mode = 13;
                this.marker = 0;
            }
            int num1;
            if ((num1 = this._codec.AvailableBytesIn) == 0)
                return -5;
            int index1 = this._codec.NextIn;
            int index2;
            for (index2 = this.marker; num1 != 0 && index2 < 4; --num1)
            {
                if ((int)this._codec.InputBuffer[index1] == (int)InflateManager.mark[index2])
                    ++index2;
                else
                    index2 = (int)this._codec.InputBuffer[index1] == 0 ? 4 - index2 : 0;
                ++index1;
            }
            this._codec.TotalBytesIn += (long)(index1 - this._codec.NextIn);
            this._codec.NextIn = index1;
            this._codec.AvailableBytesIn = num1;
            this.marker = index2;
            if (index2 != 4)
                return -3;
            long num2 = this._codec.TotalBytesIn;
            long num3 = this._codec.TotalBytesOut;
            this.Reset();
            this._codec.TotalBytesIn = num2;
            this._codec.TotalBytesOut = num3;
            this.mode = 7;
            return 0;
        }

        internal int SyncPoint(ZlibCodec z)
        {
            return this.blocks.SyncPoint();
        }
    }
}