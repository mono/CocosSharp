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
    public class CCLayerColor : CCLayer, ICCBlendable
    {

		internal VertexPositionColor[] SquareVertices = new VertexPositionColor[4];

        #region Properties

        public virtual CCBlendFunc BlendFunc { get; set; }

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

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                if (ContentSize != value) 
                {
                    base.ContentSize = value;
                    UpdateVerticesPosition();
                }
            }
        }


        #endregion Properties


        #region Constructors

        public CCLayerColor(CCSize size) 
            : this(new CCCamera(size), CCColor4B.Transparent)
        { }

        public CCLayerColor(CCSize size, CCColor4B color) 
            : this(new CCCamera(size), color)
        { }

        public CCLayerColor(CCCamera camera) : this(camera, CCColor4B.Transparent)
        {
        }

        /// <summary>
        /// creates a CCLayer with color, width and height in Points
        /// </summary>
        public CCLayerColor (CCCamera camera, CCColor4B color) : base(camera)
        {
            DisplayedColor = RealColor = new CCColor3B(color.R, color.G, color.B);
            DisplayedOpacity = RealOpacity = color.A;
            BlendFunc = CCBlendFunc.NonPremultiplied;
            UpdateColor();
        }

        #endregion Constructors

        protected override void VisibleBoundsChanged()
        {
            base.VisibleBoundsChanged();

            UpdateVerticesPosition();
        }

        protected override void ViewportChanged()
        {
            base.ViewportChanged();

            UpdateVerticesPosition();
        }

        protected override void Draw()
        {
			if(Camera != null)
			{
				var drawManager = Window.DrawManager;

				drawManager.TextureEnabled = false;
				drawManager.BlendFunc(BlendFunc);
				drawManager.DrawPrimitives(PrimitiveType.TriangleStrip,  SquareVertices, 0, 2);

				drawManager.ViewMatrix = Camera.ViewMatrix;
				drawManager.ProjectionMatrix = Camera.ProjectionMatrix;
			}

        }

        public override void UpdateColor()
        {
            var color = new Color(DisplayedColor.R / 255.0f, DisplayedColor.G / 255.0f, DisplayedColor.B / 255.0f, DisplayedOpacity / 255.0f);

            SquareVertices[0].Color = color;
            SquareVertices[1].Color = color;
            SquareVertices[2].Color = color;
            SquareVertices[3].Color = color;
        }

        void UpdateVerticesPosition()
        {
            CCSize contentSize = ContentSize;

            //1, 2, 3, 3
            SquareVertices[1].Position.X = contentSize.Width;
            SquareVertices[2].Position.Y = contentSize.Height;
            SquareVertices[3].Position.X = contentSize.Width;
            SquareVertices[3].Position.Y = contentSize.Height;
        }
    }

}