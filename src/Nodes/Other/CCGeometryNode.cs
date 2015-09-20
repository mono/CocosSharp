using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCGeometryNode : CCNode
    {
        const int DefaultBufferSize = 256;

        CCRawList<short> indicesArray;
        CCRawList<CCV3F_C4B_T2F> verticesArray;
        CCRawList<CCGeometryInstance> batchItemList;
        BasicEffect basicEffect;

        CCCustomCommand geomRenderCommand;


        #region Constructors

        public CCGeometryNode(int bufferSize=DefaultBufferSize)
        {
            geomRenderCommand = new CCCustomCommand(RenderBatch);

            indicesArray = new CCRawList<short>(bufferSize * 2);
            verticesArray = new CCRawList<CCV3F_C4B_T2F>(bufferSize);
            batchItemList = new CCRawList<CCGeometryInstance>(bufferSize);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            basicEffect = new BasicEffect(DrawManager.XnaGraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;
        }

        #endregion Constructors


        #region Creating/removing geometry

        public void ClearInstances()
        {
            batchItemList.Clear();
        }

        public CCGeometryInstance CreateGeometryInstance(int numberOfVertices, int numberOfIndicies, PrimitiveType primitiveType = PrimitiveType.TriangleList)
        {
            var item = new CCGeometryInstance();
            item.InstanceAttributes.PrimitiveType = primitiveType;

            if (item.GeometryPacket.Vertices.Length < numberOfVertices) 
                item.GeometryPacket.Vertices = new CCV3F_C4B_T2F[numberOfVertices];

            if (item.GeometryPacket.Indicies.Length < numberOfIndicies) 
                item.GeometryPacket.Indicies = new int[numberOfIndicies];

            item.GeometryPacket.NumberOfVertices = numberOfVertices;
            item.GeometryPacket.NumberOfIndicies = numberOfIndicies;

            batchItemList.Add(item);

            return item;
        }

        void EnsureCapacity(int numberOfVerticies, int numberOfIndicies)
        {
            verticesArray.Capacity = Math.Max(verticesArray.Capacity, numberOfVerticies);
            indicesArray.Capacity = Math.Max(indicesArray.Capacity, numberOfIndicies);
        }

        #endregion Creating/removing geometry


        #region Rendering

        protected override void VisitRenderer(ref CCAffineTransform worldTransform)
        {
            geomRenderCommand.GlobalDepth = worldTransform.Tz;
            geomRenderCommand.WorldTransform = worldTransform;
            Renderer.AddCommand(geomRenderCommand);
        }

        void RenderBatch()
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
                    FlushVertexArray(lastAttributes, numberOfVertices, numberOfIndices);
                    numberOfVertices = 0;
                    numberOfIndices = 0;
                    bool textureExists = (geometryPacket.Texture != null);
                    lastTexture = textureExists ? geometryPacket.Texture.XNATexture : null;
                    lastAttributes = geometry.InstanceAttributes;
                    DrawManager.XnaGraphicsDevice.Textures[0] = lastTexture;
                    DrawManager.XnaGraphicsDevice.SamplerStates[0] = textureExists ? geometryPacket.Texture.SamplerState : SamplerState.PointClamp;
                }

                int[] itemIndicies = geometryPacket.Indicies;
                int itemIndiciesCount = geometryPacket.NumberOfIndicies;
                for (int ii = 0, t = numberOfIndices; ii < itemIndiciesCount; ii++, t++)
                    indicesArray[t] = (short)(itemIndicies[ii] + numberOfVertices);

                numberOfIndices += itemIndiciesCount;

                Array.Copy(geometryPacket.Vertices, 0, verticesArray.Elements, numberOfVertices, geometryNumberOfVertices);
                numberOfVertices += geometryNumberOfVertices;

            }

            FlushVertexArray(lastAttributes, numberOfVertices, numberOfIndices);
        }

        void FlushVertexArray(CCGeometryInstanceAttributes instance, int numberOfVerticies, int numberOfIndices)
        {
            if (numberOfVerticies == 0) return;

            var texture = DrawManager.XnaGraphicsDevice.Textures[0] as Texture2D;

            var numberIndices = instance.PrimitiveType == PrimitiveType.TriangleList ? numberOfIndices / 3 : numberOfIndices / 2;
            DrawManager.XnaGraphicsDevice.BlendState = instance.BlendState;
            basicEffect.Projection = DrawManager.ProjectionMatrix;
            basicEffect.View = DrawManager.ViewMatrix;
            basicEffect.World = Matrix.Multiply(instance.AdditionalTransform.XnaMatrix, DrawManager.WorldMatrix);
            basicEffect.TextureEnabled = texture != null;
            basicEffect.Texture = texture;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                basicEffect.GraphicsDevice.DrawUserIndexedPrimitives(
                    instance.PrimitiveType,
                    verticesArray.Elements, 0, numberOfVerticies,
                    indicesArray.Elements, 0, numberIndices);
            }

        }

        #endregion Rendering
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