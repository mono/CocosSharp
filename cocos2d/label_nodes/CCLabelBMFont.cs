using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCLabelBMFont : CCSpriteBatchNode, ICCLabelProtocol, ICCRGBAProtocol
    {
        public const int kCCLabelAutomaticWidth = -1;
        public static Dictionary<string, CCBMFontConfiguration> s_pConfigurations;
        private bool m_bIsOpacityModifyRGB;
        private bool m_bLineBreakWithoutSpaces;
        private byte m_cOpacity;
        private float m_fWidth;
        private CCTextAlignment m_pAlignment;
        protected CCBMFontConfiguration m_pConfiguration;
        private string m_sFntFile;
        private string m_sInitialString;
        protected string m_sString = "";
        private CCColor3B m_tColor;
        private CCPoint m_tImageOffset;

        public override CCPoint AnchorPoint
        {
            get { return base.AnchorPoint; }
            set
            {
                if (!m_tAnchorPoint.Equals(value))
                {
                    base.AnchorPoint = value;
                    UpdateLabel();
                }
            }
        }

        public override float Scale
        {
            get { return base.Scale; }
            set
            {
                base.Scale = value;
                UpdateLabel();
            }
        }

        public override float ScaleX
        {
            get { return base.ScaleX; }
            set
            {
                base.ScaleX = value;
                UpdateLabel();
            }
        }

        public override float ScaleY
        {
            get { return base.ScaleY; }
            set
            {
                base.ScaleY = value;
                UpdateLabel();
            }
        }

        public string FntFile
        {
            get { return m_sFntFile; }
            set
            {
                if (value != null && m_sFntFile != value)
                {
                    CCBMFontConfiguration newConf = FNTConfigLoadFile(value);

                    Debug.Assert(newConf != null, "CCLabelBMFont: Impossible to create font. Please check file");

                    m_sFntFile = value;

                    m_pConfiguration = newConf;

                    Texture = CCTextureCache.SharedTextureCache.AddImage(m_pConfiguration.AtlasName);
                    CreateFontChars();
                }
            }
        }

        #region ICCLabelProtocol Members

        public virtual void SetString(string newString)
        {
            SetString(newString, false);
        }

        public string GetString()
        {
            return m_sInitialString;
        }

        #endregion

        #region ICCRGBAProtocol Members

        public CCColor3B Color
        {
            get { return m_tColor; }
            set
            {
                m_tColor = value;
                if (m_pChildren != null && m_pChildren.count != 0)
                {
                    CCNode[] elements = m_pChildren.Elements;
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
                    {
                        var protocol = elements[i] as ICCRGBAProtocol;
                        if (protocol != null)
                        {
                            protocol.Color = value;
                        }
                    }
                }
            }
        }

        public byte Opacity
        {
            get { return m_cOpacity; }
            set
            {
                m_cOpacity = value;
                if (m_pChildren != null && m_pChildren.count != 0)
                {
                    CCNode[] elements = m_pChildren.Elements;
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
                    {
                        var protocol = elements[i] as ICCRGBAProtocol;
                        if (protocol != null)
                        {
                            protocol.Opacity = value;
                        }
                    }
                }
            }
        }

        public bool IsOpacityModifyRGB
        {
            get { return m_bIsOpacityModifyRGB; }
            set
            {
                m_bIsOpacityModifyRGB = value;
                if (m_pChildren != null && m_pChildren.count != 0)
                {
                    CCNode[] elements = m_pChildren.Elements;
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
                    {
                        var protocol = elements[i] as ICCRGBAProtocol;
                        if (protocol != null)
                        {
                            protocol.IsOpacityModifyRGB = value;
                        }
                    }
                }
            }
        }

        #endregion

        public static void FNTConfigRemoveCache()
        {
            if (s_pConfigurations != null)
            {
                s_pConfigurations.Clear();
            }
        }

        public static void PurgeCachedData()
        {
            FNTConfigRemoveCache();
        }

        public new static CCLabelBMFont Create()
        {
            var pRet = new CCLabelBMFont();
            pRet.Init();
            return pRet;
        }

        public static CCLabelBMFont Create(string str, string fntFile, float width)
        {
            return Create(str, fntFile, width, CCTextAlignment.CCTextAlignmentLeft, CCPoint.Zero);
        }

        public static CCLabelBMFont Create(string str, string fntFile)
        {
            return Create(str, fntFile, kCCLabelAutomaticWidth, CCTextAlignment.CCTextAlignmentLeft, CCPoint.Zero);
        }

        public static CCLabelBMFont Create(string str, string fntFile, float width, CCTextAlignment alignment)
        {
            return Create(str, fntFile, width, alignment, CCPoint.Zero);
        }

        public static CCLabelBMFont Create(string str, string fntFile, float width, CCTextAlignment alignment, CCPoint imageOffset)
        {
            var pRet = new CCLabelBMFont();
            pRet.InitWithString(str, fntFile, width, alignment, imageOffset);
            return pRet;
        }

        public new bool Init()
        {
            return InitWithString(null, null, kCCLabelAutomaticWidth, CCTextAlignment.CCTextAlignmentLeft, CCPoint.Zero);
        }

        public bool InitWithString(string theString, string fntFile, float width, CCTextAlignment alignment, CCPoint imageOffset)
        {
            Debug.Assert(m_pConfiguration == null, "re-init is no longer supported");
            Debug.Assert((theString == null && fntFile == null) || (theString != null && fntFile != null), "Invalid params for CCLabelBMFont");

            CCTexture2D texture;

            if (!String.IsNullOrEmpty(fntFile))
            {
                CCBMFontConfiguration newConf = FNTConfigLoadFile(fntFile);
                Debug.Assert(newConf != null, "CCLabelBMFont: Impossible to create font. Please check file");

                m_pConfiguration = newConf;

                m_sFntFile = fntFile;

                try
                {
                texture = CCTextureCache.SharedTextureCache.AddImage(m_pConfiguration.AtlasName);
            }
                catch (Exception)
                {
                    // Try the 'images' ref location just in case.
                    try
                    {
                        texture = CCTextureCache.SharedTextureCache.AddImage(System.IO.Path.Combine("images", m_pConfiguration.AtlasName));
                    }
                    catch (Exception)
                    {
                        // Lastly, try <font_path>/images/<font_name>
                        string dir = System.IO.Path.GetDirectoryName(m_pConfiguration.AtlasName);
                        string fname = System.IO.Path.GetFileName(m_pConfiguration.AtlasName);
                        string newName = System.IO.Path.Combine(System.IO.Path.Combine(dir, "images"), fname);
                        texture = CCTextureCache.SharedTextureCache.AddImage(newName);
                    }
                }
            }
            else
            {
                texture = new CCTexture2D();
            }

            if (String.IsNullOrEmpty(theString))
            {
                theString = String.Empty;
            }

            if (base.InitWithTexture(texture, theString.Length))
            {
                m_pAlignment = alignment;
                m_tImageOffset = imageOffset;
                m_fWidth = width;
                m_cOpacity = 255;
                m_tColor = CCTypes.CCWhite;
                m_tContentSize = CCSize.Zero;
                m_bIsOpacityModifyRGB = m_pobTextureAtlas.Texture.HasPremultipliedAlpha;
                SetString(theString);
                AnchorPoint = new CCPoint(0.5f, 0.5f);
                return true;
            }
            return false;
        }

        private int KerningAmountForFirst(int first, int second)
        {
            int ret = 0;
            int key = (first << 16) | (second & 0xffff);

            if (m_pConfiguration.m_pKerningDictionary != null)
            {
                CCBMFontConfiguration.tKerningHashElement element;
                if (m_pConfiguration.m_pKerningDictionary.TryGetValue(key, out element))
                {
                    ret = element.amount;
                }
            }
            return ret;
        }

        public void CreateFontChars()
        {
            int nextFontPositionX = 0;
            int nextFontPositionY = 0;
            //unsigned short prev = -1;
            int kerningAmount = 0;

            CCSize tmpSize = CCSize.Zero;

            int longestLine = 0;
            int totalHeight = 0;

            int quantityOfLines = 1;

            int stringLen = m_sString.Length;
            if (stringLen == 0)
            {
                return;
            }

            for (int i = 0; i < stringLen - 1; ++i)
            {
                if (m_sString[i] == '\n')
                {
                    quantityOfLines++;
                }
            }

            totalHeight = m_pConfiguration.m_nCommonHeight * quantityOfLines;
            nextFontPositionY = 0 - (m_pConfiguration.m_nCommonHeight - m_pConfiguration.m_nCommonHeight * quantityOfLines);

            for (int i = 0; i < stringLen; i++)
            {
                char c = m_sString[i];

                if (c == '\n')
                {
                    nextFontPositionX = 0;
                    nextFontPositionY -= m_pConfiguration.m_nCommonHeight;
                    continue;
                }

                // unichar is a short, and an int is needed on HASH_FIND_INT
                CCBMFontConfiguration.ccBMFontDef fontDef = m_pConfiguration.m_pFontDefDictionary[c];

                CCRect rect = fontDef.rect;
                rect = CCMacros.CCRectanglePixelsToPoints(rect);

                rect.Origin.X += m_tImageOffset.X;
                rect.Origin.Y += m_tImageOffset.Y;

                CCSprite fontChar;

                fontChar = (CCSprite) (GetChildByTag(i));
                if (fontChar == null)
                {
                    fontChar = new CCSprite();
                    fontChar.InitWithTexture(m_pobTextureAtlas.Texture, rect);
                    AddChild(fontChar, 0, i);
                }
                else
                {
                    // reusing fonts
                    fontChar.SetTextureRect(rect, false, rect.Size);

                    // restore to default in case they were modified
                    fontChar.Visible = true;
                    fontChar.Opacity = 255;
                }

                // See issue 1343. cast( signed short + unsigned integer ) == unsigned integer (sign is lost!)
                int yOffset = m_pConfiguration.m_nCommonHeight - fontDef.yOffset;
                var fontPos = new CCPoint((float) nextFontPositionX + fontDef.xOffset + fontDef.rect.Size.Width * 0.5f + kerningAmount,
                                          (float) nextFontPositionY + yOffset - rect.Size.Height * 0.5f * CCMacros.CCContentScaleFactor());
                fontChar.Position = CCMacros.CCPointPixelsToPoints(fontPos);

                // update kerning
                nextFontPositionX += fontDef.xAdvance + kerningAmount;
                //prev = c;

                // Apply label properties
                fontChar.IsOpacityModifyRGB = m_bIsOpacityModifyRGB;
                // Color MUST be set before opacity, since opacity might change color if OpacityModifyRGB is on
                fontChar.Color = m_tColor;

                // only apply opacity if it is different than 255 )
                // to prevent modifying the color too (issue #610)
                if (m_cOpacity != 255)
                {
                    fontChar.Opacity = m_cOpacity;
                }

                if (longestLine < nextFontPositionX)
                {
                    longestLine = nextFontPositionX;
                }
            }

            tmpSize.Width = longestLine;
            tmpSize.Height = totalHeight;

            ContentSize = CCMacros.CCSizePixelsToPoints(tmpSize);
        }


        public virtual void SetString(string newString, bool fromUpdate)
        {
            m_sString = newString;
            m_sInitialString = newString;

            UpdateString(fromUpdate);
        }

        private void UpdateString(bool fromUpdate)
        {
            if (m_pChildren != null && m_pChildren.count != 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].Visible = false;
                }
            }

            CreateFontChars();

            if (!fromUpdate)
            {
                UpdateLabel();
            }
        }

        protected void UpdateLabel()
        {
            SetString(m_sInitialString, true);

            if (m_fWidth > 0)
            {
                // Step 1: Make multiline
                string str_whole = m_sString;
                int stringLength = m_sString.Length;
                var multiline_string = new StringBuilder(stringLength);
                var last_word = new StringBuilder(stringLength);

                int line = 1, i = 0;
                bool start_line = false, start_word = false;
                float startOfLine = -1, startOfWord = -1;
                int skip = 0;

                RawList<CCNode> children = m_pChildren;
                for (int j = 0; j < children.count; j++)
                {
                    CCSprite characterSprite;

                    while ((characterSprite = (CCSprite) GetChildByTag(j + skip)) == null)
                    {
                        skip++;
                    }

                    if (!characterSprite.Visible)
                    {
                        continue;
                    }

                    if (i >= stringLength)
                    {
                        break;
                    }

                    char character = str_whole[i];

                    if (!start_word)
                    {
                        startOfWord = GetLetterPosXLeft(characterSprite);
                        start_word = true;
                    }
                    if (!start_line)
                    {
                        startOfLine = startOfWord;
                        start_line = true;
                    }

                    // Newline.
                    if (character == '\n')
                    {
                        int len = last_word.Length;
                        while (len > 0 && Char.IsWhiteSpace(last_word[len - 1]))
                        {
                            len--;
                            last_word.Remove(len, 1);
                        }

                        multiline_string.Append(last_word);
                        multiline_string.Append('\n');

#if XBOX || XBOX360
                        last_word.Length = 0;
#else
                        last_word.Clear();
#endif

                        start_word = false;
                        start_line = false;
                        startOfWord = -1;
                        startOfLine = -1;
                        i++;
                        line++;

                        if (i >= stringLength)
                            break;

                        character = str_whole[i];

                        if (startOfWord == 0)
                        {
                            startOfWord = GetLetterPosXLeft(characterSprite);
                            start_word = true;
                        }
                        if (startOfLine == 0)
                        {
                            startOfLine = startOfWord;
                            start_line = true;
                        }
                    }

                    // Whitespace.
                    if (Char.IsWhiteSpace(character))
                    {
                        last_word.Append(character);
                        multiline_string.Append(last_word);
#if XBOX || XBOX360
                        last_word.Length = 0;
#else
                        last_word.Clear();
#endif
                        start_word = false;
                        startOfWord = -1;
                        i++;
                        continue;
                    }

                    // Out of bounds.
                    if (GetLetterPosXRight(characterSprite) - startOfLine > m_fWidth)
                    {
                        if (!m_bLineBreakWithoutSpaces)
                        {
                            last_word.Append(character);

                            int len = multiline_string.Length;
                            while (len > 0 && Char.IsWhiteSpace(multiline_string[len - 1]))
                            {
                                len--;
                                multiline_string.Remove(len, 1);
                            }

                            if (multiline_string.Length > 0)
                            {
                                multiline_string.Append('\n');
                            }

                            line++;
                            start_line = false;
                            startOfLine = -1;
                            i++;
                        }
                        else
                        {
                            int len = last_word.Length;
                            while (len > 0 && Char.IsWhiteSpace(last_word[len - 1]))
                            {
                                len--;
                                last_word.Remove(len, 1);
                            }

                            multiline_string.Append(last_word);
                            multiline_string.Append('\n');

#if XBOX || XBOX360
                            last_word.Length = 0;
#else
                        last_word.Clear();
#endif

                            start_word = false;
                            start_line = false;
                            startOfWord = -1;
                            startOfLine = -1;
                            line++;

                            if (i >= stringLength)
                                break;

                            if (startOfWord == 0)
                            {
                                startOfWord = GetLetterPosXLeft(characterSprite);
                                start_word = true;
                            }
                            if (startOfLine == 0)
                            {
                                startOfLine = startOfWord;
                                start_line = true;
                            }

                            j--;
                        }

                        continue;
                    }
                    else
                    {
                        // Character is normal.
                        last_word.Append(character);
                        i++;
                        continue;
                    }
                }

                multiline_string.Append(last_word);

                m_sString = multiline_string.ToString();

                UpdateString(true);
            }

            // Step 2: Make alignment
            if (m_pAlignment != CCTextAlignment.CCTextAlignmentLeft)
            {
                int i = 0;

                int lineNumber = 0;
                int str_len = m_sString.Length;
                var last_line = new RawList<char>();
                for (int ctr = 0; ctr <= str_len; ++ctr)
                {
                    if (ctr == str_len || m_sString[ctr] == '\n')
                    {
                        float lineWidth = 0.0f;
                        int line_length = last_line.Count;
                        // if last line is empty we must just increase lineNumber and work with next line
                        if (line_length == 0)
                        {
                            lineNumber++;
                            continue;
                        }
                        int index = i + line_length - 1 + lineNumber;
                        if (index < 0) continue;

                        var lastChar = (CCSprite) GetChildByTag(index);
                        if (lastChar == null)
                            continue;

                        lineWidth = lastChar.Position.X + lastChar.ContentSize.Width / 2.0f;

                        float shift = 0;
                        switch (m_pAlignment)
                        {
                            case CCTextAlignment.CCTextAlignmentCenter:
                                shift = ContentSize.Width / 2.0f - lineWidth / 2.0f;
                                break;
                            case CCTextAlignment.CCTextAlignmentRight:
                                shift = ContentSize.Width - lineWidth;
                                break;
                            default:
                                break;
                        }

                        if (shift != 0)
                        {
                            for (int j = 0; j < line_length; j++)
                            {
                                index = i + j + lineNumber;
                                if (index < 0) continue;

                                var characterSprite = (CCSprite) GetChildByTag(index);
                                characterSprite.Position = characterSprite.Position + new CCPoint(shift, 0.0f);
                            }
                        }

                        i += line_length;
                        lineNumber++;

                        last_line.Clear();
                        continue;
                    }

                    last_line.Add(m_sString[ctr]);
                }
            }
        }

        public void SetAlignment(CCTextAlignment alignment)
        {
            m_pAlignment = alignment;
            UpdateLabel();
        }

        public void SetWidth(float width)
        {
            m_fWidth = width;
            UpdateLabel();
        }

        public void SetLineBreakWithoutSpace(bool breakWithoutSpace)
        {
            m_bLineBreakWithoutSpaces = breakWithoutSpace;
            UpdateLabel();
        }


        private float GetLetterPosXLeft(CCSprite sp)
        {
            return sp.Position.X * m_fScaleX - (sp.ContentSize.Width * m_fScaleX * sp.AnchorPoint.X);
        }

        private float GetLetterPosXRight(CCSprite sp)
        {
            return sp.Position.X * m_fScaleX + (sp.ContentSize.Width * m_fScaleX * sp.AnchorPoint.X);
        }


        private static CCBMFontConfiguration FNTConfigLoadFile(string file)
        {
            CCBMFontConfiguration pRet = null;

            if (s_pConfigurations == null)
            {
                s_pConfigurations = new Dictionary<string, CCBMFontConfiguration>();
            }

            if (!s_pConfigurations.Keys.Contains(file))
            {
                pRet = CCBMFontConfiguration.Create(file);
                s_pConfigurations.Add(file, pRet);
            }
            else
            {
                pRet = s_pConfigurations[file];
            }

            return pRet;
        }
    }
}