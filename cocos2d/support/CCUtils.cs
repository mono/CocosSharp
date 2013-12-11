/****************************************************************************
Copyright (c) 2010 cocos2d-x.org
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
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
#if !WINDOWS_PHONE && !XBOX && !WINDOWS &&!NETFX_CORE && !PSM
#if MACOS
using MonoMac.OpenGL;
#elif WINDOWSGL || LINUX
using OpenTK.Graphics.OpenGL;
#else
using OpenTK.Graphics.ES20;
using BeginMode = OpenTK.Graphics.ES20.All;
using EnableCap = OpenTK.Graphics.ES20.All;
using TextureTarget = OpenTK.Graphics.ES20.All;
using BufferTarget = OpenTK.Graphics.ES20.All;
using BufferUsageHint = OpenTK.Graphics.ES20.All;
using DrawElementsType = OpenTK.Graphics.ES20.All;
using GetPName = OpenTK.Graphics.ES20.All;
using FramebufferErrorCode = OpenTK.Graphics.ES20.All;
using FramebufferTarget = OpenTK.Graphics.ES20.All;
using FramebufferAttachment = OpenTK.Graphics.ES20.All;
using RenderbufferTarget = OpenTK.Graphics.ES20.All;
using RenderbufferStorage = OpenTK.Graphics.ES20.All;
#endif
#endif

namespace CocosSharp
{
    public class CCUtils
    {

        #if !WINDOWS_PHONE && !XBOX && !PSM
        #if OPENGL
        private static List<string> _GLExtensions = null;
        
        public static List<string> GetGLExtensions()
        {
            // Setup extensions.
            if(_GLExtensions == null) {
                List<string> extensions = new List<string>();
                #if GLES
                var extstring = GL.GetString(RenderbufferStorage.Extensions);                       
				GraphicsExtensions.CheckGLError();
				#elif MACOS

				// for right now there are errors with GL before we even get here so the 
				// CheckGLError for MACOS is throwing errors even though the extensions are read
				// correctly.  Placed this here for now so that we can continue the processing
				// until we find the real error.
				var extstring = GL.GetString(StringName.Extensions);

				#else
                var extstring = GL.GetString(StringName.Extensions);
				GraphicsExtensions.CheckGLError();
                #endif
                
                if (!string.IsNullOrEmpty(extstring))
                {
                    extensions.AddRange(extstring.Split(' '));
                    CCLog.Log("Supported GL extensions:");
                    foreach (string extension in extensions)
                        CCLog.Log(extension);
                }
                _GLExtensions = extensions;
            }
            return _GLExtensions;
        }
        #endif
        #endif
        
        /// <summary>
        /// Returns the Cardinal Spline position for a given set of control points, tension and time
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="tension"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static CCPoint CCCardinalSplineAt(CCPoint p0, CCPoint p1, CCPoint p2, CCPoint p3, float tension, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;
            
            /*
             * Formula: s(-ttt + 2tt - t)P1 + s(-ttt + tt)P2 + (2ttt - 3tt + 1)P2 + s(ttt - 2tt + t)P3 + (-2ttt + 3tt)P3 + s(ttt - tt)P4
             */
            float s = (1 - tension) / 2;
            
            float b1 = s * ((-t3 + (2 * t2)) - t); // s(-t3 + 2 t2 - t)P1
            float b2 = s * (-t3 + t2) + (2 * t3 - 3 * t2 + 1); // s(-t3 + t2)P2 + (2 t3 - 3 t2 + 1)P2
            float b3 = s * (t3 - 2 * t2 + t) + (-2 * t3 + 3 * t2); // s(t3 - 2 t2 + t)P3 + (-2 t3 + 3 t2)P3
            float b4 = s * (t3 - t2); // s(t3 - t2)P4
            
            float x = (p0.X * b1 + p1.X * b2 + p2.X * b3 + p3.X * b4);
            float y = (p0.Y * b1 + p1.Y * b2 + p2.Y * b3 + p3.Y * b4);
            
            return new CCPoint(x, y);
        }
        
        /// <summary>
        /// Parses an int value using the default number style and the invariant culture parser.
        /// </summary>
        /// <param name="toParse">The value to parse</param>
        /// <returns>The int value of the string</returns>
        public static int CCParseInt(string toParse)
        {
            // http://www.cocos2d-x.org/boards/17/topics/11690
            // Issue #17
            // https://github.com/cocos2d/cocos2d-x-for-xna/issues/17
            return int.Parse(toParse, CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Parses aint value for the given string using the given number style and using
        /// the invariant culture parser.
        /// </summary>
        /// <param name="toParse">The value to parse.</param>
        /// <param name="ns">The number style used to parse the int value.</param>
        /// <returns>The int value of the string.</returns>
        public static int CCParseInt(string toParse, NumberStyles ns)
        {
            // http://www.cocos2d-x.org/boards/17/topics/11690
            // Issue #17
            // https://github.com/cocos2d/cocos2d-x-for-xna/issues/17
            return int.Parse(toParse, ns, CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Parses a float value using the default number style and the invariant culture parser.
        /// </summary>
        /// <param name="toParse">The value to parse</param>
        /// <returns>The float value of the string.</returns>
        public static float CCParseFloat(string toParse)
        {
            // http://www.cocos2d-x.org/boards/17/topics/11690
            // Issue #17
            // https://github.com/cocos2d/cocos2d-x-for-xna/issues/17
            return float.Parse(toParse, CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Parses a float value for the given string using the given number style and using
        /// the invariant culture parser.
        /// </summary>
        /// <param name="toParse">The value to parse.</param>
        /// <param name="ns">The number style used to parse the float value.</param>
        /// <returns>The float value of the string.</returns>
        public static float CCParseFloat(string toParse, NumberStyles ns)
        {
            // http://www.cocos2d-x.org/boards/17/topics/11690
            // https://github.com/cocos2d/cocos2d-x-for-xna/issues/17
            return float.Parse(toParse, ns, CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Returns the next Power of Two for the given value. If x = 3, then this returns 4.
        /// If x = 4 then 4 is returned. If the value is a power of two, then the same value
        /// is returned.
        /// </summary>
        /// <param name="x">The base of the POT test</param>
        /// <returns>The next power of 2 (1, 2, 4, 8, 16, 32, 64, 128, etc)</returns>
        public static long CCNextPOT(long x)
        {
            x = x - 1;
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return x + 1;
        }
        
        /// <summary>
        /// Returns the next Power of Two for the given value. If x = 3, then this returns 4.
        /// If x = 4 then 4 is returned. If the value is a power of two, then the same value
        /// is returned.
        /// </summary>
        /// <param name="x">The base of the POT test</param>
        /// <returns>The next power of 2 (1, 2, 4, 8, 16, 32, 64, 128, etc)</returns>
        public static int CCNextPOT(int x)
        {
            x = x - 1;
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return x + 1;
        }

		public static void Split(string src, string token, List<string> vect)
		{
			int nend = 0;
			int nbegin = 0;
			while (nend != -1)
			{
				nend = src.IndexOf(token, nbegin);
				if (nend == -1)
					vect.Add(src.Substring(nbegin, src.Length - nbegin));
				else
					vect.Add(src.Substring(nbegin, nend - nbegin));
				nbegin = nend + token.Length;
			}
		}
		
		// first, judge whether the form of the string like this: {x,y}
		// if the form is right,the string will be splited into the parameter strs;
		// or the parameter strs will be empty.
		// if the form is right return true,else return false.
		public static bool SplitWithForm(string pStr, List<string> strs)
		{
			bool bRet = false;
			
			do
			{
				if (pStr == null)
				{
					break;
				}
				
				// string is empty
				string content = pStr;
				if (content.Length == 0)
				{
					break;
				}
				
				int nPosLeft = content.IndexOf('{');
				int nPosRight = content.IndexOf('}');
				
				// don't have '{' and '}'
				if (nPosLeft == -1 || nPosRight == -1)
				{
					break;
				}
				// '}' is before '{'
				if (nPosLeft > nPosRight)
				{
					break;
				}
				
				string pointStr = content.Substring(nPosLeft + 1, nPosRight - nPosLeft - 1);
				// nothing between '{' and '}'
				if (pointStr.Length == 0)
				{
					break;
				}
				
				int nPos1 = pointStr.IndexOf('{');
				int nPos2 = pointStr.IndexOf('}');
				// contain '{' or '}' 
				if (nPos1 != -1 || nPos2 != -1) break;
				
				Split(pointStr, ",", strs);
				if (strs.Count != 2 || strs[0].Length == 0 || strs[1].Length == 0)
				{
					strs.Clear();
					break;
				}
				
				bRet = true;
			} while (false);
			
			return bRet;
		}
    }
}