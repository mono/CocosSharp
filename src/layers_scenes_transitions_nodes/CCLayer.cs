/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
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

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCLayer : CCNode
    {
        bool restoreScissor;
        bool noDrawChildren;

        CCRenderTexture renderTexture;
        CCClipMode childClippingMode;
        CCCamera camera;

        CCRect visibleBoundsWorldspace;

        internal event EventHandler LayerVisibleBoundsChanged = delegate {};


        #region Properties

        public override CCLayer Layer 
        {
            get { return this; }
            internal set 
            {
            }
        }

        public override CCCamera Camera
        {
            get { return camera; }
            set 
            {
                if (camera != value) 
                {
                    // Stop listening to previous camera's event
                    if(camera != null)
                        camera.OnCameraVisibleBoundsChanged -= OnCameraVisibleBoundsChanged;

                    camera = value;

                    camera.OnCameraVisibleBoundsChanged += OnCameraVisibleBoundsChanged;

                    OnCameraVisibleBoundsChanged(camera, null);
                }
            }
        }

        /// <summary>
        /// Set to true if the child drawing should be isolated in their own render target
        /// </summary>
        public CCClipMode ChildClippingMode
        {
            get { return childClippingMode; }
            set
            {
                if (childClippingMode != value)
                {
                    childClippingMode = value;
                    UpdateClipping();
                }
            }
        }

        public new CCRect VisibleBoundsWorldspace
        {
            get { return visibleBoundsWorldspace; }
        }

        public CCSize LayerSizeInPixels
        {
            get { return Viewport != null ? Viewport.ViewportInPixels.Size : CCSize.Zero; }
        }

        // Layer should have fixed size of content
        public override CCSize ContentSize
        {
            get { return VisibleBoundsWorldspace.Size; }
            set
            { }
        }

        public override CCAffineTransform AffineLocalTransform 
        {
            get 
            {
                return CCAffineTransform.Identity;
            }
        }

        #endregion Properties


        #region Constructors

        public CCLayer()
            : this(null)
        {  }

        public CCLayer(CCSize visibleBoundsDimensions, CCClipMode clipMode = CCClipMode.None)
            : this(new CCCamera(visibleBoundsDimensions), clipMode)
        {  }

        public CCLayer(CCCamera camera, CCClipMode clipMode) : base()
        {
            ChildClippingMode = clipMode;
            IgnoreAnchorPointForPosition = true;
			AnchorPoint = CCPoint.AnchorMiddle;
            Camera = camera;
        }

        public CCLayer(CCCamera camera) : this(camera, CCClipMode.None)
        {
        }

        void UpdateClipping()
        {
            if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget && Scene !=null)
            {
                CCRect bounds = VisibleBoundsWorldspace;
                CCRect viewportRect = Viewport.ViewportInPixels;

                renderTexture = new CCRenderTexture(bounds.Size, viewportRect.Size);
                renderTexture.Sprite.AnchorPoint = new CCPoint(0, 0);
            }
            else
            {
                renderTexture = null;
            }
        }

        #endregion Constructors

        #region Content layout

        protected override void AddedToScene()
        {
            base.AddedToScene();

            if(Camera == null)
            {
                Camera = new CCCamera (this.Window.DesignResolutionSize);
            }

        }

        void OnCameraVisibleBoundsChanged(object sender, EventArgs e)
        {
            CCCamera camera = sender as CCCamera;

            if(camera != null && camera == Camera && Scene != null) 
            {
                LayerVisibleBoundsChanged(this, null);
                VisibleBoundsChanged();
            }
        }

        protected override void VisibleBoundsChanged()
        {
            base.VisibleBoundsChanged();

            UpdateVisibleBoundsRect();
            UpdateClipping();
        }

        protected override void ViewportChanged()
        {
            base.ViewportChanged();

            UpdateVisibleBoundsRect();
            UpdateClipping();
        }

        internal void UpdateVisibleBoundsRect()
        {
            if(Viewport == null || Camera == null || Viewport.ViewportInPixels == CCRect.Zero)
                return;

            if (Camera.Projection == CCCameraProjection.Projection2D && Camera.OrthographicViewSizeWorldspace == CCSize.Zero)
                return;

            var viewportRectInPixels = Viewport.ViewportInPixels;

            // Want to determine worldspace bounds relative to camera target
            // Need to first find z screenspace coord of target
            CCPoint3 target = Camera.TargetInWorldspace;
            Vector3 targetVec = new Vector3(0.0f, 0.0f, target.Z);
            targetVec = Viewport.XnaViewport.Project(targetVec, Camera.ProjectionMatrix, Camera.ViewMatrix, Matrix.Identity);

            Vector3 topLeft = new Vector3(viewportRectInPixels.Origin.X, viewportRectInPixels.Origin.Y, targetVec.Z);
            Vector3 topRight = new Vector3(viewportRectInPixels.Origin.X + viewportRectInPixels.Size.Width, viewportRectInPixels.Origin.Y, targetVec.Z);
            Vector3 bottomLeft = new Vector3(viewportRectInPixels.Origin.X, viewportRectInPixels.Origin.Y + viewportRectInPixels.Size.Height, targetVec.Z);
            Vector3 bottomRight = new Vector3(viewportRectInPixels.Origin.X + viewportRectInPixels.Size.Width, viewportRectInPixels.Origin.Y + viewportRectInPixels.Size.Height, targetVec.Z);

            // Convert screen space to worldspace. Note screenspace origin is in topleft part of viewport
            topLeft = Viewport.XnaViewport.Unproject(topLeft, Camera.ProjectionMatrix, Camera.ViewMatrix, Matrix.Identity);
            topRight = Viewport.XnaViewport.Unproject(topRight, Camera.ProjectionMatrix, Camera.ViewMatrix, Matrix.Identity);
            bottomLeft = Viewport.XnaViewport.Unproject(bottomLeft, Camera.ProjectionMatrix, Camera.ViewMatrix, Matrix.Identity);
            bottomRight = Viewport.XnaViewport.Unproject(bottomRight, Camera.ProjectionMatrix, Camera.ViewMatrix, Matrix.Identity);

            CCPoint topLeftPoint = new CCPoint(topLeft.X, topLeft.Y);
            CCPoint bottomLeftPoint = new CCPoint(bottomLeft.X, bottomLeft.Y);
            CCPoint bottomRightPoint = new CCPoint(bottomRight.X, bottomRight.Y);

            visibleBoundsWorldspace = new CCRect(
                bottomLeftPoint.X, bottomLeftPoint.Y, 
                (float)Math.Ceiling(bottomRightPoint.X - bottomLeftPoint.X), 
                (float)Math.Ceiling(topLeftPoint.Y - bottomLeftPoint.Y));

            anchorPointInPoints = new CCPoint(visibleBoundsWorldspace.Size.Width * AnchorPoint.X, visibleBoundsWorldspace.Size.Height * AnchorPoint.Y);
        }

        #endregion Content layout


        #region Visiting and drawing

        public override void Visit()
        {
            if (!Visible || Window == null)
            {
                return;
            }

            // Set camera view/proj matrix even if ChildClippingMode is None
            if(Camera != null)
            {
                Window.DrawManager.ViewMatrix = Camera.ViewMatrix;
                Window.DrawManager.ProjectionMatrix = Camera.ProjectionMatrix;
            }

            if (ChildClippingMode == CCClipMode.None)
            {
                base.Visit();
                return;
            }

            Window.DrawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
                Grid.BeforeDraw();
                TransformAncestors();
            }

            Window.DrawManager.SetIdentityMatrix();

            BeforeDraw();

            if (!noDrawChildren && Children != null)
            {
                SortAllChildren();

                CCNode[] arrayData = Children.Elements;
                int count = Children.Count;
                int i = 0;

                // draw children zOrder < 0
                for (; i < count; i++)
                {
                    CCNode child = arrayData[i];
                    if (child.ZOrder < 0)
                    {
                        child.Visit();
                    }
                    else
                    {
                        break;
                    }
                }

                Draw();

                // draw children zOrder >= 0
                for (; i < count; i++)
                {
                    arrayData[i].Visit();
                }
            }
            else
            {
                Draw();
            }

            AfterDraw();

            if (Grid != null && Grid.Active)
            {
                Grid.AfterDraw(this);
            }

            Window.DrawManager.PopMatrix();
        }

        void BeforeDraw()
        {
            noDrawChildren = false;
            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;
            CCRect viewportRect = Viewport.ViewportInPixels;
            CCDrawManager drawManager = Window.DrawManager;

            if (ChildClippingMode == CCClipMode.Bounds && Window != null)
            {
                drawManager.ScissorRectInPixels = viewportRect;
            }

            else if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
            {
                restoreScissor = Window.DrawManager.ScissorRectEnabled;

                Window.DrawManager.ScissorRectEnabled = false;

                Window.DrawManager.PushMatrix();
                Window.DrawManager.WorldMatrix = Matrix.Identity;

                renderTexture.BeginWithClear(0, 0, 0, 0);
            }
        }

        void AfterDraw()
        {
            if (ChildClippingMode != CCClipMode.None)
            {
                if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
                {
                    renderTexture.End();

                    Window.DrawManager.PopMatrix();
                }

                if (restoreScissor)
                {
                    Window.DrawManager.ScissorRectEnabled = true;
                    restoreScissor = false;
                }
                else
                {
                    Window.DrawManager.ScissorRectEnabled = false;
                }

                if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
                {
                    renderTexture.Sprite.Visit();
                }
            }
        }

        #endregion Visiting and drawing


        #region Unit conversion

        public CCPoint ScreenToWorldspace(CCPoint point)
        {
            CCRect viewportRectInPixels = Viewport.ViewportInPixels;
            CCRect worldBounds = Layer.VisibleBoundsWorldspace;

            point -= viewportRectInPixels.Origin;

            // Note: Screen coordinates have origin in top left corner
            // but world coords have origin in bottom left corner
            // Therefore, Y world ratio is 1 minus Y viewport ratio
            CCPoint worldPointRatio 
            = new CCPoint(point.X / viewportRectInPixels.Size.Width, 1 - (point.Y / viewportRectInPixels.Size.Height));

            return new CCPoint (
                worldBounds.Origin.X + (worldBounds.Size.Width * worldPointRatio.X),
                worldBounds.Origin.Y + (worldBounds.Size.Height * worldPointRatio.Y));
        }

        public CCSize ScreenToWorldspace(CCSize size)
        {
            CCRect viewportRectInPixels = Viewport.ViewportInPixels;
            CCRect worldBounds = Layer.VisibleBoundsWorldspace;

            CCPoint worldSizeRatio = new CCPoint(size.Width / viewportRectInPixels.Size.Width, size.Height / viewportRectInPixels.Size.Height);

            return new CCSize(worldSizeRatio.X * worldBounds.Size.Width, worldSizeRatio.Y * worldBounds.Size.Height);
        }

        public CCSize WorldToScreenspace(CCSize size)
        {
            CCRect visibleBounds = VisibleBoundsWorldspace;
            CCRect viewportInPixels = Viewport.ViewportInPixels;

            CCPoint worldSizeRatio = new CCPoint(size.Width / visibleBounds.Size.Width, size.Height / visibleBounds.Size.Height);

            return new CCSize(worldSizeRatio.X * viewportInPixels.Size.Width, worldSizeRatio.Y * viewportInPixels.Size.Height);
        }

        public CCPoint WorldToScreenspace(CCPoint point)
        {
            CCRect worldBounds = VisibleBoundsWorldspace;
            CCRect viewportRectInPixels = Viewport.ViewportInPixels;

            point -= worldBounds.Origin;

            CCPoint worldPointRatio 
            = new CCPoint(point.X / worldBounds.Size.Width, (point.Y / worldBounds.Size.Height));

            return new CCPoint(viewportRectInPixels.Origin.X + viewportRectInPixels.Size.Width * worldPointRatio.X,
                viewportRectInPixels.Origin.Y + viewportRectInPixels.Size.Height * (1 - worldPointRatio.Y));
        }

        #endregion Unit conversion
    }
}