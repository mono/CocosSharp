/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    /// <summary>
    /// CCLayerColor is a subclass of CCLayer that implements the CCRGBAProtocol protocol
    /// All features from CCLayer are valid, plus the following new features:
    /// - opacity
    /// - RGB colors
    /// </summary>
    public class CCLayerColor : CCLayerRGBA, ICCBlendable
    {
		internal VertexPositionColor[] SquareVertices = new VertexPositionColor[4];

        #region Properties

        public virtual CCBlendFunc BlendFunc { get; set; }

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                //1, 2, 3, 3
                SquareVertices[1].Position.X = value.Width;
                SquareVertices[2].Position.Y = value.Height;
                SquareVertices[3].Position.X = value.Width;
                SquareVertices[3].Position.Y = value.Height;

                base.ContentSize = value;
            }
        }

        public override CCColor3B Color
        {
            get { return base.Color; }
            set
            {
                base.Color = value;
                UpdateColor();
            }
        }

        public override byte Opacity
        {
            get { return base.Opacity; }
            set
            {
                base.Opacity = value;
                UpdateColor();
            }
        }

        #endregion Properties


        #region Constructors

		public CCLayerColor(CCDirector director=null) : this(new CCColor4B(0, 0, 0, 0), 0.0f, 0.0f, director)
        {
        }

        /// <summary>
        /// creates a CCLayer with color. Width and height are the window size. 
        /// </summary>
		public CCLayerColor (CCColor4B color, CCDirector director=null) : this(color, 0.0f, 0.0f, director)
        {
        }

        /// <summary>
        /// creates a CCLayer with color, width and height in Points
        /// </summary>
		public CCLayerColor (CCColor4B color, float width, float height, CCDirector director=null) : base(director)
        {
            DisplayedColor = new CCColor3B(color.R, color.G, color.B);
            RealColor = DisplayedColor;
            DisplayedOpacity = RealOpacity = color.A;
            BlendFunc = CCBlendFunc.NonPremultiplied;

            if (width == 0.0f || height == 0.0f) 
            {
				CCSize s = Director.WinSize;
                width = s.Width;
                height = s.Height;
            }

            ContentSize = new CCSize(width, height);
            UpdateColor();
        }

        #endregion Constructors


        protected override void Draw()
        {
            CCDrawManager.TextureEnabled = false;
            CCDrawManager.BlendFunc(BlendFunc);
            CCDrawManager.DrawPrimitives(PrimitiveType.TriangleStrip,  SquareVertices, 0, 2);
        }

        protected virtual void UpdateColor()
        {
            var color = new Color(DisplayedColor.R / 255.0f, DisplayedColor.G / 255.0f, DisplayedColor.B / 255.0f, DisplayedOpacity / 255.0f);

            SquareVertices[0].Color = color;
            SquareVertices[1].Color = color;
            SquareVertices[2].Color = color;
            SquareVertices[3].Color = color;
        }


        #region Change size

        /// <summary>
        /// change width in Points
        /// </summary>
        /// <param name="w"></param>
        public void ChangeWidth(float w)
        {
			ContentSize = new CCSize(w, ContentSize.Height);
        }

        /// <summary>
        /// change height in Points
        /// </summary>
        /// <param name="h"></param>
        public void ChangeHeight(float h)
        {
			ContentSize = new CCSize(ContentSize.Width, h);
        }

        /// <summary>
        ///  change width and height in Points
        ///  @since v0.8
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void ChangeWidthAndHeight(float w, float h)
        {
            ContentSize = new CCSize(w, h);
        }

        #endregion Change size
    }

}