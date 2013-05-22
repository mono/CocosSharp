/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org


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

namespace Cocos2D
{
    public class CCMacros
    {
        /// <summary>
        /// simple macro that swaps 2 variables
        /// </summary>
        public static void CCSwap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }

        private static readonly System.Random rand = new System.Random();

        /// <summary>
        /// returns a random float between -1 and 1
        /// </summary>
        public static float CCRandomBetweenNegative1And1()
        {
            return (2.0f * ((float) rand.Next() / int.MaxValue)) - 1.0f;
        }

        /** @def CCRANDOM_0_1
            returns a random float between 0 and 1
         */

        public static float CCRandomBetween0And1()
        {
            return (float) rand.Next() / int.MaxValue;
        }

        /** @def CC_DEGREES_TO_RADIANS
            converts degrees to radians
        */

        public static float CCDegreesToRadians(float angle)
        {
            return angle * 0.01745329252f; // PI / 180
        }

        /** @def CC_RADIANS_TO_DEGREES
            converts radians to degrees
        */

        public static float CCRadiansToDegrees(float angle)
        {
            return angle * 57.29577951f; // PI * 180
        }

        [Obsolete("use float.Epsilon instead")]
        public static readonly float FLT_EPSILON = float.Epsilon; // Was:  1.192092896e-07F;

        public static int CCContentScaleFactor()
        {
            return 1;
        }

        public static CCRect CCRectanglePixelsToPoints(CCRect pixels)
        {
            return pixels;
        }

        public static CCRect CCRectanglePointsToPixels(CCRect points)
        {
            return points;
        }

        public static CCSize CCSizePointsToPixels(CCSize size)
        {
            return size;
        }

        public static CCSize CCSizePixelsToPoints(CCSize size)
        {
            return size;
        }

        public static CCPoint CCPointPixelsToPoints(CCPoint point)
        {
            return point;
        }

        public static CCPoint CCPointPointsToPixels(CCPoint point)
        {
            return point;
        }
        /*
        public static bool CC_HOST_IS_BIG_ENDIAN()
        {
            return !BitConverter.IsLittleEndian;
        }
        public static uint CC_SWAP_INT32_LITTLE_TO_HOST(uint i)
        {
            return (CC_HOST_IS_BIG_ENDIAN() ? CC_SWAP32(i) : (i));
        }

        public static ushort CC_SWAP_INT16_LITTLE_TO_HOST(ushort i)
        {
            return (CC_HOST_IS_BIG_ENDIAN() ? CC_SWAP16(i) : (i));
        }

        public static uint CC_SWAP_INT32_BIG_TO_HOST(uint i)
        {
            return (CC_HOST_IS_BIG_ENDIAN() ? (i) : CC_SWAP32(i));
        }

        public static ushort CC_SWAP_INT16_BIG_TO_HOST(ushort i)
        {
            return (CC_HOST_IS_BIG_ENDIAN() ? (i) : CC_SWAP16(i));
        }
        // Only unsigned int can use these functions.

        public static uint CC_SWAP32(uint i)
        {
            return ((i & 0x000000ff) << 24 | (i & 0x0000ff00) << 8 | (i & 0x00ff0000) >> 8 | (i & 0xff000000) >> 24);
        }

        public static ushort CC_SWAP16(ushort i)
        {
            return (ushort) ((i & 0x00ff) << 8 | (i & 0xff00) >> 8);
        }
        */


        /*
         * Macros of CCGeometry.h
         */

        [Obsolete("Use the CCPoint ctor")]
        public static CCPoint CCPointMake(float x, float y)
        {
            return new CCPoint(x, y);
        }

        [Obsolete("Use the CCSize ctor")]
        public static CCSize CCSizeMake(float width, float height)
        {
            return new CCSize(width, height);
        }

        [Obsolete("Use the CCRect ctor")]
        public static CCRect CCRectMake(float x, float y, float width, float height)
        {
            return new CCRect(x, y, width, height);
        }

        /*
         * Macros defined in ccConfig.h
         */
        public static readonly string CCHiResDisplayFilenameSuffix = "-hd";
        public static readonly float CCDirectorStatsUpdateIntervalInSeconds = 0.5f;

        /*
         * Macros defined in CCSprite.h
         */
        public static readonly int CCSpriteIndexNotInitialized = 320000000; // 0xffffffff; // CCSprite invalid index on the CCSpriteBatchode



#if CC_OPTIMIZE_BLEND_FUNC_FOR_PREMULTIPLIED_ALPHA
        public static readonly int CCDefaultSourceBlending = OGLES.GL_ONE;
        public static readonly int CCDefaultDestinationBlending = OGLES.GL_ONE_MINUS_SRC_ALPHA;
#else
        // Not PMA - like iOS, textures do not have alpha blended in the pixels
         public static readonly int CCDefaultSourceBlending= CCOGLES.GL_SRC_ALPHA;
         public static readonly int CCDefaultDestinationBlending=  CCOGLES.GL_ONE_MINUS_SRC_ALPHA;
#endif

        // The following macros are defined for opengl es, they are not needed.
        // #define CCDefaultDestinationBlending GL_ONE_MINUS_SRC_ALPHA

        // #define CC_ENABLE_DEFAULT_GL_STATES() {				\
        // glEnableClientState(GL_VERTEX_ARRAY);			\
        // glEnableClientState(GL_COLOR_ARRAY);			\
        // glEnableClientState(GL_TEXTURE_COORD_ARRAY);	\
        // glEnable(GL_TEXTURE_2D);			

        // #define CC_DISABLE_DEFAULT_GL_STATES() {			\
        // glDisable(GL_TEXTURE_2D);						\
        // glDisableClientState(GL_COLOR_ARRAY);			\
        // glDisableClientState(GL_TEXTURE_COORD_ARRAY);	\
        // glDisableClientState(GL_VERTEX_ARRAY);			\
    }
}