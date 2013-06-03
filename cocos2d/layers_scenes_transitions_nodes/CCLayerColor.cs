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
    public class CCLayerColor : CCLayer, ICCRGBAProtocol, ICCBlendProtocol
    {
        protected VertexPositionColor[] m_pVertices = new VertexPositionColor[4];
        private VertexBuffer m_pVertexBuffer;
        protected bool m_bChanged;

        public CCLayerColor()
        {
            m_cOpacity = 0;
            m_tColor = new CCColor3B(0, 0, 0);

            // default blend function
            m_tBlendFunc = new CCBlendFunc(CCMacros.CCDefaultSourceBlending, CCMacros.CCDefaultDestinationBlending);
            Init ();

        }

        /// <summary>
        /// creates a CCLayer with color, width and height in Points
        /// </summary>
        public CCLayerColor (CCColor4B color, float width, float height) : this()
        {
            InitWithColorWidthHeight(color, width, height);
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
                m_pVertices[0].Position = new Vector3(0, value.Height, 0);
                m_pVertices[1].Position = new Vector3(value.Width, value.Height, 0);
                m_pVertices[2].Position = new Vector3(0, 0, 0);
                m_pVertices[3].Position = new Vector3(value.Width, 0, 0);

                base.ContentSize = value;

                m_bChanged = true;
            }
        }

        #region InitWithXXX


        /// <summary>
        ///  initializes a CCLayer with color, width and height in Points
        /// </summary>
        public virtual bool InitWithColorWidthHeight(CCColor4B color, float width, float height)
        {
            // default blend function
            m_tBlendFunc.Source = CCMacros.CCDefaultSourceBlending;
            m_tBlendFunc.Destination = CCMacros.CCDefaultDestinationBlending;

            m_tColor.R = color.R;
            m_tColor.G = color.G;
            m_tColor.B = color.B;
            m_cOpacity = color.A;

            UpdateColor();
            
            ContentSize = new CCSize(width, height);

            return true;
        }

        /// <summary>
        /// initializes a CCLayer with color. Width and height are the window size.
        /// </summary>
        public virtual bool InitWithColor(CCColor4B color)
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            InitWithColorWidthHeight(color, s.Width, s.Height);
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
            ContentSize = new CCSize(w, m_tContentSize.Height);
        }

        /// <summary>
        /// change height in Points
        /// </summary>
        /// <param name="h"></param>
        public void ChangeHeight(float h)
        {
            ContentSize = new CCSize(m_tContentSize.Width, h);
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

        #region ICCRGBAProtocol

        protected byte m_cOpacity;
        protected CCBlendFunc m_tBlendFunc;
        protected CCColor3B m_tColor;

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

        #region ICCRGBAProtocol Members

        public virtual byte Opacity
        {
            get { return m_cOpacity; }
            set
            {
                m_cOpacity = value;
                UpdateColor();
            }
        }

        public virtual CCColor3B Color
        {
            get { return m_tColor; }
            set
            {
                m_tColor = value;
                UpdateColor();
            }
        }

        public bool IsOpacityModifyRGB
        {
            get { 
                return false; 
            }
            set { }
        }

        #endregion

        #endregion

        public override void Draw()
        {
            if (m_pVertexBuffer == null)
            {
                m_pVertexBuffer = new VertexBuffer(CCDrawManager.graphicsDevice, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly);
            }

            if (m_bChanged)
            {
                m_pVertexBuffer.SetData(m_pVertices);
                m_bChanged = false;
            }

            CCDrawManager.BindTexture((CCTexture2D)null);
            CCDrawManager.BlendFunc(m_tBlendFunc);
            CCDrawManager.DrawQuadsBuffer(m_pVertexBuffer, 0, 1);
        }

        protected virtual void UpdateColor()
        {
#if CC_OPTIMIZE_BLEND_FUNC_FOR_PREMULTIPLIED_ALPHA
            var color = new Color(m_tColor.R, m_tColor.G, m_tColor.B) * (m_cOpacity / 255.0f);
#else
            var color = new Color(m_tColor.R / 255.0f, m_tColor.G / 255.0f, m_tColor.B / 255.0f, m_cOpacity / 255.0f);
#endif

            m_pVertices[0].Color = m_pVertices[1].Color = m_pVertices[2].Color = m_pVertices[3].Color = color;

            m_bChanged = true;
        }
    }

}