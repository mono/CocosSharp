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
    public enum CCCameraProjection
    {
        Projection2D,           // Sets a 2D projection (orthogonal projection)
        Projection3D,           // Sets a 3D projection
        Default = Projection2D  // Default projection is 3D projection
    }

    /// <summary>
    /// A CCCamera is used in every CCNode.
    /// Useful to look at the object from different views.
    ///
    /// If the object is transformed by any of the scale, rotation or
    /// position attributes, then they will override the camera.
    /// IMPORTANT: Either your use the camera or the rotation/scale/position properties. You can't use both.
    /// World coordinates won't work if you use the camera.
    /// </summary>
    public class CCCamera
    {
        static float defaultFieldOfView = (float)Math.PI / 4.0f;
        static float defaultAspectRatio = 1.0f;
        static CCPoint defaultNearAndFarOrthoClipping = new CCPoint (1024f, -1024f);
        static CCPoint defaultNearAndFarPerspClipping = new CCPoint (0.1f, 1000f);

        internal event EventHandler OnCameraVisibleBoundsChanged = delegate {};

        CCCameraProjection cameraProjection;

        float aspectRatio;
        float fieldOfView;

        CCSize orthographicViewSizeWorldspace;

        CCPoint3 centerInWorldspace;
        CCPoint3 targetInWorldspace;
        CCPoint3 upDirection;

        CCPoint nearAndFarOrthographicZClipping;
        CCPoint nearAndFarPerspectiveClipping;

        Matrix viewMatrix;
        Matrix projectionMatrix;


        #region Properties

        internal static float ZEye
        {
            get { return 1.192092896e-07F; }
        }

        public CCCameraProjection Projection
        {
            get { return cameraProjection; }
            set 
            {
                if (cameraProjection != value) 
                {
                    cameraProjection = value;
                    UpdateCameraMatrices();
                }
            }
        }

        public float PerspectiveAspectRatio
        {
            get { return aspectRatio; }
            set 
            {
                if (aspectRatio != value)
                {
                    aspectRatio = value;
                    UpdateCameraMatrices();
                }
            }
        }

        public float PerspectiveFieldOfView
        {
            get { return fieldOfView; }
            set 
            {
                if (fieldOfView != value)
                {
                    fieldOfView = value;
                    UpdateCameraMatrices();
                }
            }
        }

        public CCPoint NearAndFarOrthographicZClipping
        {
            get { return nearAndFarOrthographicZClipping; }
            set 
            {
                if (nearAndFarOrthographicZClipping != value) 
                {
                    nearAndFarOrthographicZClipping = value;
                    UpdateCameraMatrices();
                }
            }
        }

        public CCPoint NearAndFarPerspectiveClipping
        {
            get { return nearAndFarPerspectiveClipping; }
            set 
            {
                if (nearAndFarPerspectiveClipping != value) 
                {
                    nearAndFarPerspectiveClipping = value;
                    UpdateCameraMatrices();
                }
            }
        }


        public CCSize OrthographicViewSizeWorldspace
        {
            get { return orthographicViewSizeWorldspace; }
            set
            {
                if (orthographicViewSizeWorldspace != value) 
                {
                    orthographicViewSizeWorldspace = value;
                    UpdateCameraMatrices();
                }
            }
        }

        public CCPoint3 CenterInWorldspace
        {
            get { return centerInWorldspace; }
            set 
            {
                if(centerInWorldspace != value) 
                {
                    centerInWorldspace = value;
                    UpdateCameraMatrices();
                }
            }
        }

        public CCPoint3 TargetInWorldspace
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

        public CCPoint3 UpDirection
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

        CCCamera(CCPoint3 cameraCenterPositionWorldspaceIn, CCPoint3 targetInWorldspaceIn)
        {
            targetInWorldspace = targetInWorldspaceIn;
            centerInWorldspace = cameraCenterPositionWorldspaceIn;

            nearAndFarOrthographicZClipping = defaultNearAndFarOrthoClipping;
            nearAndFarPerspectiveClipping = defaultNearAndFarPerspClipping;
            upDirection = new CCPoint3 (Vector3.Up.X, Vector3.Up.Y, Vector3.Up.Z);

            fieldOfView = defaultFieldOfView;
            aspectRatio = defaultAspectRatio;
        }

        public CCCamera(CCSize orthographicViewSizeWorldspaceIn, CCPoint3 cameraCenterPositionWorldspaceIn, CCPoint3 targetInWorldspaceIn)
            : this(cameraCenterPositionWorldspaceIn, targetInWorldspaceIn)
        {
            cameraProjection = CCCameraProjection.Projection2D;

            orthographicViewSizeWorldspace = orthographicViewSizeWorldspaceIn;

            UpdateCameraMatrices();
        }

        public CCCamera(float perspectiveFieldOfViewIn, float perspectiveAspectRatioIn, CCPoint3 cameraCenterPositionWorldspaceIn, CCPoint3 targetInWorldspaceIn)
            : this(cameraCenterPositionWorldspaceIn, targetInWorldspaceIn)
        {
            cameraProjection = CCCameraProjection.Projection3D;

            fieldOfView = perspectiveFieldOfViewIn;
            aspectRatio = perspectiveAspectRatioIn;

            UpdateCameraMatrices();
        }

        #endregion Constructors


        void UpdateCameraMatrices()
        {
            Vector3 xnaCameraCenter = new Vector3(centerInWorldspace.X, centerInWorldspace.Y, centerInWorldspace.Z);
            Vector3 xnaCameraTarget = new Vector3(targetInWorldspace.X, targetInWorldspace.Y, targetInWorldspace.Z);
            Vector3 xnaCameraUpDirection = new Vector3(upDirection.X, upDirection.Y, upDirection.Z);

            if (Projection == CCCameraProjection.Projection2D) 
            {
                projectionMatrix = Matrix.CreateOrthographic (
                    orthographicViewSizeWorldspace.Width, orthographicViewSizeWorldspace.Height, 
                    nearAndFarOrthographicZClipping.X, nearAndFarOrthographicZClipping.Y);
            } 
            else if (Projection == CCCameraProjection.Projection3D) 
            {
                projectionMatrix = Matrix.CreatePerspective(
                    fieldOfView,
                    aspectRatio,
                    nearAndFarPerspectiveClipping.X, nearAndFarPerspectiveClipping.Y
                );
            }

            viewMatrix = Matrix.CreateLookAt(xnaCameraCenter, xnaCameraTarget, xnaCameraUpDirection);

            OnCameraVisibleBoundsChanged(this, null);
        }
    }
}