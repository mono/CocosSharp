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
        Default = Projection3D  // Default projection is 3D projection
    }

    public class CCCamera
    {
        static float defaultFieldOfView = (float)Math.PI / 3.0f;
        static float defaultAspectRatio = 1.0f;
        static CCNearAndFarClipping defaultNearAndFarOrthoClipping = new CCNearAndFarClipping (1024f, -1024f);
        static CCNearAndFarClipping defaultNearAndFarPerspClipping = new CCNearAndFarClipping (0.1f, 100f);

        internal event EventHandler OnCameraVisibleBoundsChanged = delegate {};

        CCCameraProjection cameraProjection;

        float aspectRatio;
        float fieldOfView;

        CCSize orthographicViewSizeWorldspace;

        CCPoint3 centerInWorldspace;
        CCPoint3 targetInWorldspace;
        CCPoint3 upDirection;

        CCNearAndFarClipping nearAndFarOrthographicZClipping;
        CCNearAndFarClipping nearAndFarPerspectiveClipping;

        Matrix viewMatrix;
        Matrix projectionMatrix;


        #region Properties

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

        public CCNearAndFarClipping NearAndFarOrthographicZClipping
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

        public CCNearAndFarClipping NearAndFarPerspectiveClipping
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

        // Defaults for both 2d and 3d projections
        CCCamera(CCPoint3 targetInWorldspaceIn)
        {
            targetInWorldspace = targetInWorldspaceIn;

            nearAndFarOrthographicZClipping = defaultNearAndFarOrthoClipping;
            nearAndFarPerspectiveClipping = defaultNearAndFarPerspClipping;
            fieldOfView = defaultFieldOfView;
            aspectRatio = defaultAspectRatio;
            upDirection = new CCPoint3 (Vector3.Up.X, Vector3.Up.Y, Vector3.Up.Z);
        }

        public CCCamera(CCCameraProjection projection, CCSize targetVisibleDimensionsWorldspace, CCPoint3 targetInWorldspaceIn)
            : this(targetInWorldspaceIn)
        {
            cameraProjection = projection;

            if (cameraProjection == CCCameraProjection.Projection2D)
            {
                centerInWorldspace = new CCPoint3(targetInWorldspaceIn.X, 
                    targetInWorldspaceIn.Y, 
                    targetInWorldspaceIn.Z + defaultNearAndFarOrthoClipping.Near);

                orthographicViewSizeWorldspace = targetVisibleDimensionsWorldspace;
            }
            else
            {
                aspectRatio = targetVisibleDimensionsWorldspace.Width / targetVisibleDimensionsWorldspace.Height;

                centerInWorldspace 
                    = CalculatePerspectiveCameraCenter(targetVisibleDimensionsWorldspace, targetInWorldspace);


                /* Make sure the far clipping distance is longer than distance frame camera to target
                *  Give ourselves a little extra distance buffer so that there's no clipping when rotating etc
                * 
                *  If users want to customise the near and far clipping bounds, they can do that and then call
                *  UpdatePerspectiveCameraTargetBounds
                */
                nearAndFarPerspectiveClipping.Far 
                    = Math.Max(Math.Abs((centerInWorldspace.Z - targetInWorldspaceIn.Z) * 3.0f), defaultNearAndFarPerspClipping.Far);
            }

            UpdateCameraMatrices();
        }

        public CCCamera(CCCameraProjection projection, CCRect targetVisibleBoundsWorldspace) 
            : this(projection, targetVisibleBoundsWorldspace.Size, new CCPoint3(targetVisibleBoundsWorldspace.Center, 0))
        {
        }

        public CCCamera(CCCameraProjection projection, CCSize targetVisibleDimensionsWorldspace)
            : this(projection, targetVisibleDimensionsWorldspace, new CCPoint3(targetVisibleDimensionsWorldspace.Center, 0))
        {
        }

        public CCCamera(CCSize targetVisibleDimensionsWorldspace) 
            : this(CCCameraProjection.Projection3D, targetVisibleDimensionsWorldspace)
        {
        }

        public CCCamera(CCRect targetVisibleBoundsWorldspace) 
            : this(CCCameraProjection.Projection3D, targetVisibleBoundsWorldspace)
        {
        }

        public CCCamera(float perspectiveFieldOfViewIn, float perspectiveAspectRatioIn, CCPoint3 cameraCenterPositionWorldspaceIn, CCPoint3 targetInWorldspaceIn)
            : this(targetInWorldspaceIn)
        {
            cameraProjection = CCCameraProjection.Projection3D;

            centerInWorldspace = cameraCenterPositionWorldspaceIn;

            fieldOfView = perspectiveFieldOfViewIn;
            aspectRatio = perspectiveAspectRatioIn;

            UpdateCameraMatrices();
        }

        #endregion Constructors


        public void UpdatePerspectiveCameraTargetBounds(CCRect targetVisibleBoundsWorldspaceIn)
        {
            this.UpdatePerspectiveCameraTargetBounds(targetVisibleBoundsWorldspaceIn.Size, 
                new CCPoint3(targetVisibleBoundsWorldspaceIn.Center, 0));
        }

        public void UpdatePerspectiveCameraTargetBounds(CCSize targetVisibleDimensionsWorldspaceIn, CCPoint3 targetWorldspaceIn)
        {
            cameraProjection = CCCameraProjection.Projection3D;

            aspectRatio = targetVisibleDimensionsWorldspaceIn.Width / targetVisibleDimensionsWorldspaceIn.Height;

            targetInWorldspace = targetWorldspaceIn;

            centerInWorldspace 
                = CalculatePerspectiveCameraCenter(targetVisibleDimensionsWorldspaceIn, targetWorldspaceIn);

            UpdateCameraMatrices();
        }

        CCPoint3 CalculatePerspectiveCameraCenter(CCSize targetVisibleBounds, CCPoint3 target)
        {
            CCPoint3 newCenter = target;

            /*
            * Given our field of view and near and far clipping, need to find z position of camera center
            * The top coord of near bounding frustrum is y_p = - (near * y_eye) / z_eye = near * Tan(fov / 2)
            * Here, we want y_eye = 1/2 * bounds height
            * Solve for above for target z_eye
            * Finally, the center we're setting is in world coords (i.e. z_center = z_target - z_eye.
            * Note z_eye will generally have a negative value, so - z_eye > 0.
            */

            float zEye = - (targetVisibleBounds.Height / 2.0f) * (1 / (float)Math.Tan(fieldOfView / 2.0f));

            newCenter.Z -= zEye;

            return newCenter;
        }

        void UpdateCameraMatrices()
        {
            Vector3 xnaCameraCenter = new Vector3(centerInWorldspace.X, centerInWorldspace.Y, centerInWorldspace.Z);
            Vector3 xnaCameraTarget = new Vector3(targetInWorldspace.X, targetInWorldspace.Y, targetInWorldspace.Z);
            Vector3 xnaCameraUpDirection = new Vector3(upDirection.X, upDirection.Y, upDirection.Z);

            if (Projection == CCCameraProjection.Projection2D) 
            {
                projectionMatrix = Matrix.CreateOrthographic (
                    orthographicViewSizeWorldspace.Width, orthographicViewSizeWorldspace.Height, 
                    nearAndFarOrthographicZClipping.Near, nearAndFarOrthographicZClipping.Far);
            } 
            else if (Projection == CCCameraProjection.Projection3D) 
            {
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        fieldOfView,
                        aspectRatio,
                        nearAndFarPerspectiveClipping.Near, nearAndFarPerspectiveClipping.Far
                );
            }

            viewMatrix = Matrix.CreateLookAt(xnaCameraCenter, xnaCameraTarget, xnaCameraUpDirection);

            OnCameraVisibleBoundsChanged(this, null);
        }
    }
}