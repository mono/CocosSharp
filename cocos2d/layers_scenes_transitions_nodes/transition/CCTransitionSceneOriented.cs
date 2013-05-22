/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
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

namespace cocos2d
{
    public enum tOrientation
    {
        /// An horizontal orientation where the Left is nearer
        kOrientationLeftOver = 0,

        /// An horizontal orientation where the Right is nearer
        kOrientationRightOver = 1,

        /// A vertical orientation where the Up is nearer
        kOrientationUpOver = 0,

        /// A vertical orientation where the Bottom is nearer
        kOrientationDownOver = 1,
    }

    public class CCTransitionSceneOriented : CCTransitionScene
    {
        protected tOrientation m_eOrientation;

        public CCTransitionSceneOriented() { }

        /// <summary>
        /// creates a base transition with duration and incoming scene
        /// </summary>
        public CCTransitionSceneOriented (float t, CCScene scene, tOrientation orientation) : base (t, scene)
        {
            m_eOrientation = orientation;
        }

        /// <summary>
        /// initializes a transition with duration and incoming scene
        /// </summary>
        public virtual bool InitWithDuration(float t, CCScene scene, tOrientation orientation)
        {
            if (base.InitWithDuration(t, scene))
            {
                m_eOrientation = orientation;
            }

            return true;
        }
    }
}