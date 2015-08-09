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
        bool verticesPositionDirty;
        internal VertexPositionColor[] SquareVertices = new VertexPositionColor[4];

        CCCustomCommand layerRenderCommand;


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

        public CCLayerColor(CCColor4B? color = null) 
            : this(CCLayer.DefaultCameraProjection, color)
        {
        }

        public CCLayerColor(CCSize visibleBoundsDimensions, CCColor4B? color = null) 
            : this(visibleBoundsDimensions, CCLayer.DefaultCameraProjection, color)
        {
        }

        public CCLayerColor(CCSize visibleBoundsDimensions, CCCameraProjection projection, CCColor4B? color = null)
            : this(new CCCamera(projection, visibleBoundsDimensions), color)
        {
        }

        public CCLayerColor(CCCamera camera, CCColor4B? color = null) 
            : base(camera)
        {
            SetupCCLayerColor(color);
        }

        public CCLayerColor(CCCameraProjection cameraProjection, CCColor4B? color = null) 
            : base(cameraProjection)
        {
            SetupCCLayerColor(color);
        }

        void SetupCCLayerColor(CCColor4B? color = null)
        {
            layerRenderCommand = new CCCustomCommand(RenderLayer);

            var setupColor = (color.HasValue) ? color.Value : CCColor4B.Transparent;
            DisplayedColor = RealColor = new CCColor3B(setupColor.R, setupColor.G, setupColor.B);
            DisplayedOpacity = RealOpacity = setupColor.A;
            BlendFunc = CCBlendFunc.NonPremultiplied;
            UpdateColor();
        }

        #endregion Constructors

        protected override void AddedToScene()
        {
            base.AddedToScene();

            verticesPositionDirty = true;
        }

        protected override void VisibleBoundsChanged()
        {
            base.VisibleBoundsChanged();

            verticesPositionDirty = true;
        }

        protected override void ViewportChanged()
        {
            base.ViewportChanged();

            verticesPositionDirty = true;
        }

        protected override void VisitRenderer(ref CCAffineTransform worldTransform)
        {
            if(Camera != null)
            {
                layerRenderCommand.GlobalDepth = worldTransform.Tz;
                layerRenderCommand.WorldTransform = worldTransform;
                Renderer.AddCommand(layerRenderCommand);
            }
        }

        void RenderLayer()
        {
            if(Camera != null)
            {
                if (verticesPositionDirty)
                    UpdateVerticesPosition();
                
                var drawManager = DrawManager;

                bool depthTest = drawManager.DepthTest;

                // We're drawing a quad at z=0
                // We need to ensure depth testing is off so that the layer color doesn't obscure anything
                drawManager.DepthTest = false;
                drawManager.TextureEnabled = false;
                drawManager.BlendFunc(BlendFunc);
                drawManager.DrawPrimitives(PrimitiveType.TriangleStrip,  SquareVertices, 0, 2);
                drawManager.DepthTest = depthTest;
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
            CCRect visibleBounds = VisibleBoundsWorldspace;

            //1, 2, 3, 3
            SquareVertices[0].Position.X = visibleBounds.Origin.X;
            SquareVertices[0].Position.Y = visibleBounds.Origin.Y;

            SquareVertices[1].Position.X = SquareVertices[0].Position.X + visibleBounds.Size.Width;
            SquareVertices[1].Position.Y = SquareVertices[0].Position.Y;

            SquareVertices[2].Position.X = SquareVertices[0].Position.X;
            SquareVertices[2].Position.Y = SquareVertices[0].Position.Y + visibleBounds.Size.Height;

            SquareVertices[3].Position.X = SquareVertices[0].Position.X + visibleBounds.Size.Width;
            SquareVertices[3].Position.Y = SquareVertices[0].Position.Y + visibleBounds.Size.Height;

            verticesPositionDirty = false;
        }
    }

}