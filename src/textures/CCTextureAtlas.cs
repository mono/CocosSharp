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
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    /// <summary>
    /// A class that implements a Texture Atlas.
    /// </summary>
    ///<remarks>
    /// Supported features:
    ///  The atlas file can be a PVRTC, PNG or any other fomrat supported by Texture2D
    ///  Quads can be udpated in runtime
    ///  Quads can be added in runtime
    ///  Quads can be removed in runtime
    ///  Quads can be re-ordered in runtime
    ///  The TextureAtlas capacity can be increased or decreased in runtime
    ///  OpenGL component: V3F, C4B, T2F.
    /// The quads are rendered using an OpenGL ES VBO.
    /// To render the quads using an interleaved vertex array list, you should modify the ccConfig.h file 
    ///</remarks>
    public class CCTextureAtlas 
    {
        CCQuadVertexBuffer vertexBuffer;


        #region Properties

        protected internal CCRawList<CCV3F_C4B_T2F_Quad> Quads { get; private set; }
        public CCTexture2D Texture { get; set; }

        // Indicates whether or not the array buffer of the VBO needs to be updated
        protected internal bool Dirty { get; set; }                                 

        public int TotalQuads
        {
            get { return Quads.Count; }
        }

        public int Capacity
        {
            get { return Quads.Capacity; }
            set { Quads.Capacity = value; }
        }

        public bool IsAntialiased
        {
            get { return Texture.IsAntialiased; }
            set { Texture.IsAntialiased = value; }
        }

        #endregion Properties


        #region Constructors

        public CCTextureAtlas(string file, int capacity) : this(CCTextureCache.SharedTextureCache.AddImage(file), capacity)
        {
        }

        public CCTextureAtlas(CCTexture2D texture, int capacity)
        {
            Texture = texture;

            // Re-initialization is not allowed
            Debug.Assert(Quads == null);

            if (capacity < 4)
            {
                capacity = 4;
            }

            vertexBuffer = new CCQuadVertexBuffer(capacity, CCBufferUsage.WriteOnly, CCDrawManager.SharedDrawManager);
            Quads = vertexBuffer.Data;

            Dirty = true;
        }

        #endregion Constructors


        public override string ToString()
        {
            return string.Format("TotalQuads:{0}", TotalQuads);
        }


        #region Drawing

        public void DrawQuads()
        {
            DrawNumberOfQuads(TotalQuads, 0);
        }

        public void DrawNumberOfQuads(int n)
        {
            DrawNumberOfQuads(n, 0);
        }

        // draws n quads from an index (offset).
        // n + start can't be greater than the capacity of the atlas
        public void DrawNumberOfQuads(int n, int start)
        {
            if (n == 0)
            {
                return;
            }

            CCDrawManager.SharedDrawManager.BindTexture(Texture);

            if (Dirty)
            {
                vertexBuffer.UpdateBuffer();
                Dirty = false;
            }

            CCDrawManager.SharedDrawManager.DrawQuadsBuffer(vertexBuffer, start, n);
        }

        #endregion Drawing


        public void ResizeCapacity(int newCapacity)
        {
            if (newCapacity <= Quads.Capacity)
            {
                return;
            }

            vertexBuffer.Capacity = newCapacity;

            Quads = vertexBuffer.Data;

            Dirty = true;
        }


        #region Managing Quads

        public void IncreaseTotalQuadsWith(int amount)
        {
            vertexBuffer.Count += amount;
        }

        public void MoveQuadsFromIndex(int oldIndex, int amount, int newIndex)
        {
            Debug.Assert(newIndex + amount <= Quads.Count, "insertQuadFromIndex:atIndex: Invalid index");
            Debug.Assert(oldIndex < Quads.Count, "insertQuadFromIndex:atIndex: Invalid index");

            if (oldIndex == newIndex)
            {
                return;
            }

            var tmp = new CCV3F_C4B_T2F_Quad[amount];
            Array.Copy(Quads.Elements, oldIndex, tmp, 0, amount);

            if (newIndex < oldIndex)
            {
                // move quads from newIndex to newIndex + amount to make room for buffer
                Array.Copy(Quads.Elements, newIndex + amount, Quads.Elements, newIndex, oldIndex - newIndex);
            }
            else
            {
                // move quads above back
                Array.Copy(Quads.Elements, oldIndex + amount, Quads.Elements, oldIndex, newIndex - oldIndex);
            }
            Array.Copy(tmp, 0, Quads.Elements, newIndex, amount);

            Dirty = true;
        }

        public void MoveQuadsFromIndex(int index, int newIndex)
        {
            Debug.Assert(newIndex + (Quads.Count - index) <= Quads.Capacity, "moveQuadsFromIndex move is out of bounds");

            Array.Copy(Quads.Elements, index, Quads.Elements, newIndex, Quads.Count - index);
            Dirty = true;
        }

        public void FillWithEmptyQuadsFromIndex(int index, int amount)
        {
            int to = index + amount;
            CCV3F_C4B_T2F_Quad[] elements = Quads.Elements;
            var empty = new CCV3F_C4B_T2F_Quad();

            for (int i = index; i < to; i++)
            {
                elements[i] = empty;
            }

            Dirty = true;
        }

        public void UpdateQuad(ref CCV3F_C4B_T2F_Quad quad, int index)
        {
            Debug.Assert(index >= 0 && index < Quads.Capacity, "updateQuadWithTexture: Invalid index");
            Quads.Count = Math.Max(index + 1, Quads.Count);
            Quads.Elements[index] = quad;
            Dirty = true;
        }

        public void InsertQuad(ref CCV3F_C4B_T2F_Quad quad, int index)
        {
            Debug.Assert(index < Quads.Capacity, "insertQuadWithTexture: Invalid index");
            Quads.Insert(index, quad);
            Dirty = true;
        }

        public void InsertQuadFromIndex(int oldIndex, int newIndex)
        {
            Debug.Assert(newIndex >= 0 && newIndex < Quads.Count, "insertQuadFromIndex:atIndex: Invalid index");
            Debug.Assert(oldIndex >= 0 && oldIndex < Quads.Count, "insertQuadFromIndex:atIndex: Invalid index");

            if (oldIndex == newIndex)
                return;

            int howMany = Math.Abs(oldIndex - newIndex);
            int dst = oldIndex;
            int src = oldIndex + 1;
            if (oldIndex > newIndex)
            {
                dst = newIndex + 1;
                src = newIndex;
            }

            CCV3F_C4B_T2F_Quad[] elements = Quads.Elements;

            CCV3F_C4B_T2F_Quad quadsBackup = elements[oldIndex];
            Array.Copy(elements, src, elements, dst, howMany);
            elements[newIndex] = quadsBackup;

            Dirty = true;
        }

        // Removes a quad at a given index number.
        // The capacity remains the same, but the total number of quads to be drawn is reduced in 1
        public void RemoveQuadAtIndex(int index)
        {
            Debug.Assert(index < Quads.Count, "removeQuadAtIndex: Invalid index");
            Quads.RemoveAt(index);
            Dirty = true;
        }

        public void RemoveQuadsAtIndex(int index, int amount)
        {
            Debug.Assert(index + amount <= Quads.Count, "removeQuadAtIndex: Invalid index");
            Quads.RemoveAt(index, amount);
            Dirty = true;
        }

        public void RemoveAllQuads()
        {
            Quads.Clear();
            Dirty = true;
        }

        #endregion Managing Quads
    }
}