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

namespace Cocos2D
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
        internal bool Dirty = true; //indicates whether or not the array buffer of the VBO needs to be updated

        private CCQuadVertexBuffer m_pVertexBuffer;
        public CCRawList<CCV3F_C4B_T2F_Quad> m_pQuads;
        protected CCTexture2D m_pTexture;

        #region properties

        /// <summary>
        /// quantity of quads that are going to be drawn
        /// </summary>
        public int TotalQuads
        {
            get { return m_pQuads.count; }
        }

        /// <summary>
        /// quantity of quads that can be stored with the current texture atlas size
        /// </summary>
        public int Capacity
        {
            get { return m_pQuads.Capacity; }
            set { m_pQuads.Capacity = value; }
        }

        /// <summary>
        /// Texture of the texture atlas
        /// </summary>
        public CCTexture2D Texture
        {
            get { return m_pTexture; }
            set { m_pTexture = value; }
        }

		public bool IsAntialiased
		{
			get { return Texture.IsAntialiased; }

			set { Texture.IsAntialiased = value; }
		}

        #endregion

        public override string ToString()
        {
            return string.Format("TotalQuads:{0}", TotalQuads);
        }

        /// <summary>
        /// draws all the Atlas's Quads
        /// </summary>
        public void DrawQuads()
        {
            DrawNumberOfQuads(TotalQuads, 0);
        }

        /// <summary>
        /// draws n quads
        ///  can't be greater than the capacity of the Atlas
        ///  n
        /// </summary>
        public void DrawNumberOfQuads(int n)
        {
            DrawNumberOfQuads(n, 0);
        }


        /// <summary>
        /// draws n quads from an index (offset).
        /// n + start can't be greater than the capacity of the atlas
        /// @since v1.0
        /// </summary>
        public void DrawNumberOfQuads(int n, int start)
        {
            if (n == 0)
            {
                return;
            }

            CCDrawManager.BindTexture(Texture);

            if (Dirty)
            {
                m_pVertexBuffer.UpdateBuffer();
                Dirty = false;
            }

            CCDrawManager.DrawQuadsBuffer(m_pVertexBuffer, start, n);
        }

        /// <summary>
        /// resize the capacity of the CCTextureAtlas.
        /// The new capacity can be lower or higher than the current one
        /// It returns YES if the resize was successful.
        ///  If it fails to resize the capacity it will return NO with a new capacity of 0.
        /// </summary>
        public bool ResizeCapacity(int newCapacity)
        {
            if (newCapacity <= m_pQuads.Capacity)
            {
                return true;
            }

            m_pVertexBuffer.Capacity = newCapacity;

            m_pQuads = m_pVertexBuffer.Data;

            Dirty = true;

            return true;
        }

        public void IncreaseTotalQuadsWith(int amount)
        {
            m_pVertexBuffer.Count += amount;
        }

        public void MoveQuadsFromIndex(int oldIndex, int amount, int newIndex)
        {
            Debug.Assert(newIndex + amount <= m_pQuads.count, "insertQuadFromIndex:atIndex: Invalid index");
            Debug.Assert(oldIndex < m_pQuads.count, "insertQuadFromIndex:atIndex: Invalid index");

            if (oldIndex == newIndex)
            {
                return;
            }

            var tmp = new CCV3F_C4B_T2F_Quad[amount];
            Array.Copy(m_pQuads.Elements, oldIndex, tmp, 0, amount);

            if (newIndex < oldIndex)
            {
                // move quads from newIndex to newIndex + amount to make room for buffer
                Array.Copy(m_pQuads.Elements, newIndex + amount, m_pQuads.Elements, newIndex, oldIndex - newIndex);
            }
            else
            {
                // move quads above back
                Array.Copy(m_pQuads.Elements, oldIndex + amount, m_pQuads.Elements, oldIndex, newIndex - oldIndex);
            }
            Array.Copy(tmp, 0, m_pQuads.Elements, newIndex, amount);

            Dirty = true;
        }

        public void MoveQuadsFromIndex(int index, int newIndex)
        {
            Debug.Assert(newIndex + (m_pQuads.count - index) <= m_pQuads.Capacity, "moveQuadsFromIndex move is out of bounds");

            Array.Copy(m_pQuads.Elements, index, m_pQuads.Elements, newIndex, m_pQuads.count - index);
            Dirty = true;
        }

        public void FillWithEmptyQuadsFromIndex(int index, int amount)
        {
            int to = index + amount;
            CCV3F_C4B_T2F_Quad[] elements = m_pQuads.Elements;
            var empty = new CCV3F_C4B_T2F_Quad();

            for (int i = index; i < to; i++)
            {
                elements[i] = empty;
            }

            Dirty = true;
        }

        #region create and init

        /// <summary>
        /// creates a TextureAtlas with an filename and with an initial capacity for Quads.
        /// The TextureAtlas capacity can be increased in runtime.
        /// </summary>
        public static CCTextureAtlas Create(string file, int capacity)
        {
            var pTextureAtlas = new CCTextureAtlas();
            if (pTextureAtlas.InitWithFile(file, capacity))
            {
                return pTextureAtlas;
            }

            return null;
        }

        /// <summary>
        /// initializes a TextureAtlas with a filename and with a certain capacity for Quads.
        /// The TextureAtlas capacity can be increased in runtime.
        /// WARNING: Do not reinitialize the TextureAtlas because it will leak memory (issue #706)
        /// </summary>
        public bool InitWithFile(string file, int capacity)
        {
            // retained in property
            CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(file);
            if (texture != null)
            {
                return InitWithTexture(texture, capacity);
            }
            return false;
        }

        /// <summary>
        /// creates a TextureAtlas with a previously initialized Texture2D object, and
        /// with an initial capacity for n Quads. 
        /// The TextureAtlas capacity can be increased in runtime.
        /// </summary>
        public static CCTextureAtlas Create(CCTexture2D texture, int capacity)
        {
            var pTextureAtlas = new CCTextureAtlas();
            if (pTextureAtlas.InitWithTexture(texture, capacity))
            {
                return pTextureAtlas;
            }

            return null;
        }

        /// <summary>
        /// initializes a TextureAtlas with a previously initialized Texture2D object, and
        /// with an initial capacity for Quads. 
        /// The TextureAtlas capacity can be increased in runtime.
        /// WARNING: Do not reinitialize the TextureAtlas because it will leak memory (issue #706)
        /// </summary>
        public bool InitWithTexture(CCTexture2D texture, int capacity)
        {
            //Debug.Assert(texture != null);

            // retained in property
            m_pTexture = texture;

            // Re-initialization is not allowed
            Debug.Assert(m_pQuads == null);

            if (capacity < 4)
            {
                capacity = 4;
            }

            m_pVertexBuffer = new CCQuadVertexBuffer(capacity, BufferUsage.WriteOnly);
            m_pQuads = m_pVertexBuffer.Data;

            Dirty = true;

            return true;
        }

        #endregion

        #region Quads

        /// <summary>
        /// updates a Quad (texture, vertex and color) at a certain index
        /// index must be between 0 and the atlas capacity - 1
        /// @since v0.8
        /// </summary>
        public void UpdateQuad(ref CCV3F_C4B_T2F_Quad quad, int index)
        {
            Debug.Assert(index >= 0 && index < m_pQuads.Capacity, "updateQuadWithTexture: Invalid index");
            m_pQuads.count = Math.Max(index + 1, m_pQuads.count);
            m_pQuads.Elements[index] = quad;
            Dirty = true;
        }

        /// <summary>
        /// Inserts a Quad (texture, vertex and color) at a certain index
        /// index must be between 0 and the atlas capacity - 1
        /// @since v0.8
        /// </summary>
        public void InsertQuad(ref CCV3F_C4B_T2F_Quad quad, int index)
        {
            Debug.Assert(index < m_pQuads.Capacity, "insertQuadWithTexture: Invalid index");
            m_pQuads.Insert(index, quad);
            Dirty = true;
        }

        /// Removes the quad that is located at a certain index and inserts it at a new index
        /// This operation is faster than removing and inserting in a quad in 2 different steps
        /// @since v0.7.2
        public void InsertQuadFromIndex(int oldIndex, int newIndex)
        {
            Debug.Assert(newIndex >= 0 && newIndex < m_pQuads.count, "insertQuadFromIndex:atIndex: Invalid index");
            Debug.Assert(oldIndex >= 0 && oldIndex < m_pQuads.count, "insertQuadFromIndex:atIndex: Invalid index");

            if (oldIndex == newIndex)
                return;

            // because it is ambigious in iphone, so we implement abs ourself
            // unsigned int howMany = abs( oldIndex - newIndex);
            int howMany = (oldIndex - newIndex) > 0 ? (oldIndex - newIndex) : (newIndex - oldIndex);
            int dst = oldIndex;
            int src = oldIndex + 1;
            if (oldIndex > newIndex)
            {
                dst = newIndex + 1;
                src = newIndex;
            }

            CCV3F_C4B_T2F_Quad[] elements = m_pQuads.Elements;

            CCV3F_C4B_T2F_Quad quadsBackup = elements[oldIndex];
            Array.Copy(elements, src, elements, dst, howMany);
            elements[newIndex] = quadsBackup;

            Dirty = true;
        }

        /// <summary>
        /// removes a quad at a given index number.
        /// The capacity remains the same, but the total number of quads to be drawn is reduced in 1
        /// @since v0.7.2
        /// </summary>
        public void RemoveQuadAtIndex(int index)
        {
            Debug.Assert(index < m_pQuads.count, "removeQuadAtIndex: Invalid index");
            m_pQuads.RemoveAt(index);
            Dirty = true;
        }

        public void RemoveQuadsAtIndex(int index, int amount)
        {
            Debug.Assert(index + amount <= m_pQuads.count, "removeQuadAtIndex: Invalid index");
            m_pQuads.RemoveAt(index, amount);
            Dirty = true;
        }

        public void RemoveAllQuads()
        {
            m_pQuads.Clear();
            Dirty = true;
        }

        #endregion
    }
}