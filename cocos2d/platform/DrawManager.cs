using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    public static class DrawManager
    {
        private const int DefaultQuadBufferSize = 1024 * 4;
        public static string DefaultFont = "arial";

        public static BasicEffect PrimitiveEffect;

        private static BasicEffect m_defaultEffect;
        private static Effect m_currentEffect;
        private static readonly Stack<Effect> m_effectStack = new Stack<Effect>();

        public static SpriteBatch spriteBatch;
        public static GraphicsDevice graphicsDevice;

        internal static Matrix m_worldMatrix;
        internal static Matrix m_viewMatrix;
        internal static Matrix m_projectionMatrix;

        private static readonly Matrix[] m_matrixStack = new Matrix[100];
        private static int m_stackIndex;

        internal static Matrix m_Matrix;
        private static Matrix m_TmpMatrix;

        private static readonly RenderTarget2D m_renderTarget = null;

        private static Texture2D m_currentTexture;
        private static bool m_vertexColorEnabled;

        private static readonly Dictionary<ccBlendFunc, BlendState> m_blendStates = new Dictionary<ccBlendFunc, BlendState>();

        private static DepthStencilState m_DepthEnableStencilState;
        private static DepthStencilState m_DepthDisableStencilState;

        //Flags
        private static bool m_worldMatrixChanged;
        private static bool m_projectionMatrixChanged;
        private static bool m_viewMatrixChanged;
        private static bool m_textureChanged;
        private static bool m_effectChanged;

        private static int m_lastWidth;
        private static int m_lastHeight;
        private static bool m_depthTest = true;
        private static ccBlendFunc m_currBlend = new ccBlendFunc(ccMacros.CC_BLEND_SRC, ccMacros.CC_BLEND_DST);
        private static RenderTarget2D m_currRenderTarget;
        private static Viewport m_savedViewport;
        private static DynamicVertexBuffer m_quadsBuffer;
        private static IndexBuffer m_quadsIndexBuffer;

        private static ccV3F_C4B_T2F[] m_vertices;
        private static short[] m_quadIndices;

        public static int DrawCount;
        private static ccV3F_C4B_T2F[] m_quadVertices;
        private static float m_fScaleX;
        private static float m_fScaleY;
        private static CCRect m_obViewPortRect;
        private static CCSize m_obScreenSize;
        private static CCSize m_obDesignResolutionSize;
        private static ResolutionPolicy m_eResolutionPolicy = ResolutionPolicy.UnKnown;
        private static float m_fFrameZoomFactor = 1.0f;
        private static RasterizerState m_savedRasterizerState;
        private static RasterizerState m_scissorRasterizerState;

        public static bool VertexColorEnabled
        {
            get { return m_vertexColorEnabled; }
            set
            {
                if (m_vertexColorEnabled != value)
                {
                    m_vertexColorEnabled = value;
                    m_textureChanged = true;
                }
            }
        }

        public static DepthStencilState DepthStencilState
        {
            get { return graphicsDevice.DepthStencilState; }
            set { graphicsDevice.DepthStencilState = value; }
        }

        public static BlendState BlendState
        {
            get { return graphicsDevice.BlendState; }
            set
            {
                graphicsDevice.BlendState = value;
                m_currBlend.src = -1;
                m_currBlend.dst = -1;
            }
        }

        public static Matrix ViewMatrix
        {
            get { return m_viewMatrix; }
            set
            {
                m_viewMatrix = value;
                m_viewMatrixChanged = true;
            }
        }

        public static Matrix ProjectionMatrix
        {
            get { return m_projectionMatrix; }
            set
            {
                m_projectionMatrix = value;
                m_projectionMatrixChanged = true;
            }
        }

        public static Matrix WorldMatrix
        {
            get { return m_Matrix; }
            set
            {
                m_Matrix = m_worldMatrix = value;
                m_worldMatrixChanged = true;
            }
        }

        public static CCSize Size
        {
            get { return m_obDesignResolutionSize; }
        }

        public static CCSize FrameSize
        {
            get { return m_obScreenSize; }
            set { m_obDesignResolutionSize = m_obScreenSize = value; }
        }

        public static bool DepthTest
        {
            get { return m_depthTest; }
            set
            {
                m_depthTest = value;
                graphicsDevice.DepthStencilState = value ? m_DepthEnableStencilState : m_DepthDisableStencilState;
                //graphicsDevice.DepthStencilState = value ? DepthStencilState.Default : DepthStencilState.None;
            }
        }

        public static CCSize VisibleSize
        {
            get
            {
                if (m_eResolutionPolicy == ResolutionPolicy.NoBorder)
                {
                    return new CCSize(m_obScreenSize.width / m_fScaleX, m_obScreenSize.height / m_fScaleY);
                }
                else
                {
                    return m_obDesignResolutionSize;
                }
            }
        }

        public static CCPoint VisibleOrigin
        {
            get
            {
                if (m_eResolutionPolicy == ResolutionPolicy.NoBorder)
                {
                    return new CCPoint((m_obDesignResolutionSize.width - m_obScreenSize.width / m_fScaleX) / 2,
                                       (m_obDesignResolutionSize.height - m_obScreenSize.height / m_fScaleY) / 2);
                }
                else
                {
                    return CCPoint.Zero;
                }
            }
        }

        public static bool ScissorRectEnabled
        {
            get { return graphicsDevice.RasterizerState.ScissorTestEnable; }
            set
            {
                if (m_scissorRasterizerState == null)
                {
                    m_scissorRasterizerState = new RasterizerState
                        {
                            ScissorTestEnable = true
                        };
                }
                if (value)
                {
                    m_savedRasterizerState = graphicsDevice.RasterizerState;
                    graphicsDevice.RasterizerState = m_scissorRasterizerState;
                }
                else if (m_savedRasterizerState != null)
                {
                    graphicsDevice.RasterizerState = m_savedRasterizerState;
                }
            }
        }

        public static CCRect ViewPortRect
        {
            get { return m_obViewPortRect; }
        }

        public static float ScaleX
        {
            get { return m_fScaleX; }
        }

        public static float ScaleY
        {
            get { return m_fScaleY; }
        }

        public static void Init(GraphicsDevice graphicsDevice)
        {
            DrawManager.graphicsDevice = graphicsDevice;
            
            spriteBatch = new SpriteBatch(graphicsDevice);
            
            m_defaultEffect = new BasicEffect(graphicsDevice);

            PrimitiveEffect = new BasicEffect(graphicsDevice)
                {
                    TextureEnabled = false,
                    VertexColorEnabled = true
                };

            m_DepthEnableStencilState = new DepthStencilState
                {
                    DepthBufferEnable = true,
                    DepthBufferWriteEnable = true,
                    TwoSidedStencilMode = true
                };

            m_DepthDisableStencilState = new DepthStencilState
                {
                    DepthBufferEnable = false
                };

            PresentationParameters pp = graphicsDevice.PresentationParameters;
            //pp.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            //_renderTarget = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, (int)pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.PreserveContents);

            m_fScaleY = 1.0f;
            m_fScaleX = 1.0f;
            m_eResolutionPolicy = ResolutionPolicy.UnKnown;

            m_obViewPortRect = new CCRect(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
            m_obScreenSize = m_obDesignResolutionSize = m_obViewPortRect.size;

            CCDrawingPrimitives.init(graphicsDevice);
        }

        private static void ResetDevice()
        {
            m_defaultEffect.View = m_viewMatrix;
            m_defaultEffect.World = m_worldMatrix;
            m_defaultEffect.Projection = m_projectionMatrix;

            m_Matrix = m_worldMatrix;

            m_defaultEffect.Alpha = 1f;
            m_defaultEffect.VertexColorEnabled = true;
            m_defaultEffect.Texture = null;
            m_defaultEffect.TextureEnabled = false;

            m_effectStack.Clear();

            m_currentEffect = m_defaultEffect;

            m_currentTexture = null;
            m_vertexColorEnabled = true;

            m_worldMatrixChanged = false;
            m_projectionMatrixChanged = false;
            m_viewMatrixChanged = false;
            m_textureChanged = false;
            m_effectChanged = false;

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.SetRenderTarget(m_renderTarget);
            graphicsDevice.Indices = null;

            graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            graphicsDevice.BlendState = BlendState.AlphaBlend;

            DepthTest = m_depthTest;

            if (graphicsDevice.Viewport.Width != m_lastWidth || graphicsDevice.Viewport.Height != m_lastHeight)
            {
                PresentationParameters pp = graphicsDevice.PresentationParameters;
                m_obViewPortRect = new CCRect(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
                m_obScreenSize = m_obViewPortRect.size;

                if (m_eResolutionPolicy != ResolutionPolicy.UnKnown)
                {
                    SetDesignResolutionSize(m_obDesignResolutionSize.width, m_obDesignResolutionSize.height, m_eResolutionPolicy);
                }
                else
                {
                    CCDirector director = CCDirector.SharedDirector;
                    director.Projection = director.Projection;
                }

                m_lastWidth = graphicsDevice.Viewport.Width;
                m_lastHeight = graphicsDevice.Viewport.Height;
            }
        }

        public static void BeginDraw()
        {
            ResetDevice();
            graphicsDevice.Clear(Color.Black);
            DrawCount = 0;
        }

        public static void EndDraw()
        {
            Debug.Assert(m_stackIndex == 0);

            if (m_renderTarget != null)
            {
                graphicsDevice.SetRenderTarget(null);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                spriteBatch.Draw(m_renderTarget, new Vector2(0, 0), Color.White);
                spriteBatch.End();
            }

            ResetDevice();
        }

        public static void PushEffect(Effect effect)
        {
            m_effectStack.Push(m_currentEffect);
            m_currentEffect = effect;
            m_effectChanged = true;
        }

        public static void PopEffect()
        {
            m_currentEffect = m_effectStack.Pop();
            m_effectChanged = true;
        }

        private static void ApplyEffectTexture()
        {
            if (m_currentEffect is BasicEffect)
            {
                var effect = (BasicEffect) m_currentEffect;

                if (m_currentTexture != null)
                {
                    effect.TextureEnabled = true;
                    effect.VertexColorEnabled = m_vertexColorEnabled;
                    effect.Texture = m_currentTexture;
                }
                else
                {
                    effect.TextureEnabled = false;
                    effect.VertexColorEnabled = m_vertexColorEnabled;
                }
            }
            else if (m_currentEffect is AlphaTestEffect)
            {
                var effect = (AlphaTestEffect) m_currentEffect;
                effect.VertexColorEnabled = m_vertexColorEnabled;
                effect.Texture = m_currentTexture;
            }
            else
            {
                throw new Exception(String.Format("Effect {0} not supported", m_currentEffect.GetType().Name));
            }
        }

        private static void ApplyEffectParams()
        {
            if (m_effectChanged)
            {
                var matrices = m_currentEffect as IEffectMatrices;

                if (matrices != null)
                {
                    matrices.Projection = m_projectionMatrix;
                    matrices.View = m_viewMatrix;
                    matrices.World = m_Matrix;
                }

                ApplyEffectTexture();
            }
            else
            {
                if (m_worldMatrixChanged || m_projectionMatrixChanged || m_viewMatrixChanged)
                {
                    var matrices = m_currentEffect as IEffectMatrices;

                    if (matrices != null)
                    {
                        if (m_worldMatrixChanged)
                        {
                            matrices.World = m_Matrix;
                        }
                        if (m_projectionMatrixChanged)
                        {
                            matrices.Projection = m_projectionMatrix;
                        }
                        if (m_viewMatrixChanged)
                        {
                            matrices.View = m_viewMatrix;
                        }
                    }
                }

                if (m_textureChanged)
                {
                    ApplyEffectTexture();
                }
            }

            m_effectChanged = false;
            m_textureChanged = false;
            m_worldMatrixChanged = false;
            m_projectionMatrixChanged = false;
            m_viewMatrixChanged = false;
        }

        public static void DrawPrimitives<T>(PrimitiveType type, T[] vertices, int offset, int count) where T : struct, IVertexType
        {
            if (count <= 0)
            {
                return;
            }

            ApplyEffectParams();

            EffectPassCollection passes = m_currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawUserPrimitives(type, vertices, offset, count);
            }

            DrawCount++;
        }

        public static void DrawIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData,
                                                    int indexOffset, int primitiveCount) where T : struct, IVertexType
        {
            if (primitiveCount <= 0)
            {
                return;
            }

            ApplyEffectParams();

            EffectPassCollection passes = m_currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawUserIndexedPrimitives(primitiveType, vertexData, vertexOffset, numVertices, indexData, indexOffset,
                                                         primitiveCount);
            }

            DrawCount++;
        }


        public static void BlendFunc(ccBlendFunc blendFunc)
        {
            if (m_currBlend.dst != blendFunc.dst || m_currBlend.src != blendFunc.src)
            {
                BlendState bs = null;
                if (blendFunc.src == OGLES.GL_ONE && blendFunc.dst == OGLES.GL_ONE_MINUS_SRC_ALPHA)
                {
                    bs = BlendState.AlphaBlend;
                }
                else if (blendFunc.src == OGLES.GL_SRC_ALPHA && blendFunc.dst == OGLES.GL_ONE)
                {
                    bs = BlendState.Additive;
                }
                else if (blendFunc.src == OGLES.GL_SRC_ALPHA && blendFunc.dst == OGLES.GL_ONE_MINUS_SRC_ALPHA)
                {
                    bs = BlendState.NonPremultiplied;
                }
                else if (blendFunc.src == OGLES.GL_ONE && blendFunc.dst == OGLES.GL_ZERO)
                {
                    bs = BlendState.Opaque;
                }
                else
                {
                    if (!m_blendStates.TryGetValue(blendFunc, out bs))
                    {
                        bs = new BlendState();

                        bs.ColorSourceBlend = OGLES.GetXNABlend(blendFunc.src);
                        bs.AlphaSourceBlend = OGLES.GetXNABlend(blendFunc.src);
                        bs.ColorDestinationBlend = OGLES.GetXNABlend(blendFunc.dst);
                        bs.AlphaDestinationBlend = OGLES.GetXNABlend(blendFunc.dst);

                        m_blendStates.Add(blendFunc, bs);
                    }
                }

                graphicsDevice.BlendState = bs;

                m_currBlend.src = blendFunc.src;
                m_currBlend.dst = blendFunc.dst;
            }
        }

        public static void BindTexture(Texture2D texture)
        {
            if (m_currentTexture != texture)
            {
                m_currentTexture = texture;
                m_textureChanged = true;
            }
        }

        public static void BindTexture(CCTexture2D texture)
        {
            Texture2D tex = texture == null ? null : texture.Texture2D;

            if (!graphicsDevice.IsDisposed && graphicsDevice.GraphicsDeviceStatus == GraphicsDeviceStatus.Normal)
            {
            if (texture != null && texture.m_bAliased)
            {
                graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            }
            else
            {
                graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            }

            if (m_currentTexture != tex)
            {
                m_currentTexture = tex;
                m_textureChanged = true;
            }
        }
        }

        public static void CreateRenderTarget(CCTexture2D pTexture, RenderTargetUsage usage)
        {
            CCSize size = pTexture.ContentSizeInPixels;
            pTexture.texture2D = CreateRenderTarget((int) size.width, (int) size.height, SurfaceFormat.Color, DepthFormat.Depth24, usage);
        }

        public static RenderTarget2D CreateRenderTarget(int width, int height, RenderTargetUsage usage)
        {
            return CreateRenderTarget(width, height, SurfaceFormat.Color, DepthFormat.None, usage);
        }

        public static RenderTarget2D CreateRenderTarget(int width, int height, SurfaceFormat colorFormat, RenderTargetUsage usage)
        {
            return CreateRenderTarget(width, height, colorFormat, DepthFormat.None, usage);
        }

        public static RenderTarget2D CreateRenderTarget(int width, int height, SurfaceFormat colorFormat, DepthFormat depthFormat,
                                                        RenderTargetUsage usage)
        {
            return new RenderTarget2D(graphicsDevice, width, height, false, colorFormat, depthFormat, 0, usage);
        }

        public static Texture2D CreateTexture2D(int width, int height)
        {
            PresentationParameters pp = graphicsDevice.PresentationParameters;
#if IPHONE || NEXUS
            width = (int)ccUtils.ccNextPOT((long)width);
            height = (int)ccUtils.ccNextPOT((long)height);
#endif
            return new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
        }

        public static void SetRenderTarget(CCTexture2D pTexture)
        {
            if (pTexture == null)
            {
                SetRenderTarget((RenderTarget2D) null);
            }
            else
            {
                Debug.Assert(pTexture.texture2D is RenderTarget2D);
                SetRenderTarget((RenderTarget2D) pTexture.texture2D);
            }
        }

        public static void SetRenderTarget(RenderTarget2D renderTarget)
        {
            if (graphicsDevice.GraphicsDeviceStatus == GraphicsDeviceStatus.Normal)
            {
            if (renderTarget == null)
            {
                graphicsDevice.SetRenderTarget(m_renderTarget);
                graphicsDevice.Viewport = m_savedViewport;
            }
            else
            {
                m_savedViewport = graphicsDevice.Viewport;
                graphicsDevice.SetRenderTarget(renderTarget);
            }
            }
            m_currRenderTarget = renderTarget;
        }

        public static RenderTarget2D GetRenderTarget()
        {
            return m_currRenderTarget;
        }

        private static void CheckQuadsIndexBuffer(int capacity)
        {
            if (m_quadsIndexBuffer == null || m_quadsIndexBuffer.IndexCount < capacity * 6)
            {
                capacity = Math.Max(capacity, DefaultQuadBufferSize);

                m_quadsIndexBuffer = new IndexBuffer(CCApplication.SharedApplication.GraphicsDevice, typeof(short), capacity * 6,
                                                     BufferUsage.WriteOnly);

                m_quadIndices = new short[capacity * 6];

                int i6 = 0;
                int i4 = 0;
                short[] indices = m_quadIndices;

                for (int i = 0; i < capacity; ++i)
                {
                    indices[i6 + 0] = (short) (i4 + 0);
                    indices[i6 + 1] = (short) (i4 + 2);
                    indices[i6 + 2] = (short) (i4 + 1);

                    indices[i6 + 3] = (short) (i4 + 1);
                    indices[i6 + 4] = (short) (i4 + 2);
                    indices[i6 + 5] = (short) (i4 + 3);

                    i6 += 6;
                    i4 += 4;
                }

                m_quadsIndexBuffer.SetData(indices);
            }
        }

        private static void CheckQuadsVertexBuffer(int capacity)
        {
            if (m_quadsBuffer == null || m_quadsBuffer.IsContentLost || m_quadsBuffer.VertexCount < capacity * 4)
            {
                capacity = Math.Max(capacity, DefaultQuadBufferSize);

                if (m_quadsBuffer != null)
                {
                    m_quadsBuffer.Dispose();
                }
				
                m_quadsBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(ccV3F_C4B_T2F), capacity * 4, BufferUsage.WriteOnly);

                if (m_vertices == null || m_vertices.Length < capacity * 4)
                {
                    m_vertices = new ccV3F_C4B_T2F[capacity * 4];
                }
            }
        }

        public static void DrawQuad(ref ccV3F_C4B_T2F_Quad quad)
        {
            ccV3F_C4B_T2F[] vertices = m_quadVertices;

            if (vertices == null)
            {
                vertices = m_quadVertices = new ccV3F_C4B_T2F[4];
                CheckQuadsIndexBuffer(1);
            }

            vertices[0] = quad.tl;
            vertices[1] = quad.bl;
            vertices[2] = quad.tr;
            vertices[3] = quad.br;

            DrawIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, m_quadIndices, 0, 2);
        }

        public static void DrawQuads(RawList<ccV3F_C4B_T2F_Quad> quads, int start, int n)
        {
            if (n == 0)
            {
                return;
            }

            CheckQuadsIndexBuffer(start + n);
            CheckQuadsVertexBuffer(start + n);

            SetQuadsToBuffer(m_quadsBuffer, quads, start, n);

            //DrawIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 4 * start, 4 * n, _quadIndices, start * 6, 2 * n);

            graphicsDevice.SetVertexBuffer(m_quadsBuffer);
            graphicsDevice.Indices = m_quadsIndexBuffer;

            ApplyEffectParams();

            EffectPassCollection passes = m_currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_vertices.Length, start * 6, n * 2);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;

            DrawCount++;
        }

        public static void DrawBuffer(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, int start, int count)
        {
            graphicsDevice.Indices = indexBuffer;
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            ApplyEffectParams();

            EffectPassCollection passes = m_currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, start, count);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;

            DrawCount++;
        }

        public static void DrawQuadsBuffer(VertexBuffer vertexBuffer, int start, int n)
        {
            if (n == 0)
            {
                return;
            }

            CheckQuadsIndexBuffer(start + n);

            graphicsDevice.Indices = m_quadsIndexBuffer;
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            ApplyEffectParams();

            EffectPassCollection passes = m_currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, start * 6, n * 2);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;

            DrawCount++;
        }

        public static void SetQuadsToBuffer(VertexBuffer vertexBuffer, RawList<ccV3F_C4B_T2F_Quad> quads, int start, int n)
        {
            if (m_vertices == null || m_vertices.Length < quads.Capacity * 4)
            {
                int capacity = Math.Max(quads.Capacity, DefaultQuadBufferSize);
                m_vertices = new ccV3F_C4B_T2F[capacity * 4];
            }

            ccV3F_C4B_T2F_Quad[] elements = quads.Elements;

#if ANDROID
            vertexBuffer.SetData(elements, start, n);
#else
            ccV3F_C4B_T2F[] vertices = m_vertices;
            int i4 = 0;
            for (int i = start; i < start + n; i++)
            {
                vertices[i4 + 0] = elements[i].tl;
                vertices[i4 + 1] = elements[i].bl;
                vertices[i4 + 2] = elements[i].tr;
                vertices[i4 + 3] = elements[i].br;

                i4 += 4;
            }

            vertexBuffer.SetData(vertices, start * 4, n * 4);
#endif
        }

        public static void Clear(ClearOptions options, Color color, float depth, int stencil)
        {
            graphicsDevice.Clear(options, color, depth, stencil);
        }

        public static void Clear(Color color, float depth, int stencil)
        {
            graphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil | ClearOptions.DepthBuffer, color, depth, stencil);
        }

        public static void Clear(Color color, float depth)
        {
            graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, color, depth, 0);
        }

        public static void Clear(Color color)
        {
            graphicsDevice.Clear(color);
        }

        /// <summary>
        /// Set zoom factor for frame. This method is for debugging big resolution (e.g.new ipad) app on
        /// desktop.
        /// </summary>
        public static void SetFrameZoom(float zoomFactor)
        {
            m_fFrameZoomFactor = zoomFactor;
        }

        public static void SetViewPort(int x, int y, int width, int height)
        {
            graphicsDevice.Viewport = new Viewport(x, y, width, height);
        }

        public static void SetViewPortInPoints(int x, int y, int width, int height)
        {
            graphicsDevice.Viewport = new Viewport(
                (int) (x * m_fScaleX * m_fFrameZoomFactor + m_obViewPortRect.origin.x * m_fFrameZoomFactor),
                (int) (y * m_fScaleY * m_fFrameZoomFactor + m_obViewPortRect.origin.y * m_fFrameZoomFactor),
                (int) (width * m_fScaleX * m_fFrameZoomFactor),
                (int) (height * m_fScaleY * m_fFrameZoomFactor)
                );
        }

        public static void SetDesignResolutionSize(float width, float height, ResolutionPolicy resolutionPolicy)
        {
            Debug.Assert(resolutionPolicy != ResolutionPolicy.UnKnown, "should set resolutionPolicy");

            if (width == 0.0f || height == 0.0f)
            {
                return;
            }

            m_obDesignResolutionSize.width = width;
            m_obDesignResolutionSize.height = height;

            m_fScaleX = m_obScreenSize.width / m_obDesignResolutionSize.width;
            m_fScaleY = m_obScreenSize.height / m_obDesignResolutionSize.height;

            if (resolutionPolicy == ResolutionPolicy.NoBorder)
            {
                m_fScaleX = m_fScaleY = Math.Max(m_fScaleX, m_fScaleY);
            }

            if (resolutionPolicy == ResolutionPolicy.ShowAll)
            {
                m_fScaleX = m_fScaleY = Math.Min(m_fScaleX, m_fScaleY);
            }

            // calculate the rect of viewport    
            float viewPortW = m_obDesignResolutionSize.width * m_fScaleX;
            float viewPortH = m_obDesignResolutionSize.height * m_fScaleY;

            m_obViewPortRect = new CCRect((m_obScreenSize.width - viewPortW) / 2, (m_obScreenSize.height - viewPortH) / 2, viewPortW, viewPortH);

            m_eResolutionPolicy = resolutionPolicy;

            // reset director's member variables to fit visible rect
            CCDirector.SharedDirector.m_obWinSizeInPoints = Size;
            CCDirector.SharedDirector.m_obWinSizeInPixels = new CCSize(m_obDesignResolutionSize.width * ccMacros.CC_CONTENT_SCALE_FACTOR(),
                                                                       m_obDesignResolutionSize.height * ccMacros.CC_CONTENT_SCALE_FACTOR());
            CCDirector.SharedDirector.CreateStatsLabel();
            CCDirector.SharedDirector.SetGlDefaultValues();
        }

        public static void SetScissorInPoints(float x, float y, float w, float h)
        {
            graphicsDevice.ScissorRectangle = new Rectangle(
                (int) (x * m_fScaleX + m_obViewPortRect.origin.x),
                (int) (y * m_fScaleY + m_obViewPortRect.origin.y),
                (int) (w * m_fScaleX),
                (int) (h * m_fScaleY)
                );
        }

        private static GraphicsDeviceManager m_GraphicsDeviceMgr;

        public static void SetOrientation(DisplayOrientation supportedOrientations)
        {
            SetOrientation(supportedOrientations, true);
        }

        private static void SetOrientation(DisplayOrientation supportedOrientations, bool bUpdateDimensions)
        {
            bool ll = (supportedOrientations & DisplayOrientation.LandscapeLeft) == DisplayOrientation.LandscapeLeft;
            bool lr = (supportedOrientations & DisplayOrientation.LandscapeRight) == DisplayOrientation.LandscapeRight;
            bool p = (supportedOrientations & DisplayOrientation.Portrait) == DisplayOrientation.Portrait;

            bool onlyLandscape = (ll || lr) && !p;
            bool onlyPortrait = !(ll || lr) && p;
            bool bSwapDims = bUpdateDimensions && ((m_GraphicsDeviceMgr.SupportedOrientations & supportedOrientations) ==  DisplayOrientation.Default);
            if (bSwapDims && (ll || lr))
            {
                // Check for landscape changes that do not need a swap
                if (((m_GraphicsDeviceMgr.SupportedOrientations & DisplayOrientation.LandscapeLeft) != DisplayOrientation.Default) ||
                    ((m_GraphicsDeviceMgr.SupportedOrientations & DisplayOrientation.LandscapeRight) != DisplayOrientation.Default))
                {
                    bSwapDims = false;
                }
            }
            if (bSwapDims)
            {
                CCSize newSize = m_obDesignResolutionSize.Inverted;
                DrawManager.SetDesignResolutionSize(newSize.width, newSize.height, m_eResolutionPolicy);
                /*
                m_obViewPortRect = m_obViewPortRect.InvertedSize;
                m_obDesignResolutionSize = m_obDesignResolutionSize.Inverted;
                CCDirector.SharedDirector.m_obWinSizeInPoints = CCDirector.SharedDirector.m_obWinSizeInPoints.Inverted;
                CCDirector.SharedDirector.m_obWinSizeInPixels = CCDirector.SharedDirector.m_obWinSizeInPixels.Inverted;
                m_obScreenSize = m_obScreenSize.Inverted;
                float f = m_fScaleX;
                m_fScaleX = m_fScaleY;
                m_fScaleY = f;
                 */
            }
            int preferredBackBufferWidth = m_GraphicsDeviceMgr.PreferredBackBufferWidth;
            int preferredBackBufferHeight = m_GraphicsDeviceMgr.PreferredBackBufferHeight;
#if ANDROID
            if (onlyLandscape && m_GraphicsDeviceMgr.PreferredBackBufferHeight > m_GraphicsDeviceMgr.PreferredBackBufferWidth)
            {
                m_GraphicsDeviceMgr.PreferredBackBufferWidth = preferredBackBufferHeight;
                m_GraphicsDeviceMgr.PreferredBackBufferHeight = preferredBackBufferWidth;
            }
            m_GraphicsDeviceMgr.SupportedOrientations = supportedOrientations;
#endif
#if WINDOWS_PHONE
            if (bSwapDims)
            {
                m_GraphicsDeviceMgr.PreferredBackBufferWidth = preferredBackBufferHeight;
                m_GraphicsDeviceMgr.PreferredBackBufferHeight = preferredBackBufferWidth;
            }
            else {
                if (onlyLandscape)
                {
                    m_GraphicsDeviceMgr.PreferredBackBufferWidth = 840;
                    m_GraphicsDeviceMgr.PreferredBackBufferHeight = 480;
                }
                else if (onlyPortrait)
                {
                    m_GraphicsDeviceMgr.PreferredBackBufferWidth = 480;
                    m_GraphicsDeviceMgr.PreferredBackBufferHeight = 840;
                }
            }
            m_GraphicsDeviceMgr.SupportedOrientations = supportedOrientations;
#endif
#if WINDOWS
            if (bSwapDims)
            {
                m_GraphicsDeviceMgr.PreferredBackBufferWidth = preferredBackBufferHeight;
                m_GraphicsDeviceMgr.PreferredBackBufferHeight = preferredBackBufferWidth;
            }
            else {
                if (onlyPortrait)
                {
                    m_GraphicsDeviceMgr.PreferredBackBufferWidth = 480;
                    m_GraphicsDeviceMgr.PreferredBackBufferHeight = 800;
                }
                else
                {
                    m_GraphicsDeviceMgr.PreferredBackBufferWidth = 800;
                    m_GraphicsDeviceMgr.PreferredBackBufferHeight = 480;
                }
            }
#endif
            m_GraphicsDeviceMgr.ApplyChanges();
        }

        public static void InitializeDisplay(Game game, GraphicsDeviceManager graphics, DisplayOrientation supportedOrientations)
        {
            m_GraphicsDeviceMgr = graphics;
            SetOrientation(supportedOrientations, false);

            //graphics.PreparingDeviceSettings += PreparingDeviceSettings;


#if ANDROID
            graphics.IsFullScreen = true;
#endif

#if WINDOWS_PHONE
            graphics.IsFullScreen = true;
#endif

#if WINDOWS
            game.IsMouseVisible = true;
            graphics.IsFullScreen = false;
#endif
        }

        public static CCPoint ScreenToWorld(float x, float y)
        {
            return new CCPoint(
                (x - m_obViewPortRect.MinX) / m_fScaleX,
                (y - m_obViewPortRect.MinY) / m_fScaleY
                );
        }

        #region Matrix

        private static Matrix m_pTransform = Matrix.Identity;

        public static void SetIdentityMatrix()
        {
            m_Matrix = Matrix.Identity;
            m_worldMatrixChanged = true;
        }

        public static void PushMatrix()
        {
            m_matrixStack[m_stackIndex++] = m_Matrix;
        }

        public static void PopMatrix()
        {
            m_Matrix = m_matrixStack[--m_stackIndex];
            m_worldMatrixChanged = true;
            Debug.Assert(m_stackIndex >= 0);
        }

        public static void Translate(float x, float y, int z)
        {
            m_TmpMatrix = Matrix.CreateTranslation(x, y, z);
            Matrix.Multiply(ref m_TmpMatrix, ref m_Matrix, out m_Matrix);
            m_worldMatrixChanged = true;
        }

        public static void MultMatrix(ref Matrix matrix)
        {
            Matrix.Multiply(ref matrix, ref m_Matrix, out m_Matrix);
            m_worldMatrixChanged = true;
        }

        //protected Matrix m_tCCNodeTransform;

        // | m[0] m[4] m[8]  m[12] |     | m11 m21 m31 m41 |     | a c 0 tx |
        // | m[1] m[5] m[9]  m[13] |     | m12 m22 m32 m42 |     | b d 0 ty |
        // | m[2] m[6] m[10] m[14] | <=> | m13 m23 m33 m43 | <=> | 0 0 1  0 |
        // | m[3] m[7] m[11] m[15] |     | m14 m24 m34 m44 |     | 0 0 0  1 |        
        public static void MultMatrix(CCAffineTransform transform, float z)
        {
            MultMatrix(ref transform, z);
        }

        public static void MultMatrix(ref CCAffineTransform transform, float z)
        {
            m_pTransform.M11 = transform.a;
            m_pTransform.M21 = transform.c;
            m_pTransform.M12 = transform.b;
            m_pTransform.M22 = transform.d;
            m_pTransform.M41 = transform.tx;
            m_pTransform.M42 = transform.ty;
            m_pTransform.M43 = z;

            Matrix.Multiply(ref m_pTransform, ref m_Matrix, out m_Matrix);

            m_worldMatrixChanged = true;
        }

        #endregion
    }

    public enum ResolutionPolicy
    {
        // The entire application is visible in the specified area without trying to preserve the original aspect ratio. 
        // Distortion can occur, and the application may appear stretched or compressed.
        ExactFit,
        // The entire application fills the specified area, without distortion but possibly with some cropping, 
        // while maintaining the original aspect ratio of the application.
        NoBorder,
        // The entire application is visible in the specified area without distortion while maintaining the original 
        // aspect ratio of the application. Borders can appear on two sides of the application.
        ShowAll,

        UnKnown,
    }
}