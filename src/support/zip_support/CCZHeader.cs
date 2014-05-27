using System;

namespace CocosSharp
{
    public class CCZHeader
    {
		public byte[] Sig=new byte[4];				// signature. Should be 'CCZ!' 4 bytes
		public ushort Compression_type;				// should 0
		public ushort Version;						// should be 2 (although version type==1 is also supported)
		public uint  Reserved;						// Reserverd for users.
		public uint Len;							// size of the uncompressed file
    }
}
