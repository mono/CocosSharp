/******************************************************************************
 * Spine Runtimes Software License
 * Version 2
 * 
 * Copyright (c) 2013, Esoteric Software
 * All rights reserved.
 * 
 * You are granted a perpetual, non-exclusive, non-sublicensable and
 * non-transferable license to install, execute and perform the Spine Runtimes
 * Software (the "Software") solely for internal use. Without the written
 * permission of Esoteric Software, you may not (a) modify, translate, adapt or
 * otherwise create derivative works, improvements of the Software or develop
 * new applications using the Software or (b) remove, delete, alter or obscure
 * any trademarks or any copyright, trademark, patent or other intellectual
 * property or proprietary rights notices on or in the Software, including
 * any copy thereof. Redistributions in binary or source form must include
 * this license and terms. THIS SOFTWARE IS PROVIDED BY ESOTERIC SOFTWARE
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
 * TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL ESOTERIC SOFTARE BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using CocosSharp;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CocosSharp.Spine
{

    public enum AttachmentType
    {
        ATTACHMENT_REGION = 1, ATTACHMENT_REGION_SEQUENCE = 2, ATTACHMENT_BOUNDING_BOX = 3
    }

    public class CCSkeleton : CCNodeRGBA
    {

        public float FLT_MAX = 3.402823466e+38F;     /* max value */
        public float FLT_MIN = 1.175494351e-38F;     /* min positive value */

        public static int ATTACHMENT_REGION = 0;
        public static int ATTACHMENT_REGION_SEQUENCE = 1;
        public static int ATTACHMENT_BOUNDING_BOX = 2;

        public Skeleton Skeleton { get; private set; }
        public CCMeshBatcher batcher;

        public bool DebugSlots { get; set; }
        public CCColor4B DebugSlotColor { get; set; }
        public bool DebugBones { get; set; }
        public CCColor4B DebugBoneColor { get; set; }
        public CCColor4B DebugJointColor { get; set; }
        public bool PremultipliedAlpha { get; set; }
        public string ImagesDirectory { get; set; }

        bool ownsSkeletonData;
        Atlas atlas;

        float[] vertices = new float[8];
        int[] quadTriangles = { 0, 1, 2, 1, 3, 2 };

        private const int TL = 0;
        private const int TR = 1;
        private const int BL = 2;
        private const int BR = 3;
        BlendState defaultBlendState;

        public void SetSkeletonData(SkeletonData skeletonData, bool ownsSkeletonData)
        {
            Skeleton = new Skeleton(skeletonData);
            this.ownsSkeletonData = ownsSkeletonData;
        }

        public CCSkeleton()
        {
            Initialize();
        }

        public CCSkeleton(SkeletonData skeletonData, bool ownsSkeletonData = false)
        {
            Initialize();
            SetSkeletonData(skeletonData, ownsSkeletonData);
        }

        public CCSkeleton(string skeletonDataFile, Atlas atlas, float scale = 0)
        {
            var json = new SkeletonJson(atlas);
			json.Scale = scale == 0 ? (1 / Director.ContentScaleFactor) : scale;
            SkeletonData skeletonData = json.ReadSkeletonData(skeletonDataFile);
            SetSkeletonData(skeletonData, true);
        }

        public CCSkeleton(string skeletonDataFile, string atlasFile, float scale = 0)
        {

            Initialize();

            using (StreamReader atlasStream = new StreamReader(CCFileUtils.GetFileStream(atlasFile)))
            {
                atlas = new Atlas(atlasStream, "", new CocosSharpTextureLoader());
            }

            SkeletonJson json = new SkeletonJson(atlas);

            json.Scale = scale == 0 ? (1 / Director.ContentScaleFactor) : scale;

            using (StreamReader skeletonDataStream = new StreamReader(CCFileUtils.GetFileStream(skeletonDataFile)))
            {
                SkeletonData skeletonData = json.ReadSkeletonData(skeletonDataStream);
                skeletonData.Name = skeletonDataFile;
                SetSkeletonData(skeletonData, true);
            }
        }

        public void Initialize()
        {
            atlas = null;
            DebugSlots = false;
            DebugBones = false;
            PremultipliedAlpha = false;
            ImagesDirectory = string.Empty;
            DebugSlotColor = CCColor4B.Magenta;
            DebugBoneColor = CCColor4B.Blue;
            DebugJointColor = CCColor4B.Red;
            batcher = new CCMeshBatcher(CCDrawManager.GraphicsDevice);
            OpacityModifyRGB = true;

            Schedule();
        }
        
        ~CCSkeleton()
        {
			CCLog.Log ("finalize");
            //Eliminamos el skleñetp
            //if (ownsSkeletonData!=null) 

            //SkeletonData_dispose(skeleton->data);
            //if (atlas) spAtlas_dispose(atlas);
            //spSkeleton_dispose(skeleton);
            
        }
      
        public override void Update(float dt)
        {
			base.Update(dt);
        }

        protected override void Draw()
        {
            defaultBlendState = PremultipliedAlpha ? BlendState.AlphaBlend : BlendState.NonPremultiplied;
           

            float[] vertices = this.vertices;
            List<Slot> drawOrder = Skeleton.DrawOrder;
            float x = Skeleton.X, y = Skeleton.Y;

            CCColor3B color3b = Color;
            float skeletonR = color3b.R / 255f;
            float skeletonG = color3b.G / 255f;
            float skeletonB = color3b.B / 255f;
            float skeletonA = Opacity / 255f;

            batcher.BlendState = defaultBlendState;

            batcher.Begin();
            
            for (int i = 0, n = drawOrder.Count; i < n; i++)
            {
                Slot slot = drawOrder[i];
                Attachment attachment = slot.Attachment;
                if (attachment is RegionAttachment)
                {
                    RegionAttachment regionAttachment = (RegionAttachment)attachment;
                    BlendState blend = slot.Data.AdditiveBlending ? BlendState.Additive : defaultBlendState;
                    //batcher.BlendState = blend;

                    if (CCDrawManager.GraphicsDevice.BlendState != blend)
                    {
                        batcher.End();
                        batcher.BlendState = blend;
                        batcher.Begin();
                    }

                    MeshItem item = batcher.NextItem(4, 6);
                    item.triangles = quadTriangles;
                    VertexPositionColorTexture[] itemVertices = item.vertices;

                    AtlasRegion region = (AtlasRegion)regionAttachment.RendererObject;
                    item.texture = (Texture2D)region.page.rendererObject;

                    Color color;
                    float a = skeletonA * slot.A * regionAttachment.A;
                    if (PremultipliedAlpha)
                    {
                        color = new Color(
                                skeletonR * slot.R * regionAttachment.R * a,
                                skeletonG * slot.G * regionAttachment.G * a,
                                skeletonB * slot.B * regionAttachment.B * a, a);
                    }
                    else
                    {
                        color = new Color(
                                skeletonR * slot.R * regionAttachment.R,
                                skeletonG * slot.G * regionAttachment.G,
                                skeletonB * slot.B * regionAttachment.B, a);
                    }
                    itemVertices[TL].Color = color;
                    itemVertices[BL].Color = color;
                    itemVertices[BR].Color = color;
                    itemVertices[TR].Color = color;

                    regionAttachment.ComputeWorldVertices(x, y, slot.Bone, vertices);
                    itemVertices[TL].Position.X = vertices[RegionAttachment.X1];
                    itemVertices[TL].Position.Y = vertices[RegionAttachment.Y1];
                    itemVertices[TL].Position.Z = 0;
                    itemVertices[BL].Position.X = vertices[RegionAttachment.X2];
                    itemVertices[BL].Position.Y = vertices[RegionAttachment.Y2];
                    itemVertices[BL].Position.Z = 0;
                    itemVertices[BR].Position.X = vertices[RegionAttachment.X3];
                    itemVertices[BR].Position.Y = vertices[RegionAttachment.Y3];
                    itemVertices[BR].Position.Z = 0;
                    itemVertices[TR].Position.X = vertices[RegionAttachment.X4];
                    itemVertices[TR].Position.Y = vertices[RegionAttachment.Y4];
                    itemVertices[TR].Position.Z = 0;

                    float[] uvs = regionAttachment.UVs;
                    itemVertices[TL].TextureCoordinate.X = uvs[RegionAttachment.X1];
                    itemVertices[TL].TextureCoordinate.Y = uvs[RegionAttachment.Y1];
                    itemVertices[BL].TextureCoordinate.X = uvs[RegionAttachment.X2];
                    itemVertices[BL].TextureCoordinate.Y = uvs[RegionAttachment.Y2];
                    itemVertices[BR].TextureCoordinate.X = uvs[RegionAttachment.X3];
                    itemVertices[BR].TextureCoordinate.Y = uvs[RegionAttachment.Y3];
                    itemVertices[TR].TextureCoordinate.X = uvs[RegionAttachment.X4];
                    itemVertices[TR].TextureCoordinate.Y = uvs[RegionAttachment.Y4];
                }
                else if (attachment is MeshAttachment)
                {
                    MeshAttachment mesh = (MeshAttachment)attachment;
                    int vertexCount = mesh.Vertices.Length;
                    if (vertices.Length < vertexCount) vertices = new float[vertexCount];
                    mesh.ComputeWorldVertices(x, y, slot, vertices);

                    int[] triangles = mesh.Triangles;
                    MeshItem item = batcher.NextItem(vertexCount, triangles.Length);
                    item.triangles = triangles;

                    AtlasRegion region = (AtlasRegion)mesh.RendererObject;
                    item.texture = (Texture2D)region.page.rendererObject;

                    Color color;
                    float a = skeletonA * slot.A * mesh.A;
                    if (PremultipliedAlpha)
                    {
                        color = new Color(
                                skeletonR * slot.R * mesh.R * a,
                                skeletonG * slot.G * mesh.G * a,
                                skeletonB * slot.B * mesh.B * a, a);
                    }
                    else
                    {
                        color = new Color(
                                skeletonR * slot.R * mesh.R,
                                skeletonG * slot.G * mesh.G,
                                skeletonB * slot.B * mesh.B, a);
                    }

                    float[] uvs = mesh.UVs;
                    VertexPositionColorTexture[] itemVertices = item.vertices;
                    for (int ii = 0, v = 0; v < vertexCount; ii++, v += 2)
                    {
                        itemVertices[ii].Color = color;
                        itemVertices[ii].Position.X = vertices[v];
                        itemVertices[ii].Position.Y = vertices[v + 1];
                        itemVertices[ii].Position.Z = 0;
                        itemVertices[ii].TextureCoordinate.X = uvs[v];
                        itemVertices[ii].TextureCoordinate.Y = uvs[v + 1];
                    }
                }
                else if (attachment is SkinnedMeshAttachment)
                {
                    SkinnedMeshAttachment mesh = (SkinnedMeshAttachment)attachment;
                    int vertexCount = mesh.UVs.Length;
                    if (vertices.Length < vertexCount) vertices = new float[vertexCount];
                    mesh.ComputeWorldVertices(x, y, slot, vertices);

                    int[] triangles = mesh.Triangles;
                    MeshItem item = batcher.NextItem(vertexCount, triangles.Length);
                    item.triangles = triangles;

                    AtlasRegion region = (AtlasRegion)mesh.RendererObject;
                    item.texture = (Texture2D)region.page.rendererObject;

                    Color color;
                    float a = skeletonA * slot.A * mesh.A;
                    if (PremultipliedAlpha)
                    {
                        color = new Color(
                                skeletonR * slot.R * mesh.R * a,
                                skeletonG * slot.G * mesh.G * a,
                                skeletonB * slot.B * mesh.B * a, a);
                    }
                    else
                    {
                        color = new Color(
                                skeletonR * slot.R * mesh.R,
                                skeletonG * slot.G * mesh.G,
                                skeletonB * slot.B * mesh.B, a);
                    }

                    float[] uvs = mesh.UVs;
                    VertexPositionColorTexture[] itemVertices = item.vertices;
                    for (int ii = 0, v = 0; v < vertexCount; ii++, v += 2)
                    {
                        itemVertices[ii].Color = color;
                        itemVertices[ii].Position.X = vertices[v];
                        itemVertices[ii].Position.Y = vertices[v + 1];
                        itemVertices[ii].Position.Z = 0;
                        itemVertices[ii].TextureCoordinate.X = uvs[v];
                        itemVertices[ii].TextureCoordinate.Y = uvs[v + 1];
                    }
                }
            }

            batcher.End();

            if (DebugBones || DebugSlots)
            {
                if (DebugSlots)
                {

                    for (int i = 0; i < Skeleton.Slots.Count; ++i)
                    {

                        var slot = Skeleton.Slots[i];

                        if (slot.Attachment == null) continue;

                        var verticesCount = 0;
                        var worldVertices = new float[1000]; // Max number of vertices per mesh.
                        if (slot.Attachment is RegionAttachment)
                        {
                            var attachment = (RegionAttachment)slot.Attachment;
                            attachment.ComputeWorldVertices(Skeleton.X, Skeleton.Y, slot.bone, worldVertices);
                            verticesCount = 8;
                        }
                        else if (slot.Attachment is MeshAttachment)
                        {
                            var mesh = (MeshAttachment)slot.Attachment;
                            mesh.ComputeWorldVertices(Skeleton.X, Skeleton.Y, slot, worldVertices);
                            verticesCount = mesh.Vertices.Length;
                        }
                        else if (slot.Attachment is SkinnedMeshAttachment)
                        {
                            var mesh = (SkinnedMeshAttachment)slot.Attachment;
                            mesh.ComputeWorldVertices(Skeleton.X, Skeleton.Y, slot, worldVertices);
                            verticesCount = mesh.UVs.Length;
                        }
                        else
                            continue;

                        CCPoint[] slotVertices = new CCPoint[verticesCount/2];

                        for (int ii = 0, si = 0; ii < verticesCount; ii += 2, si++)
                        {
                            slotVertices[si].X = worldVertices[ii] * ScaleX;
                            slotVertices[si].Y = worldVertices[ii + 1] * ScaleY;
                        }

                        CCDrawingPrimitives.Begin();
                        CCDrawingPrimitives.DrawPoly(slotVertices, verticesCount / 2, true, DebugSlotColor);
                        CCDrawingPrimitives.End();

                    }


                }

                if (DebugBones)
                {
                    // Bone lengths.
                    for (int i = 0; i < Skeleton.Bones.Count; i++)
                    {
                        Bone bone = Skeleton.Bones[i];
                        x = bone.Data.Length * bone.M00 + bone.WorldX;
                        y = bone.Data.Length * bone.M10 + bone.WorldY;

                        CCDrawingPrimitives.Begin();
                        CCDrawingPrimitives.DrawLine(new CCPoint(bone.WorldX, bone.WorldY), new CCPoint(x, y), DebugJointColor);
                        CCDrawingPrimitives.End();
                    }

                    // Bone origins.
                    for (int i = 0; i < Skeleton.Bones.Count; i++)
                    {
                        Bone bone = Skeleton.Bones[i];
                        CCDrawingPrimitives.Begin();
                        CCDrawingPrimitives.DrawPoint(new CCPoint(bone.WorldX, bone.WorldY), 4, DebugBoneColor);
                        CCDrawingPrimitives.End();

                    }
                }
            }

        }

        public override CCSize ContentSize
        {
            get
            {
                var bbox = boundingBox();
                return new CCSize(bbox.Size.Width, bbox.Size.Height);   
            }
            set
            {
                
            }
        }

        CCRect boundingBox()
        {

            float minX = FLT_MAX, minY = FLT_MAX, maxX = FLT_MIN, maxY = FLT_MIN;

            for (int i = 0; i < Skeleton.Slots.Count; ++i)
            {

                var slot = Skeleton.Slots[i];

                if (slot.Attachment == null) continue;

                var verticesCount = 0;
                var worldVertices = new float[1000]; // Max number of vertices per mesh.
                if (slot.Attachment is RegionAttachment)
                {
                    var attachment = (RegionAttachment)slot.Attachment;
                    attachment.ComputeWorldVertices(Skeleton.X, Skeleton.Y, slot.bone, worldVertices);
                    verticesCount = 8;
                }
                else if (slot.Attachment is MeshAttachment)
                {
                    var mesh = (MeshAttachment)slot.Attachment;
                    mesh.ComputeWorldVertices(Skeleton.X, Skeleton.Y, slot, worldVertices);
                    verticesCount = mesh.Vertices.Length;
                }
                else if (slot.Attachment is SkinnedMeshAttachment)
                {
                    var mesh = (SkinnedMeshAttachment)slot.Attachment;
                    mesh.ComputeWorldVertices(Skeleton.X, Skeleton.Y, slot, worldVertices);
                    verticesCount = mesh.UVs.Length;
                }
                else
                    continue;
                for (int ii = 0; ii < verticesCount; ii += 2)
                {
                    float x = worldVertices[ii] * ScaleX, y = worldVertices[ii + 1] * ScaleY;
                    minX = Math.Min(minX, x);
                    minY = Math.Min(minY, y);
                    maxX = Math.Max(maxX, x);
                    maxY = Math.Max(maxY, y);
                }

            }
            CCPoint position = Position;

            return new CCRect(position.X + minX, position.Y + minY, maxX - minX, maxY - minY);

        }

        // --- Convenience methods for common Skeleton_* functions.
        public void UpdateWorldTransform()
        {
            Skeleton.UpdateWorldTransform();
        }

        public void SetToSetupPose()
        {
            Skeleton.SetToSetupPose();
        }
        public void SetBonesToSetupPose()
        {
            Skeleton.SetBonesToSetupPose();
        }
        public void SetSlotsToSetupPose()
        {
            Skeleton.SetSlotsToSetupPose();
        }

        /* Returns 0 if the bone was not found. */
        public Bone FindBone(string boneName)
        {
            return Skeleton.FindBone(boneName);
        }
        /* Returns 0 if the slot was not found. */
        public Slot FindSlot(string slotName)
        {
            return Skeleton.FindSlot(slotName);
        }

        /* Sets the skin used to look up attachments not found in the SkeletonData defaultSkin. Attachments from the new skin are
         * attached if the corresponding attachment from the old skin was attached. Returns false if the skin was not found.
         * @param skin May be 0.*/
        public bool SetSkin(string skinName)
        {
            Skeleton.SetSkin(skinName);
            return true;
        }

        /* Returns 0 if the slot or attachment was not found. */
        public Attachment GetAttachment(string slotName, string attachmentName)
        {
            return Skeleton.GetAttachment(slotName, attachmentName);
        }
        /* Returns false if the slot or attachment was not found. */
        public bool SetAttachment(string slotName, string attachmentName)
        {
            Skeleton.SetAttachment(slotName, attachmentName);
            return true;
        }

        public bool OpacityModifyRGB
        {
            get
            {
                return PremultipliedAlpha;
            }

            set
            {
                PremultipliedAlpha = value;
            }
        }
      
        //virtual cocos2d::CCTextureAtlas* getTextureAtlas (RegionAttachment regionAttachment);
        #region SpinesCocos2d

        void UpdateRegionAttachmentQuad(RegionAttachment self, Slot slot, ref CCV3F_C4B_T2F_Quad quad, bool premultipliedAlpha = false)
        {

            float[] vertices = new float[8];

            self.ComputeWorldVertices(slot.Skeleton.X, slot.Skeleton.Y, slot.Bone, vertices);

            float r = slot.Skeleton.R * slot.R * 255;
            float g = slot.Skeleton.G * slot.G * 255;
            float b = slot.Skeleton.B * slot.B * 255;

            float normalizedAlpha = slot.Skeleton.A * slot.A;
            if (premultipliedAlpha)
            {
                r *= normalizedAlpha;
                g *= normalizedAlpha;
                b *= normalizedAlpha;
            }

            float a = normalizedAlpha * 255;
            quad.BottomLeft.Colors.R = (byte)r;
            quad.BottomLeft.Colors.G = (byte)g;
            quad.BottomLeft.Colors.B = (byte)b;
            quad.BottomLeft.Colors.A = (byte)a;
            quad.TopLeft.Colors.R = (byte)r;
            quad.TopLeft.Colors.G = (byte)g;
            quad.TopLeft.Colors.B = (byte)b;
            quad.TopLeft.Colors.A = (byte)a;
            quad.TopRight.Colors.R = (byte)r;
            quad.TopRight.Colors.G = (byte)g;
            quad.TopRight.Colors.B = (byte)b;
            quad.TopRight.Colors.A = (byte)a;
            quad.BottomRight.Colors.R = (byte)r;
            quad.BottomRight.Colors.G = (byte)g;
            quad.BottomRight.Colors.B = (byte)b;
            quad.BottomRight.Colors.A = (byte)a;

            quad.BottomLeft.Vertices.X = vertices[RegionAttachment.X1];
            quad.BottomLeft.Vertices.Y = vertices[RegionAttachment.Y1];
            quad.TopLeft.Vertices.X = vertices[RegionAttachment.X2];
            quad.TopLeft.Vertices.Y = vertices[RegionAttachment.Y2];
            quad.TopRight.Vertices.X = vertices[RegionAttachment.X3];
            quad.TopRight.Vertices.Y = vertices[RegionAttachment.Y3];
            quad.BottomRight.Vertices.X = vertices[RegionAttachment.X4];
            quad.BottomRight.Vertices.Y = vertices[RegionAttachment.Y4];

            quad.BottomLeft.TexCoords.U = self.UVs[RegionAttachment.X1];
            quad.BottomLeft.TexCoords.V = self.UVs[RegionAttachment.Y1];
            quad.TopLeft.TexCoords.U = self.UVs[RegionAttachment.X2];
            quad.TopLeft.TexCoords.V = self.UVs[RegionAttachment.Y2];
            quad.TopRight.TexCoords.U = self.UVs[RegionAttachment.X3];
            quad.TopRight.TexCoords.V = self.UVs[RegionAttachment.Y3];
            quad.BottomRight.TexCoords.U = self.UVs[RegionAttachment.X4];
            quad.BottomRight.TexCoords.V = self.UVs[RegionAttachment.Y4];

        }

        #endregion

     

    }
}
