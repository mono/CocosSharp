/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2010      Ricardo Quesada
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
using System.Text;

namespace CocosSharp
{
	#region Enums

	public enum CCGlesVersion
    {
        GLES_VER_INVALID,
        GLES_VER_1_0,
        GLES_VER_1_1,
        GLES_VER_2_0,
    }

	#endregion Enums


    public class CCConfiguration
    {
		static CCConfiguration sharedConfiguration = new CCConfiguration();

		#region Properties

		// Returns a shared instance of the CCConfiguration
		public static CCConfiguration SharedConfiguration
		{
			get 
			{
				if (!sharedConfiguration.Inited) 
				{
					sharedConfiguration.Inited = true;
				}

				return sharedConfiguration;
			}
		}

		public int MaxTextureSize { get; private set; } 				// OpenGL Max texture size.
		public int MaxModelviewStackDepth { get; private set; } 		// OpenGL Max Modelview Stack Depth
		public bool IsSupportsNPOT { get; private set; }         		// Whether or not the GPU supports NPOT (Non Power Of Two) textures.
		public bool IsSupportsPVRTC { get; private set; } 				// Whether or not PVR Texture Compressed is supported
		public bool IsSupportsBGRA8888 { get; private set; }        	// Whether or not BGRA8888 textures are supported.
		public bool IsSupportsDiscardFramebuffer { get; private set; }	// Whether or not glDiscardFramebufferEXT is supported
		public uint OSVersion { get; private set; }       				// Returns the OS version. iOS = firmware version. Mac = OS version
		protected bool Inited { get; private set; }

        public CCGlesVersion GlesVersion
        {
			get { return CCGlesVersion.GLES_VER_2_0; }
        }

		// Returns whether or not an OpenGL is supported
        public bool CheckForGLExtension(string searchName)
        {
            throw new NotImplementedException();
        }

		#endregion Properties


		#region Constructors

		CCConfiguration()
		{ 
			MaxTextureSize = 0x0D33;
		}

		#endregion Constructors

    }
}
