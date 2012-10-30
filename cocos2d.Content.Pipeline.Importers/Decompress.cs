using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace cocos2d.Content.Pipeline.Importers
{
    public class Decompress
    {
        public static byte[] GZipDecompress(Byte[] bytes)
        {
            using (MemoryStream tempMs = new MemoryStream())
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    GZipStream Decompress = new GZipStream(ms, CompressionMode.Decompress);
                    Decompress.CopyTo(tempMs);
                    Decompress.Close();
                    return tempMs.ToArray();
                }
            }
        }
    }
}
