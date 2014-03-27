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
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    #region Enums

    public enum CCTextAlignment
    {
        Left,
        Center,
        Right,
    }

    public enum CCVerticalTextAlignment
    {
        Top,
        Center,
        Bottom
    }

    #endregion Enums


    #region Colors

    /// <summary>
    /// RGB color composed of bytes 3 bytes
    /// @since v0.8
    /// </summary>
    public struct CCColor3B
    {
        public static readonly CCColor3B White = new CCColor3B(255, 255, 255);
        public static readonly CCColor3B Yellow = new CCColor3B(255, 255, 0);
        public static readonly CCColor3B Blue = new CCColor3B(0, 0, 255);
        public static readonly CCColor3B Green = new CCColor3B(0, 255, 0);
        public static readonly CCColor3B Red = new CCColor3B(255, 0, 0);
        public static readonly CCColor3B Magenta = new CCColor3B(255, 0, 255);
        public static readonly CCColor3B Black = new CCColor3B(0, 0, 0);
        public static readonly CCColor3B Orange = new CCColor3B(255, 127, 0);
        public static readonly CCColor3B Gray = new CCColor3B(166, 166, 166);

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        #region Constructors

        public CCColor3B(byte inr, byte ing, byte inb) : this()
        {
            R = inr;
            G = ing;
            B = inb;
        }

        public CCColor3B(CCColor4B color4B): this(color4B.R, color4B.G, color4B.B)
        {
        }

        #endregion Constructors


        #region Operators

        public static CCColor3B operator *(CCColor3B p1, CCColor3B p2)
        {
            return new CCColor3B((byte)(p1.R * p2.R), (byte)(p1.G * p2.G), (byte)(p1.B * p2.B));
        }

        public static CCColor3B operator /(CCColor3B p1, float div)
        {
            return new CCColor3B((byte)(p1.R / div), (byte)(p1.G / div), (byte)(p1.B / div));
        }

        public static bool operator ==(CCColor3B p1, CCColor3B p2)
        {
            return p1.R == p2.R && p1.G == p2.G && p1.B == p2.B;
        }

        public static bool operator !=(CCColor3B p1, CCColor3B p2)
        {
            return p1.R != p2.R || p1.G != p2.G || p1.B != p2.B;
        }

        public override int GetHashCode()
        {
            return (unchecked (R ^ G ^ B));
        }

        public override bool Equals(object obj)        
        {            
            if (!(obj is CCColor3B))                
                return false;             

            return Equals((CCColor3B)obj);        
        }         

        public bool Equals(CCColor3B other)        
        {            
            return this == other;       
        } 

        #endregion Operators
    }

    /// <summary>
    /// RGBA color composed of 4 bytes
    /// @since v0.8
    /// </summary>
    public struct CCColor4B
    {
        public static readonly CCColor4B White = new CCColor4B(255, 255, 255, 255);
        public static readonly CCColor4B Yellow = new CCColor4B(255, 255, 0, 255);
        public static readonly CCColor4B Blue = new CCColor4B(0, 0, 255, 255);
        public static readonly CCColor4B Green = new CCColor4B(0, 255, 0, 255);
        public static readonly CCColor4B Red = new CCColor4B(255, 0, 0, 255);
        public static readonly CCColor4B Magenta = new CCColor4B(255, 0, 255, 255);
        public static readonly CCColor4B Black = new CCColor4B(0, 0, 0, 255);
        public static readonly CCColor4B Orange = new CCColor4B(255, 127, 0, 255);
        public static readonly CCColor4B Gray = new CCColor4B(166, 166, 166, 255);
        public static readonly CCColor4B AliceBlue = new CCColor4B(240, 248, 255, 255);
        public static readonly CCColor4B Aquamarine = new CCColor4B (127, 255, 212, 255);


        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }


        #region Constructors

        public CCColor4B(byte inr, byte ing, byte inb, byte ina) : this()
        {
            R = inr;
            G = ing;
            B = inb;
            A = ina;
        }

        public CCColor4B(byte inr, byte ing, byte inb) : this(inr, ing, inb, 255)
        {
        }

        #endregion Constructors


        public override string ToString()
        {
            return (string.Format("{0},{1},{2},{3}", R, G, B, A));
        }

        public static CCColor4B Parse(string s)
        {
            string[] f = s.Split(',');
            return (new CCColor4B(byte.Parse(f[0]), byte.Parse(f[1]), byte.Parse(f[2]), byte.Parse(f[3])));
        }


        #region Operators

        public static CCColor4B operator *(CCColor4B p1, CCColor4B p2)
        {
            return new CCColor4B((byte)(p1.R * p2.R), (byte)(p1.G * p2.G), (byte)(p1.B * p2.B), (byte)(p1.A * p2.A));
        }

        public static CCColor4B operator /(CCColor4B p1, float div)
        {
            return new CCColor4B((byte)(p1.R / div), (byte)(p1.G / div), (byte)(p1.B / div), (byte)(p1.A / div));
        }

        public static bool operator ==(CCColor4B p1, CCColor4B p2)
        {
            return p1.R == p2.R && p1.G == p2.G && p1.B == p2.B && p1.A == p2.A;
        }

        public static bool operator !=(CCColor4B p1, CCColor4B p2)
        {
            return p1.R != p2.R || p1.G != p2.G || p1.B != p2.B || p1.A != p2.A;
        }

        public override int GetHashCode()
        {
            return (unchecked (R ^ G ^ B ^ A));
        }

        public override bool Equals(object obj)        
        {            
            if (!(obj is CCColor4B))                
                return false;             

            return Equals((CCColor4B)obj);        
        }         

        public bool Equals(CCColor4B other)        
        {            
            return this == other;       
        } 

        #endregion Operators


        public static CCColor4B Lerp(CCColor4B value1, CCColor4B value2, float amount)
        {
            CCColor4B color = new CCColor4B();

            color.A = (byte)(value1.A + ((value2.A - value1.A) * amount));
            color.R = (byte)(value1.R + ((value2.R - value1.R) * amount));
            color.G = (byte)(value1.G + ((value2.G - value1.G) * amount));
            color.B = (byte)(value1.B + ((value2.B - value1.B) * amount));

            return color;
        }
    }

    /// <summary>
    /// RGBA color composed of 4 floats
    /// @since v0.8
    /// </summary>
    public struct CCColor4F
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }


        #region Constructors

        public CCColor4F(float inr, float ing, float inb, float ina) : this()
        {
            R = inr;
            G = ing;
            B = inb;
            A = ina;
        }

        public CCColor4F(CCColor3B c) : this(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, 1.0f)
        {
        }

        public CCColor4F(CCColor4B c) : this(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, c.A / 255.0f)
        {
        }

        #endregion Constructors


        public override string ToString()
        {
            return (string.Format("{0},{1},{2},{3}", R, G, B, A));
        }

        public static CCColor4F Parse(string s)
        {
            string[] f = s.Split(',');
            return (new CCColor4F(float.Parse(f[0]), float.Parse(f[1]), float.Parse(f[2]), float.Parse(f[3])));
        }


        #region Operators

        public static implicit operator Color(CCColor4F point)
        {
            return new Color(point.R, point.G, point.B, point.A);
        }

        public static bool operator ==(CCColor4F a, CCColor4F b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator !=(CCColor4F a, CCColor4F b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        public override int GetHashCode()
        {
            // Components should be clamped between 0.0f and 1.0f
            // Therefore scale them up to get an int between 0 and 400
            float scaledSum = (R + G + B + A) * 100.0f;
            return (int)scaledSum;
        }

        public override bool Equals(object obj)        
        {            
            if (!(obj is CCColor4F))                
                return false;             

            return Equals((CCColor4F)obj);        
        }         

        public bool Equals(CCColor4F other)        
        {            
            return this == other;       
        } 

        #endregion Operators
    }

    #endregion Colors


    #region Vertices

    /// <summary>
    /// A vertex composed of 2 floats: x, y
    /// @since v0.8
    /// </summary>
    public struct CCVertex2F
    {
        public static readonly CCVertex2F ZeroVector = new CCVertex2F(0.0f, 0.0f);

        public float X { get; set; }
        public float Y { get; set; }


        #region Constructors

        public CCVertex2F(float inx, float iny) : this()
        {
            X = inx;
            Y = iny;
        }

        public CCVertex2F(CCVertex3F ver3) : this(ver3.X, ver3.Y)
        {
        }

        #endregion Constructors
    }

    /// <summary>
    /// A vertex composed of 2 floats: x, y
    /// @since v0.8
    /// </summary>
    public struct CCVertex3F
    {
        public static readonly CCVertex3F Zero = new CCVertex3F();

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }


        #region Constructors
        
        public CCVertex3F(float inx, float iny, float inz) : this()
        {
            X = inx;
            Y = iny;
            Z = inz;
        }

        #endregion Constructors


        public override string ToString()
        {
            return String.Format("CCVertex3F x:{0}, y:{1}, z:{2}", X, Y, Z);
        }
    }

    #endregion Vertices


    #region Tex coords

    /// <summary>
    /// A texcoord composed of 2 floats: u, y
    /// @since v0.8
    /// </summary>
    public struct CCTex2F
    {
        public float U { get; set; }
        public float V { get; set; }

        public CCTex2F(float inu, float inv) : this()
        {
            U = inu;
            V = inv;
        }

        public override string ToString()
        {
            return String.Format("CCTex2F u:{0}, v:{1}", U, V);
        }
    }

    #endregion Tex coords


    #region Quads

    /// <summary>
    /// A 2D Quad. 4 * 2 floats
    /// </summary>
    public struct CCQuad2
    {
        public CCVertex2F TopLeft;
        public CCVertex2F TopRight;
        public CCVertex2F BottomLeft;
        public CCVertex2F BottomRight;

        public CCQuad2(CCVertex2F tL = new CCVertex2F(), CCVertex2F tR = new CCVertex2F(), 
            CCVertex2F bL = new CCVertex2F(), CCVertex2F bR = new CCVertex2F()) : this()
        {
            TopLeft = tL;
            TopRight = tR;
            BottomLeft = bL;
            BottomRight = bR;
        }
    }

    /// <summary>
    /// A 3D Quad. 4 * 3 floats
    /// </summary>
    public struct CCQuad3
    {
        public CCVertex3F BottomLeft;
        public CCVertex3F BottomRight;
        public CCVertex3F TopLeft;
        public CCVertex3F TopRight;

        public CCQuad3(CCVertex3F tL = new CCVertex3F(), CCVertex3F tR = new CCVertex3F(), 
            CCVertex3F bL = new CCVertex3F(), CCVertex3F bR = new CCVertex3F()) : this()
        {
            TopLeft = tL;
            TopRight = tR;
            BottomLeft = bL;
            BottomRight = bR;
        }
    }

    #endregion Quads



    #region Points

    /// <summary>
    /// Point Sprite component
    /// </summary>
    public class CCPointSprite
    {
        public CCVertex2F Position { get; set; }
        public CCColor4B Color { get; set; }
        public float Size { get; set; }

        public CCPointSprite()
        {
            Position = new CCVertex2F();
            Color = new CCColor4B();
            Size = 0.0f;
        }
    }

    public struct CCPointI
    {
        public int X;
        public int Y;

        public CCPointI(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Distance(ref CCPointI p)
        {
            var hside = X - p.X;
            var vside = Y - p.Y;

            return (int)Math.Sqrt(hside * hside + vside * vside);
        }

        public static implicit operator CCPoint(CCPointI p)
        {
            return new CCPoint(p.X, p.Y);
        }


        #region Equality and Operators

        public bool Equals(ref CCPointI p)
        {
            return X == p.X && Y == p.Y;
        }

        public override bool Equals(object obj)        
        {            
            if (!(obj is CCPointI))                
                return false;             

            return Equals((CCPointI)obj);        
        }         

        public bool Equals(CCPointI other)        
        {            
            return this == other;       
        }

        public override int GetHashCode()
        {
            return (unchecked (X ^ Y));
        }

        public static bool operator ==(CCPointI p1, CCPointI p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(CCPointI p1, CCPointI p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public static CCPointI operator -(CCPointI p1, CCPointI p2)
        {
            CCPointI pt;
            pt.X = p1.X - p2.X;
            pt.Y = p1.Y - p2.Y;
            return pt;
        }

        public static CCPointI operator -(CCPointI p1)
        {
            CCPointI pt;
            pt.X = -p1.X;
            pt.Y = -p1.Y;
            return pt;
        }

        public static CCPointI operator +(CCPointI p1, CCPointI p2)
        {
            CCPointI pt;
            pt.X = p1.X + p2.X;
            pt.Y = p1.Y + p2.Y;
            return pt;
        }

        public static CCPointI operator +(CCPointI p1)
        {
            CCPointI pt;
            pt.X = +p1.X;
            pt.Y = +p1.Y;
            return pt;
        }

        #endregion Equality and Operators
    }

    #endregion Points


    /// <summary>
    /// A 2D grid size
    /// </summary>
    public struct CCGridSize
    {
        public static readonly CCGridSize Zero = new CCGridSize(0,0);
        public static readonly CCGridSize One = new CCGridSize(1,1);

        public int X;
        public int Y;

        public CCGridSize(int inx, int iny)
        {
            X = inx;
            Y = iny;
        }
    }

    public struct CCSizeI
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public CCSizeI(int width, int height) : this()
        {
            Width = width;
            Height = height;
        }

        public static implicit operator CCSize(CCSizeI p)
        {
            return new CCSize(p.Width, p.Height);
        }
    }

    public struct CCBoundingBoxI
    {
        public static readonly CCBoundingBoxI Zero = new CCBoundingBoxI(0, 0, 0, 0);
        public static readonly CCBoundingBoxI Null = new CCBoundingBoxI(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public CCSizeI Size
        {
            get { return new CCSizeI(MaxX - MinX, MaxY - MinY); }
        }


        #region Constructors

        public CCBoundingBoxI(int minx, int miny, int maxx, int maxy) : this()
        {
            MinX = minx;
            MinY = miny;
            MaxX = maxx;
            MaxY = maxy;
        }

        #endregion Constructors


        public void ExpandToCircle(int x, int y, int radius)
        {
            Debug.Assert(radius >= 0);

            MinX = Math.Min(MinX, x - radius);
            MinY = Math.Min(MinY, y - radius);
            MaxX = Math.Max(MaxX, x + radius);
            MaxY = Math.Max(MaxY, x + radius);
        }

        public void ExpandToCircle(ref CCPointI point, int radius)
        {
            ExpandToCircle(point.X, point.Y, radius);
        }

        public void ExpandToPoint(int x, int y)
        {
            MinX = Math.Min(MinX, x);
            MinY = Math.Min(MinY, y);
            MaxX = Math.Max(MaxX, x);
            MaxY = Math.Max(MaxY, y);
        }

        public void ExpandToPoint(ref CCPointI point)
        {
            ExpandToPoint(point.X, point.Y);
        }

        public void ExpandToRect(ref CCBoundingBoxI r)
        {
            MinX = Math.Min(MinX, r.MinX);
            MinY = Math.Min(MinY, r.MinY);
            MaxX = Math.Max(MaxX, r.MaxX);
            MaxY = Math.Max(MaxY, r.MaxY);
        }

        public bool Intersects(ref CCBoundingBoxI rect)
        {
            return !(MaxX < rect.MinX || rect.MaxX < MinX || MaxY < rect.MinY || rect.MaxY < MinY);
        }

        public void SetLerp(CCBoundingBoxI a, CCBoundingBoxI b, float ratio)
        {
            MinX = CCMathHelper.Lerp(a.MinX, b.MinX, ratio);
            MinY = CCMathHelper.Lerp(a.MinY, b.MinY, ratio);
            MaxX = CCMathHelper.Lerp(a.MaxX, b.MaxX, ratio);
            MaxY = CCMathHelper.Lerp(a.MaxY, b.MaxY, ratio);
        }

        public CCBoundingBoxI Transform(CCAffineTransform matrix)
        {
            var top = MinY;
            var left = MinX;
            var right = MaxX;
            var bottom = MaxY;

            var topLeft = new CCPointI(left, top);
            var topRight = new CCPointI(right, top);
            var bottomLeft = new CCPointI(left, bottom);
            var bottomRight = new CCPointI(right, bottom);

            matrix.Transform(ref topLeft.X, ref topLeft.Y);
            matrix.Transform(ref topRight.Y, ref topRight.Y);
            matrix.Transform(ref bottomLeft.X, ref bottomLeft.Y);
            matrix.Transform(ref bottomRight.X, ref bottomRight.Y);

            int minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            int maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            int minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            int maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            return new CCBoundingBoxI(minX, minY, maxX, maxY);
        }

        public static implicit operator CCRect(CCBoundingBoxI box)
        {
            return new CCRect(box.MinX, box.MinY, box.MaxX - box.MinX, box.MaxY - box.MinY);
        }
    }

    #region Drawing buffer structures

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4B
    /// </summary>
    public class CCV2F_C4B_T2F
    {
        public CCVertex2F Vertices;
        public CCColor4B Colors;
        public CCTex2F TexCoords;

        public CCV2F_C4B_T2F()
        {
            Vertices = new CCVertex2F();
            Colors = new CCColor4B();
            TexCoords = new CCTex2F();
        }
    }

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4F
    /// </summary>
    public class CCV2F_C4F_T2F
    {
        public CCVertex2F Vertices;
        public CCColor4F Colors;
        public CCTex2F TexCoords;

        public CCV2F_C4F_T2F()
        {
            Vertices = new CCVertex2F();
            Colors = new CCColor4F();
            TexCoords = new CCTex2F();
        }
    }

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4B
    /// </summary>
    //TODO: Use VertexPositionColorTexture
    public struct CCV3F_C4B_T2F : IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration;

        public CCVertex3F Vertices;
        public CCColor4B Colors;
        public CCTex2F TexCoords;

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

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
    }

    /// <summary>
    /// 4 ccVertex2FTex2FColor4B Quad
    /// </summary>
    public class CCV2F_C4B_T2F_Quad
    {
        public CCV2F_C4B_T2F BottomLeft;
        public CCV2F_C4B_T2F BottomRight;
        public CCV2F_C4B_T2F TopLeft;
        public CCV2F_C4B_T2F TopRight;

        public CCV2F_C4B_T2F_Quad()
        {
            BottomLeft = new CCV2F_C4B_T2F();
            BottomRight = new CCV2F_C4B_T2F();
            TopLeft = new CCV2F_C4B_T2F();
            TopRight = new CCV2F_C4B_T2F();
        }
    }

    /// <summary>
    /// 4 ccVertex3FTex2FColor4B
    /// </summary>
    public struct CCV3F_C4B_T2F_Quad : IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration;

        public CCV3F_C4B_T2F TopLeft;
        public CCV3F_C4B_T2F BottomLeft;
        public CCV3F_C4B_T2F TopRight;
        public CCV3F_C4B_T2F BottomRight;

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        static CCV3F_C4B_T2F_Quad()
        {
            var elements = new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                    new VertexElement(0x10, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                };
            VertexDeclaration = new VertexDeclaration(elements);
        }
    }

    /// <summary>
    /// 4 ccVertex2FTex2FColor4F Quad
    /// </summary>
    public class CCV2F_C4F_T2F_Quad
    {
        public CCV2F_C4F_T2F BottomLeft;
        public CCV2F_C4F_T2F BottomRight;
        public CCV2F_C4F_T2F TopLeft;
        public CCV2F_C4F_T2F TopRight;


        public CCV2F_C4F_T2F_Quad()
        {
            TopLeft = new CCV2F_C4F_T2F();
            BottomLeft = new CCV2F_C4F_T2F();
            TopRight = new CCV2F_C4F_T2F();
            BottomRight = new CCV2F_C4F_T2F();
        }
    }

    #endregion Drawing buffer structures

    /// <summary>
    /// Blend Function used for textures
    /// </summary>
    public struct CCBlendFunc
    {
        public static readonly CCBlendFunc AlphaBlend = new CCBlendFunc(CCOGLES.GL_ONE, CCOGLES.GL_ONE_MINUS_SRC_ALPHA);
        public static readonly CCBlendFunc Additive = new CCBlendFunc(CCOGLES.GL_SRC_ALPHA, CCOGLES.GL_ONE);
        public static readonly CCBlendFunc NonPremultiplied = new CCBlendFunc(CCOGLES.GL_SRC_ALPHA, CCOGLES.GL_ONE_MINUS_SRC_ALPHA);
        public static readonly CCBlendFunc Opaque = new CCBlendFunc(CCOGLES.GL_ONE, CCOGLES.GL_ZERO);

        public int Source { get; set; }
        public int Destination { get; set; }


        public CCBlendFunc(int src, int dst) : this()
        {
            this.Source = src;
            this.Destination = dst;
        }

        #region Equality

        public static bool operator ==(CCBlendFunc b1, CCBlendFunc b2)
        {
            return b1.Source == b2.Source && b1.Destination == b2.Destination;
        }

        public static bool operator !=(CCBlendFunc b1, CCBlendFunc b2)
        {
            return b1.Source != b2.Source || b1.Destination != b2.Destination;
        }

        public override bool Equals(object obj)        
        {            
            if (!(obj is CCBlendFunc))                
                return false;             

            return Equals((CCBlendFunc)obj);        
        }         

        public bool Equals(CCBlendFunc other)        
        {            
            return this == other;       
        } 

        public override int GetHashCode()
        {
            return unchecked(Source ^ Destination);
        }

        #endregion Equality
    }
}

