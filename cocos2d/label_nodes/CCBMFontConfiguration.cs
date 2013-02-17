using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using cocos2d.Framework;

namespace cocos2d
{
#if IOS
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif
    public class CCBMFontConfiguration : CCObject
    {
        [ContentSerializer] 
        internal int m_nCommonHeight;

        [ContentSerializer] 
        internal Dictionary<int, ccBMFontDef> m_pFontDefDictionary = new Dictionary<int, ccBMFontDef>();

        [ContentSerializer] 
        internal Dictionary<int, tKerningHashElement> m_pKerningDictionary = new Dictionary<int, tKerningHashElement>();

        [ContentSerializer] 
        internal string m_sAtlasName;

        [ContentSerializer] 
        internal ccBMFontPadding m_tPadding;

        public string AtlasName
        {
            get { return m_sAtlasName; }
            set { m_sAtlasName = value; }
        }

        public CCBMFontConfiguration() {
        }

        public static CCBMFontConfiguration Create(string fntFile)
        {
            return CCApplication.SharedApplication.Content.Load<CCBMFontConfiguration>(fntFile);
            /*
            var pRet = new CCBMFontConfiguration();
            if (pRet.initWithFNTfile(FNTfile))
            {
                return pRet;
            }
            return null;
            */
        }

        public bool InitWithFnTfile(string fntFile)
        {
            var data = CCApplication.SharedApplication.Content.Load<CCContent>(fntFile);
            return InitWithString(data.Content, fntFile);
        }

        public bool InitWithString(string data, string fntFile)
        {
            m_pKerningDictionary.Clear();
            m_pFontDefDictionary.Clear();

            return ParseConfigFile(data, fntFile);
        }

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
                    var characterDefinition = new ccBMFontDef();
                    parseCharacterDefinition(line, characterDefinition);

                    m_pFontDefDictionary.Add(characterDefinition.charID, characterDefinition);
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

            return true;
        }

        private void parseCharacterDefinition(string line, ccBMFontDef characterDefinition)
        {
            //////////////////////////////////////////////////////////////////////////
            // line to parse:
            // char id=32   x=0     y=0     width=0     height=0     xoffset=0     yoffset=44    xadvance=14     page=0  chnl=0 
            //////////////////////////////////////////////////////////////////////////

            // Character ID
            int index = line.IndexOf("id=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            characterDefinition.charID = ccUtils.ccParseInt(value.Replace("id=", ""));
            //CCAssert(characterDefinition->charID < kCCBMFontMaxChars, "BitmpaFontAtlas: CharID bigger than supported");

            // Character x
            index = line.IndexOf("x=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.origin.x = ccUtils.ccParseFloat(value.Replace("x=", ""));

            // Character y
            index = line.IndexOf("y=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.origin.y = ccUtils.ccParseFloat(value.Replace("y=", ""));

            // Character width
            index = line.IndexOf("width=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.size.width = ccUtils.ccParseFloat(value.Replace("width=", ""));

            // Character height
            index = line.IndexOf("height=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.size.height = ccUtils.ccParseFloat(value.Replace("height=", ""));

            // Character xoffset
            index = line.IndexOf("xoffset=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.xOffset = ccUtils.ccParseInt(value.Replace("xoffset=", ""));

            // Character yoffset
            index = line.IndexOf("yoffset=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.yOffset = ccUtils.ccParseInt(value.Replace("yoffset=", ""));

            // Character xadvance
            index = line.IndexOf("xadvance=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.xAdvance = ccUtils.ccParseInt(value.Replace("xadvance=", ""));
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
            m_tPadding.top = ccUtils.ccParseInt(temp[0]);
            m_tPadding.right = ccUtils.ccParseInt(temp[1]);
            m_tPadding.bottom = ccUtils.ccParseInt(temp[2]);
            m_tPadding.left = ccUtils.ccParseInt(temp[3]);

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
            m_nCommonHeight = ccUtils.ccParseInt(value.Replace("lineHeight=", ""));

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

            m_sAtlasName = CCFileUtils.fullPathFromRelativeFile(value, fntFile);
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
            first = ccUtils.ccParseInt(value.Replace("first=", ""));

            // second
            int second;
            index = line.IndexOf("second=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            second = ccUtils.ccParseInt(value.Replace("second=", ""));

            // amount
            int amount;
            index = line.IndexOf("amount=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index);
            amount = ccUtils.ccParseInt(value.Replace("amount=", ""));

            try
            {
            var element = new tKerningHashElement();
            element.amount = amount;
            element.key = (first << 16) | (second & 0xffff);
            m_pKerningDictionary.Add(element.key, element);
        }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to parse font line: {0}", line);
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
        public class ccBMFontDef
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

        internal struct ccBMFontPadding
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

        public struct tKerningHashElement
        {
            public int amount;

            public int key; //key for the hash. 16-bit for 1st element, 16-bit for 2nd element
        }

        #endregion
    }
}