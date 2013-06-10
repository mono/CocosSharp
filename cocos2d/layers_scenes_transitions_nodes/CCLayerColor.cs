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

namespace Cocos2D
{
    /// <summary>
    /// CCLayerColor is a subclass of CCLayer that implements the CCRGBAProtocol protocol
    /// All features from CCLayer are valid, plus the following new features:
    /// - opacity
    /// - RGB colors
    /// </summary>
    public class CCLayerColor : CCLayerRGBA, ICCBlendProtocol
    {
        protected VertexPositionColor[] m_pSquareVertices = new VertexPositionColor[4];
        protected CCBlendFunc m_tBlendFunc;

        public CCLayerColor()
        {
            Init();
        }

        /// <summary>
        /// creates a CCLayer with color, width and height in Points
        /// </summary>
        public CCLayerColor (CCColor4B color, float width, float height) : this()
        {
            InitWithColor(color, width, height);
        }
        
        /// <summary>
        /// creates a CCLayer with color. Width and height are the window size. 
        /// </summary>
        public CCLayerColor (CCColor4B color) : this()
        {
            InitWithColor(color);
        }

        /// <summary>
        /// override contentSize
        /// </summary>
        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                //1, 2, 3, 3
                m_pSquareVertices[1].Position.X = value.Width;
                m_pSquareVertices[2].Position.Y = value.Height;
                m_pSquareVertices[3].Position.X = value.Width;
                m_pSquareVertices[3].Position.Y = value.Height;

                //m_pSquareVertices[1].Position.X = value.Height;
                //m_pSquareVertices[2].Position.Y = value.Width;
                //m_pSquareVertices[3].Position.X = value.Width;
                //m_pSquareVertices[3].Position.Y = value.Height;

                base.ContentSize = value;
            }
        }

        #region InitWithXXX

        public override bool Init()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            return InitWithColor(new CCColor4B(0, 0, 0, 0), s.Width, s.Height);
        }

        /// <summary>
        /// initializes a CCLayer with color
        /// </summary>
        public virtual bool InitWithColor(CCColor4B color)
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            return InitWithColor(color, s.Width, s.Height);
        }

        /// <summary>
        /// initializes a CCLayer with color, width and height in Points
        /// </summary>
        public virtual bool InitWithColor(CCColor4B color, float width, float height)
        {
            base.Init();

            // default blend function
            m_tBlendFunc = CCBlendFunc.NonPremultiplied;

            _displayedColor.R = _realColor.R = color.R;
            _displayedColor.G = _realColor.G = color.G;
            _displayedColor.B = _realColor.B = color.B;
            _displayedOpacity = _realOpacity = color.A;

            for (int i = 0; i < m_pSquareVertices.Length; i++)
            {
                m_pSquareVertices[i].Position.X = 0.0f;
                m_pSquareVertices[i].Position.Y = 0.0f;
            }

            UpdateColor();
            ContentSize = new CCSize(width, height);
            
            return true;
        }

        #endregion

        #region changesize

        /// <summary>
        /// change width in Points
        /// </summary>
        /// <param name="w"></param>
        public void ChangeWidth(float w)
        {
            ContentSize = new CCSize(w, m_obContentSize.Height);
        }

        /// <summary>
        /// change height in Points
        /// </summary>
        /// <param name="h"></param>
        public void ChangeHeight(float h)
        {
            ContentSize = new CCSize(m_obContentSize.Width, h);
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

        #endregion

        #region ICCBlendProtocol Members

        /// <summary>
        /// BlendFunction. Conforms to CCBlendProtocol protocol 
        /// </summary>
        public virtual CCBlendFunc BlendFunc
        {
            get { return m_tBlendFunc; }
            set { m_tBlendFunc = value; }
        }

        #endregion

        #region RGBA Protocol

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

        #endregion

        public override void Draw()
        {
            CCDrawManager.TextureEnabled = false;
            CCDrawManager.BlendFunc(m_tBlendFunc);
            CCDrawManager.DrawPrimitives(PrimitiveType.TriangleStrip,  m_pSquareVertices, 0, 2);
        }

        protected virtual void UpdateColor()
        {
            var color = new Color(_displayedColor.R / 255.0f, _displayedColor.G / 255.0f, _displayedColor.B / 255.0f, _displayedOpacity / 255.0f);

            m_pSquareVertices[0].Color = color;
            m_pSquareVertices[1].Color = color;
            m_pSquareVertices[2].Color = color;
            m_pSquareVertices[3].Color = color;
        }
    }

}