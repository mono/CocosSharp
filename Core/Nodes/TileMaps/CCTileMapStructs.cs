using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public enum CCTileMapType
    {
        None = -1,
        Ortho = 0,
        Hex = 1,
        Iso = 2,
        Staggered = 3
    }

    [Flags]
    public enum CCTileDataCompressionType
    {
        None = 1 << 0,
        Base64 = 1 << 1,
        Gzip = 1 << 2,
        Zlib = 1 << 3,
        Csv = 1 << 4
    }

    public enum CCTileMapProperty
    {
        None,
        Map,
        Layer,
        ObjectGroup,
        Object,
        Tile,
        TileAnimation
    }

    [Flags]
    public enum CCTileFlags : short
    {
        None = 1,
        Horizontal = 2,
        Vertical = 4,
        TileDiagonal = 8,
        FlippedAll = (Horizontal | Vertical | TileDiagonal),
        FlippedMask = ~(FlippedAll)
    }

    public struct CCTileAnimationKeyFrame
    {
        public short Gid { get; set; }
        public short Duration { get; set; }

        public CCTileAnimationKeyFrame(short gid, short duration) : this()
        {
            Gid = gid;
            Duration = duration;
        }
    }

    public struct CCTileGidAndFlags
    {
        public static readonly CCTileGidAndFlags EmptyTile = new CCTileGidAndFlags(0, 0);

        public short Gid { get; set; }
        public CCTileFlags Flags  { get; set; }

        #region Constructors

        public CCTileGidAndFlags(short gid, CCTileFlags flags) : this()
        {
            Gid = gid;
            Flags = flags;
        }

        public CCTileGidAndFlags(short gid) : this(gid, CCTileFlags.None)
        {
        }

        #endregion Constructors


        #region Equality

        public static bool Equal(ref CCTileGidAndFlags gid1, ref CCTileGidAndFlags gid2)
        {
            return ((gid1.Gid == gid2.Gid) && (gid1.Flags == gid2.Flags));
        }

        public override bool Equals(object obj)
        {
            return (Equals((CCTileGidAndFlags) obj));
        }

        public bool Equals(CCTileGidAndFlags gidAndFlag)
        {
            return this == gidAndFlag;
        }

        public override int GetHashCode()
        {
            return Gid.GetHashCode() + Flags.GetHashCode();
        }

        public static bool operator ==(CCTileGidAndFlags gid1, CCTileGidAndFlags gid2)
        {
            return gid1.Gid == gid2.Gid && gid1.Flags == gid2.Flags;
        }

        public static bool operator !=(CCTileGidAndFlags gid1, CCTileGidAndFlags gid2)
        {
            return gid1.Gid != gid2.Gid || gid1.Flags != gid2.Flags;
        }

        #endregion Equality
    }


    internal struct CCTileMapFileEncodedTileFlags
    {
        internal static uint Horizontal = 0x80000000;
        internal static uint Vertical = 0x40000000;
        internal static uint TileDiagonal = 0x20000000;
        internal static uint FlippedAll = (Horizontal | Vertical | TileDiagonal);
        internal static uint FlippedMask = ~(FlippedAll);

        internal static CCTileGidAndFlags DecodeGidAndFlags(uint encodedGidAndFlags)
        {
            CCTileGidAndFlags gidAndFlags= new CCTileGidAndFlags(0, 0);

            gidAndFlags.Gid = (short)(encodedGidAndFlags & CCTileMapFileEncodedTileFlags.FlippedMask);

            if (gidAndFlags.Gid != 0)
            {
                gidAndFlags.Gid = gidAndFlags.Gid;
            }

            if((encodedGidAndFlags & CCTileMapFileEncodedTileFlags.Horizontal) != 0)
                gidAndFlags.Flags = gidAndFlags.Flags | CCTileFlags.Horizontal;

            if((encodedGidAndFlags & CCTileMapFileEncodedTileFlags.Vertical) != 0)
                gidAndFlags.Flags = gidAndFlags.Flags | CCTileFlags.Vertical;

            if ((encodedGidAndFlags & CCTileMapFileEncodedTileFlags.TileDiagonal) != 0)
                gidAndFlags.Flags = gidAndFlags.Flags | CCTileFlags.TileDiagonal;

            return gidAndFlags;
        }
    }

    public struct CCTileMapCoordinates
    {
        public static readonly CCTileMapCoordinates Zero = new CCTileMapCoordinates(0,0);

        public int Row;
        public int Column;

        public CCTileMapCoordinates(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public CCTileMapCoordinates(CCPoint pt) : this((int)pt.X, (int)pt.Y)
        {
        }

        public CCPoint Point
        {
            get { return new CCPoint((float)Column, (float)Row); }
        }

        public CCSize Size
        {
            get { return new CCSize((float)Column, (float)Row); }
        }
    }
}