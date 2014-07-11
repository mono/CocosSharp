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
using System.Diagnostics;
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


        #region Properties

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

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                base.ContentSize = value;
                UpdateClipping();
            }
        }

        #endregion Properties


        #region Constructors

        public CCLayer(CCClipMode clipMode) : base()
        {
            ChildClippingMode = clipMode;
            IgnoreAnchorPointForPosition = true;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
        }

        public CCLayer() : this(CCClipMode.None)
        {
        }

        void UpdateClipping()
        {
            if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget && Scene !=null)
            {
                CCRect bounds = Camera.VisibleBoundsWorldspace;
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


        #region CCNode - scene layout callbacks

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            if(ContentSize == CCSize.Zero)
                ContentSize = Scene.VisibleBoundsWorldspace.Size;
        }

        protected override void VisibleBoundsChanged()
        {
            base.VisibleBoundsChanged();

            UpdateClipping();
        }

        protected override void ViewportChanged()
        {
            base.ViewportChanged();

            UpdateClipping();
        }

        #endregion CCNode - scene layout callbacks


        #region Visiting and drawing

        public override void Visit()
        {
            if (!Visible || Window == null)
            {
                return;
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

            Transform ();

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
            CCRect visibleBounds = Camera.VisibleBoundsWorldspace;
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
    }
}