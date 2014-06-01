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
    /// <summary>
    /// A CCCamera is used in every CCNode.
    /// Useful to look at the object from different views.
    /// The OpenGL gluLookAt() function is used to locate the camera.
    ///
    ///	If the object is transformed by any of the scale, rotation or
    /// position attributes, then they will override the camera.
    /// IMPORTANT: Either your use the camera or the rotation/scale/position properties. You can't use both.
    /// World coordinates won't work if you use the camera.
    ///
    /// Limitations:
    ///	 - Some nodes, like CCParallaxNode, CCParticle uses world node coordinates, and they won't work properly if you move them (or any of their ancestors)
    /// using the camera.
    /// - It doesn't work on batched nodes like CCSprite objects when they are parented to a CCSpriteBatchNode object.
    /// - It is recommended to use it ONLY if you are going to create 3D effects. For 2D effects, use the action CCFollow or position/scale/rotate.
    /// </summary>
    public class CCCamera
    {
		protected float CenterX { get; set; }
		protected float CenterY { get; set; }
		protected float CenterZ { get; set; }
		protected float EyeX { get; set; }
		protected float EyeY { get; set; }
		protected float EyeZ { get; set; }

		protected float UpX { get; set; }
		protected float UpY { get; set; }
		protected float UpZ { get; set; }
		private Matrix lookupMatrix;


        /// <summary>
        ///  sets \ get the dirty value
        /// </summary>
		public bool IsDirty { get; protected set; }


        #region Constructors

        public CCCamera()
        {
            Restore();
        }

        #endregion Constructors


        public override string ToString()
        {
            return String.Format("<CCCamera | center = ({0},{1},{2})>", CenterX, CenterY, CenterZ);
        }

        /// <summary>
        /// sets the camera in the default position
        /// </summary>
        public void Restore()
        {
            EyeX = EyeY = 0.0f;
            EyeZ = ZEye;

            CenterX = CenterY = CenterZ = 0.0f;

            UpX = 0.0f;
            UpY = 1.0f;
            UpZ = 0.0f;

            lookupMatrix = Matrix.Identity;

            IsDirty = false;
        }

        /// <summary>
        ///  Sets the camera using gluLookAt using its eye, center and up_vector
        /// </summary>
        public void Locate()
        {
            if (IsDirty)
            {
                lookupMatrix = Matrix.CreateLookAt(new Vector3(EyeX, EyeY, EyeZ),
                                                     new Vector3(CenterX, CenterY, CenterZ),
                                                     new Vector3(UpX, UpY, UpZ));
                IsDirty = false;
            }

            CCDrawManager.MultMatrix(ref lookupMatrix);
        }

        /// <summary>
        /// sets the eye values in points
        /// </summary>
        /// <param name="fEyeX"></param>
        /// <param name="fEyeY"></param>
        /// <param name="fEyeZ"></param>
        public void SetEyeXyz(float eyeX, float eyeY, float eyeZ)
        {
            EyeX = eyeX;
            EyeY = eyeY;
            EyeZ = eyeZ;

            IsDirty = true;
        }

        /// <summary>
        /// sets the center values in points
        /// </summary>
        /// <param name="fCenterX"></param>
        /// <param name="fCenterY"></param>
        /// <param name="fCenterZ"></param>
        public void SetCenterXyz(float centerX, float centerY, float centerZ)
        {
            CenterX = centerX;
            CenterY = centerY;
            CenterZ = centerZ;

            IsDirty = true;
        }

        /// <summary>
        ///  sets the up values
        /// </summary>
        /// <param name="fUpX"></param>
        /// <param name="fUpY"></param>
        /// <param name="fUpZ"></param>
        public void SetUpXyz(float upX, float upY, float upZ)
        {
            UpX = upX;
            UpY = upY;
            UpZ = upZ;

            IsDirty = true;
        }

        /// <summary>
        ///  get the eye vector values in points
        /// </summary>
        /// <param name="pEyeX"></param>
        /// <param name="pEyeY"></param>
        /// <param name="pEyeZ"></param>
        public void GetEyeXyz(out float eyeX, out float eyeY, out float eyeZ)
        {
            eyeX = EyeX;
            eyeY = EyeY;
            eyeZ = EyeZ;
        }

        /// <summary>
        ///  get the center vector values int points 
        /// </summary>
        /// <param name="pCenterX"></param>
        /// <param name="pCenterY"></param>
        /// <param name="pCenterZ"></param>
        public void GetCenterXyz(out float centerX, out float centerY, out float centerZ)
        {
            centerX = CenterX;
            centerY = CenterY;
            centerZ = CenterZ;
        }

        /// <summary>
        ///  get the up vector values
        /// </summary>
        /// <param name="pUpX"></param>
        /// <param name="pUpY"></param>
        /// <param name="pUpZ"></param>
        public void GetUpXyz(out float upX, out float upY, out float upZ)
        {
            upX = UpX;
            upY = UpY;
            upZ = UpZ;
        }

        /// <summary>
        /// returns the Z eye
        /// </summary>
        /// <returns></returns>
        public static float ZEye
        {
			get 
			{
				return 1.192092896e-07F;
			}
        }
    }
}