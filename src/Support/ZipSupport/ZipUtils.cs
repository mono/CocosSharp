using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#if !WINDOWS_PHONE
using System.IO.Compression;
using GZipInputStream=System.IO.Compression.GZipStream;
#else
using GZipInputStream=WP7Contrib.Communications.Compression.GZipStream; // Found in Support/Compression/GZipStream
#endif
using MonoGame.Utilities;

namespace CocosSharp
{

    internal class ZipUtils
    {

        internal enum CompressionFormat
        {
            Zlib,               // zlib format.
            Bzip2,              // bzip2 format (not supported yet)
            Gzip,               // gzip format (not supported yet)
            None,               // plain (not supported yet)
        };

        /// <summary>
        /// Decompresses the given data stream from its source ZIP or GZIP format.
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        internal static byte[] Inflate(byte[] dataBytes)
        {

            return Inflate (new MemoryStream (dataBytes));
        }

        /// <summary>
        /// Helper to convert a Stream into a byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        /// <param name="inputStream">Input stream.</param>
        internal static byte[] StreamToByteArray(Stream inputStream)
        {
            if(inputStream == null)
            {
                return null;
            }

            byte[] retBytes = null;
            using(var memoryStream = new MemoryStream())
            {
                inputStream.CopyTo(memoryStream);
                retBytes = memoryStream.ToArray();
            }

            return retBytes;
        }

        /// <summary>
        /// Decompresses the given data stream from its source ZIP or GZIP format.
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        internal static byte[] Inflate(Stream dataStream)
        {

            byte[] outputBytes = null;
            try
            {
                using (var deflateStream = new ZlibStream(dataStream, MonoGame.Utilities.CompressionMode.Decompress))
                {
                    outputBytes = StreamToByteArray(deflateStream);
                }
            }
            catch  
            {
                try 
                {
                    dataStream.Seek (0, SeekOrigin.Begin);
                    #if !WINDOWS_PHONE
                    using (var gzipInputStream = new GZipInputStream(dataStream, System.IO.Compression.CompressionMode.Decompress))
                    #else
                    using (var gzipInputStream = new GZipInputStream(dataStream))
                    #endif
                    {
                        outputBytes = StreamToByteArray(gzipInputStream);
                    }
                } 
                catch (Exception exc) 
                {
                    CCLog.Log ("Error decompressing image data: " + exc.Message);
                }
            }


            return outputBytes;
        }

        /// <summary>
        /// Decompresses the given data stream from its source ZIP or GZIP format.
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        internal static byte[] Inflate (Stream dataStream, CompressionFormat format)
        {
            byte[] outputBytes = null;

            switch (format) 
            {
            case CompressionFormat.Zlib:

                try {
                    try {
                        using (var deflateStream = new ZlibStream (dataStream, MonoGame.Utilities.CompressionMode.Decompress)) 
                        {
                            outputBytes = StreamToByteArray(deflateStream);
                        }
                    } 
                    catch (Exception exc) 
                    {
                        CCLog.Log ("Error decompressing image data: " + exc.Message);
                    }

                } 
                catch (Exception exc) 
                {
                    CCLog.Log ("Error decompressing image data: " + exc.Message);
                }
                break;
            case CompressionFormat.Gzip:

                try 
                {
                    #if !WINDOWS_PHONE
                    using (var gzipInputStream = new GZipInputStream(dataStream, System.IO.Compression.CompressionMode.Decompress))
                    #else
                    using (var gzipInputStream = new GZipInputStream(dataStream))
                    #endif
                    {
                        outputBytes = StreamToByteArray(gzipInputStream);
                    }
                } 
                catch (Exception exc) 
                {
                    CCLog.Log ("Error decompressing image data: " + exc.Message);
                }
                break;
            }

            return outputBytes;
        }
    }
}