// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InflateBlocks.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
// http://www.sharpgis.net/post/2011/08/28/GZIP-Compressed-Web-Requests-in-WP7-Take-2.aspx
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WP7Contrib.Communications.Compression
{
    using System;

    internal sealed class InflateBlocks
    {
		static readonly int[] inflate_mask = new int[17]
                                                         {
                                                             0,
                                                             1,
                                                             3,
                                                             7,
                                                             15,
                                                             31,
                                                             63,
                                                             (int) sbyte.MaxValue,
                                                             (int) byte.MaxValue,
                                                             511,
                                                             1023,
                                                             2047,
                                                             4095,
                                                             8191,
                                                             16383,
                                                             (int) short.MaxValue,
                                                             (int) ushort.MaxValue
                                                         };
        internal static readonly int[] border = new int[19]
                                                    {
                                                        16,
                                                        17,
                                                        18,
                                                        0,
                                                        8,
                                                        7,
                                                        9,
                                                        6,
                                                        10,
                                                        5,
                                                        11,
                                                        4,
                                                        12,
                                                        3,
                                                        13,
                                                        2,
                                                        14,
                                                        1,
                                                        15
                                                    };

        const int MANY = 1440;
        const int TYPE = 0;
        const int LENS = 1;
        const int STORED = 2;
        const int TABLE = 3;
        const int BTREE = 4;
        const int DTREE = 5;
        const int CODES = 6;
        const int DRY = 7;
        const int DONE = 8;
        const int BAD = 9;

		internal int[] Bb = new int[1];
		internal int[] Tb = new int[1];
		internal InflateCodes Codes = new InflateCodes();
		internal InfTree Inftree = new InfTree();
		internal int Mode;
		internal int Left;
		internal int Table;
		internal int Index;
		internal int[] Blens;
		internal int Last;
		internal ZlibCodec Codec;
		internal int Bitk;
		internal int Bitb;
		internal int[] Hufts;
		internal byte[] Window;
		internal int End;
		internal int Read;
		internal int Write;
		internal object Checkfn;
		internal long Check;


		#region Constructors

        static InflateBlocks()
        {
        }

        internal InflateBlocks(ZlibCodec codec, object Checkfn, int w)
        {
            this.Codec = codec;
            this.Hufts = new int[4320];
			this.Window = new byte[w];
            this.End = w;
            this.Checkfn = Checkfn;
            this.Mode = 0;
            this.Reset((long[])null);
        }

		#endregion Constructors


        internal void Reset(long[] c)
        {
            if (c != null)
                c[0] = this.Check;
            if (this.Mode != 4 && this.Mode != 5)
            {}
            if (this.Mode != 6)
            {}
            this.Mode = 0;
            this.Bitk = 0;
            this.Bitb = 0;
            this.Read = this.Write = 0;
            if (this.Checkfn == null)
                return;
            this.Codec.Adler32 = this.Check = Adler.Adler32(0L, (byte[])null, 0, 0);
        }

        internal int Process(int r)
        {
            int sourceIndex = this.Codec.NextIn;
            int num1 = this.Codec.AvailableBytesIn;
            int number1 = this.Bitb;
            int num2 = this.Bitk;
            int destinationIndex = this.Write;
            int num3 = destinationIndex < this.Read ? this.Read - destinationIndex - 1 : this.End - destinationIndex;
            int num4;
            int num5;
            while (true)
            {
                switch (this.Mode)
                {
                    case 0:
                        while (num2 < 3)
                        {
                            if (num1 != 0)
                            {
                                r = 0;
                                --num1;
                                number1 |= ((int)this.Codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                num2 += 8;
                            }
                            else
                            {
                                this.Bitb = number1;
                                this.Bitk = num2;
                                this.Codec.AvailableBytesIn = num1;
                                this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
                                this.Codec.NextIn = sourceIndex;
                                this.Write = destinationIndex;
                                return this.Flush(r);
                            }
                        }
                        int number2 = number1 & 7;
                        this.Last = number2 & 1;
                        switch (SharedUtils.URShift(number2, 1))
                        {
                            case 0:
                                int number3 = SharedUtils.URShift(number1, 3);
                                int num6 = num2 - 3;
                                int bits1 = num6 & 7;
                                number1 = SharedUtils.URShift(number3, bits1);
                                num2 = num6 - bits1;
                                this.Mode = 1;
                                break;
                            case 1:
                                int[] bl1 = new int[1];
                                int[] bd1 = new int[1];
                                int[][] tl1 = new int[1][];
                                int[][] td1 = new int[1][];
                                InfTree.inflate_trees_fixed(bl1, bd1, tl1, td1, this.Codec);
                                this.Codes.Init(bl1[0], bd1[0], tl1[0], 0, td1[0], 0);
                                number1 = SharedUtils.URShift(number1, 3);
                                num2 -= 3;
                                this.Mode = 6;
                                break;
                            case 2:
                                number1 = SharedUtils.URShift(number1, 3);
                                num2 -= 3;
                                this.Mode = 3;
                                break;
                            case 3:
                                goto label_9;
                        }
                        break;
                    case 1:
                        while (num2 < 32)
                        {
                            if (num1 != 0)
                            {
                                r = 0;
                                --num1;
                                number1 |= ((int)this.Codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                num2 += 8;
                            }
                            else
                            {
                                this.Bitb = number1;
                                this.Bitk = num2;
                                this.Codec.AvailableBytesIn = num1;
                                this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
                                this.Codec.NextIn = sourceIndex;
                                this.Write = destinationIndex;
                                return this.Flush(r);
                            }
                        }
                        if ((SharedUtils.URShift(~number1, 16) & (int)ushort.MaxValue) == (number1 & (int)ushort.MaxValue))
                        {
                            this.Left = number1 & (int)ushort.MaxValue;
                            number1 = num2 = 0;
                            this.Mode = this.Left != 0 ? 2 : (this.Last != 0 ? 7 : 0);
                            break;
                        }
                        else
                            goto label_15;
                    case 2:
                        if (num1 != 0)
                        {
                            if (num3 == 0)
                            {
                                if (destinationIndex == this.End && this.Read != 0)
                                {
                                    destinationIndex = 0;
                                    num3 = destinationIndex < this.Read ? this.Read - destinationIndex - 1 : this.End - destinationIndex;
                                }
                                if (num3 == 0)
                                {
                                    this.Write = destinationIndex;
                                    r = this.Flush(r);
                                    destinationIndex = this.Write;
                                    num3 = destinationIndex < this.Read ? this.Read - destinationIndex - 1 : this.End - destinationIndex;
                                    if (destinationIndex == this.End && this.Read != 0)
                                    {
                                        destinationIndex = 0;
                                        num3 = destinationIndex < this.Read ? this.Read - destinationIndex - 1 : this.End - destinationIndex;
                                    }
                                    if (num3 == 0)
                                        goto label_26;
                                }
                            }
                            r = 0;
                            int length = this.Left;
                            if (length > num1)
                                length = num1;
                            if (length > num3)
                                length = num3;
                            Array.Copy((Array)this.Codec.InputBuffer, sourceIndex, (Array)this.Window, destinationIndex, length);
                            sourceIndex += length;
                            num1 -= length;
                            destinationIndex += length;
                            num3 -= length;
                            if ((this.Left -= length) == 0)
                            {
                                this.Mode = this.Last != 0 ? 7 : 0;
                                break;
                            }
                            else
                                break;
                        }
                        else
                            goto label_18;
                    case 3:
                        while (num2 < 14)
                        {
                            if (num1 != 0)
                            {
                                r = 0;
                                --num1;
                                number1 |= ((int)this.Codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                num2 += 8;
                            }
                            else
                            {
                                this.Bitb = number1;
                                this.Bitk = num2;
                                this.Codec.AvailableBytesIn = num1;
                                this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
                                this.Codec.NextIn = sourceIndex;
                                this.Write = destinationIndex;
                                return this.Flush(r);
                            }
                        }
                        int num7;
                        this.Table = num7 = number1 & 16383;
                        if ((num7 & 31) <= 29 && (num7 >> 5 & 31) <= 29)
                        {
                            int length = 258 + (num7 & 31) + (num7 >> 5 & 31);
                            if (this.Blens == null || this.Blens.Length < length)
                            {
                                this.Blens = new int[length];
                            }
                            else
                            {
                                for (int Index = 0; Index < length; ++Index)
                                    this.Blens[Index] = 0;
                            }
                            number1 = SharedUtils.URShift(number1, 14);
                            num2 -= 14;
                            this.Index = 0;
                            this.Mode = 4;
                            goto case 4;
                        }
                        else
                            goto label_39;
                    case 4:
                        while (this.Index < 4 + SharedUtils.URShift(this.Table, 10))
                        {
                            while (num2 < 3)
                            {
                                if (num1 != 0)
                                {
                                    r = 0;
                                    --num1;
                                    number1 |= ((int)this.Codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                    num2 += 8;
                                }
                                else
                                {
                                    this.Bitb = number1;
                                    this.Bitk = num2;
                                    this.Codec.AvailableBytesIn = num1;
                                    this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
                                    this.Codec.NextIn = sourceIndex;
                                    this.Write = destinationIndex;
                                    return this.Flush(r);
                                }
                            }
                            this.Blens[InflateBlocks.border[this.Index++]] = number1 & 7;
                            number1 = SharedUtils.URShift(number1, 3);
                            num2 -= 3;
                        }
                        while (this.Index < 19)
                            this.Blens[InflateBlocks.border[this.Index++]] = 0;
                        this.Bb[0] = 7;
                        num4 = this.Inftree.inflate_trees_bits(this.Blens, this.Bb, this.Tb, this.Hufts, this.Codec);
                        if (num4 == 0)
                        {
                            this.Index = 0;
                            this.Mode = 5;
                            goto case 5;
                        }
                        else
                            goto label_55;
                    case 5:
                        while (true)
                        {
                            int num8 = this.Table;
                            if (this.Index < 258 + (num8 & 31) + (num8 >> 5 & 31))
                            {
                                int Index = this.Bb[0];
                                while (num2 < Index)
                                {
                                    if (num1 != 0)
                                    {
                                        r = 0;
                                        --num1;
                                        number1 |= ((int)this.Codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                        num2 += 8;
                                    }
                                    else
                                    {
                                        this.Bitb = number1;
                                        this.Bitk = num2;
                                        this.Codec.AvailableBytesIn = num1;
                                        this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
                                        this.Codec.NextIn = sourceIndex;
                                        this.Write = destinationIndex;
                                        return this.Flush(r);
                                    }
                                }
                                if (this.Tb[0] != -1)
                                {}
                                int bits2 = this.Hufts[(this.Tb[0] + (number1 & InflateBlocks.inflate_mask[Index])) * 3 + 1];
                                int num9 = this.Hufts[(this.Tb[0] + (number1 & InflateBlocks.inflate_mask[bits2])) * 3 + 2];
                                if (num9 < 16)
                                {
                                    number1 = SharedUtils.URShift(number1, bits2);
                                    num2 -= bits2;
                                    this.Blens[this.Index++] = num9;
                                }
                                else
                                {
                                    int bits3 = num9 == 18 ? 7 : num9 - 14;
                                    int num10 = num9 == 18 ? 11 : 3;
                                    while (num2 < bits2 + bits3)
                                    {
                                        if (num1 != 0)
                                        {
                                            r = 0;
                                            --num1;
                                            number1 |= ((int)this.Codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                            num2 += 8;
                                        }
                                        else
                                        {
                                            this.Bitb = number1;
                                            this.Bitk = num2;
                                            this.Codec.AvailableBytesIn = num1;
                                            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
                                            this.Codec.NextIn = sourceIndex;
                                            this.Write = destinationIndex;
                                            return this.Flush(r);
                                        }
                                    }
                                    int number4 = SharedUtils.URShift(number1, bits2);
                                    int num11 = num2 - bits2;
                                    int num12 = num10 + (number4 & InflateBlocks.inflate_mask[bits3]);
                                    number1 = SharedUtils.URShift(number4, bits3);
                                    num2 = num11 - bits3;
                                    int num13 = this.Index;
                                    int num14 = this.Table;
                                    if (num13 + num12 <= 258 + (num14 & 31) + (num14 >> 5 & 31) && (num9 != 16 || num13 >= 1))
                                    {
                                        int num15 = num9 == 16 ? this.Blens[num13 - 1] : 0;
                                        do
                                        {
                                            this.Blens[num13++] = num15;
                                        }
                                        while (--num12 != 0);
                                        this.Index = num13;
                                    }
                                    else
                                        goto label_73;
                                }
                            }
                            else
                                break;
                        }
                        this.Tb[0] = -1;
                        int[] bl2 = new int[1]
                                        {
                                            9
                                        };
                        int[] bd2 = new int[1]
                                        {
                                            6
                                        };
                        int[] tl2 = new int[1];
                        int[] td2 = new int[1];
                        int num16 = this.Table;
                        num5 = this.Inftree.inflate_trees_dynamic(257 + (num16 & 31), 1 + (num16 >> 5 & 31), this.Blens, bl2, bd2, tl2, td2, this.Hufts, this.Codec);
                        switch (num5)
                        {
                            case 0:
                                this.Codes.Init(bl2[0], bd2[0], this.Hufts, tl2[0], this.Hufts, td2[0]);
                                this.Mode = 6;
                                goto label_83;
                            case -3:
                                goto label_80;
                            default:
                                goto label_81;
                        }
                    case 6:
                        label_83:
                        this.Bitb = number1;
                        this.Bitk = num2;
                        this.Codec.AvailableBytesIn = num1;
                        this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
                        this.Codec.NextIn = sourceIndex;
                        this.Write = destinationIndex;
                        if ((r = this.Codes.Process(this, r)) == 1)
                        {
                            r = 0;
                            sourceIndex = this.Codec.NextIn;
                            num1 = this.Codec.AvailableBytesIn;
                            number1 = this.Bitb;
                            num2 = this.Bitk;
                            destinationIndex = this.Write;
                            num3 = destinationIndex < this.Read ? this.Read - destinationIndex - 1 : this.End - destinationIndex;
                            if (this.Last == 0)
                            {
                                this.Mode = 0;
                                break;
                            }
                            else
                                goto label_87;
                        }
                        else
                            goto label_84;
                    case 7:
                        goto label_88;
                    case 8:
                        goto label_91;
                    case 9:
                        goto label_92;
                    default:
                        goto label_93;
                }
            }
            label_9:
            int num17 = SharedUtils.URShift(number1, 3);
            int num18 = num2 - 3;
            this.Mode = 9;
            this.Codec.Message = "invalid block type";
            r = -3;
            this.Bitb = num17;
            this.Bitk = num18;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_15:
            this.Mode = 9;
            this.Codec.Message = "invalid stored block lengths";
            r = -3;
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_18:
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_26:
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_39:
            this.Mode = 9;
            this.Codec.Message = "too many length or distance symbols";
            r = -3;
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_55:
            r = num4;
            if (r == -3)
            {
                this.Blens = (int[])null;
                this.Mode = 9;
            }
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_73:
            this.Blens = (int[])null;
            this.Mode = 9;
            this.Codec.Message = "invalid bit length repeat";
            r = -3;
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_80:
            this.Blens = (int[])null;
            this.Mode = 9;
            label_81:
            r = num5;
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_84:
            return this.Flush(r);
            label_87:
            this.Mode = 7;
            label_88:
            this.Write = destinationIndex;
            r = this.Flush(r);
            destinationIndex = this.Write;
            int num19 = destinationIndex < this.Read ? this.Read - destinationIndex - 1 : this.End - destinationIndex;
            if (this.Read != this.Write)
            {
                this.Bitb = number1;
                this.Bitk = num2;
                this.Codec.AvailableBytesIn = num1;
                this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
                this.Codec.NextIn = sourceIndex;
                this.Write = destinationIndex;
                return this.Flush(r);
            }
            else
                this.Mode = 8;
            label_91:
            r = 1;
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_92:
            r = -3;
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
            label_93:
            r = -2;
            this.Bitb = number1;
            this.Bitk = num2;
            this.Codec.AvailableBytesIn = num1;
            this.Codec.TotalBytesIn += (long)(sourceIndex - this.Codec.NextIn);
            this.Codec.NextIn = sourceIndex;
            this.Write = destinationIndex;
            return this.Flush(r);
        }

        internal void Free()
        {
            this.Reset((long[])null);
            this.Window = (byte[])null;
            this.Hufts = (int[])null;
        }

        internal void SetDictionary(byte[] d, int start, int n)
        {
            Array.Copy((Array)d, start, (Array)this.Window, 0, n);
            this.Read = this.Write = n;
        }

        internal int SyncPoint()
        {
            return this.Mode == 1 ? 1 : 0;
        }

        internal int Flush(int r)
        {
            int destinationIndex1 = this.Codec.NextOut;
            int num1 = this.Read;
            int num2 = (num1 <= this.Write ? this.Write : this.End) - num1;
            if (num2 > this.Codec.AvailableBytesOut)
                num2 = this.Codec.AvailableBytesOut;
            if (num2 != 0 && r == -5)
                r = 0;
            this.Codec.AvailableBytesOut -= num2;
            this.Codec.TotalBytesOut += (long)num2;
            if (this.Checkfn != null)
                this.Codec.Adler32 = this.Check = Adler.Adler32(this.Check, this.Window, num1, num2);
            Array.Copy((Array)this.Window, num1, (Array)this.Codec.OutputBuffer, destinationIndex1, num2);
            int destinationIndex2 = destinationIndex1 + num2;
            int num3 = num1 + num2;
            if (num3 == this.End)
            {
                int num4 = 0;
                if (this.Write == this.End)
                    this.Write = 0;
                int num5 = this.Write - num4;
                if (num5 > this.Codec.AvailableBytesOut)
                    num5 = this.Codec.AvailableBytesOut;
                if (num5 != 0 && r == -5)
                    r = 0;
                this.Codec.AvailableBytesOut -= num5;
                this.Codec.TotalBytesOut += (long)num5;
                if (this.Checkfn != null)
                    this.Codec.Adler32 = this.Check = Adler.Adler32(this.Check, this.Window, num4, num5);
                Array.Copy((Array)this.Window, num4, (Array)this.Codec.OutputBuffer, destinationIndex2, num5);
                destinationIndex2 += num5;
                num3 = num4 + num5;
            }
            this.Codec.NextOut = destinationIndex2;
            this.Read = num3;
            return r;
        }
    }
}