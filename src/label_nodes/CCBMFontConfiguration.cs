using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;

[assembly: InternalsVisibleTo("CocosSharp.Content.Pipeline.Importers")]
[assembly: InternalsVisibleTo("Microsoft.Xna.Framework.Content")]

namespace CocosSharp
{

#if IOS
    [Foundation.Preserve (AllMembers = true)]
#endif
    internal sealed class CCBMFontConfiguration
    {
        // Due to a bug in MonoGame's Custom Content loading not being able to recognize properties that are not
        // marked public we have to leave the Serialized information as variables and not properties.
        // This is an inconsistency with how XNA works.  Until this bug is fixed this needs to remain as it is.
        [ContentSerializer]
        internal int CommonHeight;// { get; set; }

        [ContentSerializer]
        internal Dictionary<int, CCBMGlyphDef> Glyphs; // { get; set; }

        [ContentSerializer]
        internal Dictionary<int, CCKerningHashElement> GlyphKernings; // { get; set; }

        [ContentSerializer]
        internal string AtlasName; // { get; set; }

        [ContentSerializer]
        internal CCBMGlyphPadding Padding;

        // Changed from List to Dictionary (i.e. hash table) which uses twice as much space but it MUCH faster -- O(1) vs. O(n)
        // then removed completely because Glyphs is exactly that and CharacterSet is actually redundant.
        // We need to keep this here so loading .xnb files will work correctly
        [ContentSerializer]
        internal List<int> CharacterSet { get; set; }

        #region Constructors

        internal static CCBMFontConfiguration FontConfigurationWithFile(string fntFile)
        {
            try
            {
                return CCContentManager.SharedContentManager.Load<CCBMFontConfiguration>(fntFile);
            }
            catch (ContentLoadException)
            {
                return new CCBMFontConfiguration(fntFile);
            }
        }

        internal CCBMFontConfiguration()
        {
            Glyphs = new Dictionary<int, CCBMGlyphDef>();
            // Removed because Glyphs (above) has the same data
            //CharacterSet = new Dictionary<int, char>();
            GlyphKernings = new Dictionary<int, CCKerningHashElement>();
        }

        internal CCBMFontConfiguration(string fntFile)
            : this(CCContentManager.SharedContentManager.Load<string>(fntFile), fntFile)
        { }
        

        // Content pipeline makes use of this constructor
        internal CCBMFontConfiguration(string data, string fntFile) : base()
        {
            Glyphs = new Dictionary<int, CCBMGlyphDef>();
            // Removed because Glyphs (above) has the same data
            //CharacterSet = new Dictionary<int, char>();
            GlyphKernings = new Dictionary<int, CCKerningHashElement>();

            GlyphKernings.Clear();
            Glyphs.Clear();

            // Removed because Glyphs (above) has the same data
            //CharacterSet = ParseConfigFile(data, fntFile);

            // Build Glyphs
            ParseConfigFile(data, fntFile);
        }

        #endregion Constructors


        // Unnecessary to return this (waste of memory)
        // Now unused
        //private Dictionary<int, char> ParseConfigFile(string pBuffer, string fntFile)
        private bool ParseConfigFile(string pBuffer, string fntFile)
        {
            long nBufSize = pBuffer.Length;

            Debug.Assert(pBuffer != null, "CCBMFontConfiguration::parseConfigFile | Open file error.");

            if (string.IsNullOrEmpty(pBuffer))
            {
                return false;
            }

            // parse spacing / padding
            string line;
            string strLeft = pBuffer;
            while (strLeft.Length > 0)
            {
                int pos = strLeft.IndexOf('\n');

                if (pos != -1)
                {
                    // the data is more than a line.get one line
                    line = strLeft.Substring(0, pos);
                    strLeft = strLeft.Substring(pos + 1);
                }
                else
                {
                    // get the left data
                    line = strLeft;
                    strLeft = null;
                }

                if (line.StartsWith("info face"))
                {
                    // XXX: info parsing is incomplete
                    // Not needed for the Hiero editors, but needed for the AngelCode editor
                    //			[self parseInfoArguments:line];
                    parseInfoArguments(line);
                }

                    // Check to see if the start of the line is something we are interested in
                else if (line.StartsWith("common lineHeight"))
                {
                    parseCommonArguments(line);
                }

                else if (line.StartsWith("page id"))
                {
                    parseImageFileName(line, fntFile);
                }

                else if (line.StartsWith("chars c"))
                {
                    // Ignore this line
                }
                else if (line.StartsWith("char"))
                {
                    // Parse the current line and create a new CharDef
                    var characterDefinition = new CCBMGlyphDef();
                    parseCharacterDefinition(line, characterDefinition);

                    Glyphs.Add(characterDefinition.Character, characterDefinition);

                    // Glyphs (above) has the same information
                    //validCharsString.Add(characterDefinition.Character, characterDefinition.Character);
                }
                //else if (line.StartsWith("kernings count"))
                //{
                //    this.parseKerningCapacity(line);
                //}
                else if (line.StartsWith("kerning first"))
                {
                    parseKerningEntry(line);
                }
            }

            return true; // validCharsString;
        }

