using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CocosSharp
{
    public class CCLabelBMFont : CCSpriteBatchNode, ICCTextContainer, ICCColor
    {
        public const int AutomaticWidth = -1;

        internal static Dictionary<string, CCBMFontConfiguration> fontConfigurations = new Dictionary<string, CCBMFontConfiguration>();

        protected bool lineBreakWithoutSpaces;
        protected CCTextAlignment horzAlignment = CCTextAlignment.Center;
        protected CCVerticalTextAlignment vertAlignment = CCVerticalTextAlignment.Top;
        internal CCBMFontConfiguration FontConfiguration { get; set; }
        protected string fntConfigFile;
        protected string labelInitialText;
        protected string labelText = string.Empty;
        protected CCPoint ImageOffset { get; set; }
        protected CCSize labelDimensions;
        protected CCSprite m_pReusedChar;
        protected bool IsDirty { get; set; }

        protected byte displayedOpacity = 255;
        protected byte realOpacity = 255;
        protected CCColor3B displayedColor = CCColor3B.White;
        protected CCColor3B realColor = CCColor3B.White;
        protected bool isColorCascaded = true;
        protected bool isOpacityCascaded = true;
        protected bool isColorModifiedByOpacity = false;


        public override CCPoint AnchorPoint
        {
            get { return base.AnchorPoint; }
            set
            {
                if (!AnchorPoint.Equals(value))
                {
                    base.AnchorPoint = value;
                    IsDirty = true;
                }
            }
        }

        public override float Scale
        {
            set
            {
                if (!value.Equals(base.ScaleX) || !value.Equals(base.ScaleY)) 
                {
                    base.Scale = value;
                    IsDirty = true;
                }
            }
        }

        public override float ScaleX
        {
            get { return base.ScaleX; }
            set
            {
                if (!value.Equals(base.ScaleX)) 
                {
                    base.ScaleX = value;
                    IsDirty = true;
                }
            }
        }

        public override float ScaleY
        {
            get { return base.ScaleY; }
            set
            {
                if (!value.Equals(base.ScaleY)) 
                {
                    base.ScaleY = value;
                    IsDirty = true;
                }
            }
        }

        public CCTextAlignment HorizontalAlignment
        {
            get { return horzAlignment; }
            set
            {
                if (horzAlignment != value)
                {
                    horzAlignment = value;
                    IsDirty = true;
                }
            }
        }

        public CCVerticalTextAlignment VerticalAlignment
        {
            get { return vertAlignment; }
            set
            {
                if (vertAlignment != value)
                {
                    vertAlignment = value;
                    IsDirty = true;
                }
            }
        }

        public CCSize Dimensions
        {
            get { return labelDimensions; }
            set
            {
                if (labelDimensions != value)
                {
                    labelDimensions = value;
                    IsDirty = true;
                }
            }
        }

        public bool LineBreakWithoutSpace
        {
            get { return lineBreakWithoutSpaces; }
            set
            {
                lineBreakWithoutSpaces = value;
                IsDirty = true;
            }
        }

        public string FntFile
        {
            get { return fntConfigFile; }
            set
            {
                if (value != null && fntConfigFile != value)
                {
                    CCBMFontConfiguration newConf = FNTConfigLoadFile(value);

                    Debug.Assert(newConf != null, "CCLabelBMFont: Impossible to create font. Please check file");

                    fntConfigFile = value;

                    FontConfiguration = newConf;

                    Texture = CCApplication.SharedApplication.TextureCache.AddImage(FontConfiguration.AtlasName);

                    IsDirty = true;
                }
            }
        }

        public virtual string Text
        {
            get { return labelInitialText; }
            set
            {
                if (labelInitialText != value)
                {
                    labelInitialText = value;
                    IsDirty = true;
                }
            }
        }


        #region ICCRGBAProtocol properties

        public virtual CCColor3B Color
        {
            get { return realColor; }
            set
            {
                displayedColor = realColor = value;

                if (isColorCascaded)
                {
                    var parentColor = CCColor3B.White;
                    var parent = Parent as ICCColor;
                    if (parent != null && parent.IsColorCascaded)
                    {
                        parentColor = parent.DisplayedColor;
                    }

                    UpdateDisplayedColor(parentColor);
                }
            }
        }

        public virtual CCColor3B DisplayedColor
        {
            get { return displayedColor; }
        }

        public virtual byte Opacity
        {
            get { return realOpacity; }
            set
            {
                displayedOpacity = realOpacity = value;

                if (isOpacityCascaded)
                {
                    byte parentOpacity = 255;
                    var pParent = Parent as ICCColor;
                    if (pParent != null && pParent.IsOpacityCascaded)
                    {
                        parentOpacity = pParent.DisplayedOpacity;
                    }
                    UpdateDisplayedOpacity(parentOpacity);
                }
            }
        }

        public virtual byte DisplayedOpacity
        {
            get { return displayedOpacity; }
        }

        public virtual bool IsColorModifiedByOpacity
        {
            get { return isColorModifiedByOpacity; }
            set
            {
                isColorModifiedByOpacity = value;
                if (Children != null && Children.Count > 0)
                {
                    for (int i = 0, count = Children.Count; i < count; i++)
                    {
                        var item = Children.Elements[i] as ICCColor;
                        if (item != null)
                        {
                            item.IsColorModifiedByOpacity = value;
                        }
                    }
                }
            }
        }

        public virtual bool IsColorCascaded
        {
            get { return false; }
            set { isColorCascaded = value; }
        }

        public virtual bool IsOpacityCascaded
        {
            get { return false; }
            set { isOpacityCascaded = value; }
        }

        #endregion


        public static void PurgeCachedData()
        {
            if (fontConfigurations != null)
            {
                fontConfigurations.Clear();
            }
        }


        #region Constructors

        public CCLabelBMFont() : this("", "")
        {
        }

        public CCLabelBMFont(string str, string fntFile)
            : this(str, fntFile, 0.0f)
        {
        }

        public CCLabelBMFont(string str, string fntFile, float width)
            : this(str, fntFile, width, CCTextAlignment.Left)
        {
        }

        public CCLabelBMFont(string str, string fntFile, float width, CCTextAlignment alignment)
            : this(str, fntFile, width, alignment, CCPoint.Zero)
        {
        }

        public CCLabelBMFont(string str, string fntFile, float width, CCTextAlignment alignment, CCPoint imageOffset) 
            : this(str, fntFile, width, alignment, imageOffset, null)
        {
        }

        public CCLabelBMFont(string str, string fntFile, float width, CCTextAlignment alignment, CCPoint imageOffset, CCTexture2D texture)
            : this(str, fntFile, width, alignment, CCVerticalTextAlignment.Top, imageOffset, null)
        {
        }

        public CCLabelBMFont(string str, string fntFile, float width, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, 
            CCPoint imageOffset, CCTexture2D texture)
            : this(str, fntFile, new CCSize(width, 0), hAlignment, vAlignment, imageOffset, texture)
        {
        }

        public CCLabelBMFont(string str, string fntFile, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, 
            CCPoint imageOffset, CCTexture2D texture)
        {
            InitCCLabelBMFont(str, fntFile, dimensions, hAlignment, vAlignment, imageOffset, texture);
        }

        protected void InitCCLabelBMFont(string theString, string fntFile, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, 
            CCPoint imageOffset, CCTexture2D texture)
        {
            Debug.Assert(FontConfiguration == null, "re-init is no longer supported");
            Debug.Assert((theString == null && fntFile == null) || (theString != null && fntFile != null),
                "Invalid params for CCLabelBMFont");

            if (!String.IsNullOrEmpty(fntFile))
            {
                CCBMFontConfiguration newConf = FNTConfigLoadFile(fntFile);
                if (newConf == null)
                {
                    CCLog.Log("CCLabelBMFont: Impossible to create font. Please check file: '{0}'", fntFile);
                    return;
                }

                FontConfiguration = newConf;

                fntConfigFile = fntFile;

                if (texture == null)
                {
                    try
                    {
                        texture = CCApplication.SharedApplication.TextureCache.AddImage(FontConfiguration.AtlasName);
                    }
                    catch (Exception)
                    {
                        // Try the 'images' ref location just in case.
                        try
                        {
                            texture =
                                CCApplication.SharedApplication.TextureCache.AddImage(System.IO.Path.Combine("images",
                                    FontConfiguration
                                    .AtlasName));
                        }
                        catch (Exception)
                        {
                            // Lastly, try <font_path>/images/<font_name>
                            string dir = System.IO.Path.GetDirectoryName(FontConfiguration.AtlasName);
                            string fname = System.IO.Path.GetFileName(FontConfiguration.AtlasName);
                            string newName = System.IO.Path.Combine(System.IO.Path.Combine(dir, "images"), fname);
                            texture = CCApplication.SharedApplication.TextureCache.AddImage(newName);
                        }
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

            base.InitCCSpriteBatchNode(texture, theString.Length);

            this.labelDimensions = dimensions;

            horzAlignment = hAlignment;
            vertAlignment = vAlignment;

            displayedOpacity = realOpacity = 255;
            displayedColor = realColor = CCColor3B.White;
            isOpacityCascaded = true;
            isColorCascaded = true;

            ContentSize = CCSize.Zero;

            isColorModifiedByOpacity = TextureAtlas.Texture.HasPremultipliedAlpha;
            AnchorPoint = new CCPoint(0.5f, 0.5f);

            ImageOffset = imageOffset;

            m_pReusedChar = new CCSprite();
            m_pReusedChar.InitWithTexture(TextureAtlas.Texture, CCRect.Zero, false);
            m_pReusedChar.BatchNode = this;

            SetString(theString, true);
        }

        #endregion Constructors


        public virtual void UpdateDisplayedColor(CCColor3B parentColor)
        {
            displayedColor.R = (byte) (realColor.R * parentColor.R / 255.0f);
            displayedColor.G = (byte) (realColor.G * parentColor.G / 255.0f);
            displayedColor.B = (byte) (realColor.B * parentColor.B / 255.0f);

            if (Children != null)
            {
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    ((CCSprite) Children.Elements[i]).UpdateDisplayedColor(displayedColor);
                }
            }
        }

        public virtual void UpdateDisplayedOpacity(byte parentOpacity)
        {
            displayedOpacity = (byte) (realOpacity * parentOpacity / 255.0f);

            if (Children != null)
            {
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    ((CCSprite) Children.Elements[i]).UpdateDisplayedOpacity(displayedOpacity);
                }
            }
        }

        private int KerningAmountForFirst(int first, int second)
        {
            int ret = 0;
            int key = (first << 16) | (second & 0xffff);

            if (FontConfiguration.GlyphKernings != null)
            {
                CCBMFontConfiguration.CCKerningHashElement element;
                if (FontConfiguration.GlyphKernings.TryGetValue(key, out element))
                {
                    ret = element.Amount;
                }
            }
            return ret;
        }

        public void CreateFontChars()
        {
            int nextFontPositionX = 0;
            int nextFontPositionY = 0;
            char prev = (char) 255;
            int kerningAmount = 0;

            CCSize tmpSize = CCSize.Zero;

            int longestLine = 0;
            int totalHeight = 0;

            int quantityOfLines = 1;

            if (String.IsNullOrEmpty(labelText))
            {
                return;
            }

            int stringLen = labelText.Length;

            var charSet = FontConfiguration.CharacterSet;
            if (charSet.Count == 0)
            {
                throw (new InvalidOperationException(
                    "Can not compute the size of the font because the character set is empty."));
            }

            for (int i = 0; i < stringLen - 1; ++i)
            {
                if (labelText[i] == '\n')
                {
                    quantityOfLines++;
                }
            }

            var commonHeight = FontConfiguration.CommonHeight;

            totalHeight = commonHeight * quantityOfLines;
            nextFontPositionY = 0 -
                (commonHeight - commonHeight * quantityOfLines);

            CCBMFontConfiguration.CCBMGlyphDef fontDef = null;
            CCRect rect;

            for (int i = 0; i < stringLen; i++)
            {
                char c = labelText[i];

                if (c == '\n')
                {
                    nextFontPositionX = 0;
                    nextFontPositionY -= commonHeight;
                    continue;
                }

                if (charSet.IndexOf(c) == -1)
                {
                    CCLog.Log("CocosSharp: CCLabelBMFont: Attempted to use character not defined in this bitmap: {0}",
                        (int) c);
                    continue;
                }

                kerningAmount = this.KerningAmountForFirst(prev, c);

                // unichar is a short, and an int is needed on HASH_FIND_INT
                if (!FontConfiguration.Glyphs.TryGetValue(c, out fontDef))
                {
                    CCLog.Log("CocosSharp: CCLabelBMFont: characer not found {0}", (int) c);
                    continue;
                }

                rect = fontDef.Subrect;
				rect = rect.PixelsToPoints(Director.ContentScaleFactor);

                rect.Origin.X += ImageOffset.X;
                rect.Origin.Y += ImageOffset.Y;

                CCSprite fontChar;

                //bool hasSprite = true;
                fontChar = (CCSprite) (this[i]);
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
                        fontChar = new CCSprite(TextureAtlas.Texture, rect);
                        AddChild(fontChar, i, i);
                    }

                    // Apply label properties
                    fontChar.IsColorModifiedByOpacity = isColorModifiedByOpacity;

                    // Color MUST be set before opacity, since opacity might change color if OpacityModifyRGB is on
                    fontChar.UpdateDisplayedColor(displayedColor);
                    fontChar.UpdateDisplayedOpacity(displayedOpacity);
                }

                // updating previous sprite
                fontChar.SetTextureRect(rect, false, rect.Size);

                // See issue 1343. cast( signed short + unsigned integer ) == unsigned integer (sign is lost!)
                int yOffset = FontConfiguration.CommonHeight - fontDef.YOffset;

                var fontPos =
                    new CCPoint(
                        (float) nextFontPositionX + fontDef.XOffset + fontDef.Subrect.Size.Width * 0.5f + kerningAmount,
                        (float) nextFontPositionY + yOffset - rect.Size.Height * 0.5f * CCMacros.CCContentScaleFactor());

                fontChar.Position = fontPos.PixelsToPoints();

                // update kerning
                nextFontPositionX += fontDef.XAdvance + kerningAmount;
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
            if (fontDef.XAdvance < fontDef.Subrect.Size.Width)
            {
                tmpSize.Width = longestLine + fontDef.Subrect.Size.Width - fontDef.XAdvance;
            }
            else
            {
                tmpSize.Width = longestLine;
            }
            tmpSize.Height = totalHeight;
            var tmpDimensions = labelDimensions;

            tmpSize = new CCSize(
                tmpDimensions.Width > 0 ? tmpDimensions.Width : tmpSize.Width,
                tmpDimensions.Height > 0 ? tmpDimensions.Height : tmpSize.Height
            );

            ContentSize = tmpSize.PixelsToPoints();
        }

        public virtual void SetString(string newString, bool needUpdateLabel)
        {
            if (!needUpdateLabel)
            {
                labelText = newString;
            }
            else
            {
                labelInitialText = newString;
            }

            UpdateString(needUpdateLabel);
        }

        private void UpdateString(bool needUpdateLabel)
        {
            if (Children != null && Children.Count != 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
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
            SetString(labelInitialText, false);

            if (labelText == null)
            {
                return;
            }

            if (labelDimensions.Width > 0)
            {
                // Step 1: Make multiline
                string str_whole = labelText;
                int stringLength = str_whole.Length;
                var multiline_string = new StringBuilder(stringLength);
                var last_word = new StringBuilder(stringLength);

                int line = 1, i = 0;
                bool start_line = false, start_word = false;
                float startOfLine = -1, startOfWord = -1;
                int skip = 0;

                CCRawList<CCNode> children = Children;
                for (int j = 0; j < children.Count; j++)
                {
                    CCSprite characterSprite;
                    int justSkipped = 0;

                    while ((characterSprite = (CCSprite) this[(j + skip + justSkipped)]) == null)
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

                        last_word.Clear();

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
                        last_word.Clear();
                        start_word = false;
                        startOfWord = -1;
                        i++;
                        continue;
                    }

                    // Out of bounds.
                    if (GetLetterPosXRight(characterSprite) - startOfLine > labelDimensions.Width)
                    {
                        if (!lineBreakWithoutSpaces)
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

                            last_word.Clear();

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
            if (horzAlignment != CCTextAlignment.Left)
            {
                int i = 0;

                int lineNumber = 0;
                int str_len = labelText.Length;
                var last_line = new CCRawList<char>();
                for (int ctr = 0; ctr <= str_len; ++ctr)
                {
                    if (ctr == str_len || labelText[ctr] == '\n')
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

                        var lastChar = (CCSprite) this[index];
                        if (lastChar == null)
                            continue;

                        lineWidth = lastChar.Position.X + lastChar.ContentSize.Width / 2.0f;

                        float shift = 0;
                        switch (horzAlignment)
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

                                var characterSprite = (CCSprite) this[index];
                                characterSprite.Position = characterSprite.Position + new CCPoint(shift, 0.0f);
                            }
                        }

                        i += line_length;
                        lineNumber++;

                        last_line.Clear();
                        continue;
                    }

                    last_line.Add(labelText[ctr]);
                }
            }

            if (vertAlignment != CCVerticalTextAlignment.Bottom && labelDimensions.Height > 0)
            {
                int lineNumber = 1;
                int str_len = labelText.Length;
                for (int ctr = 0; ctr < str_len; ++ctr)
                {
                    if (labelText[ctr] == '\n')
                    {
                        lineNumber++;
                    }
                }

                float yOffset = labelDimensions.Height - FontConfiguration.CommonHeight * lineNumber;

                if (vertAlignment == CCVerticalTextAlignment.Center)
                {
                    yOffset /= 2f;
                }

                for (int i = 0; i < str_len; i++)
                {
                    var characterSprite = this[i];
                    characterSprite.PositionY += yOffset;
                }
            }
        }

        private float GetLetterPosXLeft(CCSprite sp)
        {
            return sp.Position.X * ScaleX - (sp.ContentSize.Width * ScaleX * sp.AnchorPoint.X);
        }

        private float GetLetterPosXRight(CCSprite sp)
        {
            return sp.Position.X * ScaleX + (sp.ContentSize.Width * ScaleX * sp.AnchorPoint.X);
        }


        private static CCBMFontConfiguration FNTConfigLoadFile(string file)
        {
            CCBMFontConfiguration pRet;

            if (!fontConfigurations.TryGetValue(file, out pRet))
            {
                pRet = CCBMFontConfiguration.FontConfigurationWithFile(file);
                fontConfigurations.Add(file, pRet);
            }

            return pRet;
        }

        protected override void Draw()
        {
            if (IsDirty)
            {
                UpdateLabel();
                IsDirty = false;
            }

            base.Draw();
        }
    }
}