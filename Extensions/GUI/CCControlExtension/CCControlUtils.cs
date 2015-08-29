/*
 * ColourUtils.H
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

namespace CocosSharp
{
	internal struct RGBA
	{
		public float R;       // percent
		public float G;       // percent
		public float B;       // percent
		public float A;       // percent
	}

	public struct HSV
	{
		public float H;       // angle in degrees
		public float S;       // percent
		public float V;       // percent
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
    
			min = value.R < value.G ? value.R : value.G;
			min = min  < value.B ? min  : value.B;
    
			max = value.R > value.G ? value.R : value.G;
			max = max  > value.B ? max  : value.B;
    
			o.V = max;											// v
			delta = max - min;
			if( max > 0.0f )
			{
				o.S = (delta / max);							// s
			} else
			{
				// r = g = b = 0								// s = 0, v is undefined
				o.S = 0.0f;
				o.H = -1;										// its now undefined (don't know if setting to NAN is a good idea)
				return o;
			}
			if( value.R >= max )								// > is bogus, just keeps compilor happy
			{
				o.H = ( value.G - value.B ) / delta;			// between yellow & magenta
			} else
			{
				if( value.G >= max )
					o.H = 2.0f + ( value.B - value.R ) / delta;  // between cyan & yellow
				else
					o.H = 4.0f + ( value.R - value.G ) / delta;  // between magenta & cyan
			}
    
			o.H *= 60.0f;										// degrees
    
			if( o.H < 0.0f )
				o.H += 360.0f;
    
			return o;
		}

		public static RGBA RGBfromHSV(HSV value)
		{
			float hh, p, q, t, ff;
			long i;
			RGBA o;
			o.A = 1f;
    
			if (value.S <= 0.0f) // < is bogus, just shuts up warnings
			{       
				if (double.IsNaN(value.H)) // value.H == NAN
				{   
					o.R = value.V;
					o.G = value.V;
					o.B = value.V;
					return o;
				}
        
				// error - should never happen
				o.R = 0.0f;
				o.G = 0.0f;
				o.B = 0.0f;
				return o;
			}
    
			hh = value.H;
			if(hh >= 360.0f) hh = 0.0f;
			hh /= 60.0f;
			i = (long)hh;
			ff = hh - i;
			p = value.V * (1.0f - value.S);
			q = value.V * (1.0f - (value.S * ff));
			t = value.V * (1.0f - (value.S * (1.0f - ff)));
    
			switch(i)
			{
				case 0:
					o.R = value.V;
					o.G = t;
					o.B = p;
					break;
				case 1:
					o.R = q;
					o.G = value.V;
					o.B = p;
					break;
				case 2:
					o.R = p;
					o.G = value.V;
					o.B = t;
					break;
            
				case 3:
					o.R = p;
					o.G = q;
					o.B = value.V;
					break;
				case 4:
					o.R = t;
					o.G = p;
					o.B = value.V;
					break;
				case 5:
				default:
					o.R = value.V;
					o.G = p;
					o.B = q;
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
    
            result.Origin.X = x1;
            result.Origin.Y = y1;
            result.Size.Width = x2 - x1;
            result.Size.Height = y2 - y1;
			return result;
		}
	}
}