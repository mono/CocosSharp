// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Crc32.cs" company="XamlNinja">
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

    internal class Crc32
    {
        private uint _RunningCrc32Result = uint.MaxValue;
        private const int BUFFER_SIZE = 8192;
        private long _TotalBytesRead;
        private static uint[] crc32Table;

        public long TotalBytesRead
        {
            get
            {
                return this._TotalBytesRead;
            }
        }

        public int Crc32Result
        {
            get
            {
                return ~(int)this._RunningCrc32Result;
            }
        }

        static Crc32()
        {
            uint num1 = 3988292384U;
            Crc32.crc32Table = new uint[256];
            for (uint index1 = 0U; index1 < 256U; ++index1)
            {
                uint num2 = index1;
                for (uint index2 = 8U; index2 > 0U; --index2)
                {
                    if (((int)num2 & 1) == 1)
                        num2 = num2 >> 1 ^ num1;
                    else
                        num2 >>= 1;
                }
                Crc32.crc32Table[(int) index1] = num2;
            }
        }

        public int GetCrc32(Stream input)
        {
            return this.GetCrc32AndCopy(input, (Stream)null);
        }

        public int GetCrc32AndCopy(Stream input, Stream output)
        {
            if (input == null)
                throw new ZlibException("The input stream must not be null.");
            byte[] numArray = new byte[8192];
            int count1 = 8192;
            this._TotalBytesRead = 0L;
            int count2 = input.Read(numArray, 0, count1);
            if (output != null)
                output.Write(numArray, 0, count2);
            this._TotalBytesRead += (long)count2;
            while (count2 > 0)
            {
                this.SlurpBlock(numArray, 0, count2);
                count2 = input.Read(numArray, 0, count1);
                if (output != null)
                    output.Write(numArray, 0, count2);
                this._TotalBytesRead += (long)count2;
            }
            return ~(int)this._RunningCrc32Result;
        }

        public int ComputeCrc32(int W, byte B)
        {
            return this._InternalComputeCrc32((uint)W, B);
        }

        internal int _InternalComputeCrc32(uint W, byte B)
        {
            return (int)Crc32.crc32Table[((int)W ^ (int)B) & (int)byte.MaxValue] ^ (int)(W >> 8);
        }

        public void SlurpBlock(byte[] block, int offset, int count)
        {
            if (block == null)
                throw new ZlibException("The data buffer must not be null.");
            for (int index1 = 0; index1 < count; ++index1)
            {
                int index2 = offset + index1;
                this._RunningCrc32Result = this._RunningCrc32Result >> 8 ^ Crc32.crc32Table[(int) ((uint)block[index2] ^ this._RunningCrc32Result & (uint)byte.MaxValue)];
            }
            this._TotalBytesRead += (long)count;
        }
    }
}