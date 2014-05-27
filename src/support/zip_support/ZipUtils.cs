using System;
using System.Collections.Generic;
using System.Text;
using WP7Contrib.Communications.Compression;
using System.IO;

namespace CocosSharp
{
    public enum CCZ_COMPRESSION
    {
        Zlib,				// zlib format.
        Bzip2,				// bzip2 format (not supported yet)
        Gzip,				// gzip format (not supported yet)
        None,				// plain (not supported yet)
    };

    public class ZipUtils
    {

        /// <summary>
        /// 	* Inflates either zlib or gzip deflated memory. The inflated memory is
        ///	* expected to be freed by the caller.
        ///	*
        ///	* It will allocate 256k for the destination buffer. If it is not enought it will multiply the previous buffer size per 2, until there is enough memory.
        ///	* @returns the length of the deflated buffer
        ///	*
        ///	@since v0.8.1
        /// </summary>
        /// <param name="parameterin"></param>
        /// <param name="inLength"></param>
        /// <param name="parameterout"></param>
        /// <returns></returns>
        public static int InflateMemory(byte[] parameterin, uint inLength, byte[] parameterout)
        {
            MemoryStream ms = new MemoryStream(parameterout, true);
            try
            {
                GZipStream gs = new GZipStream(new MemoryStream(parameterin, false)); // , CompressionMode.Decompress, false);
#if XBOX
                byte[] b = new byte[8096];
                while (gs.CanRead)
                {
                    int amt = gs.Read(b, 0, b.Length);
                    if (amt <= 0)
                    {
                        break;
                    }
                    ms.Write(b, 0, amt);
                }
#else
                gs.CopyTo(ms);
#endif
                return ((int)ms.Length);
            }
            catch (Exception)
            {
                // Nog a gzip stream, could be zlib stream
            }
            return (0);
        }

        /** 
        * Inflates either zlib or gzip deflated memory. The inflated memory is
        * expected to be freed by the caller.
        *
        * outLenghtHint is assumed to be the needed room to allocate the inflated buffer.
        *
        * @returns the length of the deflated buffer
        *
        @since v1.0.0
        */
        public static int InflateMemoryWithHint(byte[] parameterin, uint inLength, byte[] parameterout, int outLenghtHint)
        {
            return (InflateMemory(parameterin, inLength, parameterout));
        }

        /** inflates a GZip file into memory
        *
        * @returns the length of the deflated buffer
        *
        * @since v0.99.5
        */
        public static int InflateGZipFile(char filename, byte[] parameterout)
        {
            throw new NotImplementedException();
        }

        /** inflates a CCZ file into memory
        *
        * @returns the length of the deflated buffer
        *
        * @since v0.99.5
        */
        public static int InflateCCZFile(string filename, byte[] parameterout)
        {
            throw new NotImplementedException();
        }

        private static int InflateMemoryWithHint(byte[] parameterin, uint inLength, byte[] parameterout, uint[] outLength,
                 uint outLenghtHint)
        {
            //    /* ret value */
            //int err = Z_OK;

            //int bufferSize = outLenghtHint;
            //*out = new unsigned char[bufferSize];

            //z_stream d_stream; /* decompression stream */	
            //d_stream.zalloc = (alloc_func)0;
            //d_stream.zfree = (free_func)0;
            //d_stream.opaque = (voidpf)0;

            //d_stream.next_in  = in;
            //d_stream.avail_in = inLength;
            //d_stream.next_out = *out;
            //d_stream.avail_out = bufferSize;

            ///* window size to hold 256k */
            //if( (err = inflateInit2(&d_stream, 15 + 32)) != Z_OK )
            //    return err;

            //for (;;) 
            //{
            //    err = inflate(&d_stream, Z_NO_FLUSH);

            //    if (err == Z_STREAM_END)
            //    {
            //        break;
            //    }

            //    switch (err) 
            //    {
            //    case Z_NEED_DICT:
            //        err = Z_DATA_ERROR;
            //    case Z_DATA_ERROR:
            //    case Z_MEM_ERROR:
            //        inflateEnd(&d_stream);
            //        return err;
            //    }

            //    // not enough memory ?
            //    if (err != Z_STREAM_END) 
            //    {
            //        delete [] *out;
            //        *out = new unsigned char[bufferSize * BUFFER_INC_FACTOR];

            //        /* not enough memory, ouch */
            //        if (! *out ) 
            //        {
            //            CCLOG("cocos2d: ZipUtils: realloc failed");
            //            inflateEnd(&d_stream);
            //            return Z_MEM_ERROR;
            //        }

            //        d_stream.next_out = *out + bufferSize;
            //        d_stream.avail_out = bufferSize;
            //        bufferSize *= BUFFER_INC_FACTOR;
            //    }
            //}

            //*outLength = bufferSize - d_stream.avail_out;
            //err = inflateEnd(&d_stream);
            //return err;
            throw new NotImplementedException();
        }
    }
}
