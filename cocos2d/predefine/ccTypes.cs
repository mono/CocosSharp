/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /// <summary>
    /// RGB color composed of bytes 3 bytes
    /// @since v0.8
    /// </summary>
    public struct CCColor3B
    {
        /*
        public CCColor3B()
        {
            r = 0;
            g = 0;
            b = 0;
        }
        */
        public CCColor3B(byte inr, byte ing, byte inb)
        {
            R = inr;
            G = ing;
            B = inb;
        }

        /// <summary>
        /// Convert Color value of XNA Framework to CCColor3B type
        /// </summary>
        public CCColor3B(Microsoft.Xna.Framework.Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public byte R;
        public byte G;
        public byte B;
    }

    /// <summary>
    /// RGBA color composed of 4 bytes
    /// @since v0.8
    /// </summary>
    public struct CCColor4B
    {
        /*
        public CCColor4B()
        {
            r = 0;
            g = 0;
            b = 0;
            a = 0;
        }
        */

        public CCColor4B(byte inr, byte ing, byte inb, byte ina)
        {
            R = inr;
            G = ing;
            B = inb;
            A = ina;
        }

        public CCColor4B(float inr, float ing, float inb, float ina)
        {
            R = (byte)inr;
            G = (byte)ing;
            B = (byte)inb;
            A = (byte)ina;
        }

        /// <summary>
        /// Convert Color value of XNA Framework to CCColor4B type
        /// </summary>
        public CCColor4B(Microsoft.Xna.Framework.Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }

        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public override string ToString()
        {
            return (string.Format("{0},{1},{2},{3}", R, G, B, A));
        }

        public static CCColor4B Parse(string s)
        {
            string[] f = s.Split(',');
            return (new CCColor4B(byte.Parse(f[0]), byte.Parse(f[1]), byte.Parse(f[2]), byte.Parse(f[3])));
        }
    }

    /// <summary>
    /// RGBA color composed of 4 floats
    /// @since v0.8
    /// </summary>
    public struct CCColor4F
    {
        public CCColor4F(float inr, float ing, float inb, float ina)
        {
            R = inr;
            G = ing;
            B = inb;
            A = ina;
        }

        public float R;
        public float G;
        public float B;
        public float A;

        public override string ToString()
        {
            return (string.Format("{0},{1},{2},{3}", R, G, B, A));
        }

        public static CCColor4F Parse(string s)
        {
            string[] f = s.Split(',');
            return (new CCColor4F(float.Parse(f[0]), float.Parse(f[1]), float.Parse(f[2]), float.Parse(f[3])));
        }
    }

    /// <summary>
    /// A vertex composed of 2 floats: x, y
    /// @since v0.8
    /// </summary>
    public struct CCVertex2F
    {
        /*
        public ccVertex2F()
        {
            x = 0.0f;
            y = 0.0f;
        }
        */

        public CCVertex2F(float inx, float iny)
        {
            X = inx;
            Y = iny;
        }

        public float X;
        public float Y;
    }

    /// <summary>
    /// A vertex composed of 2 floats: x, y
    /// @since v0.8
    /// </summary>
    public struct CCVertex3F
    {
        public static readonly CCVertex3F Zero = new CCVertex3F();

        public CCVertex3F(float inx, float iny, float inz)
        {
            X = inx;
            Y = iny;
            Z = inz;
        }

        public float X;
        public float Y;
        public float Z;

        public override string ToString()
        {
            return String.Format("ccVertex3F x:{0}, y:{1}, z:{2}", X, Y, Z);
        }
    }

    /// <summary>
    /// A texcoord composed of 2 floats: u, y
    /// @since v0.8
    /// </summary>
    public struct CCTex2F
    {
        /*
        public ccTex2F()
        {
            u = 0.0f;
            v = 0.0f;
        }
        */
        public CCTex2F(float inu, float inv)
        {
            U = inu;
            V = inv;
        }

        public float U;
        public float V;

        public override string ToString()
        {
            return String.Format("ccTex2F u:{0}, v:{1}", U, V);
        }
    }

    /// <summary>
    /// Point Sprite component
    /// </summary>
    public class CCPointSprite
    {
        public CCPointSprite()
        {
            Position = new CCVertex2F();
            Color = new CCColor4B();
            Size = 0.0f;
        }

        public CCVertex2F Position;		// 8 bytes
        public CCColor4B Color;		// 4 bytes
        public float Size;		// 4 bytes
    }

    /// <summary>
    /// A 2D Quad. 4 * 2 floats
    /// </summary>
    public class CCQuad2
    {
        public CCQuad2()
        {
            TopLeft = new CCVertex2F();
            TopRight = new CCVertex2F();
            BottomLeft = new CCVertex2F();
            BottomRight = new CCVertex2F();
        }

        public CCVertex2F TopLeft;
        public CCVertex2F TopRight;
        public CCVertex2F BottomLeft;
        public CCVertex2F BottomRight;
    }

    /// <summary>
    /// A 3D Quad. 4 * 3 floats
    /// </summary>
    public struct CCQuad3
    {
        /*
        public ccQuad3()
        {
            tl = new ccVertex3F();
            tr = new ccVertex3F();
            bl = new ccVertex3F();
            br = new ccVertex3F();
        }
        */
        public CCVertex3F BottomLeft;
        public CCVertex3F BottomRight;
        public CCVertex3F TopLeft;
        public CCVertex3F TopRight;
    }

    /// <summary>
    /// A 2D grid size
    /// </summary>
    public struct CCGridSize
    {
        public CCGridSize(int inx, int iny)
        {
            X = inx;
            Y = iny;
        }

        public int X;
        public int Y;
    }

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4B
    /// </summary>
    public class CCV2F_C4B_T2F
    {
        public CCV2F_C4B_T2F()
        {
            Vertices = new CCVertex2F();
            Colors = new CCColor4B();
            TexCoords = new CCTex2F();
        }

        /// <summary>
        /// vertices (2F)
        /// </summary>
        public CCVertex2F Vertices;

        /// <summary>
        /// colors (4B)
        /// </summary>
        public CCColor4B Colors;

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        public CCTex2F TexCoords;
    }

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4F
    /// </summary>
    public class CCV2F_C4F_T2F
    {
        public CCV2F_C4F_T2F()
        {
            Vertices = new CCVertex2F();
            Colors = new CCColor4F();
            TexCoords = new CCTex2F();
        }

        /// <summary>
        /// vertices (2F)
        /// </summary>
        public CCVertex2F Vertices;

        /// <summary>
        /// colors (4F)
        /// </summary>
        public CCColor4F Colors;

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        public CCTex2F TexCoords;
    }

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4B
    /// </summary>
    //TODO: Use VertexPositionColorTexture
    public struct CCV3F_C4B_T2F : IVertexType
    {
        /// <summary>
        /// vertices (3F)
        /// </summary>
        public CCVertex3F Vertices;			// 12 bytes

        /// <summary>
        /// colors (4B)
        /// </summary>
        public CCColor4B Colors;				// 4 bytes

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        public CCTex2F TexCoords;			// 8 byts

        public static readonly VertexDeclaration VertexDeclaration;

        static CCV3F_C4B_T2F()
        {
            var elements = new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                    new VertexElement(0x10, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                };
            VertexDeclaration = new VertexDeclaration(elements);
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }

    /// <summary>
    /// 4 ccVertex2FTex2FColor4B Quad
    /// </summary>
    public class ccV2F_C4B_T2F_Quad
    {
        public ccV2F_C4B_T2F_Quad()
        {
            bl = new CCV2F_C4B_T2F();
            br = new CCV2F_C4B_T2F();
            tl = new CCV2F_C4B_T2F();
            tr = new CCV2F_C4B_T2F();
        }

        /// <summary>
        /// bottom left
        /// </summary>
        public CCV2F_C4B_T2F bl;

        /// <summary>
        /// bottom right
        /// </summary>
        public CCV2F_C4B_T2F br;

        /// <summary>
        /// top left
        /// </summary>
        public CCV2F_C4B_T2F tl;

        /// <summary>
        /// top right
        /// </summary>
        public CCV2F_C4B_T2F tr;
    }

    /// <summary>
    /// 4 ccVertex3FTex2FColor4B
    /// </summary>
    public struct ccV3F_C4B_T2F_Quad : IVertexType
    {
        /// <summary>
        /// top left
        /// </summary>
        public CCV3F_C4B_T2F tl;

        /// <summary>
        /// bottom left
        /// </summary>
        public CCV3F_C4B_T2F bl;

        /// <summary>
        /// top right
        /// </summary>
        public CCV3F_C4B_T2F tr;

        /// <summary>
        /// bottom right
        /// </summary>
        public CCV3F_C4B_T2F br;

        public static readonly VertexDeclaration VertexDeclaration;

        static ccV3F_C4B_T2F_Quad()
        {
            var elements = new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                    new VertexElement(0x10, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                };
            VertexDeclaration = new VertexDeclaration(elements);
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }

    /// <summary>
    /// 4 ccVertex2FTex2FColor4F Quad
    /// </summary>
    public class ccV2F_C4F_T2F_Quad
    {
        public ccV2F_C4F_T2F_Quad()
        {
            tl = new CCV2F_C4F_T2F();
            bl = new CCV2F_C4F_T2F();
            tr = new CCV2F_C4F_T2F();
            br = new CCV2F_C4F_T2F();
        }

        /// <summary>
        /// bottom left
        /// </summary>
        public CCV2F_C4F_T2F bl;

        /// <summary>
        /// bottom right
        /// </summary>
        public CCV2F_C4F_T2F br;

        /// <summary>
        /// top left
        /// </summary>
        public CCV2F_C4F_T2F tl;

        /// <summary>
        /// top right
        /// </summary>
        public CCV2F_C4F_T2F tr;
    }

    /// <summary>
    /// Blend Function used for textures
    /// </summary>
    public struct ccBlendFunc
    {
        public ccBlendFunc(int src, int dst)
        {
            this.src = src;
            this.dst = dst;
        }

        /// <summary>
        /// source blend function
        /// </summary>
        public int src;

        /// <summary>
        /// destination blend function
        /// </summary>
        public int dst;
    }

    public enum CCTextAlignment
    {
        CCTextAlignmentLeft,
        CCTextAlignmentCenter,
        CCTextAlignmentRight,
    }

    public enum CCVerticalTextAlignment
    {
        CCVerticalTextAlignmentTop,
        CCVerticalTextAlignmentCenter,
        CCVerticalTextAlignmentBottom
    }

    public class ccTypes
    {
        //ccColor3B predefined colors
        //! White color (255,255,255)
        public static readonly CCColor3B ccWHITE = new CCColor3B(255, 255, 255);
        //! Yellow color (255,255,0)
        public static readonly CCColor3B ccYELLOW = new CCColor3B(255, 255, 0);
        //! Blue color (0,0,255)
        public static readonly CCColor3B ccBLUE = new CCColor3B(0, 0, 255);
        //! Green Color (0,255,0)
        public static readonly CCColor3B ccGREEN = new CCColor3B(0, 255, 0);
        //! Red Color (255,0,0,)
        public static readonly CCColor3B ccRED = new CCColor3B(255, 0, 0);
        //! Magenta Color (255,0,255)
        public static readonly CCColor3B ccMAGENTA = new CCColor3B(255, 0, 255);
        //! Black Color (0,0,0)
        public static readonly CCColor3B ccBLACK = new CCColor3B(0, 0, 0);
        //! Orange Color (255,127,0)
        public static readonly CCColor3B ccORANGE = new CCColor3B(255, 127, 0);
        //! Gray Color (166,166,166)
        public static readonly CCColor3B ccGRAY = new CCColor3B(166, 166, 166);

        //! helper macro that creates an ccColor3B type
        static public CCColor3B ccc3(byte r, byte g, byte b)
        {
            CCColor3B c = new CCColor3B(r, g, b);
            return c;
        }

        //! helper macro that creates an ccColor4B type
        public static CCColor4B ccc4(byte r, byte g, byte b, byte o)
        {
            CCColor4B c = new CCColor4B(r, g, b, o);
            return c;
        }

        /** Returns a ccColor4F from a ccColor3B. Alpha will be 1.
         @since v0.99.1
         */
        public static CCColor4F ccc4FFromccc3B(CCColor3B c)
        {
            CCColor4F c4 = new CCColor4F(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, 1.0f);
            return c4;
        }

        /** Returns a ccColor4F from a ccColor4B.
         @since v0.99.1
         */
        public static CCColor4F ccc4FFromccc4B(CCColor4B c)
        {
            CCColor4F c4 = new CCColor4F(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, c.A / 255.0f);
            return c4;
        }

        /** returns YES if both ccColor4F are equal. Otherwise it returns NO.
         @since v0.99.1
         */
        public static bool ccc4FEqual(CCColor4F a, CCColor4F b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static CCVertex2F vertex2(float x, float y)
        {
            CCVertex2F c = new CCVertex2F(x, y);
            return c;
        }

        public static CCVertex3F vertex3(float x, float y, float z)
        {
            CCVertex3F c = new CCVertex3F(x, y, z);
            return c;
        }

        public static CCTex2F tex2(float u, float v)
        {
            CCTex2F t = new CCTex2F(u, v);
            return t;
        }

        //! helper function to create a ccGridSize
        public static CCGridSize ccg(int x, int y)
        {
            CCGridSize v = new CCGridSize(x, y);
            return v;
        }
    }

}//namespace   cocos2d 

