using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCGeometryBatch : IDisposable
    {
        const int DefaultBufferSize = 256;

        // a basic effect, which contains the shaders that we will use to draw our
        // primitives.
        readonly BasicEffect basicEffect;

        // the device that we will issue draw calls to.
        readonly GraphicsDevice device;

        /// <summary>
        /// The list of batch CCGeometryInstances items to process.
        /// </summary>
        private readonly List<CCGeometryInstance> batchItemList;

        /// <summary>
        /// The available CCGeometryInstance queue so that we reuse these objects when we can.
        /// </summary>
        private readonly Queue<CCGeometryInstance> freeBatchItemQueue;

        /// <summary>
        /// Vertex index array. The values in this array never change.
        /// </summary>
        private short[] indiciesArray = { };
        private CCV3F_C4B_T2F[] verticiesArray = { };

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        bool hasBegun;
        bool isDisposed;

        CCCustomCommand renderGeometryBatch;

        public bool AutoClearInstances { get; set; }

        #region Properties

        internal CCDrawManager DrawManager { get; set; }

        #endregion Properties


        #region Constructors

        internal CCGeometryBatch(CCDrawManager drawManager, int bufferSize=DefaultBufferSize)
        {
            DrawManager = drawManager;

            if (drawManager.XnaGraphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
            device = drawManager.XnaGraphicsDevice;

            batchItemList = new List<CCGeometryInstance>(bufferSize);
            freeBatchItemQueue = new Queue<CCGeometryInstance>(bufferSize);

            EnsureCapacity(bufferSize, bufferSize * 2);

            // set up a new basic effect, and enable vertex colors.
            basicEffect = new BasicEffect(drawManager.XnaGraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;

            AutoClearInstances = true;

            renderGeometryBatch = new CCCustomCommand(0) { Action = RenderBatch };
        }

        public CCGeometryBatch(int bufferSize=DefaultBufferSize)
            : this (CCDrawManager.SharedDrawManager, bufferSize)
        {  }

        #endregion Constructors


        #region Cleaning up

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                if (basicEffect != null)
                    basicEffect.Dispose();

                batchItemList.Clear();
                freeBatchItemQueue.Clear();

                isDisposed = true;
            }
        }

        #endregion Cleaning up

        // Remove all instances from the geometry batch
        //
        public void ClearInstances()
        {
            var itemCount = batchItemList.Count;
            for (int i = 0; i < itemCount; i++)
            {
                var item = batchItemList[i];
                item.Clear();
                freeBatchItemQueue.Enqueue(item);

            }
            batchItemList.Clear();
        }

        // Begin is called to tell the PrimitiveBatch what kind of primitives will be
        // drawn, and to prepare the graphics card to render those primitives.
        public void Begin()
        {
            if (hasBegun)
            {
                throw new InvalidOperationException("End must be called before Begin can be called again.");
            }

            //tell our basic effect to begin.
            UpdateMatrix();

            // flip the error checking boolean. It's now ok to call AddVertex, Flush,
            // and End.
            hasBegun = true;
        }

        public void UpdateMatrix()
        {
            basicEffect.Projection = DrawManager.ProjectionMatrix; ;
            basicEffect.View = DrawManager.ViewMatrix;
            basicEffect.World = DrawManager.WorldMatrix;
            basicEffect.CurrentTechnique.Passes[0].Apply();
        }

        public bool IsReady()
        {
            return hasBegun; 
        }

        /// <summary>
        /// Create an instance of CCGeometryInstance if there is none available in the free item queue. Otherwise,
        /// a previously allocated SpriteBatchItem is reused.
        /// </summary>
        /// <returns></returns>
        public CCGeometryInstance CreateGeometryInstance(int numberOfVertices, int numberOfIndicies)
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before CreateGeometryInstance can be called.");
            }

            var item = freeBatchItemQueue.Count > 0 ? freeBatchItemQueue.Dequeue() : new CCGeometryInstance();

            if (item.GeometryPacket.Vertices.Length < numberOfVertices) 
                item.GeometryPacket.Vertices = new CCV3F_C4B_T2F[numberOfVertices];

            if (item.GeometryPacket.Indicies.Length < numberOfIndicies) 
                item.GeometryPacket.Indicies = new int[numberOfIndicies];

            item.GeometryPacket.NumberOfVertices = numberOfVertices;
            item.GeometryPacket.NumberOfIndicies = numberOfIndicies;

            batchItemList.Add(item);

            return item;
        }


        /// <summary>
        /// Resize and recreate the missing indices for the index and vertex position color buffers.
        /// </summary>
        /// <param name="numberOfVertices"></param>
        /// <param name="numberOfIndices"></param>
        private void EnsureCapacity(int numberOfVerticies, int numberOfIndicies)
        {
            if (verticiesArray.Length < numberOfVerticies) verticiesArray = new CCV3F_C4B_T2F[numberOfVerticies];
            if (indiciesArray.Length < numberOfIndicies) indiciesArray = new short[numberOfIndicies];
        }

        // Commit all instances, to be called once before the 
        // render loop begins and after every change to the
        // instances collection 
        //
        public virtual void Commit()
        {
            if (batchItemList.Count == 0)
                return;
        }

        // Update the geometry batch, eventually prepare GPU-specific
        // data ready to be submitted to the driver, fill vertex and 
        // index buffers as necessary, to be called once per frame
        //
        public virtual void Update()
        {

        }

        // This will be called from the Renderer
        void RenderBatch ()
        {
            if (batchItemList.Count == 0) return;

            int itemCount = batchItemList.Count;
            int numberOfVertices = 0, numberOfIndices = 0;

            for (int i = 0; i < itemCount; i++)
            {
                var item = batchItemList[i].GeometryPacket;
                numberOfVertices += item.NumberOfVertices;
                numberOfIndices += item.NumberOfIndicies;
            }

            EnsureCapacity(numberOfVertices, numberOfIndices);

            numberOfVertices = 0;
            numberOfIndices = 0;

            Texture2D lastTexture = null;
            CCGeometryInstanceAttributes lastAttributes = new CCGeometryInstanceAttributes();

            for (int i = 0; i < itemCount; i++)
            {
                var geometry = batchItemList[i];
                var geometryPacket = geometry.GeometryPacket;
                int geometryNumberOfVertices = geometryPacket.NumberOfVertices;

                // if the texture changed, we need to flush and bind the new texture
                var shouldFlush = (geometryPacket.Texture != null && !ReferenceEquals(geometryPacket.Texture.XNATexture, lastTexture))
                    || lastAttributes != geometry.InstanceAttributes
                    || numberOfVertices + geometryNumberOfVertices > short.MaxValue;


                if (shouldFlush)
                {
                    FlushVertexArray(geometry.InstanceAttributes, numberOfVertices, numberOfIndices);
                    numberOfVertices = 0;
                    numberOfIndices = 0;
                    lastTexture = (geometryPacket.Texture != null) ? geometryPacket.Texture.XNATexture : null;
                    lastAttributes = geometry.InstanceAttributes;
                    device.Textures[0] = lastTexture;
                }

                int[] itemIndicies = geometryPacket.Indicies;
                int itemIndiciesCount = geometryPacket.NumberOfIndicies;
                for (int ii = 0, t = numberOfIndices; ii < itemIndiciesCount; ii++, t++)
                    indiciesArray[t] = (short)(itemIndicies[ii] + numberOfVertices);

                numberOfIndices += itemIndiciesCount;

                Array.Copy(geometryPacket.Vertices, 0, verticiesArray, numberOfVertices, geometryNumberOfVertices);
                numberOfVertices += geometryNumberOfVertices;

            }

            FlushVertexArray(lastAttributes, numberOfVertices, numberOfIndices);

            if (AutoClearInstances)
                ClearInstances();
            
        }

        Matrix savedRenderingStateProjection;
        Matrix savedRenderingStateView;
        Matrix savedRenderingStateWorld;

        void SaveBatchState()
        {

            // We need to save these state values off so that we can use them later in the RenderBatch method
            // The state of these values may have changed between the time we entered the command and the 
            // renderer gets around to calling us back.
            savedRenderingStateProjection = DrawManager.ProjectionMatrix;
            savedRenderingStateView = DrawManager.ViewMatrix;
            savedRenderingStateWorld = DrawManager.WorldMatrix;
        }

        // Submit the batch to the driver, typically implemented
        // with a call to DrawIndexedPrimitive 
        //
        public virtual void Draw()
        {  
            SaveBatchState();
            DrawManager.Renderer.AddCommand(renderGeometryBatch);
        }

        private void FlushVertexArray(CCGeometryInstanceAttributes instance, int numberOfVerticies, int numberOfIndices)
        {
            if (numberOfVerticies == 0) return;

            // update our drawing count
            DrawManager.DrawCount++;

            if (device.Textures[0] == null)
                basicEffect.TextureEnabled = false;
            else
                basicEffect.TextureEnabled = true;

            device.BlendState = instance.BlendState;
            basicEffect.Projection = savedRenderingStateProjection;
            basicEffect.View = savedRenderingStateView;
            //basicEffect.World = DrawManager.WorldMatrix;
            basicEffect.World = Matrix.Multiply(instance.AdditionalTransform.XnaMatrix, savedRenderingStateWorld) ;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                basicEffect.GraphicsDevice.DrawUserIndexedPrimitives(
                    instance.PrimitiveType,
                    verticiesArray, 0, numberOfVerticies,
                    indiciesArray, 0, numberOfIndices / 3);
            }

        }

        // End is called once all the primitives have been drawn using AddVertex.
        // it will call Flush to actually submit the draw call to the graphics card, and
        // then tell the basic effect to end.
        public void End()
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before End can be called.");
            }

            Draw();

            // Draw whatever the user wanted us to draw
            hasBegun = false;
        }

    }

    public class CCGeometryInstanceAttributes 
    {
        // Future use
        internal PrimitiveType PrimitiveType;
        public CCAffineTransform AdditionalTransform;
        public BlendState BlendState;

        internal CCGeometryInstanceAttributes ()
        {
            BlendState = BlendState.AlphaBlend;
            AdditionalTransform = CCAffineTransform.Identity;
            PrimitiveType = PrimitiveType.TriangleList;
        }

        public override bool Equals(object other)
        {
            if (!(other is CCGeometryInstanceAttributes))
                return false;

            return Equals(other as CCGeometryInstanceAttributes);
        }

        public override int GetHashCode()
        {
            return AdditionalTransform.GetHashCode() + BlendState.GetHashCode() + PrimitiveType.GetHashCode();
        }

        public bool Equals(CCGeometryInstanceAttributes other)
        {
            if (other == null) { return false; }
            if (object.ReferenceEquals(this, other)) { return true; }
            return this.AdditionalTransform == other.AdditionalTransform && this.BlendState == other.BlendState
                && this.PrimitiveType == other.PrimitiveType;
        }
 
        public static bool operator ==(CCGeometryInstanceAttributes item1, CCGeometryInstanceAttributes item2)
        {
            if (object.ReferenceEquals(item1, item2)) { return true; }
            if ((object)item1 == null || (object)item2 == null) { return false; }
            return item1.AdditionalTransform == item2.AdditionalTransform && item1.BlendState == item2.BlendState
                && item1.PrimitiveType == item2.PrimitiveType;
        }

        public static bool operator !=(CCGeometryInstanceAttributes item1, CCGeometryInstanceAttributes item2)
        {
            return !(item1 == item2);
        }
    }

    public class CCGeometryPacket
    {
        public CCTexture2D Texture;
        public int NumberOfVertices;
        public int NumberOfIndicies;
        public CCV3F_C4B_T2F[] Vertices;
        public int[] Indicies;

        internal CCGeometryPacket()
        {
            Texture = null;
            NumberOfVertices = 0;
            NumberOfIndicies = 0;
            Vertices = new CCV3F_C4B_T2F[] { };
            Indicies = new int[] { };
        }

    }

    public class CCGeometryInstance
    {
        public CCGeometryPacket GeometryPacket;
        public CCGeometryInstanceAttributes InstanceAttributes;

        internal CCGeometryInstance ()
        {
            GeometryPacket = new CCGeometryPacket();
            InstanceAttributes = new CCGeometryInstanceAttributes();
        }

        internal void Clear()
        {
            GeometryPacket.Texture = null;
            InstanceAttributes.AdditionalTransform = CCAffineTransform.Identity;
            InstanceAttributes.BlendState = BlendState.AlphaBlend;
            InstanceAttributes.PrimitiveType = PrimitiveType.TriangleList;
        }
    }


}