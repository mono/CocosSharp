using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CocosSharp
{
    public class CCLabelBMFont : CCSpriteBatchNode, ICCTextContainer
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
        protected bool IsDirty { get; set; }
        protected bool RecreateSprites { get; set; }

        protected bool isColorModifiedByOpacity = false;


        // Static properties

        public new static float DefaultTexelToContentSizeRatio
        {
            set { DefaultTexelToContentSizeRatios = new CCSize(value, value); }
        }

        public new static CCSize DefaultTexelToContentSizeRatios { get; set; }


        // Instance properties

        // 2015-03-06 Expose calculated values
        public float LineHeight { get; internal set; }
        public int LineCount { get; internal set; }

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

        public override CCPoint Position
        {
            get { return base.Position; }
            set
            {
                if (base.Position != value)
                {
                    base.Position = value;
                    IsDirty = true;
                }
            }
        }

        public override float PositionX
        {
            get { return base.PositionX; }
            set
            {
                if (base.PositionX != value)
                {
                    base.PositionX = value;
                    IsDirty = true;
                }
            }
        }

        public override float PositionY
        {
            get { return base.PositionY; }
            set
            {
                if (base.PositionY != value)
                {
                    base.PositionY = value;
                    IsDirty = true;
                }
            }
        }

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }

            //[Obsolete("ContentSize is now read-only, use Dimensions instead.", true)]
            set
            {
                //Debug.Assert(false, "ContentSize is now read-only, use Dimensions instead.");
                base.ContentSize = value;
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

                    Texture = CCTextureCache.SharedTextureCache.AddImage(FontConfiguration.AtlasName);

                    IsDirty = true;
                }
            }
        }

        public virtual string Text
        {
            get { return labelText; }
            set
            {
                if (labelText != value)
                {
                    labelText = value;
                    IsDirty = true;
                    RecreateSprites = true;
                }
            }
        }

        public static void PurgeCachedData()
        {
            if (fontConfigurations != null)
            {
                fontConfigurations.Clear();
            }
        }


        #region Constructors

        static CCLabelBMFont()
        {
            DefaultTexelToContentSizeRatios = CCSize.One;
        }

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
                        texture = CCTextureCache.SharedTextureCache.AddImage(FontConfiguration.AtlasName);
                    }
                    catch (Exception)
                    {
                        // Try the 'images' ref location just in case.
                        try
                        {
                            texture =
                                CCTextureCache.SharedTextureCache.AddImage(System.IO.Path.Combine("images",
                                    FontConfiguration
                                    .AtlasName));
                        }
                        catch (Exception)
                        {
                            // Lastly, try <font_path>/images/<font_name>
                            string dir = System.IO.Path.GetDirectoryName(FontConfiguration.AtlasName);
                            string fname = System.IO.Path.GetFileName(FontConfiguration.AtlasName);
                            string newName = System.IO.Path.Combine(System.IO.Path.Combine(dir, "images"), fname);
                            texture = CCTextureCache.SharedTextureCache.AddImage(newName);
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

            IsOpacityCascaded = true;

            // Set the value in the base -- this is now read-only for CCLabelBMFont
            base.ContentSize = CCSize.Zero;

            IsColorModifiedByOpacity = TextureAtlas.Texture.HasPremultipliedAlpha;
            AnchorPoint = CCPoint.AnchorMiddle;

            ImageOffset = imageOffset;

            labelText = theString;      // At this point IsDirty RecreateSprites will be true so that the next call of Draw() will call CreateFontChars()
        }

        #endregion Constructors


        #region Scene handling

        protected override void AddedToScene()
        {
            base.AddedToScene();

            if (Scene != null)
            {
                CreateFontChars();
            }
        }

        #endregion Scene handling


        public override void UpdateColor()
        {
            base.UpdateColor();

            if (Children != null)
            {
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    ((CCSprite)Children.Elements[i]).UpdateDisplayedColor(DisplayedColor);
                }
            }

        }

        public override bool IsColorModifiedByOpacity
        {
            get { return isColorModifiedByOpacity; }
            set
            {
                if (isColorModifiedByOpacity != value)
                {
                    isColorModifiedByOpacity = value;
                    if (Children != null)
                    {
                        for (int i = 0, count = Children.Count; i < count; i++)
                        {
                            Children.Elements[i].IsColorModifiedByOpacity = isColorModifiedByOpacity;
                        }
                    }
                    UpdateColor();
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

        /// <summary>
        /// This function creates the label by creating a sprite for each character.
        /// 
        /// NOTE: rewritten 3/15/2015 (TOP 10)
        ///     - #1 optomized so that there aren't 4+ interations through the string
        ///     - #2 optomized to remove O(n) lookups for every character (4+ times) - e.g. CCBMFontConfiguration.CharacterSet (List<>)
        ///     - #3 bug fix to remove crashing bugs because of trying to use this[i] == null
        ///     - #4 bug fix to properly align horizontally -- wasn't calculating real character position
        ///     - #5 rewrote in a more readable way, forever removed skip, previousSkip, etc.
        ///     - #6 bug fix to properly handle whitespace at beginning and end of lines when doing horizontal alignment
        ///     - #7 returning the correct ContentSize once calculated
        ///     - #8 removed the second copy of labelText so there is now only 1
        ///     - #9 now work with (and support) AnchorPoint
        ///     
        ///     - and many more things which I have forgotten
        /// 
        /// NOTE2: n-spaces at the end of the line are basically ignored/removed if they take offset past the end of a line that needs to be split
        /// 
        /// NOTE3: There are some special cases which aren't handled consistently like what happens if the string starts with a bunch of blank spaces?
        ///        It will different than if there are a string of blank spaces at the end of a line that is automatically broken.  Maybe this is as expected?
        /// 
        /// QUESTION: Would it make sense to Trim() the label before using or do you want to be able to add blank lines at the end?
        ///           For now you can add blank lines anywhere.
        /// </summary>
        public void CreateFontChars()
        {
            // Clear out all the sprites if (for example) we are changing the string
            if (RecreateSprites)
            {
                RemoveAllChildren(true);
                RecreateSprites = false;
            }

            // Values that we will calculate
            base.ContentSize = CCSize.Zero;

            // Quit if there's nothing to build
            if (string.IsNullOrEmpty(labelText))
            {
                LineHeight = LineCount = 0;
                return;
            }

            // Quit if no characters have been created
            if (FontConfiguration.Glyphs.Count <= 0)
            {
                throw (new InvalidOperationException("Can not compute the size of the font because the character set is empty."));
            }

            // General calculations
            LineHeight = FontConfiguration.CommonHeight;
            LineCount = 1;
            float fLongestLine = 0;
            float fMaxWidth = Dimensions.Width > 0 ? Dimensions.Width : float.MaxValue;
            Dictionary<int, int> phashLastIndex = new Dictionary<int, int>();

            // PASS#1 - go through each character of the label and create sprites for each character, splitting the line if necessary
            {
                CCPoint ptCurrent = CCPoint.Zero;
                char chPrev = (char)255;
                int nFirstIndexOfCurrentWord = -1;        // The index of first printable character of the current word (this is where we might possibly break and move to next line)
                int nLastIndexOfLastWord = -1;            // The index of the last printable (non-whitespace) character of the previous word (this might become the end of the line)
                int nLastIndexOfCurrentWord = -1;         // The last index of a printable character -- this is not simply i-1 because of NULLs in strings with characters that can't be displayed
                CCSprite pSpriteForChar = null;
                CCBMFontConfiguration.CCBMGlyphDef pCurrentGlyphDef = null;

                for (int i = 0; i <= labelText.Length; i++)  // NOTE: string.Length is O(1) so there's no need to cache the value - see http://stackoverflow.com/questions/2836223/what-order-of-time-does-the-net-system-string-length-property-take
                {
                    // Special case when we're done -- put here because we don't have access to 'i' outside the loop // and don't want to count blanks at the end of the line
                    if (i == labelText.Length)
                    {
                        // EOL#1 - used up all the characters
                        if (nLastIndexOfCurrentWord >= 0)
                        {
                            // Figure out the last non-whitespace character considering that we might have ended with a bunch of spaces
                            int nIndexTmp = Math.Max(nLastIndexOfCurrentWord, nLastIndexOfLastWord);

                            phashLastIndex[LineCount - 1] = nIndexTmp;

                            // Possibly bump up the longest line [EOL#1]
                            if (nIndexTmp >= 0 && this[nIndexTmp] != null && this[nIndexTmp].BoundingBox.MaxX > fLongestLine)
                                fLongestLine = this[nIndexTmp].BoundingBox.MaxX;
                        }
                        break;
                    }

                    char chCurrent = labelText[i];

                    // Skip to the next line?     [EOL#2 - hard carriage return]
                    if (chCurrent == '\n')
                    {
                        // Figure out the last non-whitespace character considering that we might have ended with a bunch of spaces
                        int nIndexTmp = Math.Max(nLastIndexOfCurrentWord, nLastIndexOfLastWord);

                        // Store line length
                        phashLastIndex[LineCount - 1] = nIndexTmp;    // NOTE: might be -1 for a line of all whitespace

                        // Possibly bump up the longest line [EOL#2]
                        if (nIndexTmp >= 0 && this[nIndexTmp] != null && this[nIndexTmp].BoundingBox.MaxX > fLongestLine)
                            fLongestLine = this[nIndexTmp].BoundingBox.MaxX;

                        // If this isn't the end of the entire string then add to the line count
                        if (i < labelText.Length - 1)
                        {
                            LineCount++;

                            // Move the pointers on to the next line and reset x/prev
                            ptCurrent.Y -= LineHeight;
                            ptCurrent.X = 0;
                            chPrev = (char)255;
                            nFirstIndexOfCurrentWord = nLastIndexOfCurrentWord = -1;  // Haven't found the beginning or end of the next word yet
                        }
                        // No need to loop through again we know that we're done
                        else
                            break;

                        continue;
                    }

                    // Skip if we don't have a way of drawing this character
                    if (!FontConfiguration.Glyphs.TryGetValue(chCurrent, out pCurrentGlyphDef))
                    {
                        CCLog.Log("CocosSharp: CCLabelBMFont: characer not found {0}", (int)chCurrent);
                        continue;
                    }

                    // Make some calculations for this character to be
                    int nKerningAmount = this.KerningAmountForFirst(chPrev, chCurrent);
                    CCRect rectCharInTexture = pCurrentGlyphDef.Subrect;   // << the bitmap rect for this chacter from the sprite sheet
                    rectCharInTexture.Origin.X += ImageOffset.X;
                    rectCharInTexture.Origin.Y += ImageOffset.Y;
                    CCSize rectSpriteContentSize = rectCharInTexture.Size / DefaultTexelToContentSizeRatios;

                    // End the previous word if this is a whitespace character
                    if (char.IsWhiteSpace(chCurrent))
                    {
                        // If a word was previously started then we've found the end
                        if (nLastIndexOfCurrentWord >= 0)
                            nLastIndexOfLastWord = nLastIndexOfCurrentWord;

                        // Either way -- this character does not begin (or end) a word -- i.e. no current word
                        nFirstIndexOfCurrentWord = nLastIndexOfCurrentWord = -1;
                    }
                    // OTHERWISE move on to add some non-whitespace character
                    else
                    {
                        nLastIndexOfCurrentWord = i;

                        // Is this is the beginning of the next word (if not set yet)
                        if (nFirstIndexOfCurrentWord < 0)
                            nFirstIndexOfCurrentWord = i;

                        // Have we already created this sprite -- if so, update size and position
                        pSpriteForChar = (CCSprite)(this[i]);
                        if (pSpriteForChar != null)
                        {
                            // Reusing previous Sprite
                            pSpriteForChar.Visible = true;

                            // updating previous sprite
                            pSpriteForChar.IsTextureRectRotated = false;
                            pSpriteForChar.ContentSize = rectSpriteContentSize;
                            pSpriteForChar.TextureRectInPixels = rectCharInTexture;
                        }
                        // Otherwise create a new one
                        else
                        {
                            pSpriteForChar = new CCSprite(TextureAtlas.Texture, rectCharInTexture); // NOTE: implies AnchorPoint = CCPoint.AnchorMiddle because of CCSprint.InitWithTexture()
                            pSpriteForChar.ContentSize = rectSpriteContentSize;
                            AddChild(pSpriteForChar, ZOrder, i);

                            // Apply label properties
                            pSpriteForChar.IsColorModifiedByOpacity = IsColorModifiedByOpacity;

                            // Color MUST be set before opacity, since opacity might change color if OpacityModifyRGB is on
                            pSpriteForChar.UpdateDisplayedColor(DisplayedColor);
                            pSpriteForChar.UpdateDisplayedOpacity(DisplayedOpacity);
                        }

                        // Don't go past the end of the line [EOL#3 - break the line in order to fit Dimension.Width]
                        if (ptCurrent.X + pCurrentGlyphDef.XOffset + pCurrentGlyphDef.Subrect.Size.Width + nKerningAmount > fMaxWidth)
                        {
                            // Move onto the next line
                            ptCurrent.X = 0;
                            ptCurrent.Y -= LineHeight;

                            // If breaking in the middle of a word OR if this is the only word on the line
                            if (LineBreakWithoutSpace || nLastIndexOfLastWord < 0)
                            {
                                // Nothing previous to use for calculating kerning
                                nKerningAmount = this.KerningAmountForFirst(255, chCurrent);

                                // Special case of calculating the last index when the character that will bump us past the length is proceeded by whitespace.
                                int nIndexTmp = i - 1;
                                if (this[nIndexTmp] == null)
                                    nIndexTmp = nLastIndexOfLastWord;

                                // Store the index where the previous line ended -- it will break just before this character because we break in the middle of the word
                                //      NOTE: this[i-1] could possibly represent whitespace
                                phashLastIndex[LineCount - 1] = nIndexTmp;

                                // Possibly bump up the longest line [EOL#3a]
                                if (nIndexTmp >= 0 && this[nIndexTmp] != null && this[nIndexTmp].BoundingBox.MaxX > fLongestLine)
                                    fLongestLine = this[nIndexTmp].BoundingBox.MaxX;

                                // nLastIndexOfCurrentWord stays the same but this is now the start of a the current word as well
                                nFirstIndexOfCurrentWord = i;
                            }
                            // Otherwise we will need to move the remains of the current word down (if breaking at spaces)
                            else
                            {
                                // Move the currently started word down
                                RepositionSprites(nFirstIndexOfCurrentWord, i - nFirstIndexOfCurrentWord, ref ptCurrent);

                                // This line ended with the beginning of the last word
                                //    NOTE: that there must be a last word or we would have previously split the full word
                                phashLastIndex[LineCount - 1] = nLastIndexOfLastWord;

                                // Possibly bump up the longest line [EOL#3b]
                                if (nLastIndexOfLastWord >= 0 && this[nLastIndexOfLastWord] != null && this[nLastIndexOfLastWord].BoundingBox.MaxX > fLongestLine)
                                    fLongestLine = this[nLastIndexOfLastWord].BoundingBox.MaxX;
                            }

                            // As of now there is no last word because we're on a new line
                            nLastIndexOfLastWord = -1;
                            LineCount++;
                        }

                        pSpriteForChar.Position =
                            new CCPoint(
                                (float)ptCurrent.X + pCurrentGlyphDef.XOffset + (pCurrentGlyphDef.Subrect.Size.Width * 0.5f) + nKerningAmount,
                                (float)ptCurrent.Y + pCurrentGlyphDef.YOffset);
                    }

                    // Advance the current position, applying kerning
                    ptCurrent.X += pCurrentGlyphDef.XAdvance + nKerningAmount;
                    chPrev = chCurrent;
                }

                // NOTE: This causes all the children to be touched and called with ParentUpdatedTransform() from UpdateTransform().
                //       If this is not necessary then there should be a direct method to change the contentSize
                base.ContentSize = new CCSize(fLongestLine, LineCount * LineHeight);
            }

            // PASS #2 - now go through all the characters and align (horizontally and vertically)
            {
                // Anchor shift plus vertical alignment shift
                CCPoint ptShiftAnchorPlusVertical = AnchorPoint * new CCPoint(ContentSize.Width - Dimensions.Width, ContentSize.Height - Dimensions.Height);
                //ptShiftAnchorPlusVertical += new CCPoint(ContentSize.Width / 2f, ContentSize.Height / 2f);
                
                // Always move down a half line to make up for characters being center anchored -- this normalizes everything to ZERO
                ptShiftAnchorPlusVertical.Y -= (LineHeight / 2);

                // Vertical shift calculation
                if (vertAlignment == CCVerticalTextAlignment.Top)
                {
                    // Move to the top of the rectangle and subtract one half line because we want the line inside
                    ptShiftAnchorPlusVertical.Y += Dimensions.Height;
                }
                else if (vertAlignment == CCVerticalTextAlignment.Center)
                {
                    // Center
                    ptShiftAnchorPlusVertical.Y += (Dimensions.Height + ContentSize.Height) / 2f;
                }
                else // if (vertAlignment == CCVerticalTextAlignment.Bottom)
                {
                    // Push the text up into the box
                    ptShiftAnchorPlusVertical.Y += ContentSize.Height;
                }

                int nFirstIndexOfLine = 0;

                // Go through each line
                for (int i = 0; i < LineCount; i++)
                {
                    int nLastIndexOfLine = phashLastIndex[i];

                    // Skip empty lines
                    if (nLastIndexOfLine < 0)
                        continue;

                    // Calculate the width of this line and the amount to shift (if necessary)
                    // by using the bounding box of the last valid (non-blank) character on the line
                    CCSprite pSpriteTmp;
                    for (int j = nLastIndexOfLine; (pSpriteTmp = this[j] as CCSprite) == null && j >= nFirstIndexOfLine; j--) ;
                    
                    // Move along if this line contains no characters
                    if (pSpriteTmp == null)
                        continue;

                    float fLineWidth = pSpriteTmp.BoundingBox.MaxX;

                    CCPoint ptShift = ptShiftAnchorPlusVertical;

                    // Horizontal shift calculation for this line
                    if (horzAlignment == CCTextAlignment.Right)
                        ptShift.X += Dimensions.Width - fLineWidth;
                    else if (horzAlignment == CCTextAlignment.Center)
                        ptShift.X += (Dimensions.Width - fLineWidth) / 2f;

                    // Finally go through each character in this line and shift things as appropriate
                    for (int j = nFirstIndexOfLine; j <= nLastIndexOfLine; j++)
                    {
                        pSpriteTmp = this[j] as CCSprite;
                        if (pSpriteTmp != null)
                            pSpriteTmp.Position += ptShift;
                    }

                    nFirstIndexOfLine = nLastIndexOfLine + 1;
                }
            }
        }

        /// <summary>
        /// Helper function of CreateFontChars -- moves sprites around like to the next line.
        /// </summary>
        /// <param name="nStartIndex"></param>
        /// <param name="nCount"></param>
        /// <param name="ptCurrent"></param>
        private void RepositionSprites(int nStartIndex, int nCount, ref CCPoint ptCurrent)
        {
            for (int i = nStartIndex; i < nStartIndex + nCount; i++)
            {
                CCSprite pSpriteTmp = (CCSprite)(this[i]);
                if (pSpriteTmp != null)
                {
                    pSpriteTmp.PositionY = ptCurrent.Y + pSpriteTmp.ContentSize.Height / 2f;
                    pSpriteTmp.PositionX = ptCurrent.X + pSpriteTmp.ContentSize.Width / 2f;

                    ptCurrent.X += pSpriteTmp.ContentSize.Width;
                }
            }
        }

        public virtual void SetString(string newString, bool updateLabel)
        {
            this.Text = newString;

            // Force a redraw now?
            if (updateLabel)
                this.Draw();
        }

        /// <summary>
        /// UpdateLabel() runs after CreateFontChars() has created all the child sprites that represent
        /// each character.  [CreateFontChars() is PASS #1]
        /// 
        /// [PASS #2] UpdateLable() splits lines when they are too long for the supplied Dimension.
        ///              *** NOTE: this pass could have been done when the characters were created in CreateFontChars()
        ///                 
        /// [PASS #3] UpdateLabel() then optionally nudges the characters along to preform a left or right horizontal alignment.
        /// 
        /// [PASS #4] UpdateLabel() then optionally nudges the lines up and/or down to preform a top or bottom vertical alignment.
        /// 
        /// 2015-03-15 Ultimately I just threw this function out.
        /// </summary>
        //protected void UpdateLabel()
        //{
        //    // NOTE: SetString() was likely called by the thread that got us here but with a true value
        //    //       SetString(true) -> UpdateString(true) -> CreateFontChars() -> UpdateLabel()
        //    SetString(labelInitialText, false);

        //    return;
        //}

        // These functions didn't work in every case, I tried to fix for the cases that I was seeing but it's best not to use.

        //// 2015-03-15 BUG FIX: this function doesn't give the correct value when AnchorPoint is {0.5, 0.5} because sp.Position.X is in the middle of the sprite
        //private float GetLetterPosXLeft(CCSprite sp)
        //{
        //    // 2015-03-12 removed because it doesn't properly use the anchor which always seems to be set at AnchorPointMiddle (0.5, 0.5)
        //    //return sp.Position.X * ScaleX;

        //    Debug.Assert(sp.AnchorPoint == CCPoint.AnchorMiddle, "ERROR - expected anchor point to always be int the middle");
        //    return (sp.Position.X - (sp.ContentSize.Width / 2f)) * ScaleX;
        //}

        //// 2015-03-15 BUG FIX: this function doesn't give the correct value when AnchorPoint is {0.5, 0.5} because sp.Position.X is in the middle of the sprite
        //private float GetLetterPosXRight(CCSprite sp)
        //{
        //    // 2015-03-12 removed because it doesn't properly use the anchor which always seems to be set at AnchorPointMiddle (0.5, 0.5)
        //    //return (sp.Position.X + sp.ContentSize.Width) * ScaleX;

        //    Debug.Assert(sp.AnchorPoint == CCPoint.AnchorMiddle, "ERROR - expected anchor point to always be int the middle");
        //    return (sp.Position.X + (sp.ContentSize.Width / 2f)) * ScaleX;
        //}

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
            if (IsDirty || RecreateSprites)
            {
                CreateFontChars();
                IsDirty = false;
            }
            base.Draw();
        }
    }
}