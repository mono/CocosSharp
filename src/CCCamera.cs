/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
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
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    enum CCCameraProjection
    {
        Projection2D,           /// Sets a 2D projection (orthogonal projection)
        Projection3D,           /// Sets a 3D projection with a fovy=60, znear=0.5f and zfar=1500.
        Custom,                 /// Calls "updateProjection" on the projection delegate.
        Default = Projection3D  /// Default projection is 3D projection
    }

    /// <summary>
    /// A CCCamera is used in every CCNode.
    /// Useful to look at the object from different views.
    /// The OpenGL gluLookAt() function is used to locate the camera.
    ///
    /// If the object is transformed by any of the scale, rotation or
    /// position attributes, then they will override the camera.
    /// IMPORTANT: Either your use the camera or the rotation/scale/position properties. You can't use both.
    /// World coordinates won't work if you use the camera.
    ///
    /// Limitations:
    ///  - Some nodes, like CCParallaxNode, CCParticle uses world node coordinates, and they won't work properly if you move them (or any of their ancestors)
    /// using the camera.
    /// - It doesn't work on batched nodes like CCSprite objects when they are parented to a CCSpriteBatchNode object.
    /// - It is recommended to use it ONLY if you are going to create 3D effects. For 2D effects, use the action CCFollow or position/scale/rotate.
    /// </summary>
    public class CCCamera
    {
        internal event EventHandler OnCameraVisibleBoundsChanged = delegate {};

        CCRect visibleBoundsWorldspace;
        CCPoint targetInWorldspace;
        CCPoint upDirection;

        Matrix viewMatrix;
        Matrix projectionMatrix;


        #region Properties

        internal static float ZEye
        {
            get { return 1.192092896e-07F; }
        }

        public CCRect VisibleBoundsWorldspace
        {
            get { return visibleBoundsWorldspace; }
            set 
            {
                if(visibleBoundsWorldspace != value) 
                {
                    visibleBoundsWorldspace = value;
                    UpdateCameraMatrices();
                }
            }
        }

        public CCPoint CenterInWorldspace
        {
            get { return visibleBoundsWorldspace.Center; }
            set 
            {
                if(visibleBoundsWorldspace.Center != value) 
                {
                    visibleBoundsWorldspace.Origin = new CCPoint(
                        value.X - (float)Math.Floor(visibleBoundsWorldspace.Size.Width / 2.0f),
                        value.Y - (float)Math.Floor(visibleBoundsWorldspace.Size.Height / 2.0f)
                    );

                    UpdateCameraMatrices();
                }
            }
        }

        public CCPoint TargetInWorldspace
        {
            get { return targetInWorldspace; }
            set 
            {
                if(targetInWorldspace != value) 
                {
                    targetInWorldspace = value;
                    UpdateCameraMatrices();
                }
            }
        }

        public CCPoint UpDirection
        {
            get { return upDirection; }
            set 
            {
                if(upDirection != value) 
                {
                    upDirection = value;
                    UpdateCameraMatrices();
                }
            }
        }

        internal Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        internal Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        #endregion Properties


        #region Constructors

        public CCCamera(CCRect visibleBoundsWorldspaceIn, CCPoint targetInWorldspaceIn)
        {
            visibleBoundsWorldspace = visibleBoundsWorldspaceIn;
            targetInWorldspace = targetInWorldspaceIn;
            UpdateCameraMatrices();
        }

        #endregion Constructors


        void UpdateCameraMatrices()
        {
            projectionMatrix = Matrix.CreateOrthographic(
                visibleBoundsWorldspace.Size.Width, visibleBoundsWorldspace.Size.Height, 1024.0f, -1024.0f);

            CCPoint cameraCenter = CenterInWorldspace;
            Vector3 xnaCameraCenter = new Vector3(cameraCenter.X, cameraCenter.Y, 100.0f);
            Vector3 xnaCameraTarget = new Vector3(targetInWorldspace.X, targetInWorldspace.Y, 0.0f);
            Vector3 xnaCameraUpDirection = new Vector3(upDirection.X, upDirection.Y, 0.0f);

            viewMatrix = Matrix.CreateLookAt(xnaCameraCenter, xnaCameraTarget, Vector3.Up);

            OnCameraVisibleBoundsChanged(this, null);
        }
    }
}