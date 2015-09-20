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
        public float FontSize;
        public CCTextAlignment Alignment;
        public CCVerticalTextAlignment LineAlignment;
        public CCSize Dimensions;
        public CCColor3B FontFillColor;
        public byte FontAlpha;
        public CCLabelLineBreak LineBreak;
        public bool isShouldAntialias;
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

    public sealed class CCLabelFormat : IDisposable
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

        public static CCLabelFormat SpriteFont 
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


    [Flags]
    public enum CCLabelType 
    {
        SpriteFont,
        BitMapFont,
        CharacterMap,
        SystemFont
    };

    public partial class CCLabel : CCNode, ICCTextContainer
    {

        const int defaultSpriteBatchCapacity = 29;

        internal static Dictionary<string, CCBMFontConfiguration> fontConfigurations = new Dictionary<string, CCBMFontConfiguration>();

        protected CCLabelLineBreak lineBreak;
        protected CCTextAlignment horzAlignment = CCTextAlignment.Center;
        protected CCVerticalTextAlignment vertAlignment = CCVerticalTextAlignment.Top;
        internal CCBMFontConfiguration FontConfiguration { get; set; }
        protected string fntConfigFile;
        protected string labelText;

        protected CCPoint ImageOffset { get; set; }
        protected CCSize labelDimensions;
        protected bool IsDirty { get; set; }
        public CCTextureAtlas TextureAtlas { get ; private set; }
        protected CCRawList<CCSprite> Descendants { get; private set; }

        protected bool isColorModifiedByOpacity = false;

        public CCBlendFunc BlendFunc { get; set; }

        public CCLabelType LabelType { get; protected internal set; }
        private CCFontAtlas fontAtlas;
        private List<LetterInfo> lettersInfo = new List<LetterInfo>();

        //! used for optimization
        CCSprite reusedLetter;
        CCRect reusedRect;

        // System font
        bool systemFontDirty;
        string systemFont;
        float systemFontSize;

        float lineHeight = 0;
        float additionalKerning = 0;

        CCLabelFormat labelFormat;

        CCQuadCommand quadCommand = null;

        // Static properties

        public static float DefaultTexelToContentSizeRatio
        {
            set { DefaultTexelToContentSizeRatios = new CCSize(value, value); }
        }

        public static CCSize DefaultTexelToContentSizeRatios { get; set; }


        // Instance properties

        public float LineHeight 
        { 
            get { return lineHeight; } 
            set
            {
                if (LabelType == CCLabelType.SystemFont)
                    CCLog.Log("Not supported system font!");
                
                if (value != lineHeight)
                {
                    lineHeight = value;
                    IsDirty = true;
                }
            }
        }

        public float AdditionalKerning 
        { 
            get { return additionalKerning; } 
            set
            {
                if (LabelType == CCLabelType.SystemFont)
                    CCLog.Log("Not supported system font!");

                if (value != additionalKerning)
                {
                    additionalKerning = value;
                    IsDirty = true;
                }
            }
        }

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

        public override CCSize ContentSize
        {
            get 
            {
                if (IsDirty || systemFontDirty)
                    UpdateContent();
                
                return base.ContentSize; 
            }
            set
            {
                if (base.ContentSize != value)
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

        bool isAntialiased = CCTexture2D.DefaultIsAntialiased;
        public bool IsAntialiased
        {
            get 
            {
                if (TextureAtlas != null && TextureAtlas.Texture != null)
                    return TextureAtlas.Texture.IsAntialiased;
                else if (textSprite != null)
                    return textSprite.IsAntialiased;
                else
                    return CCTexture2D.DefaultIsAntialiased;
            }

            set 
            { 
                if (value != isAntialiased)
                {
                    if (TextureAtlas != null && TextureAtlas.Texture != null)
                        TextureAtlas.Texture.IsAntialiased = value;
                    else if (textSprite != null)
                        textSprite.IsAntialiased = value;

                    isAntialiased = value;

                }
            }
        }

        public CCTexture2D Texture
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
            quadCommand.BlendType = BlendFunc;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        public CCLabel() : this("", "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        public CCLabel(string str, string fntFile)
            : this(str, fntFile, 0.0f)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        public CCLabel(string str, string fntFile, float size)
            : this(str, fntFile, size, CCTextAlignment.Left)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        /// <param name="alignment">Horizontal Alignment of the text.</param>
        public CCLabel(string str, string fntFile, float size, CCTextAlignment alignment)
            : this(str, fntFile, size, alignment, CCPoint.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        /// <param name="alignment">Horizontal Alignment of the text.</param>
        /// <param name="imageOffset">Image offset.</param>
        public CCLabel(string str, string fntFile, float size, CCTextAlignment alignment, CCPoint imageOffset) 
            : this(str, fntFile, size, alignment, imageOffset, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        /// <param name="alignment">Horizontal Alignment of the text.</param>
        /// <param name="imageOffset">Image offset.</param>
        /// <param name="texture">Texture Atlas to be used.</param>
        public CCLabel(string str, string fntFile, float size, CCTextAlignment alignment, CCPoint imageOffset, CCTexture2D texture)
            : this(str, fntFile, size, alignment, CCVerticalTextAlignment.Top, imageOffset, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        /// <param name="hAlignment">Horizontal Alignment of the text.</param>
        /// <param name="vAlignment">Vertical alignment.</param>
        /// <param name="imageOffset">Image offset.</param>
        /// <param name="texture">Texture Atlas to be used.</param>
        public CCLabel(string str, string fntFile, float size, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, 
            CCPoint imageOffset, CCTexture2D texture)
            : this(str, fntFile, size, CCSize.Zero,
                new CCLabelFormat() { Alignment = hAlignment, LineAlignment = vAlignment}, 
                imageOffset, texture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        public CCLabel(string str, string fntFile, CCSize dimensions)
            : this (str, fntFile, dimensions, 
                new CCLabelFormat(), CCPoint.Zero, null)
        {   }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        /// <param name="hAlignment">Horizontal alignment of the text.</param>
        public CCLabel(string str, string fntFile, CCSize dimensions, CCTextAlignment hAlignment)
            : this (str, fntFile, dimensions, 
                new CCLabelFormat() { Alignment = hAlignment}, 
                CCPoint.Zero, null)
        {   }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        /// <param name="hAlignment">Horizontal alignment of the text.</param>
        /// <param name="vAlignement">Vertical alignement of the text.</param>
        public CCLabel(string str, string fntFile, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignement)
            : this (str, fntFile, dimensions, hAlignment, vAlignement, CCPoint.Zero, null)
        {   }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        /// <param name="hAlignment">Horizontal alignment of the text.</param>
        /// <param name="vAlignement">Vertical alignement of the text.</param>
        /// <param name="imageOffset">Image offset.</param>
        /// <param name="texture">Texture Atlas to be used.</param>
        public CCLabel(string str, string fntFile, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, 
            CCPoint imageOffset, CCTexture2D texture)
            : this (str, fntFile, dimensions, 
                new CCLabelFormat() { Alignment = hAlignment, LineAlignment = vAlignment}, 
                imageOffset, texture)
        {   }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        /// <param name="labelFormat">Label format <see cref="CocosSharp.CCLabelFormat"/>.</param>
        public CCLabel(string str, string fntFile, CCSize dimensions, CCLabelFormat labelFormat)
            : this(str, fntFile, dimensions, labelFormat, CCPoint.Zero, null)
        {   }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        /// <param name="labelFormat">Label format <see cref="CocosSharp.CCLabelFormat"/>.</param>
        public CCLabel(string str, string fntFile, float size, CCLabelFormat labelFormat)
            : this (str, fntFile, size, CCSize.Zero, labelFormat)
        {   }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        /// <param name="labelFormat">Label format <see cref="CocosSharp.CCLabelFormat"/>.</param>
        public CCLabel(string str, string fntFile, float size, CCSize dimensions, CCLabelFormat labelFormat)
            : this (str, fntFile, size, dimensions, labelFormat, CCPoint.Zero, null)
        {   }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        /// <param name="labelFormat">Label format <see cref="CocosSharp.CCLabelFormat"/>.</param>
        /// <param name="imageOffset">Image offset.</param>
        /// <param name="texture">Texture atlas to be used.</param>
        public CCLabel(string str, string fntFile, CCSize dimensions, CCLabelFormat labelFormat, CCPoint imageOffset, CCTexture2D texture)
            : this (str, fntFile, 0.0f, dimensions, labelFormat, imageOffset, texture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        /// <param name="labelFormat">Label format <see cref="CocosSharp.CCLabelFormat"/>.</param>
        /// <param name="imageOffset">Image offset.</param>
        /// <param name="texture">Texture atlas to be used.</param>
        public CCLabel(CCFontFNT fntFontConfig, string str, CCSize dimensions, CCLabelFormat labelFormat)
        {
            quadCommand = new CCQuadCommand(str.Length);

            labelFormat.FormatFlags = CCLabelFormatFlags.BitmapFont;
            AnchorPoint = CCPoint.AnchorMiddle;

            try
            {
                FontAtlas = CCFontAtlasCache.GetFontAtlasFNT(fntFontConfig);
            }
            catch { }

            if (FontAtlas == null)
            {
                CCLog.Log("Bitmap Font CCLabel: Impossible to create font. Please check CCFontFNT file: ");
                return;
            }

            LabelType = CCLabelType.BitMapFont;
            this.labelFormat = labelFormat;

            if (String.IsNullOrEmpty(str))
            {
                str = String.Empty;
            }

            // Initialize the TextureAtlas along with children.
            var capacity = str.Length;

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

            horzAlignment = labelFormat.Alignment;
            vertAlignment = labelFormat.LineAlignment;

            IsOpacityCascaded = true;

            // We use base here so we do not trigger an update internally.
            base.ContentSize = CCSize.Zero;

            IsColorModifiedByOpacity = TextureAtlas.Texture.HasPremultipliedAlpha;
            AnchorPoint = CCPoint.AnchorMiddle;

            ImageOffset = CCPoint.Zero;

            Text = str;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CocosSharp.CCLabel"/> class.
        /// </summary>
        /// <param name="str">Initial text of the label.</param>
        /// <param name="fntFile">Font definition file to use.</param>
        /// <param name="size">Font point size.</param>
        /// <param name="dimensions">Dimensions that the label should use to layout it's text.</param>
        /// <param name="labelFormat">Label format <see cref="CocosSharp.CCLabelFormat"/>.</param>
        /// <param name="imageOffset">Image offset.</param>
        /// <param name="texture">Texture atlas to be used.</param>
        public CCLabel(string str, string fntFile, float size, CCSize dimensions, CCLabelFormat labelFormat, CCPoint imageOffset, CCTexture2D texture)
        {
            quadCommand = new CCQuadCommand(str.Length);

            this.labelFormat = (size == 0 && labelFormat.FormatFlags == CCLabelFormatFlags.Unknown) 
                ? CCLabelFormat.BitMapFont 
                : labelFormat;

            if (this.labelFormat.FormatFlags == CCLabelFormatFlags.Unknown)
            {
                // First we try loading BitMapFont
                CCLog.Log("Label Format Unknown: Trying BitmapFont ...");
                InitBMFont(str, fntFile, dimensions, labelFormat.Alignment, labelFormat.LineAlignment, imageOffset, texture);
                // If we do not load a Bitmap Font then try a SpriteFont
                if (FontAtlas == null)
                {
                    CCLog.Log("Label Format Unknown: Trying SpriteFont ...");
                    InitSpriteFont(str, fntFile, size, dimensions, labelFormat, imageOffset, texture);
                }
                // If we do not load a Bitmap Font nor a SpriteFont then try the last time for a System Font
                if (FontAtlas == null)
                {
                    CCLog.Log("Label Format Unknown: Trying System Font ...");
                    LabelType = CCLabelType.SystemFont;
                    SystemFont = fntFile;
                    SystemFontSize = size;
                    Dimensions = dimensions;
                    AnchorPoint = CCPoint.AnchorMiddle;
                    BlendFunc = CCBlendFunc.AlphaBlend;
                    Text = str;
                }
            }
            else if(this.labelFormat.FormatFlags == CCLabelFormatFlags.BitmapFont)
            {
                // Initialize the BitmapFont
                InitBMFont(str, fntFile, dimensions, labelFormat.Alignment, labelFormat.LineAlignment, imageOffset, texture);
            }
            else if(this.labelFormat.FormatFlags == CCLabelFormatFlags.SpriteFont)
            {
                // Initialize the SpriteFont
                InitSpriteFont(str, fntFile, size, dimensions, labelFormat, imageOffset, texture);

            }
            else if(this.labelFormat.FormatFlags == CCLabelFormatFlags.SystemFont)
            {
                LabelType = CCLabelType.SystemFont;
                SystemFont = fntFile;
                SystemFontSize = size;
                Dimensions = dimensions;
                AnchorPoint = CCPoint.AnchorMiddle;
                BlendFunc = CCBlendFunc.AlphaBlend;
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

                        quadCommand.Texture = TextureAtlas.Texture;

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
                    CCLog.Log("Bitmap Font CCLabel: Impossible to create font. Please check file: '{0}'", fntFile);
                    return;
                }

            }

            AnchorPoint = CCPoint.AnchorMiddle;

            FontConfiguration = CCBMFontConfiguration.FontConfigurationWithFile(fntFile);

            LabelType = CCLabelType.BitMapFont;

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

            // We use base here so we do not trigger an update internally.
            base.ContentSize = CCSize.Zero;

            IsColorModifiedByOpacity = TextureAtlas.Texture.HasPremultipliedAlpha;
            AnchorPoint = CCPoint.AnchorMiddle;

            ImageOffset = imageOffset;

            Text = theString;
        }

        protected void InitSpriteFont(string theString, string fntFile, float fontSize, CCSize dimensions, CCLabelFormat labelFormat, 
            CCPoint imageOffset, CCTexture2D texture)
        {
            Debug.Assert((theString == null && fntFile == null) || (theString != null && fntFile != null),
                "Invalid params for CCLabel SpriteFont");

            if (!String.IsNullOrEmpty(fntFile))
            {
                try
                {
                    FontAtlas = CCFontAtlasCache.GetFontAtlasSpriteFont(fntFile, fontSize, imageOffset);
                    Scale = FontAtlas.Font.FontScale;
                }
                catch {}

                if (FontAtlas == null)
                {
                    CCLog.Log("SpriteFont CCLabel: Impossible to create font. Please check file: '{0}'", fntFile);
                    return;
                }

            }

            AnchorPoint = CCPoint.AnchorMiddle;

            LabelType = CCLabelType.SpriteFont;

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

            horzAlignment = labelFormat.Alignment;
            vertAlignment = labelFormat.LineAlignment;

            IsOpacityCascaded = true;

            ContentSize = CCSize.Zero;

            IsColorModifiedByOpacity = TextureAtlas.Texture.HasPremultipliedAlpha;
            AnchorPoint = CCPoint.AnchorMiddle;

            ImageOffset = imageOffset;

            Text = theString;
        }

        #endregion Constructors

        public override void UpdateDisplayedColor(CCColor3B parentColor)
        {
            var displayedColor = CCColor3B.White;
            displayedColor.R = (byte)(RealColor.R * parentColor.R / 255.0f);
            displayedColor.G = (byte)(RealColor.G * parentColor.G / 255.0f);
            displayedColor.B = (byte)(RealColor.B * parentColor.B / 255.0f);

            base.UpdateDisplayedColor(displayedColor);

            if (LabelType == CCLabelType.SystemFont && textSprite != null)
            {
                textSprite.UpdateDisplayedColor(displayedColor);
            }

        }

        protected internal override void UpdateDisplayedOpacity(byte parentOpacity)
        {
            var displayedOpacity = (byte) (RealOpacity * parentOpacity / 255.0f);

            base.UpdateDisplayedOpacity(displayedOpacity);

            if (LabelType == CCLabelType.SystemFont && textSprite != null)
            {
                textSprite.UpdateDisplayedOpacity(displayedOpacity);
            }

        }


        public override void UpdateColor()
        {
            if (TextureAtlas != null && !string.IsNullOrEmpty(labelText))
            {
                quadCommand.RequestUpdateQuads(UpdateColorCallback);
            }
        }

        void UpdateColorCallback(ref CCV3F_C4B_T2F_Quad[] quads)
        {

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

            if (quads != null)
            {
                
                var totalQuads = quads.Length;
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

        bool isUpdatingContent = false;

        protected void UpdateContent()
        {

            if (isUpdatingContent)
                return;

            isUpdatingContent = true;

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
                if (LabelType == CCLabelType.SystemFont)
                {
                    var fontDefinition = new CCFontDefinition();

                    fontDefinition.FontName = systemFont;
                    fontDefinition.FontSize = systemFontSize;

                    fontDefinition.Alignment = labelFormat.Alignment;
                    fontDefinition.LineAlignment = labelFormat.LineAlignment;

                    fontDefinition.Dimensions = Dimensions;

                    fontDefinition.FontFillColor = DisplayedColor;
                    fontDefinition.FontAlpha = DisplayedOpacity;
                    fontDefinition.LineBreak = labelFormat.LineBreaking;

                    fontDefinition.isShouldAntialias = IsAntialiased;

                    CreateSpriteWithFontDefinition(fontDefinition);
                }
            }

            IsDirty = false;
            isUpdatingContent = false;
        }

        CCSprite textSprite = null;
        void CreateSpriteWithFontDefinition(CCFontDefinition fontDefinition)
        {

            if (textSprite != null)
                textSprite.RemoveFromParent();

            var texture = CreateTextSprite(Text, fontDefinition);

            textSprite = new CCSprite(texture);
            textSprite.IsAntialiased = isAntialiased;
            textSprite.BlendFunc = BlendFunc;
            textSprite.AnchorPoint = CCPoint.AnchorLowerLeft;
            base.ContentSize = textSprite.ContentSize;

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

            var contentScaleFactor = DefaultTexelToContentSizeRatios;

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
                    reusedLetter.ContentSize = reusedRect.Size / contentScaleFactor;

                    reusedLetter.Position = lettersInfo[ctr].Position;

                    index = TextureAtlas.TotalQuads;
                    var letterInfo = lettersInfo[ctr];
                    letterInfo.AtlasIndex = index;
                    lettersInfo[ctr] = letterInfo;

                    InsertGlyph(reusedLetter, index);
                }     
            }
            quadCommand.Quads = TextureAtlas.Quads.Elements;
            quadCommand.QuadCount = TextureAtlas.Quads.Count;

        }

        const float MAX_BOUNDS = 8388608;
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

            if (insetBounds.Width <= 0) 
            {
                insetBounds.Width = MAX_BOUNDS;
                layoutAvailable = false;
            }

            if (insetBounds.Height <= 0) 
            {
                insetBounds.Height = MAX_BOUNDS;
                layoutAvailable = false;
            }


            var contentScaleFactorWidth = CCLabel.DefaultTexelToContentSizeRatios.Width;
            var contentScaleFactorHeight = CCLabel.DefaultTexelToContentSizeRatios.Height;
            var scaleX = ScaleX;
            var scaleY = ScaleY;

            List<CCTLLine> lineList = new List<CCTLLine>();

            var boundingSize = CCSize.Zero;

            while (start < length)
            {

                // Now we ask the typesetter to break off a line for us.
                // This also will take into account line feeds embedded in the text.
                //  Example: "This is text \n with a line feed embedded inside it"
                int count = typesetter.SuggestLineBreak(start, insetBounds.Width);
                var line = typesetter.GetLine(start, start + count);

                lineList.Add(line);

                if (line.Bounds.Width > boundingSize.Width)
                    boundingSize.Width = line.Bounds.Width;
                
                boundingSize.Height += line.Bounds.Height;

                start += count;
            }

            if (!layoutAvailable)
            {
                if (insetBounds.Width == MAX_BOUNDS)
                {
                    insetBounds.Width = boundingSize.Width;
                }
                if (insetBounds.Height == MAX_BOUNDS)
                {
                    insetBounds.Height = boundingSize.Height;
                }
            }

            // Calculate our vertical starting position
            var totalHeight = lineList.Count * LineHeight;
            var nextFontPositionY = totalHeight;

            if (layoutAvailable && labelDimensions.Height > 0)
            {
                var labelHeightPixel = labelDimensions.Height / scaleY * contentScaleFactorHeight;
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
            float longestLine = 0;

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
                var flushWidth = (layoutAvailable) ? insetBounds.Width / scaleX : boundingSize.Width;
                var flush = line.PenOffsetForFlush(flushFactor, flushWidth) ;

                foreach (var glyph in gliphRun)
                {
                    var letterPosition = glyph.Position;
                    var letterDef = glyph.Definition;
                    lastCharWidth = letterDef.Width * contentScaleFactorWidth;

                    letterPosition.X += flush;
                    letterPosition.Y = (nextFontPositionY - letterDef.YOffset) / contentScaleFactorHeight;

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

            if (labelDimensions.Height > 0)
            {
                tmpSize.Height = labelDimensions.Height * contentScaleFactorHeight;
            }

            // We use base here so we do not trigger an update internally.
            base.ContentSize = tmpSize / CCLabel.DefaultTexelToContentSizeRatios;

            lineList.Clear();

            CCRect uvRect;
            CCSprite letterSprite;

            for (int c = 0; c < Children.Count; c++) 
            {
                letterSprite = (CCSprite)Children[c];
                if (letterSprite == null)
                    continue;

                int tag = letterSprite.Tag;
                if(tag >= length)
                {
                    RemoveChild(letterSprite, true);
                }
                else if(tag >= 0)
                {
                    uvRect = lettersInfo[tag].Definition.Subrect;
                    letterSprite.TextureRectInPixels = uvRect;
                    letterSprite.ContentSize = uvRect.Size;
                }
            }

            UpdateQuads();
            UpdateColor();

        }

        public new CCNode this[int letterIndex]
        {
            get 
            { 
                if (LabelType == CCLabelType.SystemFont)
                {
                    return null;
                }

                if (IsDirty)
                {
                    UpdateContent();
                }

                var contentScaleFactor = DefaultTexelToContentSizeRatios;
                if (letterIndex < lettersInfo.Count)
                {
                    var letter = lettersInfo[letterIndex];

                    if(! letter.Definition.IsValidDefinition)
                        return null;

                    var sp = (this.GetChildByTag(letterIndex)) as CCSprite;

                    if (sp == null)
                    {
                        var uvRect = letter.Definition.Subrect;

                        sp = new CCSprite(FontAtlas.GetTexture(letter.Definition.TextureID), uvRect);

                        // The calculations for untrimmed size already take into account the
                        // content scale factor so here we back it out or the sprite shows
                        // up with the incorrect ratio.
                        sp.UntrimmedSizeInPixels = uvRect.Size * contentScaleFactor;

                        // Calc position offset taking into account the content scale factor.
                        var offset = new CCSize((uvRect.Size.Width * 0.5f) / contentScaleFactor.Width,
                            (uvRect.Size.Height * 0.5f) / contentScaleFactor.Height);

                        // apply the offset to the letter position
                        sp.PositionX = letter.Position.X + offset.Width;
                        sp.PositionY = letter.Position.Y - offset.Height;

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

            sprite.UpdateLocalTransformedSpriteTextureQuads();

        }

        #endregion

        protected void IncreaseAtlasCapacity()
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

        public override void Visit(ref CCAffineTransform parentWorldTransform)
        {

            if (!Visible || string.IsNullOrEmpty(Text))
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

            var worldTransform = CCAffineTransform.Identity;
            var affineLocalTransform = AffineLocalTransform;
            CCAffineTransform.Concat(ref affineLocalTransform, ref parentWorldTransform, out worldTransform);

            if (textSprite != null)
                textSprite.Visit(ref worldTransform);
            else
                VisitRenderer(ref worldTransform);
        }

        protected override void VisitRenderer(ref CCAffineTransform worldTransform)
        {

            // Optimization: Fast Dispatch  
            if (TextureAtlas == null || TextureAtlas.TotalQuads == 0)
            {
                return;
            }

            // Loop through each of our children nodes that may have actions attached.
            foreach(CCSprite child in Children)
            {
                if (child.Tag >= 0)
                {
                    child.UpdateLocalTransformedSpriteTextureQuads();
                }
            }
                
            quadCommand.GlobalDepth = worldTransform.Tz;
            quadCommand.WorldTransform = worldTransform;
                
            Renderer.AddCommand(quadCommand);
        }
    }

}
