using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public class CCLabelBMFont : CCSpriteBatchNode, ICCLabelProtocol, ICCRGBAProtocol
    {
        public const int kCCLabelAutomaticWidth = -1;
        public static Dictionary<string, CCBMFontConfiguration> s_pConfigurations;
        private bool m_bLineBreakWithoutSpaces;
        private float m_fWidth = -1.0f;
        private CCTextAlignment m_pAlignment = CCTextAlignment.Center;
        protected CCBMFontConfiguration m_pConfiguration;
        private string m_sFntFile;
        private string m_sInitialString;
        protected string m_sString = "";
        private CCPoint m_tImageOffset;
        private CCSprite m_pReusedChar;

        public override CCPoint AnchorPoint
        {
            get { return base.AnchorPoint; }
            set
            {
                if (!m_obAnchorPoint.Equals(value))
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

        public virtual string Text
        {
            get { return m_sInitialString; }
            set 
            {
                SetString(value, false);
            }
        }

        [Obsolete("Use Label Property")]
        public void SetString(string label)
        {
            Text = label;
        }
        
        [Obsolete("Use Label Property")]
        public string GetString() 
        {
            return Text;
        }

        #endregion

        #region ICCRGBAProtocol Members

        protected byte m_cDisplayedOpacity = 255;
        protected byte m_cRealOpacity = 255;
        protected CCColor3B m_tDisplayedColor = CCTypes.CCWhite;
        protected CCColor3B m_tRealColor = CCTypes.CCWhite;
        protected bool m_bCascadeColorEnabled = true;
        protected bool m_bCascadeOpacityEnabled = true;
        protected bool m_bIsOpacityModifyRGB = false;

        public virtual CCColor3B Color
        {
            get { return m_tRealColor; }
            set
            {
                m_tDisplayedColor = m_tRealColor = value;

                if (m_bCascadeColorEnabled)
                {
                    var parentColor = CCTypes.CCWhite;
                    var parent = m_pParent as ICCRGBAProtocol;
                    if (parent != null && parent.CascadeColorEnabled)
                    {
                        parentColor = parent.DisplayedColor;
                    }

                    UpdateDisplayedColor(parentColor);
                }
            }
        }

        public virtual CCColor3B DisplayedColor
        {
            get { return m_tDisplayedColor; }
        }

        public virtual byte Opacity
        {
            get { return m_cRealOpacity; }
            set
            {
                m_cDisplayedOpacity = m_cRealOpacity = value;

                if (m_bCascadeOpacityEnabled)
                {
                    byte parentOpacity = 255;
                    var pParent = m_pParent as ICCRGBAProtocol;
                    if (pParent != null && pParent.CascadeOpacityEnabled)
                    {
                        parentOpacity = pParent.DisplayedOpacity;
                    }
                    UpdateDisplayedOpacity(parentOpacity);
                }
            }
        }

        public virtual byte DisplayedOpacity
        {
            get { return m_cDisplayedOpacity; }
        }

        public virtual bool IsOpacityModifyRGB
        {
            get { return m_bIsOpacityModifyRGB; }
            set
            {
                m_bIsOpacityModifyRGB = value;
                if (m_pChildren != null && m_pChildren.count > 0)
                {
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
                    {
                        var item = m_pChildren.Elements[i] as ICCRGBAProtocol;
                        if (item != null)
                        {
                            item.IsOpacityModifyRGB = value;
                        }
                    }
                }
            }
        }
                            
        public virtual bool CascadeColorEnabled
        {
            get { return false; }
            set { m_bCascadeColorEnabled = value; }
        }

        public virtual bool CascadeOpacityEnabled
        {
            get { return false; }
            set { m_bCascadeOpacityEnabled = value; }
        }

        public virtual void UpdateDisplayedColor(CCColor3B parentColor)
        {
            m_tDisplayedColor.R = (byte) (m_tRealColor.R * parentColor.R / 255.0f);
            m_tDisplayedColor.G = (byte) (m_tRealColor.G * parentColor.G / 255.0f);
            m_tDisplayedColor.B = (byte) (m_tRealColor.B * parentColor.B / 255.0f);

            if (m_pChildren != null)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    ((CCSprite) m_pChildren.Elements[i]).UpdateDisplayedColor(m_tDisplayedColor);
                }
            }
        }

        public virtual void UpdateDisplayedOpacity(byte parentOpacity)
        {
            m_cDisplayedOpacity = (byte) (m_cRealOpacity * parentOpacity / 255.0f);

            if (m_pChildren != null)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    ((CCSprite)m_pChildren.Elements[i]).UpdateDisplayedOpacity(m_cDisplayedOpacity);
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

        public CCLabelBMFont()
        {
            Init();
        }

        public CCLabelBMFont(string str, string fntFile, float width) : this(str, fntFile, width, CCTextAlignment.Left, CCPoint.Zero)
        {
        }

        public CCLabelBMFont(string str, string fntFile)
            : this(str, fntFile, kCCLabelAutomaticWidth, CCTextAlignment.Left, CCPoint.Zero)
        {
        }

        public CCLabelBMFont(string str, string fntFile, float width, CCTextAlignment alignment)
            : this(str, fntFile, width, alignment, CCPoint.Zero)
        {
        }

        public CCLabelBMFont(string str, string fntFile, float width, CCTextAlignment alignment, CCPoint imageOffset)
        {
            InitWithString(str, fntFile, width, alignment, imageOffset);
        }

        public override bool Init()
        {
            return InitWithString(null, null, kCCLabelAutomaticWidth, CCTextAlignment.Left, CCPoint.Zero);
        }

        protected virtual bool InitWithString(string theString, string fntFile, float width, CCTextAlignment alignment, CCPoint imageOffset)
        {
            Debug.Assert(m_pConfiguration == null, "re-init is no longer supported");
            Debug.Assert((theString == null && fntFile == null) || (theString != null && fntFile != null), "Invalid params for CCLabelBMFont");

            CCTexture2D texture;

            if (!String.IsNullOrEmpty(fntFile))
            {
                CCBMFontConfiguration newConf = FNTConfigLoadFile(fntFile);
                if (newConf == null)
                {
                    CCLog.Log("CCLabelBMFont: Impossible to create font. Please check file: '%s'", fntFile);
                    return false;
                }

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
                m_fWidth = width;
                m_pAlignment = alignment;
                
                m_cDisplayedOpacity = m_cRealOpacity = 255;
                m_tDisplayedColor = m_tRealColor = CCTypes.CCWhite;
                m_bCascadeOpacityEnabled = true;
                m_bCascadeColorEnabled = true;

                m_obContentSize = CCSize.Zero;

                m_bIsOpacityModifyRGB = m_pobTextureAtlas.Texture.HasPremultipliedAlpha;
                AnchorPoint = new CCPoint(0.5f, 0.5f);

                m_tImageOffset = imageOffset;

                m_pReusedChar = new CCSprite();
                m_pReusedChar.InitWithTexture(m_pobTextureAtlas.Texture, CCRect.Zero, false);
                m_pReusedChar.BatchNode = this;

                SetString(theString, true);

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
                CCBMFontConfiguration.CCKerningHashElement element;
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
            char prev = (char)255;
            int kerningAmount = 0;

            CCSize tmpSize = CCSize.Zero;

            int longestLine = 0;
            int totalHeight = 0;

            int quantityOfLines = 1;

            if (String.IsNullOrEmpty(m_sString))
            {
                return;
            }

            int stringLen = m_sString.Length;

            var charSet = m_pConfiguration.CharacterSet;

            for (int i = 0; i < stringLen - 1; ++i)
            {
                if (m_sString[i] == '\n')
                {
                    quantityOfLines++;
                }
            }

            totalHeight = m_pConfiguration.m_nCommonHeight * quantityOfLines;
            nextFontPositionY = 0 - (m_pConfiguration.m_nCommonHeight - m_pConfiguration.m_nCommonHeight * quantityOfLines);

            CCBMFontConfiguration.CCBMFontDef fontDef = null;
            CCRect rect;

            for (int i = 0; i < stringLen; i++)
            {
                char c = m_sString[i];

                if (c == '\n')
                {
                    nextFontPositionX = 0;
                    nextFontPositionY -= m_pConfiguration.m_nCommonHeight;
                    continue;
                }

                if (charSet.IndexOf(c) == -1)
                {
                    CCLog.Log("Cocos2D.CCLabelBMFont: Attempted to use character not defined in this bitmap: %d", (int)c);
                    continue;
                }

                kerningAmount = this.KerningAmountForFirst(prev, c);

                // unichar is a short, and an int is needed on HASH_FIND_INT
                if (!m_pConfiguration.m_pFontDefDictionary.TryGetValue(c, out fontDef))
                {
                    CCLog.Log("cocos2d::CCLabelBMFont: characer not found %d", (int)c);
                    continue;
                }

                rect = fontDef.rect;
                rect = CCMacros.CCRectanglePixelsToPoints(rect);

                rect.Origin.X += m_tImageOffset.X;
                rect.Origin.Y += m_tImageOffset.Y;

                CCSprite fontChar;

                //bool hasSprite = true;
                fontChar = (CCSprite) (GetChildByTag(i));
                if (fontChar != null)
                {
                    // Reusing previous Sprite
		        	fontChar.Visible = true;
                }
                else
                {
                    // New Sprite ? Set correct color, opacity, etc...
                    //if( false )
                    //{
				    //    /* WIP: Doesn't support many features yet.
				    //     But this code is super fast. It doesn't create any sprite.
				    //     Ideal for big labels.
				    //     */
				    //    fontChar = m_pReusedChar;
				    //    fontChar.BatchNode = null;
				    //    hasSprite = false;
			        //}
                    //else
                    {
                        fontChar = new CCSprite();
                        fontChar.InitWithTexture(m_pobTextureAtlas.Texture, rect);
                        AddChild(fontChar, i, i);
			        }
            
                    // Apply label properties
			        fontChar.IsOpacityModifyRGB = m_bIsOpacityModifyRGB;
            
			        // Color MUST be set before opacity, since opacity might change color if OpacityModifyRGB is on
			        fontChar.UpdateDisplayedColor(m_tDisplayedColor);
			        fontChar.UpdateDisplayedOpacity(m_cDisplayedOpacity);
                }

                // updating previous sprite
                fontChar.SetTextureRect(rect, false, rect.Size);

                // See issue 1343. cast( signed short + unsigned integer ) == unsigned integer (sign is lost!)
                int yOffset = m_pConfiguration.m_nCommonHeight - fontDef.yOffset;
                var fontPos = new CCPoint((float) nextFontPositionX + fontDef.xOffset + fontDef.rect.Size.Width * 0.5f + kerningAmount,
                                          (float) nextFontPositionY + yOffset - rect.Size.Height * 0.5f * CCMacros.CCContentScaleFactor());
                fontChar.Position = CCMacros.CCPointPixelsToPoints(fontPos);

                // update kerning
                nextFontPositionX += fontDef.xAdvance + kerningAmount;
                prev = c;

                if (longestLine < nextFontPositionX)
                {
                    longestLine = nextFontPositionX;
                }
        
                //if (! hasSprite)
                //{
                //  UpdateQuadFromSprite(fontChar, i);
                //}
            }

            // If the last character processed has an xAdvance which is less that the width of the characters image, then we need
            // to adjust the width of the string to take this into account, or the character will overlap the end of the bounding
            // box
            if (fontDef.xAdvance < fontDef.rect.Size.Width)
            {
                tmpSize.Width = longestLine + fontDef.rect.Size.Width - fontDef.xAdvance;
            }
            else
            {
                tmpSize.Width = longestLine;
            }
            tmpSize.Height = totalHeight;

            ContentSize = CCMacros.CCSizePixelsToPoints(tmpSize);
        }


        public virtual void SetString(string newString, bool needUpdateLabel)
        {
            if (!needUpdateLabel)
            {
                m_sString = newString;
            }
            else
            {
                m_sInitialString = newString;
            }

            UpdateString(needUpdateLabel);
        }

        private void UpdateString(bool needUpdateLabel)
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

            if (needUpdateLabel)
            {
                UpdateLabel();
            }
        }

        protected void UpdateLabel()
        {
            SetString(m_sInitialString, false);

            if (m_sString == null)
            {
                return;
            }
            if (m_fWidth > 0)
            {
                // Step 1: Make multiline
                string str_whole = m_sString;
                int stringLength = str_whole.Length;
                var multiline_string = new StringBuilder(stringLength);
                var last_word = new StringBuilder(stringLength);

                int line = 1, i = 0;
                bool start_line = false, start_word = false;
                float startOfLine = -1, startOfWord = -1;
                int skip = 0;

                CCRawList<CCNode> children = m_pChildren;
                for (int j = 0; j < children.count; j++)
                {
                    CCSprite characterSprite;
                    int justSkipped = 0;

                    while ((characterSprite = (CCSprite)GetChildByTag(j + skip + justSkipped)) == null)
                    {
                        justSkipped++;
                    }

                    skip += justSkipped;

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
                        i += justSkipped; 
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

                SetString(multiline_string.ToString(), false);
            }

            // Step 2: Make alignment
            if (m_pAlignment != CCTextAlignment.Left)
            {
                int i = 0;

                int lineNumber = 0;
                int str_len = m_sString.Length;
                var last_line = new CCRawList<char>();
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
                            case CCTextAlignment.Center:
                                shift = ContentSize.Width / 2.0f - lineWidth / 2.0f;
                                break;
                            case CCTextAlignment.Right:
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