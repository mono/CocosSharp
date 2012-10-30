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
        private static readonly int[] inflate_mask = new int[17]
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
        internal int[] bb = new int[1];
        internal int[] tb = new int[1];
        internal InflateCodes codes = new InflateCodes();
        internal InfTree inftree = new InfTree();
        private const int MANY = 1440;
        private const int TYPE = 0;
        private const int LENS = 1;
        private const int STORED = 2;
        private const int TABLE = 3;
        private const int BTREE = 4;
        private const int DTREE = 5;
        private const int CODES = 6;
        private const int DRY = 7;
        private const int DONE = 8;
        private const int BAD = 9;
        internal int mode;
        internal int left;
        internal int table;
        internal int index;
        internal int[] blens;
        internal int last;
        internal ZlibCodec _codec;
        internal int bitk;
        internal int bitb;
        internal int[] hufts;
        internal byte[] window;
        internal int end;
        internal int read;
        internal int write;
        internal object checkfn;
        internal long check;

        static InflateBlocks()
        {
        }

        internal InflateBlocks(ZlibCodec codec, object checkfn, int w)
        {
            this._codec = codec;
            this.hufts = new int[4320];
            this.window = new byte[w];
            this.end = w;
            this.checkfn = checkfn;
            this.mode = 0;
            this.Reset((long[])null);
        }

        internal void Reset(long[] c)
        {
            if (c != null)
                c[0] = this.check;
            if (this.mode != 4 && this.mode != 5)
            {}
            if (this.mode != 6)
            {}
            this.mode = 0;
            this.bitk = 0;
            this.bitb = 0;
            this.read = this.write = 0;
            if (this.checkfn == null)
                return;
            this._codec._Adler32 = this.check = Adler.Adler32(0L, (byte[])null, 0, 0);
        }

        internal int Process(int r)
        {
            int sourceIndex = this._codec.NextIn;
            int num1 = this._codec.AvailableBytesIn;
            int number1 = this.bitb;
            int num2 = this.bitk;
            int destinationIndex = this.write;
            int num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
            int num4;
            int num5;
            while (true)
            {
                switch (this.mode)
                {
                    case 0:
                        while (num2 < 3)
                        {
                            if (num1 != 0)
                            {
                                r = 0;
                                --num1;
                                number1 |= ((int)this._codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                num2 += 8;
                            }
                            else
                            {
                                this.bitb = number1;
                                this.bitk = num2;
                                this._codec.AvailableBytesIn = num1;
                                this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
                                this._codec.NextIn = sourceIndex;
                                this.write = destinationIndex;
                                return this.Flush(r);
                            }
                        }
                        int number2 = number1 & 7;
                        this.last = number2 & 1;
                        switch (SharedUtils.URShift(number2, 1))
                        {
                            case 0:
                                int number3 = SharedUtils.URShift(number1, 3);
                                int num6 = num2 - 3;
                                int bits1 = num6 & 7;
                                number1 = SharedUtils.URShift(number3, bits1);
                                num2 = num6 - bits1;
                                this.mode = 1;
                                break;
                            case 1:
                                int[] bl1 = new int[1];
                                int[] bd1 = new int[1];
                                int[][] tl1 = new int[1][];
                                int[][] td1 = new int[1][];
                                InfTree.inflate_trees_fixed(bl1, bd1, tl1, td1, this._codec);
                                this.codes.Init(bl1[0], bd1[0], tl1[0], 0, td1[0], 0);
                                number1 = SharedUtils.URShift(number1, 3);
                                num2 -= 3;
                                this.mode = 6;
                                break;
                            case 2:
                                number1 = SharedUtils.URShift(number1, 3);
                                num2 -= 3;
                                this.mode = 3;
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
                                number1 |= ((int)this._codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                num2 += 8;
                            }
                            else
                            {
                                this.bitb = number1;
                                this.bitk = num2;
                                this._codec.AvailableBytesIn = num1;
                                this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
                                this._codec.NextIn = sourceIndex;
                                this.write = destinationIndex;
                                return this.Flush(r);
                            }
                        }
                        if ((SharedUtils.URShift(~number1, 16) & (int)ushort.MaxValue) == (number1 & (int)ushort.MaxValue))
                        {
                            this.left = number1 & (int)ushort.MaxValue;
                            number1 = num2 = 0;
                            this.mode = this.left != 0 ? 2 : (this.last != 0 ? 7 : 0);
                            break;
                        }
                        else
                            goto label_15;
                    case 2:
                        if (num1 != 0)
                        {
                            if (num3 == 0)
                            {
                                if (destinationIndex == this.end && this.read != 0)
                                {
                                    destinationIndex = 0;
                                    num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
                                }
                                if (num3 == 0)
                                {
                                    this.write = destinationIndex;
                                    r = this.Flush(r);
                                    destinationIndex = this.write;
                                    num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
                                    if (destinationIndex == this.end && this.read != 0)
                                    {
                                        destinationIndex = 0;
                                        num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
                                    }
                                    if (num3 == 0)
                                        goto label_26;
                                }
                            }
                            r = 0;
                            int length = this.left;
                            if (length > num1)
                                length = num1;
                            if (length > num3)
                                length = num3;
                            Array.Copy((Array)this._codec.InputBuffer, sourceIndex, (Array)this.window, destinationIndex, length);
                            sourceIndex += length;
                            num1 -= length;
                            destinationIndex += length;
                            num3 -= length;
                            if ((this.left -= length) == 0)
                            {
                                this.mode = this.last != 0 ? 7 : 0;
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
                                number1 |= ((int)this._codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                num2 += 8;
                            }
                            else
                            {
                                this.bitb = number1;
                                this.bitk = num2;
                                this._codec.AvailableBytesIn = num1;
                                this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
                                this._codec.NextIn = sourceIndex;
                                this.write = destinationIndex;
                                return this.Flush(r);
                            }
                        }
                        int num7;
                        this.table = num7 = number1 & 16383;
                        if ((num7 & 31) <= 29 && (num7 >> 5 & 31) <= 29)
                        {
                            int length = 258 + (num7 & 31) + (num7 >> 5 & 31);
                            if (this.blens == null || this.blens.Length < length)
                            {
                                this.blens = new int[length];
                            }
                            else
                            {
                                for (int index = 0; index < length; ++index)
                                    this.blens[index] = 0;
                            }
                            number1 = SharedUtils.URShift(number1, 14);
                            num2 -= 14;
                            this.index = 0;
                            this.mode = 4;
                            goto case 4;
                        }
                        else
                            goto label_39;
                    case 4:
                        while (this.index < 4 + SharedUtils.URShift(this.table, 10))
                        {
                            while (num2 < 3)
                            {
                                if (num1 != 0)
                                {
                                    r = 0;
                                    --num1;
                                    number1 |= ((int)this._codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                    num2 += 8;
                                }
                                else
                                {
                                    this.bitb = number1;
                                    this.bitk = num2;
                                    this._codec.AvailableBytesIn = num1;
                                    this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
                                    this._codec.NextIn = sourceIndex;
                                    this.write = destinationIndex;
                                    return this.Flush(r);
                                }
                            }
                            this.blens[InflateBlocks.border[this.index++]] = number1 & 7;
                            number1 = SharedUtils.URShift(number1, 3);
                            num2 -= 3;
                        }
                        while (this.index < 19)
                            this.blens[InflateBlocks.border[this.index++]] = 0;
                        this.bb[0] = 7;
                        num4 = this.inftree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, this._codec);
                        if (num4 == 0)
                        {
                            this.index = 0;
                            this.mode = 5;
                            goto case 5;
                        }
                        else
                            goto label_55;
                    case 5:
                        while (true)
                        {
                            int num8 = this.table;
                            if (this.index < 258 + (num8 & 31) + (num8 >> 5 & 31))
                            {
                                int index = this.bb[0];
                                while (num2 < index)
                                {
                                    if (num1 != 0)
                                    {
                                        r = 0;
                                        --num1;
                                        number1 |= ((int)this._codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                        num2 += 8;
                                    }
                                    else
                                    {
                                        this.bitb = number1;
                                        this.bitk = num2;
                                        this._codec.AvailableBytesIn = num1;
                                        this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
                                        this._codec.NextIn = sourceIndex;
                                        this.write = destinationIndex;
                                        return this.Flush(r);
                                    }
                                }
                                if (this.tb[0] != -1)
                                {}
                                int bits2 = this.hufts[(this.tb[0] + (number1 & InflateBlocks.inflate_mask[index])) * 3 + 1];
                                int num9 = this.hufts[(this.tb[0] + (number1 & InflateBlocks.inflate_mask[bits2])) * 3 + 2];
                                if (num9 < 16)
                                {
                                    number1 = SharedUtils.URShift(number1, bits2);
                                    num2 -= bits2;
                                    this.blens[this.index++] = num9;
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
                                            number1 |= ((int)this._codec.InputBuffer[sourceIndex++] & (int)byte.MaxValue) << num2;
                                            num2 += 8;
                                        }
                                        else
                                        {
                                            this.bitb = number1;
                                            this.bitk = num2;
                                            this._codec.AvailableBytesIn = num1;
                                            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
                                            this._codec.NextIn = sourceIndex;
                                            this.write = destinationIndex;
                                            return this.Flush(r);
                                        }
                                    }
                                    int number4 = SharedUtils.URShift(number1, bits2);
                                    int num11 = num2 - bits2;
                                    int num12 = num10 + (number4 & InflateBlocks.inflate_mask[bits3]);
                                    number1 = SharedUtils.URShift(number4, bits3);
                                    num2 = num11 - bits3;
                                    int num13 = this.index;
                                    int num14 = this.table;
                                    if (num13 + num12 <= 258 + (num14 & 31) + (num14 >> 5 & 31) && (num9 != 16 || num13 >= 1))
                                    {
                                        int num15 = num9 == 16 ? this.blens[num13 - 1] : 0;
                                        do
                                        {
                                            this.blens[num13++] = num15;
                                        }
                                        while (--num12 != 0);
                                        this.index = num13;
                                    }
                                    else
                                        goto label_73;
                                }
                            }
                            else
                                break;
                        }
                        this.tb[0] = -1;
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
                        int num16 = this.table;
                        num5 = this.inftree.inflate_trees_dynamic(257 + (num16 & 31), 1 + (num16 >> 5 & 31), this.blens, bl2, bd2, tl2, td2, this.hufts, this._codec);
                        switch (num5)
                        {
                            case 0:
                                this.codes.Init(bl2[0], bd2[0], this.hufts, tl2[0], this.hufts, td2[0]);
                                this.mode = 6;
                                goto label_83;
                            case -3:
                                goto label_80;
                            default:
                                goto label_81;
                        }
                    case 6:
                        label_83:
                        this.bitb = number1;
                        this.bitk = num2;
                        this._codec.AvailableBytesIn = num1;
                        this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
                        this._codec.NextIn = sourceIndex;
                        this.write = destinationIndex;
                        if ((r = this.codes.Process(this, r)) == 1)
                        {
                            r = 0;
                            sourceIndex = this._codec.NextIn;
                            num1 = this._codec.AvailableBytesIn;
                            number1 = this.bitb;
                            num2 = this.bitk;
                            destinationIndex = this.write;
                            num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
                            if (this.last == 0)
                            {
                                this.mode = 0;
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
            this.mode = 9;
            this._codec.Message = "invalid block type";
            r = -3;
            this.bitb = num17;
            this.bitk = num18;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_15:
            this.mode = 9;
            this._codec.Message = "invalid stored block lengths";
            r = -3;
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_18:
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_26:
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_39:
            this.mode = 9;
            this._codec.Message = "too many length or distance symbols";
            r = -3;
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_55:
            r = num4;
            if (r == -3)
            {
                this.blens = (int[])null;
                this.mode = 9;
            }
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_73:
            this.blens = (int[])null;
            this.mode = 9;
            this._codec.Message = "invalid bit length repeat";
            r = -3;
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_80:
            this.blens = (int[])null;
            this.mode = 9;
            label_81:
            r = num5;
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_84:
            return this.Flush(r);
            label_87:
            this.mode = 7;
            label_88:
            this.write = destinationIndex;
            r = this.Flush(r);
            destinationIndex = this.write;
            int num19 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
            if (this.read != this.write)
            {
                this.bitb = number1;
                this.bitk = num2;
                this._codec.AvailableBytesIn = num1;
                this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
                this._codec.NextIn = sourceIndex;
                this.write = destinationIndex;
                return this.Flush(r);
            }
            else
                this.mode = 8;
            label_91:
            r = 1;
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_92:
            r = -3;
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
            label_93:
            r = -2;
            this.bitb = number1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = num1;
            this._codec.TotalBytesIn += (long)(sourceIndex - this._codec.NextIn);
            this._codec.NextIn = sourceIndex;
            this.write = destinationIndex;
            return this.Flush(r);
        }

        internal void Free()
        {
            this.Reset((long[])null);
            this.window = (byte[])null;
            this.hufts = (int[])null;
        }

        internal void SetDictionary(byte[] d, int start, int n)
        {
            Array.Copy((Array)d, start, (Array)this.window, 0, n);
            this.read = this.write = n;
        }

        internal int SyncPoint()
        {
            return this.mode == 1 ? 1 : 0;
        }

        internal int Flush(int r)
        {
            int destinationIndex1 = this._codec.NextOut;
            int num1 = this.read;
            int num2 = (num1 <= this.write ? this.write : this.end) - num1;
            if (num2 > this._codec.AvailableBytesOut)
                num2 = this._codec.AvailableBytesOut;
            if (num2 != 0 && r == -5)
                r = 0;
            this._codec.AvailableBytesOut -= num2;
            this._codec.TotalBytesOut += (long)num2;
            if (this.checkfn != null)
                this._codec._Adler32 = this.check = Adler.Adler32(this.check, this.window, num1, num2);
            Array.Copy((Array)this.window, num1, (Array)this._codec.OutputBuffer, destinationIndex1, num2);
            int destinationIndex2 = destinationIndex1 + num2;
            int num3 = num1 + num2;
            if (num3 == this.end)
            {
                int num4 = 0;
                if (this.write == this.end)
                    this.write = 0;
                int num5 = this.write - num4;
                if (num5 > this._codec.AvailableBytesOut)
                    num5 = this._codec.AvailableBytesOut;
                if (num5 != 0 && r == -5)
                    r = 0;
                this._codec.AvailableBytesOut -= num5;
                this._codec.TotalBytesOut += (long)num5;
                if (this.checkfn != null)
                    this._codec._Adler32 = this.check = Adler.Adler32(this.check, this.window, num4, num5);
                Array.Copy((Array)this.window, num4, (Array)this._codec.OutputBuffer, destinationIndex2, num5);
                destinationIndex2 += num5;
                num3 = num4 + num5;
            }
            this._codec.NextOut = destinationIndex2;
            this.read = num3;
            return r;
        }
    }
}