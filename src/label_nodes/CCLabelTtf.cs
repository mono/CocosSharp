using Microsoft.Xna.Framework.Graphics;
using System;

namespace CocosSharp
{
    [Obsolete("Use CCLabel instead.")]
    public class CCLabelTtf : CCSprite, ICCTextContainer
    {
        float fontSize;
        CCTextAlignment horzTextAlignment;
        string fontName;
        CCSize dimensions;
        CCVerticalTextAlignment vertTextAlignment;

        protected string labelText = String.Empty;


        // Static properties

        public new static float DefaultTexelToContentSizeRatio
        {
            set { DefaultTexelToContentSizeRatios = new CCSize(value, value); }
        }

        public new static CCSize DefaultTexelToContentSizeRatios { get; set; }


        // Instance properties

        public string FontName
        {
            get { return fontName; }
            set
            {
                if (fontName != value)
                {
                    fontName = value;
                    if (labelText.Length > 0)
                    {
                        Refresh();
                    }
                }
            }
        }

        public float FontSize
        {
            get { return fontSize; }
            set
            {
                if (fontSize != value)
                {
                    fontSize = value;
                    if (labelText.Length > 0)
                    {
                        Refresh();
                    }
                }
            }
        }

        public CCSize Dimensions
        {
            get { return dimensions; }
            set
            {
                if (!dimensions.Equals(value))
                {
                    dimensions = value;
                    if (labelText.Length > 0)
                    {
                        Refresh();
                    }
                }
            }
        }

        public CCVerticalTextAlignment VerticalAlignment
        {
            get { return vertTextAlignment; }
            set
            {
                if (vertTextAlignment != value)
                {
                    vertTextAlignment = value;
                    if (labelText.Length > 0)
                    {
                        Refresh();
                    }
                }
            }
        }

        public CCTextAlignment HorizontalAlignment
        {
            get { return horzTextAlignment; }
            set
            {
                if (horzTextAlignment != value)
                {
                    horzTextAlignment = value;
                    if (labelText.Length > 0)
                    {
                        Refresh();
                    }
                }
                    }
                }


        #region Constructors

        static CCLabelTtf()
        {
            DefaultTexelToContentSizeRatios = CCSize.One;
        }

        public CCLabelTtf () : this("", "Helvetica", 12.0f)
        {
        }

        public CCLabelTtf (string text, string fontName, float fontSize) : 
        this (text, fontName, fontSize, CCSize.Zero, CCTextAlignment.Center,
            CCVerticalTextAlignment.Top)
        { 
        }

        public CCLabelTtf (string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment) :
        this (text, fontName, fontSize, dimensions, hAlignment, CCVerticalTextAlignment.Top)
        { 
        }

        public CCLabelTtf (string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment)
        {
            InitCCLabelTTF(text, fontName, fontSize, dimensions, hAlignment, vAlignment);
        }

        private void InitCCLabelTTF(string text, string fontName, float fontSize, 
            CCSize dimensions, CCTextAlignment hAlignment,
            CCVerticalTextAlignment vAlignment)
        {
            // shader program
            //this->setShaderProgram(CCShaderCache::sharedShaderCache()->programForKey(SHADER_PROGRAM));

            this.dimensions = new CCSize(dimensions.Width, dimensions.Height);
            horzTextAlignment = hAlignment;
            vertTextAlignment = vAlignment;
			if (fontName == null)
				fontName = "arial";
			this.fontName = (!string.IsNullOrEmpty(fontName.Trim())) ? fontName : "arial";
            this.fontSize = fontSize;

            this.Text = text;          
        }

        #endregion Constructors


        internal void Refresh()
        {
            UpdateTexture();
        }

        #region ICCLabelProtocol Members

/*
 * This is where the texture should be created, but it messes with the drawing 
 * of the object tree
 * 
        public override void Draw()
        {
            if (Dirty)
            {
                updateTexture();
                Dirty = false;
            }
            base.Draw();
        }
*/
        public string Text
        {
            get { return labelText; }
            set
            {
                // This is called in the update() call, so it should not do any drawing ...
                if (labelText != value)
                {
                    labelText = value;
                    UpdateTexture();
                }
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("FontName:{0}, FontSize:{1}", fontName, fontSize);
        }

        void UpdateTexture()
        {

            CCTexture2D tex;

            // Dump the old one
            if (Texture != null)
            {
                Texture.Dispose();
            }

            // let system compute label's width or height when its value is 0
            // refer to cocos2d-x issue #1430
            tex = new CCTexture2D(
                labelText,
				dimensions,
                horzTextAlignment,
                vertTextAlignment,
                fontName,
                fontSize);

//#if MACOS || IPHONE || IOS
//			// There was a problem loading the text for some reason or another if result is not true
//			// For MonoMac and IOS Applications we will try to create a Native Label automatically
//			// If the font is not found then a default font will be selected by the device and used.
//			if (!result && !string.IsNullOrEmpty(m_pString)) 
//			{
//				tex = CCLabelUtilities.CreateLabelTexture (m_pString,
//				                                           CCMacros.CCSizePointsToPixels (m_tDimensions),
//				                                           m_hAlignment,
//				                                           m_vAlignment,
//				                                           m_pFontName,
//				                                           m_fFontSize * CCMacros.CCContentScaleFactor (),
//				                                           new CCColor4B(Microsoft.Xna.Framework.Color.White) );
//			}
//#endif
            Texture = tex;

            CCRect rect = CCRect.Zero;
            rect.Size = Texture.ContentSizeInPixels;
            TextureRectInPixels = rect;
            ContentSize = rect.Size / DefaultTexelToContentSizeRatios;
        }
    }
}