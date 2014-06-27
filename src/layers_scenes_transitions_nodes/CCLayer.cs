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
        CCRect saveScissorRect;
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
                    InitClipping();
                }
            }
        }

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                base.ContentSize = value;
                InitClipping();
            }
        }

        #endregion Properties


        #region Constructors

        public CCLayer(CCClipMode clipMode) : base()
        {
            ChildClippingMode = clipMode;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            IgnoreAnchorPointForPosition = true;
        }

        public CCLayer() : this(CCClipMode.None)
        {
        }

        void InitClipping()
        {
            if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget && Director !=null)
            {
                if (renderTexture == null || renderTexture.ContentSize.Width < ContentSize.Width 
                    || renderTexture.ContentSize.Height < ContentSize.Height)
                {
                    renderTexture = new CCRenderTexture((int)ContentSize.Width, (int)ContentSize.Height, Director.ContentScaleFactor);
                    renderTexture.Sprite.AnchorPoint = new CCPoint(0, 0);
                }
                renderTexture.Sprite.TextureRect = new CCRect(0, 0, ContentSize.Width, ContentSize.Height);
            }
            else
            {
                renderTexture = null;
            }
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

			if (Director != null && (ContentSize.Width == 0.0f || ContentSize.Height == 0.0f))
            {
                ContentSize = Director.WindowSizeInPoints;
            }
        }

        #endregion Setup content


        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {

            base.OnExit();
        }

        #region Visiting and drawing

        public override void Visit()
        {
            if (!Visible || Director == null)
            {
                return;
            }
            if (ChildClippingMode == CCClipMode.None)
            {
                base.Visit();
                return;
            }

            CCDrawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
                Grid.BeforeDraw();
                TransformAncestors();
            }

            Transform();

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

            CCDrawManager.PopMatrix();
        }

        void BeforeDraw()
        {
            noDrawChildren = false;

            if (ChildClippingMode == CCClipMode.Bounds && Director != null)
            {
                // We always clip to the bounding box
                CCSize contentSize = ContentSize;
                var rect = new CCRect(0, 0, contentSize.Width, contentSize.Height);
                var bounds = CCAffineTransform.Transform(rect, NodeToWorldTransform);

                var winSize = Director.WindowSizeInPoints;

                CCRect prevScissorRect;
                if (CCDrawManager.ScissorRectEnabled)
                {
                    prevScissorRect = CCDrawManager.ScissorRect;
                }
                else
                {
                    prevScissorRect = new CCRect(0, 0, winSize.Width, winSize.Height);
                }

                if (!bounds.IntersectsRect(prevScissorRect))
                {
                    noDrawChildren = true;
                    return;
                }

                float minX = Math.Max(bounds.MinX, prevScissorRect.MinX);
                float minY = Math.Max(bounds.MinY, prevScissorRect.MinY);
                float maxX = Math.Min(bounds.MaxX, prevScissorRect.MaxX);
                float maxY = Math.Min(bounds.MaxY, prevScissorRect.MaxY);

                if (CCDrawManager.ScissorRectEnabled)
                {
                    restoreScissor = true;
                }
                else
                {
                    CCDrawManager.ScissorRectEnabled = true;
                }

                saveScissorRect = prevScissorRect;

                CCDrawManager.SetScissorInPoints(minX, minY, maxX - minX, maxY - minY);
            }
            else if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
            {
                saveScissorRect = CCDrawManager.ScissorRect;
                restoreScissor = CCDrawManager.ScissorRectEnabled;

                CCDrawManager.ScissorRectEnabled = false;

                CCDrawManager.PushMatrix();
                CCDrawManager.SetIdentityMatrix();

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

                    CCDrawManager.PopMatrix();
                }

                if (restoreScissor)
                {
                    CCDrawManager.SetScissorInPoints(
                        saveScissorRect.Origin.X, saveScissorRect.Origin.Y,
                        saveScissorRect.Size.Width, saveScissorRect.Size.Height);

                    CCDrawManager.ScissorRectEnabled = true;

                    restoreScissor = false;
                }
                else
                {
                    CCDrawManager.ScissorRectEnabled = false;
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