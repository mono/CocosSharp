// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharedUtils.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
// http://www.sharpgis.net/post/2011/08/28/GZIP-Compressed-Web-Requests-in-WP7-Take-2.aspx
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WP7Contrib.Communications.Compression
{
    using System.IO;
    using System.Text;

    internal class SharedUtils
    {
        public static int URShift(int number, int bits)
        {
            return (int)((uint)number >> bits);
        }

        public static long URShift(long number, int bits)
        {
            return (long)((ulong)number >> bits);
        }

        public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
        {
            if (target.Length == 0)
                return 0;
            char[] buffer = new char[target.Length];
            int num = sourceTextReader.Read(buffer, start, count);
            if (num == 0)
                return -1;
            for (int index = start; index < start + num; ++index)
                target[index] = (byte)buffer[index];
            return num;
        }

        internal static byte[] ToByteArray(string sourceString)
        {
            return Encoding.UTF8.GetBytes(sourceString);
        }

        internal static char[] ToCharArray(byte[] byteArray)
        {
            return Encoding.UTF8.GetChars(byteArray);
        }
    }
}