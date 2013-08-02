/*
 * ColourUtils.h
 *
 * Copyright 2012 Stewart Hamilton-Arrandale.
 * http://creativewax.co.uk
 *
 * Modified by Yannick Loriot.
 * http://yannickloriot.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 *
 * Converted to c++ / cocos2d-x by Angus C
 */

using System;

namespace Cocos2D
{
	internal struct RGBA
	{
		public float r;       // percent
		public float g;       // percent
		public float b;       // percent
		public float a;       // percent
	}

	public struct HSV
	{
		public float h;       // angle in degrees
		public float s;       // percent
		public float v;       // percent
	}

	internal class CCControlUtils
	{
	
		public static CCSprite AddSpriteToTargetWithPosAndAnchor(string spriteName, CCNode target, CCPoint pos, CCPoint anchor)
		{
			var sprite = new CCSprite(spriteName);
    
			if (sprite == null)
				return null;

			sprite.Position = pos;
			sprite.AnchorPoint = anchor;
			target.AddChild(sprite);

			return sprite;
		}

		public static HSV HSVfromRGB(RGBA value)
		{
			HSV o;
			float min, max, delta;
    
			min = value.r < value.g ? value.r : value.g;
			min = min  < value.b ? min  : value.b;
    
			max = value.r > value.g ? value.r : value.g;
			max = max  > value.b ? max  : value.b;
    
			o.v = max;											// v
			delta = max - min;
			if( max > 0.0f )
			{
				o.s = (delta / max);							// s
			} else
			{
				// r = g = b = 0								// s = 0, v is undefined
				o.s = 0.0f;
				o.h = -1;										// its now undefined (don't know if setting to NAN is a good idea)
				return o;
			}
			if( value.r >= max )								// > is bogus, just keeps compilor happy
			{
				o.h = ( value.g - value.b ) / delta;			// between yellow & magenta
			} else
			{
				if( value.g >= max )
					o.h = 2.0f + ( value.b - value.r ) / delta;  // between cyan & yellow
				else
					o.h = 4.0f + ( value.r - value.g ) / delta;  // between magenta & cyan
			}
    
			o.h *= 60.0f;										// degrees
    
			if( o.h < 0.0f )
				o.h += 360.0f;
    
			return o;
		}

		public static RGBA RGBfromHSV(HSV value)
		{
			float hh, p, q, t, ff;
			long i;
			RGBA o;
			o.a = 1f;
    
			if (value.s <= 0.0f) // < is bogus, just shuts up warnings
			{       
				if (double.IsNaN(value.h)) // value.h == NAN
				{   
					o.r = value.v;
					o.g = value.v;
					o.b = value.v;
					return o;
				}
        
				// error - should never happen
				o.r = 0.0f;
				o.g = 0.0f;
				o.b = 0.0f;
				return o;
			}
    
			hh = value.h;
			if(hh >= 360.0f) hh = 0.0f;
			hh /= 60.0f;
			i = (long)hh;
			ff = hh - i;
			p = value.v * (1.0f - value.s);
			q = value.v * (1.0f - (value.s * ff));
			t = value.v * (1.0f - (value.s * (1.0f - ff)));
    
			switch(i)
			{
				case 0:
					o.r = value.v;
					o.g = t;
					o.b = p;
					break;
				case 1:
					o.r = q;
					o.g = value.v;
					o.b = p;
					break;
				case 2:
					o.r = p;
					o.g = value.v;
					o.b = t;
					break;
            
				case 3:
					o.r = p;
					o.g = q;
					o.b = value.v;
					break;
				case 4:
					o.r = t;
					o.g = p;
					o.b = value.v;
					break;
				case 5:
				default:
					o.r = value.v;
					o.g = p;
					o.b = q;
					break;
			}
			return o;     
		}

		public static CCRect CCRectUnion(CCRect src1, CCRect src2)
		{
			CCRect result;
    
			float x1 = Math.Min(src1.MinX, src2.MinX);
			float y1 = Math.Min(src1.MinY, src2.MinY);
			float x2 = Math.Max(src1.MaxX, src2.MaxX);
			float y2 = Math.Max(src1.MaxY, src2.MaxY);
    
			result.Origin = new CCPoint(x1,x2);
			result.Size = new CCSize(x2-x1, y2-y1);
			return result;
		}
	}
}