using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    #region Enums

    public enum CCClipMode
    {
        None,                   // No clipping of children
        Bounds,                 // Clipping with a ScissorRect
        BoundsWithRenderTarget  // Clipping with the ScissorRect and in a RenderTarget
    }

    // Conform to XNA 4.0

    public enum CCDepthFormat
    {
        None = Microsoft.Xna.Framework.Graphics.DepthFormat.None,
        Depth16 = Microsoft.Xna.Framework.Graphics.DepthFormat.Depth16,
        Depth24 = Microsoft.Xna.Framework.Graphics.DepthFormat.Depth24,
        Depth24Stencil8 = Microsoft.Xna.Framework.Graphics.DepthFormat.Depth24Stencil8,
    }

    public enum CCBufferUsage
    {
        None,
        WriteOnly
    }

    #endregion Enums


    internal class CCDrawManager
    {
        const int DefaultQuadBufferSize = 1024 * 4;

        bool needReinitResources;
        bool textureEnabled;
        bool vertexColorEnabled;
        bool worldMatrixChanged;
        bool projectionMatrixChanged;
        bool viewMatrixChanged;
        bool textureChanged;
        bool effectChanged;
        bool depthTest;
        bool allowNonPower2Textures;
        bool hasStencilBuffer;
        bool maskOnceLog = false;

        int stackIndex;
        int maskLayer = -1;


        CCBlendFunc currBlend;
        CCDepthFormat platformDepthFormat;

        CCQuadVertexBuffer quadsBuffer;
        CCIndexBuffer<short> quadsIndexBuffer;
        CCV3F_C4B_T2F[] quadVertices;

        readonly Matrix[] matrixStack;
        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        Matrix matrix;
        Matrix tmpMatrix;
        Matrix transform;

        readonly Dictionary<CCBlendFunc, BlendState> blendStates;
        DepthStencilState depthEnableStencilState;
        DepthStencilState depthDisableStencilState;
        DepthStencilState[] maskSavedStencilStates = new DepthStencilState[8];

        MaskState[] maskStates = new MaskState[8];
        MaskDepthStencilStateCacheEntry[] maskStatesCache = new MaskDepthStencilStateCacheEntry[8];

        readonly Stack<Effect> effectStack;
        BasicEffect defaultEffect;
        Effect currentEffect;

        RenderTarget2D currentRenderTarget;
        RenderTarget2D previousRenderTarget;

        Texture2D currentTexture;

        GraphicsDevice graphicsDevice;

        List<RasterizerState> rasterizerStatesCache;


        #region Properties

        public static CCDrawManager SharedDrawManager { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        internal ulong DrawCount { get; set; }
        internal ulong DrawPrimitivesCount { get; set; }

        internal BasicEffect PrimitiveEffect { get; private set; }
        internal AlphaTestEffect AlphaTestEffect { get; private set; }
        internal CCRawList<CCV3F_C4B_T2F> TmpVertices { get; private set; }
        internal CCRenderer Renderer { get; private set; }

        public bool VertexColorEnabled
        {
            get { return vertexColorEnabled; }
            set
            {
                if (vertexColorEnabled != value)
                {
                    vertexColorEnabled = value;
                    textureChanged = true;
                }
            }
        }

        public bool TextureEnabled
        {
            get { return textureEnabled; }
            set
            {
                if (textureEnabled != value)
                {
                    textureEnabled = value;
                    textureChanged = true;
                }
            }
        }

        public bool ScissorRectEnabled
        {
            get { return graphicsDevice.RasterizerState.ScissorTestEnable; }
            set
            {
                if (graphicsDevice.RasterizerState.ScissorTestEnable != value)
                {
                    graphicsDevice.RasterizerState = GetScissorRasterizerState(value);
                }
            }
        }

        public bool DepthTest
        {
            get { return depthTest; }
            set
            {
                depthTest = value;
                // NOTE: This must be disabled when primitives are drawing, e.g. lines, polylines, etc.
                graphicsDevice.DepthStencilState = value ? depthEnableStencilState : depthDisableStencilState;
            }
        }

        internal BlendState BlendState
        {
            get { return graphicsDevice.BlendState; }
            set
            {
                graphicsDevice.BlendState = value;
                currBlend.Source = -1;
                currBlend.Destination = -1;
            }
        }

        public DepthStencilState DepthStencilState
        {
            get { return graphicsDevice.DepthStencilState; }
            set { graphicsDevice.DepthStencilState = value; }
        }

        internal CCRect ScissorRectInPixels
        {
            get { return new CCRect(graphicsDevice.ScissorRectangle); }
            set 
            {
                graphicsDevice.ScissorRectangle 
                    = new Rectangle ((int)value.Origin.X, (int)value.Origin.Y, (int)value.Size.Width, (int)value.Size.Height);
            }
        }

        internal Viewport Viewport 
        { 
            get { return graphicsDevice.Viewport; } 
            set 
            {
                graphicsDevice.Viewport = value;
            }
        }

        internal Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set
            {
                viewMatrix = value;
                viewMatrixChanged = true;
            }
        }

        internal Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
            set
            {
                projectionMatrix = value;
                projectionMatrixChanged = true;
            }
        }

        internal Matrix WorldMatrix
        {
            get { return matrix; }
            set
            {
                matrix = worldMatrix = value;
                worldMatrixChanged = true;
            }
        }


        internal GraphicsDevice XnaGraphicsDevice
        {
            get { return graphicsDevice; }
        }

        internal RenderTarget2D CurrentRenderTarget 
        { 
            get { return currentRenderTarget; }
            set 
            {
                previousRenderTarget = currentRenderTarget;
                currentRenderTarget = value;

                if (graphicsDevice != null && graphicsDevice.GraphicsDeviceStatus == GraphicsDeviceStatus.Normal) 
                {
                    graphicsDevice.SetRenderTarget(currentRenderTarget);
                }
            }
        }

        #endregion Properties


        #region Constructors

        internal CCDrawManager(GraphicsDevice device)
        {
            Renderer = new CCRenderer(this);

            depthTest = true;
            allowNonPower2Textures = true;
            hasStencilBuffer = true;
            currBlend = CCBlendFunc.AlphaBlend;
            platformDepthFormat = CCDepthFormat.Depth24;
            transform = Matrix.Identity;

            TmpVertices = new CCRawList<CCV3F_C4B_T2F>();
            matrixStack = new Matrix[100];
            blendStates = new Dictionary<CCBlendFunc, BlendState>();
            effectStack = new Stack<Effect>();

            rasterizerStatesCache = new List<RasterizerState>();
        
            hasStencilBuffer = true;

            graphicsDevice = device;
            InitializeGraphicsDevice();
        }

        void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            var gdipp = e.GraphicsDeviceInformation.PresentationParameters;

            PresentationParameters presParams = new PresentationParameters();

            presParams.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            presParams.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            presParams.BackBufferFormat = SurfaceFormat.Color;
            presParams.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            gdipp.RenderTargetUsage = presParams.RenderTargetUsage;
            gdipp.DepthStencilFormat = presParams.DepthStencilFormat;
            gdipp.BackBufferFormat = presParams.BackBufferFormat;
            gdipp.RenderTargetUsage = presParams.RenderTargetUsage;
        }

        void InitializeGraphicsDevice()
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            defaultEffect = new BasicEffect(graphicsDevice);

            AlphaTestEffect = new AlphaTestEffect(graphicsDevice);

            PrimitiveEffect = new BasicEffect(graphicsDevice)
            {
                TextureEnabled = false,
                VertexColorEnabled = true
            };

            depthEnableStencilState = new DepthStencilState
            {
                DepthBufferEnable = true,
                DepthBufferWriteEnable = true,
                TwoSidedStencilMode = true
            };

            depthDisableStencilState = new DepthStencilState
            {
                DepthBufferEnable = false
            };

            #if !WINDOWS_PHONE && !XBOX && !WINDOWS &&!NETFX_CORE
            List<string> extensions = CCUtils.GetGLExtensions();
            foreach(string s in extensions) 
            {
                switch(s) 
                {
                case "GL_OES_depth24":
                    platformDepthFormat = CCDepthFormat.Depth24;
                    break;
                case "GL_IMG_texture_npot":
                    allowNonPower2Textures = true;
                    break;
                case "GL_NV_depth_nonlinear":                       // nVidia Depth 16 non-linear
                    platformDepthFormat = CCDepthFormat.Depth16;
                    break;
                case "GL_NV_texture_npot_2D_mipmap":                // nVidia - nPot textures and mipmaps
                    allowNonPower2Textures = true;
                    break;
                }
            }

            #endif

            projectionMatrix = Matrix.Identity;
            viewMatrix = Matrix.Identity;
            worldMatrix = Matrix.Identity;
            matrix = Matrix.Identity;

            worldMatrixChanged = viewMatrixChanged = projectionMatrixChanged = true;

            graphicsDevice.Disposing += GraphicsDeviceDisposing;
            graphicsDevice.DeviceLost += GraphicsDeviceDeviceLost;
            graphicsDevice.DeviceReset += GraphicsDeviceDeviceReset;
            graphicsDevice.DeviceResetting += GraphicsDeviceDeviceResetting;
            graphicsDevice.ResourceCreated += GraphicsDeviceResourceCreated;
            graphicsDevice.ResourceDestroyed += GraphicsDeviceResourceDestroyed;

            DepthTest = false;

            ResetDevice ();
        }

        #endregion Constructors



        RasterizerState GetScissorRasterizerState(bool scissorEnabled)
        {
            var currentState = graphicsDevice.RasterizerState;

            for (int i = 0; i < rasterizerStatesCache.Count; i++)
            {
                var state = rasterizerStatesCache[i];
                if (
                    state.ScissorTestEnable == scissorEnabled &&
                    currentState.CullMode == state.CullMode &&
                    currentState.DepthBias == state.DepthBias &&
                    currentState.FillMode == state.FillMode &&
                    currentState.MultiSampleAntiAlias == state.MultiSampleAntiAlias &&
                    currentState.SlopeScaleDepthBias == state.SlopeScaleDepthBias
                )
                {
                    return state;
                }
            }

            var newState = new RasterizerState
            {
                ScissorTestEnable = scissorEnabled,
                CullMode = currentState.CullMode,
                DepthBias = currentState.DepthBias,
                FillMode = currentState.FillMode,
                MultiSampleAntiAlias = currentState.MultiSampleAntiAlias,
                SlopeScaleDepthBias = currentState.SlopeScaleDepthBias
            };

            rasterizerStatesCache.Add(newState);

            return newState;
        }


        #region GraphicsDevice callbacks

        void GraphicsDeviceResourceDestroyed(object sender, ResourceDestroyedEventArgs e)
        {
        }

        void GraphicsDeviceResourceCreated(object sender, ResourceCreatedEventArgs e)
        {
        }

        void GraphicsDeviceDeviceResetting(object sender, EventArgs e)
        {
            CCSpriteFontCache.SharedInstance.Clear();
            #if XNA
            CCContentManager.SharedContentManager.ReloadGraphicsAssets();
            #endif
            needReinitResources = true;
        }

        void GraphicsDeviceDeviceReset(object sender, EventArgs e)
        {
        }

        void GraphicsDeviceDeviceLost(object sender, EventArgs e)
        {
        }

        void GraphicsDeviceDisposing(object sender, EventArgs e)
        {
        }

        #endregion GraphicsDevice callbacks


        #region Cleanup

        public void PurgeDrawManager()
        {
            graphicsDevice = null;
            SpriteBatch = null;

            blendStates.Clear();
            depthEnableStencilState = null;
            depthDisableStencilState = null;

            effectStack.Clear();
            PrimitiveEffect = null;
            AlphaTestEffect = null;
            defaultEffect = null;
            currentEffect = null;

            currentRenderTarget = null;

            quadsBuffer = null;
            quadsIndexBuffer = null;

            currentTexture = null;

            quadVertices = null;
            TmpVertices.Clear();
        }

        internal void ResetDevice()
        {
            vertexColorEnabled = true;
            worldMatrixChanged = false;
            projectionMatrixChanged = false;
            viewMatrixChanged = false;
            textureChanged = false;
            effectChanged = false;

            DepthTest = depthTest;

            defaultEffect.VertexColorEnabled = true;
            defaultEffect.TextureEnabled = false;
            defaultEffect.Alpha = 1f;
            defaultEffect.Texture = null;

            defaultEffect.View = viewMatrix;
            defaultEffect.World = worldMatrix;
            defaultEffect.Projection = projectionMatrix;

            matrix = worldMatrix;

            effectStack.Clear();

            currentEffect = defaultEffect;
            currentTexture = null;

            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Indices = null;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        }

        #endregion Cleanup


        #region Drawing

        internal void Clear(ClearOptions options, Color color, float depth, int stencil)
        {
            graphicsDevice.Clear(options, color, depth, stencil);
        }

        public void Clear(CCColor4B color, float depth, int stencil)
        {
            Clear(ClearOptions.Target | ClearOptions.Stencil | ClearOptions.DepthBuffer, color.ToColor(), depth, stencil);
        }

        public void Clear(CCColor4B color, float depth)
        {
            Clear(color, depth, 0);
        }

        public void Clear(CCColor4B color)
        {
            Clear(color, 1.0f);
        }

        internal void BeginDraw()
        {
            if (graphicsDevice == null || graphicsDevice.IsDisposed)
            {
                // We are exiting the game
                return;
            }

            if (needReinitResources)
            {
                //CCGraphicsResource.ReinitAllResources();
                needReinitResources = false;
            }

            ResetDevice();
            if (hasStencilBuffer)
            {
                try
                {
                    Clear(CCColor4B.Transparent, 1, 0);
                }
                catch (InvalidOperationException)
                {
                    // no stencil buffer
                    hasStencilBuffer = false;
                    Clear(CCColor4B.Transparent);
                }
            }
            else
            {
                Clear(CCColor4B.Transparent);
            }

            DrawCount = 0;
            DrawPrimitivesCount = 0;
        }

        internal void UpdateStats()
        {
            var metrics = graphicsDevice.Metrics;

            DrawCount += metrics.DrawCount;
            DrawPrimitivesCount += metrics.PrimitiveCount;
        }

        internal void EndDraw()
        {
            if (graphicsDevice == null || graphicsDevice.IsDisposed)
            {
                // We are exiting the game
                return;
            }

            Debug.Assert(stackIndex == 0);

            if (currentRenderTarget != null)
            {
                graphicsDevice.SetRenderTarget(null);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, DepthStencilState, null, AlphaTestEffect);
                SpriteBatch.Draw(currentRenderTarget, new Vector2(0, 0), Color.White);
                SpriteBatch.End();
            }

            ResetDevice();

        }

        internal void DrawPrimitives<T>(PrimitiveType type, T[] vertices, int offset, int count) where T : struct, IVertexType
        {
            if (count <= 0)
            {
                return;
            }

            ApplyEffectParams();

            EffectPassCollection passes = currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawUserPrimitives(type, vertices, offset, count);
            }
        }

        internal void DrawIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData,
            int indexOffset, int primitiveCount) where T : struct, IVertexType
        {
            if (primitiveCount <= 0)
            {
                return;
            }

            ApplyEffectParams();

            EffectPassCollection passes = currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawUserIndexedPrimitives(primitiveType, vertexData, vertexOffset, numVertices, indexData, indexOffset,
                    primitiveCount);
            }
        }

        public void DrawQuad(ref CCV3F_C4B_T2F_Quad quad)
        {
            CCV3F_C4B_T2F[] vertices = quadVertices;

            if (vertices == null)
            {
                vertices = quadVertices = new CCV3F_C4B_T2F[4];
                CheckQuadsIndexBuffer(1);
            }

            vertices[0] = quad.TopLeft;
            vertices[1] = quad.BottomLeft;
            vertices[2] = quad.TopRight;
            vertices[3] = quad.BottomRight;

            DrawIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, quadsIndexBuffer.Data.Elements, 0, 2);
        }

        public void DrawQuads(CCRawList<CCV3F_C4B_T2F_Quad> quads, int start, int n)
        {
            if (n == 0)
            {
                return;
            }

            CheckQuadsIndexBuffer(start + n);
            CheckQuadsVertexBuffer(start + n);

            quadsBuffer.UpdateBuffer(quads, start, n);

            graphicsDevice.SetVertexBuffer(quadsBuffer.VertexBuffer);
            graphicsDevice.Indices = quadsIndexBuffer.IndexBuffer;

            ApplyEffectParams();

            EffectPassCollection passes = currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, n * 4, start * 6, n * 2);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;
        }

        internal void DrawBuffer<T, T2>(CCVertexBuffer<T> vertexBuffer, CCIndexBuffer<T2> indexBuffer, int start, int count)
            where T : struct, IVertexType
            where T2 : struct
        {
            graphicsDevice.Indices = indexBuffer.IndexBuffer;
            graphicsDevice.SetVertexBuffer(vertexBuffer.VertexBuffer);

            ApplyEffectParams();

            EffectPassCollection passes = currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexBuffer.VertexCount, start, count);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;
        }

        internal void DrawRawBuffer<T>(T[] vertexBuffer, int vStart, int vCount, short[] indexBuffer, int iStart, int iCount)
            where T : struct, IVertexType
        {
            ApplyEffectParams();

            EffectPassCollection passes = currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawUserIndexedPrimitives (PrimitiveType.TriangleList, 
                    vertexBuffer, vStart, vCount, 
                    indexBuffer, iStart, iCount);
            }
        }

        internal void DrawQuadsBuffer<T>(CCVertexBuffer<T> vertexBuffer, int start, int n) where T : struct, IVertexType
        {
            if (n == 0)
            {
                return;
            }

            CheckQuadsIndexBuffer(start + n);

            graphicsDevice.Indices = quadsIndexBuffer.IndexBuffer;
            graphicsDevice.SetVertexBuffer(vertexBuffer.VertexBuffer);

            ApplyEffectParams();

            EffectPassCollection passes = currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexBuffer.VertexCount, start * 6, n * 2);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;
        }

        #endregion Drawing


        #region Effect management

        internal void PushEffect(Effect effect)
        {
            effectStack.Push(currentEffect);
            currentEffect = effect;
            effectChanged = true;
        }

        internal void PopEffect()
        {
            currentEffect = effectStack.Pop();
            effectChanged = true;
        }

        void ApplyEffectTexture()
        {
            if (currentEffect is BasicEffect)
            {
                var effect = (BasicEffect)currentEffect;

                effect.TextureEnabled = textureEnabled;
                effect.VertexColorEnabled = vertexColorEnabled;
                effect.Texture = currentTexture;
            }
            else if (currentEffect is AlphaTestEffect)
            {
                var effect = (AlphaTestEffect)currentEffect;
                effect.VertexColorEnabled = vertexColorEnabled;
                effect.Texture = currentTexture;
            }
            else
            {
                throw new Exception(String.Format("Effect {0} not supported", currentEffect.GetType().Name));
            }
        }

        void ApplyEffectParams()
        {
            if (effectChanged)
            {
                var matrices = currentEffect as IEffectMatrices;

                if (matrices != null)
                {
                    matrices.Projection = projectionMatrix;
                    matrices.View = viewMatrix;
                    matrices.World = matrix;
                }

                ApplyEffectTexture();
            }
            else
            {
                if (worldMatrixChanged || projectionMatrixChanged || viewMatrixChanged)
                {
                    var matrices = currentEffect as IEffectMatrices;

                    if (matrices != null)
                    {
                        if (worldMatrixChanged)
                        {
                            matrices.World = matrix;
                        }
                        if (projectionMatrixChanged)
                        {
                            matrices.Projection = projectionMatrix;
                        }
                        if (viewMatrixChanged)
                        {
                            matrices.View = viewMatrix;
                        }
                    }
                }

                if (textureChanged)
                {
                    ApplyEffectTexture();
                }
            }

            effectChanged = false;
            textureChanged = false;
            worldMatrixChanged = false;
            projectionMatrixChanged = false;
            viewMatrixChanged = false;
        }

        #endregion Effect management


        public void BlendFunc(CCBlendFunc blendFunc)
        {
            BlendState bs = null;
            if (blendFunc == CCBlendFunc.AlphaBlend)
            {
                bs = BlendState.AlphaBlend;
            }
            else if (blendFunc == CCBlendFunc.Additive)
            {
                bs = BlendState.Additive;
            }
            else if (blendFunc == CCBlendFunc.NonPremultiplied)
            {
                bs = BlendState.NonPremultiplied;
            }
            else if (blendFunc == CCBlendFunc.Opaque)
            {
                bs = BlendState.Opaque;
            }
            else
            {
                if (!blendStates.TryGetValue(blendFunc, out bs))
                {
                    bs = new BlendState();

                    bs.ColorSourceBlend = CCOGLES.GetXNABlend(blendFunc.Source);
                    bs.AlphaSourceBlend = CCOGLES.GetXNABlend(blendFunc.Source);
                    bs.ColorDestinationBlend = CCOGLES.GetXNABlend(blendFunc.Destination);
                    bs.AlphaDestinationBlend = CCOGLES.GetXNABlend(blendFunc.Destination);

                    blendStates.Add(blendFunc, bs);
                }
            }

            graphicsDevice.BlendState = bs;

            currBlend.Source = blendFunc.Source;
            currBlend.Destination = blendFunc.Destination;
        }


        #region Texture managment

        internal Texture2D CreateTexture2D(int width, int height)
        {
            PresentationParameters pp = graphicsDevice.PresentationParameters;
            if (!allowNonPower2Textures)
            {
                width = CCUtils.CCNextPOT(width);
                height = CCUtils.CCNextPOT(height);
            }
            return new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
        }

        public void BindTexture(CCTexture2D texture)
        {
            Texture2D tex = texture != null ? texture.XNATexture : null;

            if (!graphicsDevice.IsDisposed && graphicsDevice.GraphicsDeviceStatus == GraphicsDeviceStatus.Normal)
            {
                if (tex == null)
                {
                    graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                    TextureEnabled = false;
                }
                else
                {
                    graphicsDevice.SamplerStates[0] = texture.SamplerState;
                    TextureEnabled = true;
                }

                if (currentTexture != tex)
                {
                    currentTexture = tex;
                    textureChanged = true;
                }
            }
        }

        #endregion Texture management


        #region Render target management

        internal void CreateRenderTarget(CCTexture2D texture, CCRenderTargetUsage usage)
        {
            CCSize size = texture.ContentSizeInPixels;
            var rtarget = CreateRenderTarget((int)size.Width, (int)size.Height, CCTexture2D.DefaultAlphaPixelFormat,
                platformDepthFormat, usage);
            texture.InitWithTexture(rtarget, CCTexture2D.DefaultAlphaPixelFormat, true, false);
        }

        internal RenderTarget2D CreateRenderTarget(int width, int height, CCRenderTargetUsage usage)
        {
            return CreateRenderTarget(width, height, CCTexture2D.DefaultAlphaPixelFormat, CCDepthFormat.None, usage);
        }

        internal RenderTarget2D CreateRenderTarget(int width, int height, CCSurfaceFormat colorFormat, CCRenderTargetUsage usage)
        {
            return CreateRenderTarget(width, height, colorFormat, CCDepthFormat.None, usage);
        }

        internal RenderTarget2D CreateRenderTarget(int width, int height, CCSurfaceFormat colorFormat, CCDepthFormat depthFormat,
            CCRenderTargetUsage usage)
        {
            if (!allowNonPower2Textures)
            {
                width = CCUtils.CCNextPOT(width);
                height = CCUtils.CCNextPOT(height);
            }
            return new RenderTarget2D(graphicsDevice, width, height, false, (SurfaceFormat)colorFormat, (DepthFormat)depthFormat, 0, (RenderTargetUsage)usage);
        }

        public void SetRenderTarget(CCTexture2D texture)
        {
            RenderTarget2D target = null;

            if (texture != null)
                target = texture.XNATexture as RenderTarget2D;

            CurrentRenderTarget = target;
        }

        public void RestoreRenderTarget()
        {
            CurrentRenderTarget = previousRenderTarget;
        }

        #endregion Render target management


        void CheckQuadsIndexBuffer(int capacity)
        {
            if (quadsIndexBuffer == null || quadsIndexBuffer.Capacity < capacity * 6)
            {
                capacity = Math.Max(capacity, DefaultQuadBufferSize);

                if (quadsIndexBuffer == null)
                {
                    quadsIndexBuffer = new CCIndexBuffer<short>(capacity * 6, BufferUsage.WriteOnly);
                    quadsIndexBuffer.Count = quadsIndexBuffer.Capacity;
                }

                if (quadsIndexBuffer.Capacity < capacity * 6)
                {
                    quadsIndexBuffer.Capacity = capacity * 6;
                    quadsIndexBuffer.Count = quadsIndexBuffer.Capacity;
                }

                var indices = quadsIndexBuffer.Data.Elements;

                int i6 = 0;
                int i4 = 0;

                for (int i = 0; i < capacity; ++i)
                {
                    indices[i6 + 0] = (short)(i4 + 0);
                    indices[i6 + 1] = (short)(i4 + 2);
                    indices[i6 + 2] = (short)(i4 + 1);

                    indices[i6 + 3] = (short)(i4 + 1);
                    indices[i6 + 4] = (short)(i4 + 2);
                    indices[i6 + 5] = (short)(i4 + 3);

                    i6 += 6;
                    i4 += 4;
                }

                quadsIndexBuffer.UpdateBuffer();
            }
        }

        void CheckQuadsVertexBuffer(int capacity)
        {
            if (quadsBuffer == null || quadsBuffer.Capacity < capacity)
            {
                capacity = Math.Max(capacity, DefaultQuadBufferSize);

                if (quadsBuffer == null)
                {
                    quadsBuffer = new CCQuadVertexBuffer(capacity, CCBufferUsage.WriteOnly);
                }
                else
                {
                    quadsBuffer.Capacity = capacity;
                }
            }
        }


        #region Matrix management

        public void SetIdentityMatrix()
        {
            matrix = Matrix.Identity;
            worldMatrixChanged = true;
        }

        public void PushMatrix()
        {
            matrixStack[stackIndex++] = matrix;
        }

        public void PopMatrix()
        {
            matrix = matrixStack[--stackIndex];
            worldMatrixChanged = true;
            Debug.Assert(stackIndex >= 0);
        }

        public void Translate(float x, float y, int z)
        {
            tmpMatrix = Matrix.CreateTranslation(x, y, z);
            Matrix.Multiply(ref tmpMatrix, ref matrix, out matrix);
            worldMatrixChanged = true;
        }

        public void MultMatrix(ref Matrix matrixIn)
        {
            Matrix.Multiply(ref matrixIn, ref matrix, out matrix);
            worldMatrixChanged = true;
        }
      
        public void MultMatrix(CCAffineTransform transform, float z)
        {
            MultMatrix(ref transform, z);
        }

        public void MultMatrix(ref CCAffineTransform affineTransform, float z)
        {
            transform.M11 = affineTransform.A;
            transform.M21 = affineTransform.C;
            transform.M12 = affineTransform.B;
            transform.M22 = affineTransform.D;
            transform.M41 = affineTransform.Tx;
            transform.M42 = affineTransform.Ty;
            transform.M43 = z;

            matrix = Matrix.Multiply(transform, matrix);

            worldMatrixChanged = true;
        }

        #endregion Matrix management


        #region Mask management


        public void SetClearMaskState(int layer, bool inverted)
        {
            DepthStencilState = maskStatesCache[layer].GetClearState(layer, inverted);
        }

        public void SetDrawMaskState(int layer, bool inverted)
        {
            DepthStencilState = maskStatesCache[layer].GetDrawMaskState(layer, inverted);
        }

        public void SetDrawMaskedState(int layer, bool depth)
        {
            DepthStencilState = maskStatesCache[layer].GetDrawContentState(layer, depth, maskSavedStencilStates, this.maskLayer);
        }

        public bool BeginDrawMask(CCRect screenRect, bool inverted=false, float alphaTreshold=1f)
        {
            if (maskLayer + 1 == 8) //DepthFormat.Depth24Stencil8
            {
                if (maskOnceLog)
                {
                    CCLog.Log(
                        @"Nesting more than 8 stencils is not supported. 
                        Everything will be drawn without stencil for this node and its childs."
                        );
                    maskOnceLog = false;
                }
                return false;
            }

            maskLayer++;

            var maskState = new MaskState() { Layer = maskLayer, Inverted = inverted, AlphaTreshold = alphaTreshold };

            maskStates[maskLayer] = maskState;
            maskSavedStencilStates[maskLayer] = DepthStencilState;

            int newMaskLayer = 1 << this.maskLayer;

            ///////////////////////////////////
            // CLEAR STENCIL BUFFER

            SetClearMaskState(newMaskLayer, maskState.Inverted);

            // draw a fullscreen solid rectangle to clear the stencil buffer

            XnaGraphicsDevice.Clear (ClearOptions.Target | ClearOptions.Stencil, Color.Transparent, 1, 0);

            ///////////////////////////////////
            // PREPARE TO DRAW MASK

            SetDrawMaskState(newMaskLayer, maskState.Inverted);

            if (maskState.AlphaTreshold < 1f)
            {
                AlphaTestEffect.AlphaFunction = CompareFunction.Greater;
                AlphaTestEffect.ReferenceAlpha = (byte)(255 * maskState.AlphaTreshold);

                PushEffect(AlphaTestEffect);
            }

            return true;
        }

        public void EndDrawMask()
        {
            var maskState = maskStates[maskLayer];

            ///////////////////////////////////
            // PREPARE TO DRAW MASKED CONTENT

            if (maskState.AlphaTreshold < 1)
            {
                PopEffect();
            }

            SetDrawMaskedState(maskLayer, maskSavedStencilStates[maskLayer].DepthBufferEnable);
        }

        public void EndMask()
        {
            ///////////////////////////////////
            // RESTORE STATE

            DepthStencilState = maskSavedStencilStates[maskLayer];

            maskLayer--;
        }

        #endregion
    }

    internal struct MaskState
    {
        public int Layer;
        public bool Inverted;
        public float AlphaTreshold;
    }

    internal struct MaskDepthStencilStateCacheEntry
    {
        public DepthStencilState Clear;
        public DepthStencilState ClearInvert;
        public DepthStencilState DrawMask;
        public DepthStencilState DrawMaskInvert;
        public DepthStencilState DrawContent;
        public DepthStencilState DrawContentDepth;

        public DepthStencilState GetClearState(int layer, bool inverted)
        {
            DepthStencilState result = inverted ? ClearInvert : Clear;

            if (result == null)
            {
                int maskLayer = 1 << layer;

                result = new DepthStencilState()
                {
                    DepthBufferEnable = false,

                    StencilEnable = true,

                    StencilFunction = CompareFunction.Never,

                    StencilMask = maskLayer,
                    StencilWriteMask = maskLayer,
                    ReferenceStencil = maskLayer,

                    StencilFail = !inverted ? StencilOperation.Zero : StencilOperation.Replace
                };

                if (inverted)
                {
                    ClearInvert = result;
                }
                else
                {
                    Clear = result;
                }
            }

            return result;
        }

        public DepthStencilState GetDrawMaskState(int layer, bool inverted)
        {
            DepthStencilState result = inverted ? DrawMaskInvert : DrawMask;

            if (result == null)
            {
                int maskLayer = 1 << layer;

                result = new DepthStencilState()
                {
                    DepthBufferEnable = false,

                    StencilEnable = true,

                    StencilFunction = CompareFunction.Never,

                    StencilMask = maskLayer,
                    StencilWriteMask = maskLayer,
                    ReferenceStencil = maskLayer,

                    StencilFail = !inverted ? StencilOperation.Replace : StencilOperation.Zero,
                };

                if (inverted)
                {
                    DrawMaskInvert = result;
                }
                else
                {
                    DrawMask = result;
                }
            }

            return result;
        }

        public DepthStencilState GetDrawContentState(int layer, bool depth, DepthStencilState[] maskSavedStencilStates, int currentMaskLayer)
        {
            DepthStencilState result = depth ? DrawContentDepth : DrawContent;

            if (result == null)
            {
                int maskLayer = 1 << layer;
                int maskLayerL = maskLayer - 1;
                int maskLayerLe = maskLayer | maskLayerL;

                result = new DepthStencilState()
                {
                    DepthBufferEnable = maskSavedStencilStates[currentMaskLayer].DepthBufferEnable,
                    StencilEnable = true,

                    StencilMask = maskLayerLe,
                    StencilWriteMask = 0,
                    ReferenceStencil = maskLayerLe,

                    StencilFunction = CompareFunction.Equal,

                    StencilPass = StencilOperation.Zero,
                    StencilFail = StencilOperation.Zero,
                };

                if (depth)
                {
                    DrawContentDepth = result;
                }
                else
                {
                    DrawContent = result;
                }
            }

            return result;
        }
    }

    public class CCGraphicsResource : IDisposable
    {
        bool isDisposed;

        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        #region Constructors

        public CCGraphicsResource()
        {
        }

        #endregion Constructors


        #region Cleaning up

        ~CCGraphicsResource()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                isDisposed = true;
            }
        }

        #endregion Cleaning up

        public virtual void ReinitResource()
        {
        }
    }

    internal class CCVertexBuffer<T> : CCGraphicsResource where T : struct, IVertexType
    {
        protected VertexBuffer vertexBuffer;
        protected CCBufferUsage usage;
        protected CCRawList<T> data;


        #region Properties

        internal VertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        public CCRawList<T> Data
        {
            get { return data; }
        }

        public int Count
        {
            get { return data.Count; }
            set
            {
                Debug.Assert(value <= data.Capacity);
                data.Count = value;
            }
        }

        public int Capacity
        {
            get { return data.Capacity; }
            set
            {
                if (data.Capacity != value)
                {
                    data.Capacity = value;
                    ReinitResource();
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCVertexBuffer(int vertexCount, CCBufferUsage usage)
        {
            data = new CCRawList<T>(vertexCount);
            this.usage = usage;
            ReinitResource();
        }

        #endregion Constructors


        public override void ReinitResource()
        {
            if (vertexBuffer != null && !vertexBuffer.IsDisposed)
            {
                vertexBuffer.Dispose();
            }

            vertexBuffer = new VertexBuffer(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, typeof(T), data.Capacity, (BufferUsage)usage);
        }

        public void UpdateBuffer()
        {
            UpdateBuffer(0, data.Count);
        }

        public virtual void UpdateBuffer(int startIndex, int elementCount)
        {
            if (elementCount > 0)
            {
                int vertexByteSize = vertexBuffer.VertexDeclaration.VertexStride;
                vertexBuffer.SetData(vertexByteSize * startIndex, data.Elements, startIndex, elementCount, vertexByteSize);
            }
        }
    }

    internal class CCQuadVertexBuffer : CCVertexBuffer<CCV3F_C4B_T2F_Quad>
    {
        public CCQuadVertexBuffer(int vertexCount, CCBufferUsage usage)
            : base(vertexCount, usage)
        {
        }

        public void UpdateBuffer(CCRawList<CCV3F_C4B_T2F_Quad> dataIn, int startIndex, int elementCount)
        {
            //TODO: 
            var tmp = data;
            data = dataIn;

            UpdateBuffer(startIndex, elementCount);

            data = tmp;
        }

        public override void UpdateBuffer(int startIndex, int elementCount)
        {
            if (elementCount == 0)
            {
                return;
            }

            var quads = data.Elements;
            var tmp = CCDrawManager.SharedDrawManager.TmpVertices;

            while (tmp.Capacity < elementCount)
            {
                tmp.Capacity = tmp.Capacity * 2;
            }
            tmp.Count = elementCount * 4;

            var vertices = tmp.Elements;

            int i4 = 0;
            for (int i = startIndex; i < startIndex + elementCount; i++)
            {
                vertices[i4 + 0] = quads[i].TopLeft;
                vertices[i4 + 1] = quads[i].BottomLeft;
                vertices[i4 + 2] = quads[i].TopRight;
                vertices[i4 + 3] = quads[i].BottomRight;

                i4 += 4;
            }

            int vertexByteSize = vertexBuffer.VertexDeclaration.VertexStride;

            vertexBuffer.SetData(vertexByteSize * startIndex * 4, vertices, 0, elementCount * 4, vertexByteSize);
        }

        public override void ReinitResource()
        {
            if (vertexBuffer != null && !vertexBuffer.IsDisposed)
            {
                vertexBuffer.Dispose();
            }

            vertexBuffer = new VertexBuffer(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, typeof(CCV3F_C4B_T2F), data.Capacity * 4, (BufferUsage)usage);

            UpdateBuffer();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && vertexBuffer != null && !vertexBuffer.IsDisposed)
            {
                vertexBuffer.Dispose();
            }

            vertexBuffer = null;
        }
    }

	internal class CCIndexBuffer<T> : CCGraphicsResource where T : struct
    {
        IndexBuffer indexBuffer;
        BufferUsage usage;
        CCRawList<T> data;

        #region Properties

        internal IndexBuffer IndexBuffer
        {
            get { return indexBuffer; }
        }

        public CCRawList<T> Data
        {
            get { return data; }
        }

        public int Count
        {
            get { return data.Count; }
            set
            {
                Debug.Assert(value <= data.Capacity);
                data.Count = value;
            }
        }

        public int Capacity
        {
            get { return data.Capacity; }
            set
            {
                if (data.Capacity != value)
                {
                    data.Capacity = value;
                    ReinitResource();
                }
            }
        }

        #endregion Properties

        #region Constructors

        public CCIndexBuffer(int indexCount, BufferUsage usage)
        {
            data = new CCRawList<T>(indexCount);
            this.usage = usage;
            ReinitResource();
        }

        #endregion Constructors

        public override void ReinitResource()
        {
            if (indexBuffer != null && !indexBuffer.IsDisposed)
            {
                indexBuffer.Dispose();
            }

            indexBuffer = new IndexBuffer(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, typeof(T), data.Capacity, usage);

            UpdateBuffer();
        }

        public void UpdateBuffer()
        {
            UpdateBuffer(0, data.Count);
        }

        public void UpdateBuffer(int startIndex, int elementCount)
        {
            if (elementCount > 0)
            {
                indexBuffer.SetData(data.Elements, startIndex, elementCount);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && indexBuffer != null && !indexBuffer.IsDisposed)
            {
                indexBuffer.Dispose();
            }

            indexBuffer = null;
        }
    }
}