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
    public class CCLayer : CCNode, ICCAccelerometerDelegate
    {
        // ivars
        bool isAccelerometerEnabled;
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

        public bool AccelerometerEnabled
        {
            get 
            { 
                #if !PSM
                return isAccelerometerEnabled; 
                #else
                return(false);
                #endif
            }
            set 
            {
                #if !PSM &&!NETFX_CORE
                if (value != isAccelerometerEnabled)
                {
                    isAccelerometerEnabled = value;

                    if (m_bRunning)
                    {
                        CCDirector pDirector = CCDirector.SharedDirector;
                        pDirector.Accelerometer.SetDelegate(value ? this : null);
                    }
                }
                #else
                isAccelerometerEnabled = false;
                #endif
            }
        }

        #endregion Properties


        #region Constructors

        public CCLayer() : this(CCClipMode.None)
        {
        }

        public CCLayer(CCClipMode clipMode)
        {
            ChildClippingMode = clipMode;
            TouchMode = CCTouchMode.AllAtOnce;

            AnchorPoint = new CCPoint(0.5f, 0.5f);
            IgnoreAnchorPointForPosition = true;

            CCDirector director = CCDirector.SharedDirector;
            if (director != null)
            {
                ContentSize = director.WinSize;
                isAccelerometerEnabled = false;
            }
        }

        void InitClipping()
        {
            if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
            {
                if (renderTexture == null || renderTexture.ContentSize.Width < ContentSize.Width 
                    || renderTexture.ContentSize.Height < ContentSize.Height)
                {
                    renderTexture = new CCRenderTexture((int)ContentSize.Width, (int)ContentSize.Height);
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


        public override void OnEnter()
        {
            base.OnEnter();

            if (isAccelerometerEnabled)
            {
                #if !PSM &&!NETFX_CORE
                CCDirector.SharedDirector.Accelerometer.SetDelegate(this);
                #endif
            }
        }

        public override void OnExit()
        {
            if (isAccelerometerEnabled)
            {
                #if !PSM &&!NETFX_CORE
                CCDirector.SharedDirector.Accelerometer.SetDelegate(null);
                #endif
            }

            base.OnExit();
        }

        public virtual void DidAccelerate(CCAcceleration pAccelerationValue)
        {
        }

        public override void Visit()
        {
            // quick return if not visible
            if (!Visible)
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

                // draw children zOrder < 0
                foreach (CCNode child in arrayData)
                {
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
                foreach (CCNode child in arrayData)
                {
                    child.Visit();
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

            if (ChildClippingMode == CCClipMode.Bounds)
            {
                // We always clip to the bounding box
                var rect = new CCRect(0, 0, ContentSize.Width, ContentSize.Height);
                var bounds = CCAffineTransform.Transform(rect, NodeToWorldTransform());

                var winSize = CCDirector.SharedDirector.WinSize;

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

        /**
     * retract what's done in beforeDraw so that there's no side effect to
     * other nodes.
     */
        private void AfterDraw()
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


    }
}