using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{

    public enum GlyphCollection 
    {
        Dynamic,
        NEHE,
        Ascii,
        Custom
    };

    // font attributes
    internal struct CCFontDefinition
    {

        public string FontName;
        public int FontSize;
        public CCTextAlignment Alignment;
        public CCVerticalTextAlignment LineAlignment;
        public CCSize Dimensions;
        public CCColor3B FontFillColor;
        public byte FontAlpha;
        public CCLabelLineBreak LineBreak;

    };



    internal struct LetterInfo
    {
        public CCFontLetterDefinition Definition;

        public CCPoint Position;
        public CCSize  ContentSize;
        public int   AtlasIndex;
    };

    [Flags]
    public enum CCLabelFormatFlags {
        Unknown               = 0x0001,
        SpriteFont            = 0x0002,
        BitmapFont            = 0x0004,
        CharacterMap          = 0x0008,
        SystemFont            = 0x0010,
    }

    public enum  CCLabelLineBreak {
        None              = 0,
        Character         = 1,
        Word              = 2,
    }

    public sealed class CCLabelFormat : IDisposable, ICloneable 
    {

        CCLabelFormatFlags formatFlags = CCLabelFormatFlags.Unknown;


        public CCLabelFormat ()
        {
            LineBreaking = CCLabelLineBreak.Word;
        }

        public CCLabelFormat (CCLabelFormat format)
        {
            if (format == null)
                throw new ArgumentNullException ("format");

            Alignment = format.Alignment;
            LineAlignment = format.LineAlignment;
            FormatFlags = format.FormatFlags;
        }

        public CCLabelFormat(CCLabelFormatFlags options) : this()
        {
            formatFlags = options;
        }

        ~CCLabelFormat ()
        {
            Dispose (false);
        }


        public CCTextAlignment Alignment {
            get; set;
        }

        public CCVerticalTextAlignment LineAlignment { get; set; }

        public CCLabelLineBreak LineBreaking { get; set; }

        public object Clone()
        {
            return new CCLabelFormat (this);
        }

        public void Dispose ()
        {
            Dispose (true);
            System.GC.SuppressFinalize (this);
        }

        void Dispose (bool disposing)
        {
        }

        public static CCLabelFormat BitMapFont
        {
            get {
                return new CCLabelFormat () { FormatFlags = CCLabelFormatFlags.BitmapFont };
            }
        }

        public static CCLabelFormat SystemFont 
        {
            get {
                return new CCLabelFormat () { FormatFlags = CCLabelFormatFlags.SystemFont };
            }
        }

        public static CCLabelFormat TrueTypeFont 
        {
            get {
                return new CCLabelFormat () { FormatFlags = CCLabelFormatFlags.SpriteFont };
            }
        }


        public CCLabelFormatFlags FormatFlags {
            get {               
                return formatFlags;
            }

            set {
                formatFlags = value;
            }
        }

    }



    public partial class CCLabel : CCNode, ICCTextContainer
    {

        [Flags]
        protected enum CCLabelType 
        {
            SpriteFont,
            BitMapFont,
            CharacterMap,
            SystemFont
        };

        public const int AutomaticWidth = -1;
        const int defaultSpriteBatchCapacity = 29;

        internal static Dictionary<string, CCBMFontConfiguration> fontConfigurations = new Dictionary<string, CCBMFontConfiguration>();

        protected CCLabelLineBreak lineBreak;
        protected CCTextAlignment horzAlignment = CCTextAlignment.Center;
        protected CCVerticalTextAlignment vertAlignment = CCVerticalTextAlignment.Top;
        internal CCBMFontConfiguration FontConfiguration { get; set; }
        protected string fntConfigFile;
        protected string labelInitialText;

        protected CCPoint ImageOffset { get; set; }
        protected CCSize labelDimensions;
        protected bool IsDirty { get; set; }
        public CCTextureAtlas TextureAtlas { get ; private set; }
        public CCRawList<CCSprite> Descendants { get; private set; }

        protected bool isColorModifiedByOpacity = false;

        public CCBlendFunc BlendFunc { get; set; }

        protected CCLabelType currentLabelType;
        private CCFontAtlas fontAtlas;
        private List<LetterInfo> lettersInfo = new List<LetterInfo>();

        //! used for optimization
        CCSprite reusedLetter;
        CCRect reusedRect;

        // System font
        bool systemFontDirty;
        string systemFont;
        float systemFontSize;

        CCLabelFormat labelFormat;

        // Static properties

        public static float DefaultTexelToContentSizeRatio
        {
            set { DefaultTexelToContentSizeRatios = new CCSize(value, value); }
        }

        public static CCSize DefaultTexelToContentSizeRatios { get; set; }


        // Instance properties

        protected float LineHeight { get; set; }

        public CCLabelFormat LabelFormat
        {
            get { return labelFormat; }
            set
            {
                if (!labelFormat.Equals(value))
                {
                    // TODO: Check label format flags need to be checked so they can not be
                    // changed after being set.
                    labelFormat = value;
                    IsDirty = true;
                }
            }
        }

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
            get { return labelFormat.Alignment; }
            set
            {
                if (labelFormat.Alignment != value)
                {
                    labelFormat.Alignment = value;
                    IsDirty = true;
                }
            }
        }

        public CCVerticalTextAlignment VerticalAlignment
        {
            get { return labelFormat.LineAlignment; }
            set
            {
                if (labelFormat.LineAlignment != value)
                {
                    labelFormat.LineAlignment = value;
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
            set
            {
                if (ContentSize != value)
                {
                    base.ContentSize = value;
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

        public CCLabelLineBreak LineBreak
        {
            get { return lineBreak; }
            set
            {
                lineBreak = value;
                IsDirty = true;
            }
        }

        public bool IsAntialiased
        {
            get { return Texture.IsAntialiased; }
            set { Texture.IsAntialiased = value; }
        }

        public virtual CCTexture2D Texture
        {
            get { return TextureAtlas.Texture; }
            set
            {
                TextureAtlas.Texture = value;
                UpdateBlendFunc();
            }
        }

        void UpdateBlendFunc()
        {
            if (!TextureAtlas.Texture.HasPremultipliedAlpha)
            {
                BlendFunc = CCBlendFunc.NonPremultiplied;
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
                    UpdateContent();
                }
            }
        }

        public string SystemFont
        {
            get { return systemFont; }
            set
            {
                if (systemFont != value)
                {
                    systemFont = value;
                    systemFontDirty = true;

                }
            }
        }

        public float SystemFontSize
        {
            get { return systemFontSize; }
            set
            {
                if (systemFontSize != value)
                {
                    systemFontSize = value;
                    systemFontDirty = true;

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

        static CCLabel()
        {
            DefaultTexelToContentSizeRatios = CCSize.One;
        }

        public CCLabel() : this("", "")
        {
        }

        public CCLabel(string str, string fntFile)
            : this(str, fntFile, 0.0f)
        {
        }

        public CCLabel(string str, string fntFile, float width)
            : this(str, fntFile, width, CCTextAlignment.Left)
        {
        }

        public CCLabel(string str, string fntFile, float width, CCTextAlignment alignment)
            : this(str, fntFile, width, alignment, CCPoint.Zero)
        {
        }

        public CCLabel(string str, string fntFile, float width, CCTextAlignment alignment, CCPoint imageOffset) 
            : this(str, fntFile, width, alignment, imageOffset, null)
        {
        }

        public CCLabel(string str, string fntFile, float width, CCTextAlignment alignment, CCPoint imageOffset, CCTexture2D texture)
            : this(str, fntFile, width, alignment, CCVerticalTextAlignment.Top, imageOffset, null)
        {
        }

        public CCLabel(string str, string fntFile, float width, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, 
            CCPoint imageOffset, CCTexture2D texture)
            : this(str, fntFile, new CCSize(width, 0), hAlignment, vAlignment, imageOffset, texture)
        {
        }

        public CCLabel(string str, string fntFile, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, 
            CCPoint imageOffset, CCTexture2D texture)
            : this (str, fntFile, dimensions, new CCLabelFormat() { Alignment = hAlignment, LineAlignment = vAlignment}, imageOffset, texture)
        {
            // First we try loading BitMapFont
            //InitBMFont(str, fntFile, dimensions, hAlignment, vAlignment, imageOffset, texture);
        }

        public CCLabel(string str, string fntFile, CCSize dimensions, CCLabelFormat labelFormat)
            : this(str, fntFile, dimensions, labelFormat, CCPoint.Zero, null)
        {   }

        public CCLabel(string str, string fntFile, float size, CCLabelFormat labelFormat)
            : this (str, fntFile, size, CCSize.Zero, labelFormat)
        {   }

        public CCLabel(string str, string fntFile, float size, CCSize dimensions, CCLabelFormat labelFormat)
            : this (str, fntFile, size, dimensions, labelFormat, CCPoint.Zero, null)
        {   }

        public CCLabel(string str, string fntFile, CCSize dimensions, CCLabelFormat labelFormat, CCPoint imageOffset, CCTexture2D texture)
        {
            this.labelFormat = labelFormat;
            // First we try loading BitMapFont
            InitBMFont(str, fntFile, dimensions, labelFormat.Alignment, labelFormat.LineAlignment, imageOffset, texture);
        }

        public CCLabel(string str, string fntFile, float size, CCSize dimensions, CCLabelFormat labelFormat, CCPoint imageOffset, CCTexture2D texture)
        {
            this.labelFormat = labelFormat;
            if (labelFormat.FormatFlags == CCLabelFormatFlags.Unknown)
            {
                // First we try loading BitMapFont
                InitBMFont(str, fntFile, dimensions, labelFormat.Alignment, labelFormat.LineAlignment, imageOffset, texture);
            }
            else if(labelFormat.FormatFlags == CCLabelFormatFlags.BitmapFont)
            {
                // First we try loading BitMapFont
                InitBMFont(str, fntFile, dimensions, labelFormat.Alignment, labelFormat.LineAlignment, imageOffset, texture);
            }
            else if(labelFormat.FormatFlags == CCLabelFormatFlags.SystemFont)
            {
                SystemFont = fntFile;
                SystemFontSize = size;
                Dimensions = dimensions;
                Text = str;
            }

        }

        internal void Reset ()
        {
            systemFontDirty = false;
            systemFont = "Helvetica";
            systemFontSize = 12;
        }

        internal CCFontAtlas FontAtlas 
        { 
            get {return fontAtlas;}
            set
            {
                if (value != fontAtlas)
                {
                    fontAtlas = value;


                    if (reusedLetter == null)
                    {
                        reusedLetter = new CCSprite();
                        reusedLetter.IsColorModifiedByOpacity = isColorModifiedByOpacity;
                        reusedLetter.AnchorPoint = CCPoint.AnchorUpperLeft;

                    }

                    if (fontAtlas != null)
                    {
                        if (TextureAtlas != null)
                            Texture = FontAtlas.GetTexture(0);
                        else
                            TextureAtlas = new CCTextureAtlas(FontAtlas.GetTexture(0), defaultSpriteBatchCapacity);

                        LineHeight = fontAtlas.CommonHeight;
                        IsDirty = true;
                    }

                }
            }
        }

        protected void InitBMFont(string theString, string fntFile, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, 
            CCPoint imageOffset, CCTexture2D texture)
        {
            Debug.Assert(FontConfiguration == null, "re-init is no longer supported");
            Debug.Assert((theString == null && fntFile == null) || (theString != null && fntFile != null),
                "Invalid params for CCLabelBMFont");

            if (!String.IsNullOrEmpty(fntFile))
            {
                try
                {
                    FontAtlas = CCFontAtlasCache.GetFontAtlasFNT(fntFile, imageOffset);
                }
                catch {}

                if (FontAtlas == null)
                {
                    CCLog.Log("Bitnap Font CCLabel: Impossible to create font. Please check file: '{0}'", fntFile);
                    return;
                }

            }

            FontConfiguration = CCBMFontConfiguration.FontConfigurationWithFile(fntFile);

            currentLabelType = CCLabelType.BitMapFont;

            if (String.IsNullOrEmpty(theString))
            {
                theString = String.Empty;
            }

            // Initialize the TextureAtlas along with children.
            var capacity = theString.Length;

            BlendFunc = CCBlendFunc.AlphaBlend;

            if (capacity == 0)
            {
                capacity = defaultSpriteBatchCapacity;
            }

            UpdateBlendFunc();

            // no lazy alloc in this node
            Children = new CCRawList<CCNode>(capacity);
            Descendants = new CCRawList<CCSprite>(capacity);

            this.labelDimensions = dimensions;

            horzAlignment = hAlignment;
            vertAlignment = vAlignment;

            IsOpacityCascaded = true;

            ContentSize = CCSize.Zero;

            IsColorModifiedByOpacity = TextureAtlas.Texture.HasPremultipliedAlpha;
            AnchorPoint = CCPoint.AnchorMiddle;

            ImageOffset = imageOffset;

            Text = theString;
        }

        #endregion Constructors

        public override void UpdateColor()
        {
            base.UpdateColor();

            if (TextureAtlas == null)
            {
                return;
            }

            var color4 = new CCColor4B( DisplayedColor.R, DisplayedColor.G, DisplayedColor.B, DisplayedOpacity );

            // special opacity for premultiplied textures
            if (IsColorModifiedByOpacity)
            {
                color4.R = (byte)(color4.R * DisplayedOpacity / 255.0f);
                color4.G = (byte)(color4.G * DisplayedOpacity / 255.0f);
                color4.B = (byte)(color4.B * DisplayedOpacity / 255.0f);
            }

            var quads = TextureAtlas.Quads;
            var totalQuads = TextureAtlas.TotalQuads;
            CCV3F_C4B_T2F_Quad quad;

            for (int index = 0; index < totalQuads; ++index)
            {
                quad = quads[index];
                quad.BottomLeft.Colors = color4;
                quad.BottomRight.Colors = color4;
                quad.TopLeft.Colors = color4;
                quad.TopRight.Colors = color4;
                TextureAtlas.UpdateQuad(ref quad, index);
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
                    UpdateColor();
                }
            }
        }

        protected void UpdateContent()
        {

            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            if (FontAtlas != null)
            {
                LayoutLabel();
            }
            else
            {
                var fontDefinition = new CCFontDefinition();

                fontDefinition.FontName = systemFont;
                fontDefinition.FontSize = (int)systemFontSize;

                fontDefinition.Alignment = labelFormat.Alignment;
                fontDefinition.LineAlignment = labelFormat.LineAlignment;

                fontDefinition.Dimensions = Dimensions;

                fontDefinition.FontFillColor = DisplayedColor;
                fontDefinition.FontAlpha = DisplayedOpacity;
                fontDefinition.LineBreak = labelFormat.LineBreaking;

                CreateSpriteWithFontDefinition(fontDefinition);
            }

            IsDirty = false;
        }

        CCSprite textSprite = null;
        void CreateSpriteWithFontDefinition(CCFontDefinition fontDefinition)
        {
            currentLabelType =  CCLabelType.SystemFont;

            var texture = CreateTextSprite(Text, fontDefinition);

            textSprite = new CCSprite(texture);

            textSprite.AnchorPoint = CCPoint.AnchorLowerLeft;
            ContentSize = textSprite.ContentSize;

            base.AddChild(textSprite,0,TagInvalid);

            textSprite.UpdateDisplayedColor(DisplayedColor);
            textSprite.UpdateDisplayedOpacity(DisplayedOpacity);
        }

        protected void UpdateFont()
        {
            if (FontAtlas != null)
            {
                //CCFontAtlasCache.ReleaseFontAtlas(FontAtlas);
                FontAtlas = null;
            }

            IsDirty = true;
            systemFontDirty = false;
        }

        void UpdateQuads()
        {
            int index;
            int lettersCount = lettersInfo.Count;

            for (int ctr = 0; ctr < lettersCount; ++ctr)
            {
                var letterDef = lettersInfo[ctr].Definition;

                if (letterDef.IsValidDefinition)
                {
                    reusedRect = letterDef.Subrect;

                    reusedLetter.Texture = Texture;
                    // make sure we set AtlasIndex to not initialized here or first character does not display correctly
                    reusedLetter.AtlasIndex = CCMacros.CCSpriteIndexNotInitialized;
                    reusedLetter.TextureRectInPixels = reusedRect;
                    reusedLetter.ContentSize = reusedRect.Size;

                    reusedLetter.Position = lettersInfo[ctr].Position;

                    index = TextureAtlas.TotalQuads;
                    var letterInfo = lettersInfo[ctr];
                    letterInfo.AtlasIndex = index;
                    lettersInfo[ctr] = letterInfo;

                    InsertGlyph(reusedLetter, index);
                }     
            }
        }

        void LayoutLabel ()
        {

            if (FontAtlas == null || string.IsNullOrEmpty(Text))
            {
                ContentSize = CCSize.Zero;
                return;
            }

            TextureAtlas.RemoveAllQuads();
            Descendants.Clear();
            lettersInfo.Clear();

            FontAtlas.PrepareLetterDefinitions(Text);

            var start = 0;
            var typesetter = new CCTLTextLayout(this);

            var length = Text.Length;
            var insetBounds = labelDimensions;

            var layoutAvailable = true;
            if (insetBounds == CCSize.Zero) 
            {
                insetBounds = new CCSize (8388608, 8388608);
                layoutAvailable = false;
            }

            var boundsWidth = insetBounds.Width;
            var contentScaleFactorWidth = CCLabel.DefaultTexelToContentSizeRatios.Width;
            var contentScaleFactorHeight = CCLabel.DefaultTexelToContentSizeRatios.Height;

            List<CCTLLine> lineList = new List<CCTLLine>();
            while (start < length)// && textPosition.Y < insetBounds.Bottom)
            {

                // Now we ask the typesetter to break off a line for us.
                // This also will take into account line feeds embedded in the text.
                //  Example: "This is text \n with a line feed embedded inside it"
                int count = typesetter.SuggestLineBreak(start, boundsWidth);
                var line = typesetter.GetLine(start, start + count);
                lineList.Add(line);

                start += count;
            }


            // Calculate our vertical starting position
            var totalHeight = lineList.Count * LineHeight;
            var nextFontPositionY = totalHeight;

            if (Dimensions.Height > 0)
            {
                var labelHeightPixel = Dimensions.Height * contentScaleFactorHeight;
                if (totalHeight > labelHeightPixel)
                {
                    int numLines = (int)(labelHeightPixel / LineHeight);
                    totalHeight = numLines * LineHeight;
                }
                switch (VerticalAlignment)
                {
                    case CCVerticalTextAlignment.Top:
                        nextFontPositionY = labelHeightPixel;
                        break;
                    case CCVerticalTextAlignment.Center:
                        nextFontPositionY = (labelHeightPixel + totalHeight) * 0.5f;
                        break;
                    case CCVerticalTextAlignment.Bottom:
                        nextFontPositionY = totalHeight;
                        break;
                    default:
                        break;
                }
            }


            var lineGlyphIndex = 0;
            float longestLine = (labelDimensions.Width > 0) ? labelDimensions.Width : 0;

            // Used for calculating overlapping on last line character
            var lastCharWidth = 0.0f;
            int lastCharAdvance = 0;

            // Define our horizontal justification
            var flushFactor = (float)HorizontalAlignment / (float)CCTextAlignment.Right;

            // We now loop through all of our line's glyph runs
            foreach (var line in lineList)
            {

                var gliphRun = line.GlyphRun;
                var lineWidth = line.Bounds.Width * contentScaleFactorWidth;
                var flush = line.PenOffsetForFlush(flushFactor, boundsWidth);

                foreach (var glyph in gliphRun)
                {
                    var letterPosition = glyph.Position;
                    var letterDef = glyph.Definition;
                    lastCharWidth = letterDef.Width * contentScaleFactorWidth;
                    letterPosition.X += flush;
                    letterPosition.Y = (nextFontPositionY - letterDef.YOffset) / contentScaleFactorHeight;

                    //recordLetterInfo(letterPosition, glyph.def, lineGlyphIndex++);

                    var tmpInfo = new LetterInfo();

                    tmpInfo.Definition = letterDef;
                    tmpInfo.Position = letterPosition;
                    tmpInfo.ContentSize.Width = letterDef.Width;
                    tmpInfo.ContentSize.Height = letterDef.Height;

                    if (lineGlyphIndex >= lettersInfo.Count)
                    {
                        lettersInfo.Add(tmpInfo);
                    }
                    else
                    {
                        lettersInfo[lineGlyphIndex] = tmpInfo;
                    }

                    lineGlyphIndex++;

                    lastCharAdvance         = (int)glyph.Definition.XAdvance;
                }

                // calculate our longest line which is used for calculating our ContentSize
                if (lineWidth > longestLine)
                    longestLine = lineWidth;

                nextFontPositionY -= LineHeight;
            }

            CCSize tmpSize;
            // If the last character processed has an xAdvance which is less that the width of the characters image, then we need
            // to adjust the width of the string to take this into account, or the character will overlap the end of the bounding
            // box
            if(lastCharAdvance < lastCharWidth)
            {
                tmpSize.Width = longestLine - lastCharAdvance + lastCharWidth;
            }
            else
            {
                tmpSize.Width = longestLine;
            }

            tmpSize.Height = totalHeight;

            if (Dimensions.Height > 0)
            {
                tmpSize.Height = Dimensions.Height * contentScaleFactorHeight;
            }

            ContentSize = tmpSize / CCLabel.DefaultTexelToContentSizeRatios;

            lineList.Clear();

            CCRect uvRect;
            CCSprite letterSprite;

            for (int c = 0; c < Children.Count; c++) 
            {
                letterSprite = (CCSprite)Children[c];
                int tag = letterSprite.Tag;
                if(tag >= length)
                {
                    RemoveChild(letterSprite, true);
                }
                else if(tag >= 0)
                {
                    if (letterSprite != null)
                    {
                        uvRect = lettersInfo[tag].Definition.Subrect;
                        letterSprite.TextureRectInPixels = uvRect;
                        letterSprite.ContentSize = uvRect.Size;
                    }
                }
            }

            UpdateQuads();
            UpdateColor();

        }

        public new CCNode this[int letterIndex]
        {
            get 
            { 
                if (currentLabelType == CCLabelType.SystemFont)
                {
                    return null;
                }

                if (IsDirty)
                {
                    UpdateContent();
                }

                if (letterIndex < lettersInfo.Count)
                {
                    var letter = lettersInfo[letterIndex];

                    if(! letter.Definition.IsValidDefinition)
                        return null;

                    var sp = (this.GetChildByTag(letterIndex)) as CCSprite;

                    if (sp == null)
                    {
                        var uvRect = letter.Definition.Subrect;

                        sp = new CCSprite(FontAtlas.GetTexture(letter.Definition.TextureID),uvRect);

                        sp.Position = new CCPoint(letter.Position.X + uvRect.Size.Width / 2,
                            letter.Position.Y - uvRect.Size.Height / 2);
                        sp.Opacity = Opacity;

                        AddSpriteWithoutQuad(sp, letter.AtlasIndex, letterIndex);
                    }
                    return sp;
                }

                return null;            
            }
        }

        private void AddSpriteWithoutQuad(CCSprite child, int z, int aTag)
        {
            Debug.Assert(child != null, "Argument must be non-NULL");

            // quad index is Z
            child.AtlasIndex = z;
            child.TextureAtlas = TextureAtlas;

            int i = 0;

            if (Descendants.Count > 0)
            {
                CCSprite[] elements = Descendants.Elements;
                for (int j = 0, count = Descendants.Count; j < count; j++)
                {
                    if (elements[i].AtlasIndex <= z)
                    {
                        ++i;
                    }
                }
            }

            Descendants.Insert(i, child);

            base.AddChild(child, z, aTag);

        }

        #region Child management

        public override void AddChild(CCNode child, int zOrder = 0, int tag = CCNode.TagInvalid)
        {
            Debug.Assert(false, "AddChild is not allowed on CCLabel");
        }


        private void InsertGlyph(CCSprite sprite, int atlasIndex)
        {
            Debug.Assert(sprite != null, "child should not be null");

            if (TextureAtlas.TotalQuads == TextureAtlas.Capacity)
            {
                IncreaseAtlasCapacity();
            }

            sprite.AtlasIndex = atlasIndex;
            sprite.TextureAtlas = TextureAtlas;
            var quad = sprite.Quad;

            TextureAtlas.InsertQuad(ref quad, atlasIndex);

            sprite.UpdateTransformedSpriteTextureQuads();

        }

        #endregion

        public void IncreaseAtlasCapacity()
        {
            // if we're going beyond the current TextureAtlas's capacity,
            // all the previously initialized sprites will need to redo their texture coords
            // this is likely computationally expensive
            int quantity = (TextureAtlas.Capacity + 1) * 4 / 3;

            CCLog.Log(string.Format(
                "CocosSharp: CCLabel: resizing TextureAtlas capacity from [{0}] to [{1}].",
                TextureAtlas.Capacity, quantity));

            TextureAtlas.ResizeCapacity(quantity);
        }

        public override void Visit()
        {

            if (!Visible || Text.Length == 0)
            {
                return;
            }

            if (systemFontDirty)
            {
                UpdateFont();
            }

            if (IsDirty)
            {
                UpdateContent();
            }

            Window.DrawManager.PushMatrix();

            Transform();

            if (textSprite != null)
                DrawTextSprite();
            else
                Draw();

            Window.DrawManager.PopMatrix();
        }

        void DrawTextSprite()
        {
            //            if (_fontDefinition._fontFillColor.r != _textColor.r || _fontDefinition._fontFillColor.g != _textColor.g
            //                || _fontDefinition._fontFillColor.b != _textColor.b)
            //            {
            //                updateContent();
            //            }
            //
            //            if (_shadowEnabled && _shadowNode == nullptr)
            //            {
            //                _shadowNode = Sprite::createWithTexture(_textSprite->getTexture());
            //                if (_shadowNode)
            //                {
            //                    if (_blendFuncDirty)
            //                    {
            //                        _shadowNode->setBlendFunc(_blendFunc);
            //                    }
            //                    _shadowNode->setAnchorPoint(Vec2::ANCHOR_BOTTOM_LEFT);
            //                    _shadowNode->setColor(_shadowColor);
            //                    _shadowNode->setOpacity(_shadowOpacity * _displayedOpacity);
            //                    _shadowNode->setPosition(_shadowOffset.width, _shadowOffset.height);
            //                    Node::addChild(_shadowNode,0,Node::INVALID_TAG);
            //                }
            //            }
            //            if (_shadowNode)
            //            {
            //                _shadowNode->visit(renderer, _modelViewTransform, parentFlags);
            //            }
            textSprite.Visit();
        }


        protected override void Draw()
        {
            //base.Draw();
            // Optimization: Fast Dispatch  
            //            if (TextureAtlas == null || TextureAtlas.TotalQuads == 0)
            //            {
            //                return;
            //            }

            // Loop through each of our children nodes that may have actions attached.
            foreach(CCSprite child in Children)
            {
                if (child.Tag >= 0)
                {
                    child.UpdateLocalTransformedSpriteTextureQuads();
                }
            }

            Window.DrawManager.BlendFunc(BlendFunc);
            TextureAtlas.DrawQuads();
        }

    }

}
