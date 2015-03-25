using System;
using System.Text;
using System.IO;

namespace CocosSharp
{
    internal static partial class CCLabelUtilities
    {

        // The code for reading the TTF font file is based on TrueTypeSharp project originally
        // developed by Zer at http://www.zer7.com/software/truetypesharp.
        // The original code does not parse the name table to obtain the FontFamily so a rudimentary
        // implementation is here in GetFontFamily methods.

        #region Structure Definitions

#pragma warning disable 0660, 0661 // This never goes into a collection...
        internal struct FakePtr<T>
        {
            public T[] Array; public int Offset;

            public T[] GetData(int length)
            {
                var t = new T[length];
                if (Array != null) { global::System.Array.Copy(Array, Offset, t, 0, length); }
                return t;
            }

            public void MakeNull()
            {
                Array = null; Offset = 0;
            }

            public T this[int index]
            {
                get
                {
                    try { return Array[Offset + index]; }
                    catch (IndexOutOfRangeException) { return default(T); } // Sometimes accesses are made out of range, it appears.
                    // In particular, to get all the way to char.MaxValue, this was needed.
                    // Probably bad data in the font. Also, is bounds checking done?
                    // I don't see it... Either way, it's not a problem for us here.
                }
                set { Array[Offset + index] = value; }
            }

            public T Value
            {
                get { return this[0]; }
                set { this[0] = value; }
            }

            public bool IsNull
            {
                get { return Array == null; }
            }

            public static FakePtr<T> operator +(FakePtr<T> p, int offset)
            {
                return new FakePtr<T>() { Array = p.Array, Offset = p.Offset + offset };
            }

            public static FakePtr<T> operator -(FakePtr<T> p, int offset)
            {
                return p + -offset;
            }

            public static FakePtr<T> operator +(FakePtr<T> p, uint offset)
            {
                return p + (int)offset;
            }

            public static FakePtr<T> operator ++(FakePtr<T> p)
            {
                return p + 1;
            }

            public static bool operator ==(FakePtr<T> p1, FakePtr<T> p2)
            {
                return p1.Array == p2.Array && p1.Offset == p2.Offset;
            }

            public static bool operator !=(FakePtr<T> p1, FakePtr<T> p2)
            {
                return !(p1 == p2);
            }
        }

        internal struct stbtt_fontinfo
        {
            public FakePtr<byte> data;         // pointer to .ttf file
            public int fontstart;    // offset of start of font

            public uint name; // table locations as offset from start of .ttf

            public stbtt_name_record[] nameRecords;
        }

        internal struct stbtt_name_record
        {
            public ushort platformId;
            public ushort platformSpecificId;
            public ushort languageId;
            public ushort nameId;
            public ushort length;
            public uint offset;

        }

        static stbtt_fontinfo _info;

        #endregion

        #region GetFontFamily routines 

        //
        // http://www.microsoft.com/typography/otspec/name.htm
        // and https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6name.html
        // 
        public static string GetFontFamily(string filename)
        {
            return GetFontFamily(File.ReadAllBytes(filename), 0);
        }

        //
        // http://www.microsoft.com/typography/otspec/name.htm
        // and https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6name.html
        // 
        public static string GetFontFamily(byte[] data, int offset)
        {
            CheckFontData(data, offset);

            if (0 == stbtt_InitFont(ref _info,
                new FakePtr<byte>() { Array = data }, offset))
            {
                throw new BadImageFormatException("Couldn't load TrueType file.");
            }

            var name = new StringBuilder();
            uint i;

            foreach (var nr in _info.nameRecords)
            {
                name.Clear();
                //if (nr.nameId == 1 || nr.nameId == 2 || nr.nameId == 4)
                // We only want the Name Id code 1 for Font Family
                // Reference Name Identifiers in the links above
                if (nr.nameId == 1)
                {
                    var nameBytes = new byte[nr.length];
                    var nameOffset = _info.data + nr.offset;
                    for (i = 0; i < nr.length; i++)
                    {
                        nameBytes[i] = ttBYTE(nameOffset + i);
                    }

                    // reference http://www.microsoft.com/typography/otspec/name.htm
                    // Note that OS/2 and Windows both require that all name strings be defined in Unicode. 
                    // Thus all 'name' table strings for platform ID = 3 (Windows) will require two bytes per character. 
                    // Macintosh fonts require single byte strings. 
                    if (nr.platformId == 1) // Mac
                        name.Append(Encoding.UTF8.GetString(nameBytes));
                    else
                        name.Append(Encoding.BigEndianUnicode.GetString(nameBytes));  // Microsoft 

                    return name.ToString();

                }
            }

            return string.Empty;
        }