        private void parseCharacterDefinition(string line, CCBMGlyphDef characterDefinition)
        {
            //////////////////////////////////////////////////////////////////////////
            // line to parse:
            // char id=32   x=0     y=0     width=0     height=0     xoffset=0     yoffset=44    xadvance=14     page=0  chnl=0 
            //////////////////////////////////////////////////////////////////////////

            // Character ID
            int index = line.IndexOf("id=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            characterDefinition.Character = CocosSharp.CCUtils.CCParseInt(value.Replace("id=", ""));
            //CCAssert(characterDefinition->charID < kCCBMFontMaxChars, "BitmpaFontAtlas: CharID bigger than supported");

            // Character x
            index = line.IndexOf("x=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.Subrect.Origin.X = CocosSharp.CCUtils.CCParseFloat(value.Replace("x=", ""));

            // Character y
            index = line.IndexOf("y=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.Subrect.Origin.Y = CocosSharp.CCUtils.CCParseFloat(value.Replace("y=", ""));

            // Character width
            index = line.IndexOf("width=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.Subrect.Size.Width = CocosSharp.CCUtils.CCParseFloat(value.Replace("width=", ""));

            // Character height
            index = line.IndexOf("height=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.Subrect.Size.Height = CocosSharp.CCUtils.CCParseFloat(value.Replace("height=", ""));

            // Character xoffset
            index = line.IndexOf("xoffset=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.XOffset = CocosSharp.CCUtils.CCParseInt(value.Replace("xoffset=", ""));

            // Character yoffset
            index = line.IndexOf("yoffset=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.YOffset = CocosSharp.CCUtils.CCParseInt(value.Replace("yoffset=", ""));

            // Character xadvance
            index = line.IndexOf("xadvance=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.XAdvance = CocosSharp.CCUtils.CCParseInt(value.Replace("xadvance=", ""));
        }

        // info face
        private void parseInfoArguments(string line)
        {
            //////////////////////////////////////////////////////////////////////////
            // possible lines to parse:
            // info face="Script" size=32 bold=0 italic=0 charset="" unicode=1 stretchH=100 smooth=1 aa=1 padding=1,4,3,2 spacing=0,0 outline=0
            // info face="Cracked" size=36 bold=0 italic=0 charset="" unicode=0 stretchH=100 smooth=1 aa=1 padding=0,0,0,0 spacing=1,1
            //////////////////////////////////////////////////////////////////////////

            // padding
            int index = line.IndexOf("padding=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);

            value = value.Replace("padding=", "");
            string[] temp = value.Split(',');
            Padding.Top = CocosSharp.CCUtils.CCParseInt(temp[0]);
            Padding.Right = CocosSharp.CCUtils.CCParseInt(temp[1]);
            Padding.Bottom = CocosSharp.CCUtils.CCParseInt(temp[2]);
            Padding.Left = CocosSharp.CCUtils.CCParseInt(temp[3]);

            //CCLOG("cocos2d: padding: %d,%d,%d,%d", m_tPadding.left, m_tPadding.top, m_tPadding.right, m_tPadding.bottom);
        }

        // common
        private void parseCommonArguments(string line)
        {
            //////////////////////////////////////////////////////////////////////////
            // line to parse:
            // common lineHeight=104 base=26 scaleW=1024 scaleH=512 pages=1 packed=0
            //////////////////////////////////////////////////////////////////////////

            // Height
            int index = line.IndexOf("lineHeight=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            CommonHeight = CocosSharp.CCUtils.CCParseInt(value.Replace("lineHeight=", ""));

            // scaleW. sanity check
            index = line.IndexOf("scaleW=") + "scaleW=".Length;
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            //CCAssert(atoi(value.c_str()) <= CCConfiguration::sharedConfiguration()->getMaxTextureSize(), "CCLabelBMFont: page can't be larger than supported");
            // scaleH. sanity check
            index = line.IndexOf("scaleH=") + "scaleH=".Length;
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            //CCAssert(atoi(value.c_str()) <= CCConfiguration::sharedConfiguration()->getMaxTextureSize(), "CCLabelBMFont: page can't be larger than supported");
            // pages. sanity check
            index = line.IndexOf("pages=") + "pages=".Length;
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            //CCAssert(atoi(value.c_str()) == 1, "CCBitfontAtlas: only supports 1 page");

            // packed (ignore) What does this mean ??
        }

        //page file
        private void parseImageFileName(string line, string fntFile)
        {
            //    //////////////////////////////////////////////////////////////////////////
            //// line to parse:
            //// page id=0 file="bitmapFontTest.png"
            ////////////////////////////////////////////////////////////////////////////

            // page ID. Sanity check
            int index = line.IndexOf('=') + 1;
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            try
            {
                int ivalue = int.Parse(value);
            }
            catch (Exception)
            {
                throw (new ContentLoadException("Invalid page ID for FNT descriptor. Line=" + line + ", value=" + value + ", indices=" + index + "," + index2));
            }
            //            Debug.Assert(Convert.ToInt32(value) == 0, "LabelBMFont file could not be found");
            // file 
            index = line.IndexOf('"') + 1;
            index2 = line.IndexOf('"', index);
            value = line.Substring(index, index2 - index);

            AtlasName = value;

            var directory = string.Empty;
            if (!CCFileUtils.GetDirectoryName(value, out directory))
                AtlasName = CCFileUtils.FullPathFromRelativeFile(value, fntFile);
        }

        private void parseKerningEntry(string line)
        {
            //////////////////////////////////////////////////////////////////////////
            // line to parse:
            // kerning first=121  second=44  amount=-7
            //////////////////////////////////////////////////////////////////////////

            // first
            int first;
            int index = line.IndexOf("first=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            first = CocosSharp.CCUtils.CCParseInt(value.Replace("first=", ""));

            // second
            int second;
            index = line.IndexOf("second=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            second = CocosSharp.CCUtils.CCParseInt(value.Replace("second=", ""));

            // amount
            int amount;
            index = line.IndexOf("amount=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index);
            amount = CocosSharp.CCUtils.CCParseInt(value.Replace("amount=", ""));

            try
            {
                var element = new CCKerningHashElement();
                element.Amount = amount;
                element.Key = (first << 16) | (second & 0xffff);
                GlyphKernings.Add(element.Key, element);
            }
            catch (Exception)
            {
                CocosSharp.CCLog.Log("Failed to parse font line: {0}", line);
            }
        }

        private void purgeKerningDictionary()
        {
            GlyphKernings.Clear();
        }

        #region Nested type: CCBMGlyphDef

        /// <summary>
        /// CCBMFont definition
        /// </summary>
        internal class CCBMGlyphDef
        {
            /// <summary>
            /// ID of the character
            /// </summary>
            public int Character { get; set; }

            /// <summary>
            /// origin and size of the font
            /// </summary>
            public CCRect Subrect;

            /// <summary>
            /// The amount to move the current position after drawing the character (in pixels)
            /// </summary>
            public int XAdvance { get; set; }

            /// <summary>
            /// The X amount the image should be offset when drawing the image (in pixels)
            /// </summary>
            public int XOffset { get; set; }

            /// <summary>
            /// The Y amount the image should be offset when drawing the image (in pixels)
            /// </summary>
            public int YOffset { get; set; }
        }

        #endregion

        #region Nested type: CCBMGlyphPadding

        public struct CCBMGlyphPadding
        {
            // padding left
            public int Bottom { get; set; }
            public int Left { get; set; }

            // padding top

            // padding right
            public int Right { get; set; }
            public int Top { get; set; }

            // padding bottom
        }

        #endregion

        #region Nested type: CCKerningHashElement

        public struct CCKerningHashElement
        {
            public int Amount { get; set; }

            public int Key { get; set; } //key for the hash. 16-bit for 1st element, 16-bit for 2nd element
        }

        #endregion
    }
}