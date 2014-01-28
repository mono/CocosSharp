using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace CocosSharp
{
#if IOS
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif
    public class CCBMFontConfiguration
    {
        [ContentSerializer]
        internal int m_nCommonHeight;

        [ContentSerializer]
        internal Dictionary<int, CCBMFontDef> m_pFontDefDictionary = new Dictionary<int, CCBMFontDef>();

        [ContentSerializer]
        internal Dictionary<int, CCKerningHashElement> m_pKerningDictionary = new Dictionary<int, CCKerningHashElement>();

        [ContentSerializer]
        internal string m_sAtlasName;

        [ContentSerializer]
        internal CCBMFontPadding m_tPadding;

        private List<int> m_pCharacterSet = new List<int>();

        public string AtlasName
        {
            get { return m_sAtlasName; }
            set { m_sAtlasName = value; }
        }
        
        
        public List<int> CharacterSet
        {
            set { m_pCharacterSet = value; }
            get { return m_pCharacterSet; }
        }


        #region Constructors

        public static CCBMFontConfiguration FontConfigurationWithFile(string fntFile)
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
        }

        protected CCBMFontConfiguration(string fntFile) : this(CCContentManager.SharedContentManager.Load<string>(fntFile), fntFile)
        {
        }

        protected CCBMFontConfiguration(string data, string fntFile)
        {
            InitWithString(data, fntFile);
        }

        protected virtual bool InitWithString(string data, string fntFile)
        {
            m_pKerningDictionary.Clear();
            m_pFontDefDictionary.Clear();

            m_pCharacterSet = ParseConfigFile(data, fntFile);

            if (m_pCharacterSet == null)
            {
                return false;
            }
            return true;
        }

        #endregion Constructors


        private List<int> ParseConfigFile(string pBuffer, string fntFile)
        {
            long nBufSize = pBuffer.Length;

            Debug.Assert(pBuffer != null, "CCBMFontConfiguration::parseConfigFile | Open file error.");

            if (string.IsNullOrEmpty(pBuffer))
            {
                return null;
            }

            var validCharsString = new List<int>();

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
                    var characterDefinition = new CCBMFontDef();
                    parseCharacterDefinition(line, characterDefinition);

                    m_pFontDefDictionary.Add(characterDefinition.charID, characterDefinition);

                    validCharsString.Add(characterDefinition.charID);
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

            return validCharsString;
        }

        private void parseCharacterDefinition(string line, CCBMFontDef characterDefinition)
        {
            //////////////////////////////////////////////////////////////////////////
            // line to parse:
            // char id=32   x=0     y=0     width=0     height=0     xoffset=0     yoffset=44    xadvance=14     page=0  chnl=0 
            //////////////////////////////////////////////////////////////////////////

            // Character ID
            int index = line.IndexOf("id=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            characterDefinition.charID = CocosSharp.CCUtils.CCParseInt(value.Replace("id=", ""));
            //CCAssert(characterDefinition->charID < kCCBMFontMaxChars, "BitmpaFontAtlas: CharID bigger than supported");

            // Character x
            index = line.IndexOf("x=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.Origin.X = CocosSharp.CCUtils.CCParseFloat(value.Replace("x=", ""));

            // Character y
            index = line.IndexOf("y=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.Origin.Y = CocosSharp.CCUtils.CCParseFloat(value.Replace("y=", ""));

            // Character width
            index = line.IndexOf("width=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.Size.Width = CocosSharp.CCUtils.CCParseFloat(value.Replace("width=", ""));

            // Character height
            index = line.IndexOf("height=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.Size.Height = CocosSharp.CCUtils.CCParseFloat(value.Replace("height=", ""));

            // Character xoffset
            index = line.IndexOf("xoffset=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.xOffset = CocosSharp.CCUtils.CCParseInt(value.Replace("xoffset=", ""));

            // Character yoffset
            index = line.IndexOf("yoffset=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.yOffset = CocosSharp.CCUtils.CCParseInt(value.Replace("yoffset=", ""));

            // Character xadvance
            index = line.IndexOf("xadvance=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.xAdvance = CocosSharp.CCUtils.CCParseInt(value.Replace("xadvance=", ""));
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
            m_tPadding.top = CocosSharp.CCUtils.CCParseInt(temp[0]);
            m_tPadding.right = CocosSharp.CCUtils.CCParseInt(temp[1]);
            m_tPadding.bottom = CocosSharp.CCUtils.CCParseInt(temp[2]);
            m_tPadding.left = CocosSharp.CCUtils.CCParseInt(temp[3]);

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
            m_nCommonHeight = CocosSharp.CCUtils.CCParseInt(value.Replace("lineHeight=", ""));

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

            m_sAtlasName = CocosSharp.CCFileUtils.FullPathFromRelativeFile(value, fntFile);
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
                element.amount = amount;
                element.key = (first << 16) | (second & 0xffff);
                m_pKerningDictionary.Add(element.key, element);
            }
            catch (Exception)
            {
                CocosSharp.CCLog.Log("Failed to parse font line: {0}", line);
            }
        }

        private void purgeKerningDictionary()
        {
            m_pKerningDictionary.Clear();
        }

        #region Nested type: ccBMFontDef

        /// <summary>
        /// BMFont definition
        /// </summary>
        public class CCBMFontDef
        {
            /// <summary>
            /// ID of the character
            /// </summary>
            public int charID;

            /// <summary>
            /// origin and size of the font
            /// </summary>
            public CCRect rect;

            /// <summary>
            /// The amount to move the current position after drawing the character (in pixels)
            /// </summary>
            public int xAdvance;

            /// <summary>
            /// The X amount the image should be offset when drawing the image (in pixels)
            /// </summary>
            public int xOffset;

            /// <summary>
            /// The Y amount the image should be offset when drawing the image (in pixels)
            /// </summary>
            public int yOffset;
        }

        #endregion

        #region Nested type: ccBMFontPadding

        internal struct CCBMFontPadding
        {
            // padding left
            public int bottom;
            public int left;

            // padding top

            // padding right
            public int right;
            public int top;

            // padding bottom
        }

        #endregion

        #region Nested type: tKerningHashElement

        public struct CCKerningHashElement
        {
            public int amount;

            public int key; //key for the hash. 16-bit for 1st element, 16-bit for 2nd element
        }

        #endregion
    }
}