        #endregion

        #region TrueTypeSharp helper methods

        static void CheckFontData(byte[] data, int offset)
        {
            if (data == null) { throw new ArgumentNullException("data"); }
            if (offset < 0 || offset > data.Length) { throw new ArgumentOutOfRangeException("offset"); }
        }

        static ushort ttUSHORT(FakePtr<byte> p) { return (ushort)(p[0] * 256 + p[1]); }
        static sbyte ttCHAR(FakePtr<byte> p) { return (sbyte)p.Value; }
        static uint ttULONG(FakePtr<byte> p) { return ((uint)p[0] << 24) + ((uint)p[1] << 16) + ((uint)p[2] << 8) + p[3]; }
        static byte ttBYTE(FakePtr<byte> p) { return p.Value; }

        static int stbtt_InitFont(ref stbtt_fontinfo info, FakePtr<byte> data2, int fontstart)
        {
            FakePtr<byte> data = (FakePtr<byte>)data2;
            uint i;

            info.data = data;
            info.fontstart = fontstart;

            info.name = stbtt__find_table(data, fontstart, "name"); // required
            if (info.name == 0)
                return 0;

            var format = ttUSHORT(data + info.name);
            var nameRecordCount = ttUSHORT(data + info.name + 2);
            var stringOffset = ttUSHORT(data + info.name + 4);
            var offset = info.name + stringOffset;
            var nameRecords = new stbtt_name_record[nameRecordCount];
            for (i = 0; i < nameRecordCount; ++i)
            {
                uint name_record = info.name + 6 + 12 * i;
                var nr = nameRecords[i];
                nr.platformId = ttUSHORT(data + name_record);
                nr.platformSpecificId = ttUSHORT(data + name_record + 2);
                nr.languageId = ttUSHORT(data + name_record + 4);
                nr.nameId = ttUSHORT(data + name_record + 6);
                nr.length = ttUSHORT(data + name_record + 8);
                nr.offset = ttUSHORT(data + name_record + 10) + offset;
                nameRecords[i] = nr;
            }

            info.nameRecords = nameRecords;

            return 1;
        }

        static uint stbtt__find_table(FakePtr<byte> data, int fontstart, string tag)
        {
            int num_tables = ttUSHORT(data + fontstart + 4);
            int tabledir = fontstart + 12;
            int i;
            for (i = 0; i < num_tables; ++i)
            {
                int loc = tabledir + 16 * i;
                if (stbtt_tag(data + loc + 0, tag))
                    return ttULONG(data + loc + 8);
            }
            return 0;
        }

        static bool stbtt_tag4(FakePtr<byte> p, byte c0, byte c1, byte c2, byte c3)
        {
            return ((p)[0] == (c0) && (p)[1] == (c1) && (p)[2] == (c2) && (p)[3] == (c3));
        }

        static bool stbtt_tag(FakePtr<byte> p, string str)
        {
            return stbtt_tag4(p,
                (byte)(str.Length >= 1 ? str[0] : 0),
                (byte)(str.Length >= 2 ? str[1] : 0),
                (byte)(str.Length >= 3 ? str[2] : 0),
                (byte)(str.Length >= 4 ? str[3] : 0));
        }

        #endregion
    }
}


