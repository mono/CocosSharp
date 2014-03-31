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
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public enum CCGlesVersion
    {
        GLES_VER_INVALID,
        GLES_VER_1_0,
        GLES_VER_1_1,
        GLES_VER_2_0,
    }

    public class CCConfiguration
    {
        protected int m_nMaxTextureSize = 0x0D33;
        protected int m_nMaxModelviewStackDepth;
        protected bool m_bSupportsPVRTC;
        protected bool m_bSupportsNPOT;
        protected bool m_bSupportsBGRA8888;
        protected bool m_bSupportsDiscardFramebuffer;
        protected bool m_bInited;
        protected uint m_uOSVersion;
        protected int m_nMaxSamplesAllowed;
        protected string m_pGlExtensions;

        private CCConfiguration()
        { }

        public CCGlesVersion getGlesVersion()
        {
            return CCGlesVersion.GLES_VER_2_0;
        }

        /// <summary>
        /// OpenGL Max texture size.
        /// </summary>
        public int MaxTextureSize
        {
            get { return m_nMaxTextureSize; }
        }

        /// <summary>
        /// OpenGL Max Modelview Stack Depth
        /// </summary>
        public int MaxModelviewStackDepth
        {
            get { return m_nMaxModelviewStackDepth; }
        }

        /// <summary>
        /// Whether or not the GPU supports NPOT (Non Power Of Two) textures.
        /// NPOT textures have the following limitations:
        ///- They can't have mipmaps
        ///- They only accept GL_CLAMP_TO_EDGE in GL_TEXTURE_WRAP_{S,T}
        /// @since v0.99.2
        /// </summary>
        public bool IsSupportsNPOT
        {
            get { return m_bSupportsNPOT; }
        }

        /// <summary>
        /// Whether or not PVR Texture Compressed is supported
        /// </summary>
        public bool IsSupportsPVRTC
        {
            get { return m_bSupportsPVRTC; }
        }

        /// <summary>
        /// Whether or not BGRA8888 textures are supported.
        /// @since v0.99.2
        /// </summary>
        public bool IsSupportsBGRA8888
        {
            get { return m_bSupportsBGRA8888; }
        }

        /// <summary>
        /// Whether or not glDiscardFramebufferEXT is supported
        /// @since v0.99.2
        /// </summary>
        public bool IsSupportsDiscardFramebuffer
        {
            get { return m_bSupportsDiscardFramebuffer; }
        }

        /// <summary>
        /// returns the OS version.
        ///  - On iOS devices it returns the firmware version.
        /// - On Mac returns the OS version
        /// @since v0.99.5
        /// </summary>
        public uint OSVersion
        {
            get { return m_uOSVersion; }
        }

        /// <summary>
        /// returns whether or not an OpenGL is supported
        /// </summary>
        public bool CheckForGLExtension(string searchName)
        {
            throw new NotImplementedException();
        }

        static CCConfiguration m_sharedConfiguration = new CCConfiguration();

        /// <summary>
        /// returns a shared instance of the CCConfiguration
        /// </summary>
        public static CCConfiguration SharedConfiguration
        {
            get {
            if (!m_sharedConfiguration.m_bInited)
            {
                m_sharedConfiguration.m_bInited = true;
            }

            return m_sharedConfiguration;
        }
    }
}
}
