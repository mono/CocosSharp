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
        internal bool Dirty = true; //indicates whether or not the array buffer of the VBO needs to be updated

		private CCQuadVertexBuffer vertexBuffer;
        public CCRawList<CCV3F_C4B_T2F_Quad> quads;
        protected CCTexture2D texture;

        #region properties

        /// <summary>
        /// quantity of quads that are going to be drawn
        /// </summary>
        public int TotalQuads
        {
            get { return quads.count; }
        }

        /// <summary>
        /// quantity of quads that can be stored with the current texture atlas size
        /// </summary>
        public int Capacity
        {
            get { return quads.Capacity; }
            set { quads.Capacity = value; }
        }

        /// <summary>
        /// Texture of the texture atlas
        /// </summary>
        public CCTexture2D Texture
        {
            get { return texture; }
            set { texture = value; }
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
                vertexBuffer.UpdateBuffer();
                Dirty = false;
            }

            CCDrawManager.DrawQuadsBuffer(vertexBuffer, start, n);
        }

        /// <summary>
        /// resize the capacity of the CCTextureAtlas.
        /// The new capacity can be lower or higher than the current one
        /// It returns YES if the resize was successful.
        ///  If it fails to resize the capacity it will return NO with a new capacity of 0.
        /// </summary>
        public bool ResizeCapacity(int newCapacity)
        {
            if (newCapacity <= quads.Capacity)
            {
                return true;
            }

            vertexBuffer.Capacity = newCapacity;

            quads = vertexBuffer.Data;

            Dirty = true;

            return true;
        }

        public void IncreaseTotalQuadsWith(int amount)
        {
            vertexBuffer.Count += amount;
        }

        public void MoveQuadsFromIndex(int oldIndex, int amount, int newIndex)
        {
            Debug.Assert(newIndex + amount <= quads.count, "insertQuadFromIndex:atIndex: Invalid index");
            Debug.Assert(oldIndex < quads.count, "insertQuadFromIndex:atIndex: Invalid index");

            if (oldIndex == newIndex)
            {
                return;
            }

            var tmp = new CCV3F_C4B_T2F_Quad[amount];
            Array.Copy(quads.Elements, oldIndex, tmp, 0, amount);

            if (newIndex < oldIndex)
            {
                // move quads from newIndex to newIndex + amount to make room for buffer
                Array.Copy(quads.Elements, newIndex + amount, quads.Elements, newIndex, oldIndex - newIndex);
            }
            else
            {
                // move quads above back
                Array.Copy(quads.Elements, oldIndex + amount, quads.Elements, oldIndex, newIndex - oldIndex);
            }
            Array.Copy(tmp, 0, quads.Elements, newIndex, amount);

            Dirty = true;
        }

        public void MoveQuadsFromIndex(int index, int newIndex)
        {
            Debug.Assert(newIndex + (quads.count - index) <= quads.Capacity, "moveQuadsFromIndex move is out of bounds");

            Array.Copy(quads.Elements, index, quads.Elements, newIndex, quads.count - index);
            Dirty = true;
        }

        public void FillWithEmptyQuadsFromIndex(int index, int amount)
        {
            int to = index + amount;
            CCV3F_C4B_T2F_Quad[] elements = quads.Elements;
            var empty = new CCV3F_C4B_T2F_Quad();

            for (int i = index; i < to; i++)
            {
                elements[i] = empty;
            }

            Dirty = true;
        }

        #region create and init

        public CCTextureAtlas()
        {
        }

        public CCTextureAtlas(string file, int capacity) : this()
        {
            InitWithFile (file, capacity);
        }

        public CCTextureAtlas(CCTexture2D texture, int capacity) : this()
        {
            InitWithTexture (texture, capacity);
        }

        /// <summary>
        /// initializes a TextureAtlas with a filename and with a certain capacity for Quads.
        /// The TextureAtlas capacity can be increased in runtime.
        /// WARNING: Do not reinitialize the TextureAtlas because it will leak memory (issue #706)
        /// </summary>
        private void InitWithFile(string file, int capacity)
        {
            // retained in property
            CCTexture2D texture = CCTextureCache.Instance.AddImage(file);
            if (texture != null)
            {
                InitWithTexture(texture, capacity);
            }
        }

        /// <summary>
        /// initializes a TextureAtlas with a previously initialized Texture2D object, and
        /// with an initial capacity for Quads. 
        /// The TextureAtlas capacity can be increased in runtime.
        /// WARNING: Do not reinitialize the TextureAtlas because it will leak memory (issue #706)
        /// </summary>
        // Called by CCParticleBatchNode and CCSpriteBatchNode on an already instantiated texture atlas object 
        internal void InitWithTexture(CCTexture2D texture, int capacity)
        {
            //Debug.Assert(texture != null);

            // retained in property
			this.texture = texture;

            // Re-initialization is not allowed
            Debug.Assert(quads == null);

            if (capacity < 4)
            {
                capacity = 4;
            }

			vertexBuffer = new CCQuadVertexBuffer(capacity, CCBufferUsage.WriteOnly);
            quads = vertexBuffer.Data;

            Dirty = true;
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
            Debug.Assert(index >= 0 && index < quads.Capacity, "updateQuadWithTexture: Invalid index");
            quads.count = Math.Max(index + 1, quads.count);
            quads.Elements[index] = quad;
            Dirty = true;
        }

        /// <summary>
        /// Inserts a Quad (texture, vertex and color) at a certain index
        /// index must be between 0 and the atlas capacity - 1
        /// @since v0.8
        /// </summary>
        public void InsertQuad(ref CCV3F_C4B_T2F_Quad quad, int index)
        {
            Debug.Assert(index < quads.Capacity, "insertQuadWithTexture: Invalid index");
            quads.Insert(index, quad);
            Dirty = true;
        }

        /// Removes the quad that is located at a certain index and inserts it at a new index
        /// This operation is faster than removing and inserting in a quad in 2 different steps
        /// @since v0.7.2
        public void InsertQuadFromIndex(int oldIndex, int newIndex)
        {
            Debug.Assert(newIndex >= 0 && newIndex < quads.count, "insertQuadFromIndex:atIndex: Invalid index");
            Debug.Assert(oldIndex >= 0 && oldIndex < quads.count, "insertQuadFromIndex:atIndex: Invalid index");

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

            CCV3F_C4B_T2F_Quad[] elements = quads.Elements;

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
            Debug.Assert(index < quads.count, "removeQuadAtIndex: Invalid index");
            quads.RemoveAt(index);
            Dirty = true;
        }

        public void RemoveQuadsAtIndex(int index, int amount)
        {
            Debug.Assert(index + amount <= quads.count, "removeQuadAtIndex: Invalid index");
            quads.RemoveAt(index, amount);
            Dirty = true;
        }

        public void RemoveAllQuads()
        {
            quads.Clear();
            Dirty = true;
        }

        #endregion
    }
}