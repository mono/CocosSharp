using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    public enum CCClipMode
    {
        /// <summary>
        /// No clipping of children
        /// </summary>
        None,
        /// <summary>
        /// Clipping with a ScissorRect
        /// </summary>
        Bounds,
        /// <summary>
        /// Clipping with the ScissorRect and in a RenderTarget
        /// </summary>
        BoundsWithRenderTarget
    }

    public static class CCDrawManager
    {
        private const int DefaultQuadBufferSize = 1024 * 4;
        public static string DefaultFont = "arial";

        public static BasicEffect PrimitiveEffect;

        private static BasicEffect m_defaultEffect;
        private static Effect m_currentEffect;
        private static readonly Stack<Effect> m_effectStack = new Stack<Effect>();

        public static SpriteBatch spriteBatch;
        internal static GraphicsDevice graphicsDevice;

        internal static Matrix m_worldMatrix;
        internal static Matrix m_viewMatrix;
        internal static Matrix m_projectionMatrix;

        private static readonly Matrix[] m_matrixStack = new Matrix[100];
        private static int m_stackIndex;

        internal static Matrix m_Matrix;
        private static Matrix m_TmpMatrix;

        private static RenderTarget2D m_renderTarget = null;

        private static Texture2D m_currentTexture;
        private static bool m_textureEnabled;
        private static bool m_vertexColorEnabled;

        private static readonly Dictionary<CCBlendFunc, BlendState> m_blendStates = new Dictionary<CCBlendFunc, BlendState>();

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
        private static CCBlendFunc m_currBlend = CCBlendFunc.AlphaBlend;
        private static RenderTarget2D m_currRenderTarget;
        private static Viewport m_savedViewport;
        private static CCQuadVertexBuffer m_quadsBuffer;
        private static CCIndexBuffer<short> m_quadsIndexBuffer;

        public static int DrawCount;
        private static CCV3F_C4B_T2F[] m_quadVertices;
        private static float m_fScaleX;
        private static float m_fScaleY;
        private static CCRect m_obViewPortRect;
        private static CCSize m_obScreenSize;
        private static CCSize m_obDesignResolutionSize;
        private static CCResolutionPolicy m_eResolutionPolicy = CCResolutionPolicy.UnKnown;
        private static float m_fFrameZoomFactor = 1.0f;
        private static DepthFormat m_PlatformDepthFormat = DepthFormat.Depth24;
        // ref: http://www.khronos.org/registry/gles/extensions/NV/GL_NV_texture_npot_2D_mipmap.txt
        private static bool m_AllowNonPower2Textures = true;

        internal static CCRawList<CCV3F_C4B_T2F> _tmpVertices = new CCRawList<CCV3F_C4B_T2F>();

        private static bool m_bNeedReinitResources;

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

        public static bool TextureEnabled
        {
            get { return m_textureEnabled; }
            set
            {
                if (m_textureEnabled != value)
                {
                    m_textureEnabled = value;
                    m_textureChanged = true;
                }
            }
        }

        public static DepthStencilState DepthStencilState
        {
            get { return graphicsDevice.DepthStencilState; }
            set { graphicsDevice.DepthStencilState = value; }
        }

        public static GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        public static BlendState BlendState
        {
            get { return graphicsDevice.BlendState; }
            set
            {
                graphicsDevice.BlendState = value;
                m_currBlend.Source = -1;
                m_currBlend.Destination = -1;
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

        public static CCSize DesignResolutionSize
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
                // NOTE: This must be disabled when primitives are drawing, e.g. lines, polylines, etc.
                graphicsDevice.DepthStencilState = value ? m_DepthEnableStencilState : m_DepthDisableStencilState;
                //graphicsDevice.DepthStencilState = value ? DepthStencilState.Default : DepthStencilState.None;
            }
        }

        public static CCSize VisibleSize
        {
            get
            {
                if (m_eResolutionPolicy == CCResolutionPolicy.NoBorder)
                {
                    return new CCSize(m_obScreenSize.Width / m_fScaleX, m_obScreenSize.Height / m_fScaleY);
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
                if (m_eResolutionPolicy == CCResolutionPolicy.NoBorder)
                {
                    return new CCPoint((m_obDesignResolutionSize.Width - m_obScreenSize.Width / m_fScaleX) / 2,
                                       (m_obDesignResolutionSize.Height - m_obScreenSize.Height / m_fScaleY) / 2);
                }
                else
                {
                    return CCPoint.Zero;
                }
            }
        }

        private static List<RasterizerState> _rasterizerStatesCache = new List<RasterizerState>();

        private static RasterizerState GetScissorRasterizerState(bool scissorEnabled)
        {
            var currentState = graphicsDevice.RasterizerState;
            
            for (int i = 0; i < _rasterizerStatesCache.Count; i++)
            {
                var state = _rasterizerStatesCache[i];
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

            _rasterizerStatesCache.Add(newState);

            return newState;
        }

        public static bool ScissorRectEnabled
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

        private static IGraphicsDeviceService m_graphicsService;
        private static PresentationParameters m_presentationParameters = new PresentationParameters();
        
        public static void Init(IGraphicsDeviceService service)
        {
            m_graphicsService = service;

            m_presentationParameters = new PresentationParameters()
            {
                RenderTargetUsage = RenderTargetUsage.PreserveContents,
                DepthStencilFormat = DepthFormat.Depth24Stencil8,
                BackBufferFormat = SurfaceFormat.Color
            };

            service.DeviceCreated += GraphicsDeviceDeviceCreated;

            var manager = service as GraphicsDeviceManager;

            if (manager != null)
            {
                var pp = m_presentationParameters;

                pp.BackBufferWidth = manager.PreferredBackBufferWidth;
                pp.BackBufferHeight = manager.PreferredBackBufferHeight;
                pp.BackBufferFormat = manager.PreferredBackBufferFormat;
                pp.DepthStencilFormat = manager.PreferredDepthStencilFormat;
                pp.RenderTargetUsage = RenderTargetUsage.PreserveContents; //??? DiscardContents fast

                manager.PreparingDeviceSettings += GraphicsPreparingDeviceSettings;
            }
            else
            {
                if (service.GraphicsDevice != null)
                {
                    Init(service.GraphicsDevice);
                }
            }
        }

        /// <summary>
        /// Called just before the graphics device for the presentation is created. This method callback is used to setup
        /// the device settings. The WindowSetup is used to set the presentation parameters.
        /// </summary>
        static void GraphicsPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            var gdipp = e.GraphicsDeviceInformation.PresentationParameters;
            var pp = m_presentationParameters;

            gdipp.RenderTargetUsage = pp.RenderTargetUsage;
            gdipp.DepthStencilFormat = pp.DepthStencilFormat;
            gdipp.BackBufferFormat = pp.BackBufferFormat;
            
            //if (graphicsDevice == null)
            {
                // Only set the buffer dimensions when the device was not created
                gdipp.BackBufferWidth = pp.BackBufferWidth;
                gdipp.BackBufferHeight = pp.BackBufferHeight;
            }
        }

        static void GraphicsDeviceDeviceCreated(object sender, EventArgs e)
        {
            Init(m_graphicsService.GraphicsDevice);
        }

        public static void Init(GraphicsDevice graphicsDevice)
        {
            CCDrawManager.graphicsDevice = graphicsDevice;

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
#if !WINDOWS_PHONE && !XBOX && !WINDOWS &&!NETFX_CORE && !PSM
            List<string> extensions = CCUtils.GetGLExtensions();
            foreach(string s in extensions) 
            {
                switch(s) 
                {
                    case "GL_OES_depth24":
                        m_PlatformDepthFormat = DepthFormat.Depth24;
                        break;
                    case "GL_IMG_texture_npot":
                        m_AllowNonPower2Textures = true;
                        break;
                    case "GL_NV_depth_nonlinear": // nVidia Depth 16 non-linear
                        m_PlatformDepthFormat = DepthFormat.Depth16;
                        break;
                    case "GL_NV_texture_npot_2D_mipmap": // nVidia - nPot textures and mipmaps
                        m_AllowNonPower2Textures = true;
                        break;
                }
            }
#endif
            PresentationParameters pp = graphicsDevice.PresentationParameters;
            //pp.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            //m_renderTarget = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, (int)pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.PreserveContents);

            //m_eResolutionPolicy = CCResolutionPolicy.UnKnown;
            m_obViewPortRect = new CCRect(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
            m_obScreenSize = m_obViewPortRect.Size;

            if (m_eResolutionPolicy != CCResolutionPolicy.UnKnown)
            {
                SetDesignResolutionSize(m_obDesignResolutionSize.Width, m_obDesignResolutionSize.Height, m_eResolutionPolicy);
            }
            else
            {
                m_fScaleY = 1.0f;
                m_fScaleX = 1.0f;

                m_obDesignResolutionSize = m_obScreenSize;
            }

            m_projectionMatrix = Matrix.Identity;
            m_viewMatrix = Matrix.Identity;
            m_worldMatrix = Matrix.Identity;
            m_Matrix = Matrix.Identity;

            m_worldMatrixChanged = m_viewMatrixChanged = m_projectionMatrixChanged = true;

            CCDrawingPrimitives.Init(graphicsDevice);

            graphicsDevice.Disposing += GraphicsDeviceDisposing;
            graphicsDevice.DeviceLost += GraphicsDeviceDeviceLost;
            graphicsDevice.DeviceReset += GraphicsDeviceDeviceReset;
            graphicsDevice.DeviceResetting += GraphicsDeviceDeviceResetting;
            graphicsDevice.ResourceCreated += GraphicsDeviceResourceCreated;
            graphicsDevice.ResourceDestroyed += GraphicsDeviceResourceDestroyed;
        }

        public static void PurgeDrawManager()
        {
            graphicsDevice = null;

            PrimitiveEffect = null;

            m_defaultEffect = null;
            m_currentEffect = null;
            m_effectStack.Clear();

            spriteBatch = null;

            m_renderTarget = null;

            m_currentTexture = null;

            m_blendStates.Clear();

            m_DepthEnableStencilState = null;
            m_DepthDisableStencilState = null;

            m_currRenderTarget = null;
            m_quadsBuffer = null;
            m_quadsIndexBuffer = null;

            m_quadVertices = null;
            _tmpVertices.Clear();
        }
        
        static void GraphicsDeviceResourceDestroyed(object sender, ResourceDestroyedEventArgs e)
        {
        }

        static void GraphicsDeviceResourceCreated(object sender, ResourceCreatedEventArgs e)
        {
        }

        static void GraphicsDeviceDeviceResetting(object sender, EventArgs e)
        {
//#if ANDROID
            CCGraphicsResource.DisposeAllResources();
            CCSpriteFontCache.SharedInstance.Clear();
#if XNA
            CCContentManager.SharedContentManager.ReloadGraphicsAssets();
#endif
            m_bNeedReinitResources = true;
//#endif
        }

        static void GraphicsDeviceDeviceReset(object sender, EventArgs e)
        {
//#if ANDROID
        //m_bNeedReinitResources = true;
//#endif
        }

        static void GraphicsDeviceDeviceLost(object sender, EventArgs e)
        {
        }

        static void GraphicsDeviceDisposing(object sender, EventArgs e)
        {
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
                m_obScreenSize = m_obViewPortRect.Size;

                if (m_eResolutionPolicy != CCResolutionPolicy.UnKnown)
                {
                    SetDesignResolutionSize(m_obDesignResolutionSize.Width, m_obDesignResolutionSize.Height, m_eResolutionPolicy);
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
            if (graphicsDevice == null || graphicsDevice.IsDisposed)
            {
                // We are existing the game
                return;
            }

            if (m_bNeedReinitResources)
            {
                CCGraphicsResource.ReinitAllResources();
                m_bNeedReinitResources = false;
            }

            ResetDevice();
            Clear(Color.Black);
            DrawCount = 0;
        }

        public static void EndDraw()
        {
            if (graphicsDevice == null || graphicsDevice.IsDisposed)
            {
                // We are existing the game
                return;
            }

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
                var effect = (BasicEffect)m_currentEffect;

                effect.TextureEnabled = m_textureEnabled;
                effect.VertexColorEnabled = m_vertexColorEnabled;
                effect.Texture = m_currentTexture;
            }
            else if (m_currentEffect is AlphaTestEffect)
            {
                var effect = (AlphaTestEffect)m_currentEffect;
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


        public static void BlendFunc(CCBlendFunc blendFunc)
        {

            // It looks like the blend state is being reset somewhere so this check of not setting
            // the blend states is causing multiple problems.  Took this check out and setting the 
            // blend state seems the correct modification for now.
            //if (m_currBlend.Destination != blendFunc.Destination || m_currBlend.Source != blendFunc.Source)
            //{
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
                if (!m_blendStates.TryGetValue(blendFunc, out bs))
                {
                    bs = new BlendState();

                    bs.ColorSourceBlend = CCOGLES.GetXNABlend(blendFunc.Source);
                    bs.AlphaSourceBlend = CCOGLES.GetXNABlend(blendFunc.Source);
                    bs.ColorDestinationBlend = CCOGLES.GetXNABlend(blendFunc.Destination);
                    bs.AlphaDestinationBlend = CCOGLES.GetXNABlend(blendFunc.Destination);

                    m_blendStates.Add(blendFunc, bs);
                }
            }

            graphicsDevice.BlendState = bs;

            m_currBlend.Source = blendFunc.Source;
            m_currBlend.Destination = blendFunc.Destination;

            //}
        }

        public static void BindTexture(CCTexture2D texture)
        {
            Texture2D tex = texture == null ? null : texture.XNATexture;

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
            var texture = CreateRenderTarget((int)size.Width, (int)size.Height, CCTexture2D.DefaultAlphaPixelFormat,
                                             m_PlatformDepthFormat, usage);
            pTexture.InitWithTexture(texture, CCTexture2D.DefaultAlphaPixelFormat, true, false);
        }

        public static RenderTarget2D CreateRenderTarget(int width, int height, RenderTargetUsage usage)
        {
            return CreateRenderTarget(width, height, CCTexture2D.DefaultAlphaPixelFormat, DepthFormat.None, usage);
        }

        public static RenderTarget2D CreateRenderTarget(int width, int height, SurfaceFormat colorFormat, RenderTargetUsage usage)
        {
            return CreateRenderTarget(width, height, colorFormat, DepthFormat.None, usage);
        }

        public static RenderTarget2D CreateRenderTarget(int width, int height, SurfaceFormat colorFormat, DepthFormat depthFormat,
                                                        RenderTargetUsage usage)
        {
            if (!m_AllowNonPower2Textures)
            {
                width = CCUtils.CCNextPOT(width);
                height = CCUtils.CCNextPOT(height);
            }
            return new RenderTarget2D(graphicsDevice, width, height, false, colorFormat, depthFormat, 0, usage);
        }

        public static Texture2D CreateTexture2D(int width, int height)
        {
            PresentationParameters pp = graphicsDevice.PresentationParameters;
            if (!m_AllowNonPower2Textures)
            {
                width = CCUtils.CCNextPOT(width);
                height = CCUtils.CCNextPOT(height);
            }
            return new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
        }

        public static void SetRenderTarget(CCTexture2D pTexture)
        {
            if (pTexture == null)
            {
                SetRenderTarget((RenderTarget2D)null);
            }
            else
            {
                Debug.Assert(pTexture.XNATexture is RenderTarget2D);
                SetRenderTarget((RenderTarget2D)pTexture.XNATexture);
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
            if (m_quadsIndexBuffer == null || m_quadsIndexBuffer.Capacity < capacity * 6)
            {
                capacity = Math.Max(capacity, DefaultQuadBufferSize);

                if (m_quadsIndexBuffer == null)
                {
                    m_quadsIndexBuffer = new CCIndexBuffer<short>(capacity * 6, BufferUsage.WriteOnly);
                    m_quadsIndexBuffer.Count = m_quadsIndexBuffer.Capacity;
                }

                if (m_quadsIndexBuffer.Capacity < capacity * 6)
                {
                    m_quadsIndexBuffer.Capacity = capacity * 6;
                    m_quadsIndexBuffer.Count = m_quadsIndexBuffer.Capacity;
                }

                var indices = m_quadsIndexBuffer.Data.Elements;

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

                m_quadsIndexBuffer.UpdateBuffer();
            }
        }

        private static void CheckQuadsVertexBuffer(int capacity)
        {
            if (m_quadsBuffer == null || m_quadsBuffer.Capacity < capacity)
            {
                capacity = Math.Max(capacity, DefaultQuadBufferSize);

                if (m_quadsBuffer == null)
                {
                    m_quadsBuffer = new CCQuadVertexBuffer(capacity, BufferUsage.WriteOnly);
                }
                else
                {
                    m_quadsBuffer.Capacity = capacity;
                }
            }
        }

        public static void DrawQuad(ref CCV3F_C4B_T2F_Quad quad)
        {
            CCV3F_C4B_T2F[] vertices = m_quadVertices;

            if (vertices == null)
            {
                vertices = m_quadVertices = new CCV3F_C4B_T2F[4];
                CheckQuadsIndexBuffer(1);
            }

            vertices[0] = quad.TopLeft;
            vertices[1] = quad.BottomLeft;
            vertices[2] = quad.TopRight;
            vertices[3] = quad.BottomRight;

            DrawIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, m_quadsIndexBuffer.Data.Elements, 0, 2);
        }

        public static void DrawQuads(CCRawList<CCV3F_C4B_T2F_Quad> quads, int start, int n)
        {
            if (n == 0)
            {
                return;
            }

            CheckQuadsIndexBuffer(start + n);
            CheckQuadsVertexBuffer(start + n);

            m_quadsBuffer.UpdateBuffer(quads, start, n);

            graphicsDevice.SetVertexBuffer(m_quadsBuffer.VertexBuffer);
            graphicsDevice.Indices = m_quadsIndexBuffer.IndexBuffer;

            ApplyEffectParams();

            EffectPassCollection passes = m_currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, n * 4, start * 6, n * 2);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;

            DrawCount++;
        }

        public static void DrawBuffer<T, T2>(CCVertexBuffer<T> vertexBuffer, CCIndexBuffer<T2> indexBuffer, int start, int count)
            where T : struct, IVertexType
            where T2 : struct
        {
            graphicsDevice.Indices = indexBuffer.IndexBuffer;
            graphicsDevice.SetVertexBuffer(vertexBuffer.VertexBuffer);

            ApplyEffectParams();

            EffectPassCollection passes = m_currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexBuffer.VertexCount, start, count);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;

            DrawCount++;
        }

        public static void DrawQuadsBuffer<T>(CCVertexBuffer<T> vertexBuffer, int start, int n) where T : struct, IVertexType
        {
            if (n == 0)
            {
                return;
            }

            CheckQuadsIndexBuffer(start + n);

            graphicsDevice.Indices = m_quadsIndexBuffer.IndexBuffer;
            graphicsDevice.SetVertexBuffer(vertexBuffer.VertexBuffer);

            ApplyEffectParams();

            EffectPassCollection passes = m_currentEffect.CurrentTechnique.Passes;
            for (int i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexBuffer.VertexCount, start * 6, n * 2);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;

            DrawCount++;
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
                (int)(x * m_fScaleX * m_fFrameZoomFactor + m_obViewPortRect.Origin.X * m_fFrameZoomFactor),
                (int)(y * m_fScaleY * m_fFrameZoomFactor + m_obViewPortRect.Origin.Y * m_fFrameZoomFactor),
                (int)(width * m_fScaleX * m_fFrameZoomFactor),
                (int)(height * m_fScaleY * m_fFrameZoomFactor)
                );
        }

        public static void SetScissorInPoints(float x, float y, float w, float h)
        {
            y = CCDirector.SharedDirector.WinSize.Height - y - h;

            graphicsDevice.ScissorRectangle = new Rectangle(
                (int)(x * m_fScaleX + m_obViewPortRect.Origin.X),
                (int)(y * m_fScaleY + m_obViewPortRect.Origin.Y),
                (int)(w * m_fScaleX),
                (int)(h * m_fScaleY)
                );
        }

        public static CCRect ScissorRect
        {
            get
            {
                var sr = graphicsDevice.ScissorRectangle;

                float x = (sr.X - m_obViewPortRect.Origin.X) / m_fScaleX;
                float y = (sr.Y - m_obViewPortRect.Origin.Y) / m_fScaleY;
                float w = sr.Width / m_fScaleX;
                float h = sr.Height / m_fScaleY;

                y = CCDirector.SharedDirector.WinSize.Height - y - h;

                return new CCRect(x, y, w, h);
            }
        }

        public static void SetDesignResolutionSize(float width, float height, CCResolutionPolicy resolutionPolicy)
        {
            Debug.Assert(resolutionPolicy != CCResolutionPolicy.UnKnown, "should set resolutionPolicy");

            if (width == 0.0f || height == 0.0f)
            {
                return;
            }

            m_obDesignResolutionSize.Width = width;
            m_obDesignResolutionSize.Height = height;

            m_fScaleX = m_obScreenSize.Width / m_obDesignResolutionSize.Width;
            m_fScaleY = m_obScreenSize.Height / m_obDesignResolutionSize.Height;

            if (resolutionPolicy == CCResolutionPolicy.NoBorder)
            {
                m_fScaleX = m_fScaleY = Math.Max(m_fScaleX, m_fScaleY);
            }

            if (resolutionPolicy == CCResolutionPolicy.ShowAll)
            {
                m_fScaleX = m_fScaleY = Math.Min(m_fScaleX, m_fScaleY);
            }


            if (resolutionPolicy == CCResolutionPolicy.FixedHeight)
            {
                m_fScaleX = m_fScaleY;
                m_obDesignResolutionSize.Width = (float)Math.Ceiling(m_obScreenSize.Width / m_fScaleX);
            }

            if (resolutionPolicy == CCResolutionPolicy.FixedWidth)
            {
                m_fScaleY = m_fScaleX;
                m_obDesignResolutionSize.Height = (float)Math.Ceiling(m_obScreenSize.Height / m_fScaleY);
            }

            // calculate the rect of viewport    
            float viewPortW = m_obDesignResolutionSize.Width * m_fScaleX;
            float viewPortH = m_obDesignResolutionSize.Height * m_fScaleY;

            m_obViewPortRect = new CCRect((m_obScreenSize.Width - viewPortW) / 2, (m_obScreenSize.Height - viewPortH) / 2, viewPortW, viewPortH);

            m_eResolutionPolicy = resolutionPolicy;

            // reset director's member variables to fit visible rect
            CCDirector.SharedDirector.m_obWinSizeInPoints = DesignResolutionSize;

            CCDirector.SharedDirector.CreateStatsLabel();
            CCDirector.SharedDirector.SetGlDefaultValues();
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
#if WINDOWS || WINDOWSGL || WINDOWS_PHONE
            bool bSwapDims = bUpdateDimensions && ((m_GraphicsDeviceMgr.SupportedOrientations & supportedOrientations) == DisplayOrientation.Default);
#else
            bool bSwapDims = bUpdateDimensions && ((m_GraphicsDeviceMgr.SupportedOrientations & supportedOrientations) == 0);
#endif
            if (bSwapDims && (ll || lr))
            {
                // Check for landscape changes that do not need a swap
#if WINDOWS || WINDOWSGL || WINDOWS_PHONE
                if (((m_GraphicsDeviceMgr.SupportedOrientations & DisplayOrientation.LandscapeLeft) != DisplayOrientation.Default) ||
                    ((m_GraphicsDeviceMgr.SupportedOrientations & DisplayOrientation.LandscapeRight) != DisplayOrientation.Default))
#else
                if (((m_GraphicsDeviceMgr.SupportedOrientations & DisplayOrientation.LandscapeLeft) != 0) ||
                    ((m_GraphicsDeviceMgr.SupportedOrientations & DisplayOrientation.LandscapeRight) != 0))
#endif
                {
                    bSwapDims = false;
                }
            }
            int preferredBackBufferWidth = m_GraphicsDeviceMgr.PreferredBackBufferWidth;
            int preferredBackBufferHeight = m_GraphicsDeviceMgr.PreferredBackBufferHeight;
            if (bSwapDims)
            {
                CCSize newSize = m_obDesignResolutionSize.Inverted;
                CCDrawManager.SetDesignResolutionSize(newSize.Width, newSize.Height, m_eResolutionPolicy);
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
            preferredBackBufferWidth = m_GraphicsDeviceMgr.PreferredBackBufferWidth;
            preferredBackBufferHeight = m_GraphicsDeviceMgr.PreferredBackBufferHeight;
#if ANDROID
            if (onlyLandscape && m_GraphicsDeviceMgr.PreferredBackBufferHeight > m_GraphicsDeviceMgr.PreferredBackBufferWidth)
            {
                m_GraphicsDeviceMgr.PreferredBackBufferWidth = preferredBackBufferHeight;
                m_GraphicsDeviceMgr.PreferredBackBufferHeight = preferredBackBufferWidth;
            }
            m_GraphicsDeviceMgr.SupportedOrientations = supportedOrientations;
#endif
#if WINDOWS || WINDOWS || WINDOWS_PHONE
            if (bSwapDims)
            {
                m_GraphicsDeviceMgr.PreferredBackBufferWidth = preferredBackBufferHeight;
                m_GraphicsDeviceMgr.PreferredBackBufferHeight = preferredBackBufferWidth;
            }
            else
            {
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
#if IOS || IPHONE
            if (bSwapDims)
            {
                m_GraphicsDeviceMgr.PreferredBackBufferWidth = preferredBackBufferHeight;
                m_GraphicsDeviceMgr.PreferredBackBufferHeight = preferredBackBufferWidth;
            }
            else if (onlyLandscape && m_GraphicsDeviceMgr.PreferredBackBufferHeight > m_GraphicsDeviceMgr.PreferredBackBufferWidth)
            {
                m_GraphicsDeviceMgr.PreferredBackBufferWidth = preferredBackBufferHeight;
                m_GraphicsDeviceMgr.PreferredBackBufferHeight = preferredBackBufferWidth;
            }
            m_GraphicsDeviceMgr.SupportedOrientations = supportedOrientations;
#endif
#if WINDOWS || WINDOWSGL
            if (bSwapDims)
            {
                m_GraphicsDeviceMgr.PreferredBackBufferWidth = preferredBackBufferHeight;
                m_GraphicsDeviceMgr.PreferredBackBufferHeight = preferredBackBufferWidth;
            }
            else
            {
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

#if WINDOWS || WINDOWSGL || WINDOWS_PHONE
            graphics.IsFullScreen = true;
#endif

#if WINDOWS || WINDOWSGL || MACOS
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

        #region Mask

        private struct MaskState
        {
            public int Layer;
            public bool Inverted;
            public float AlphaTreshold;
        }

        private struct MaskDepthStencilStateCacheEntry
        {
            public DepthStencilState Clear;
            public DepthStencilState ClearInvert;
            public DepthStencilState DrawMask;
            public DepthStencilState DrawMaskInvert;
            public DepthStencilState DrawContent;
            public DepthStencilState DrawContentDepth;

            public DepthStencilState GetClearState(MaskState maskState)
            {
                DepthStencilState result = maskState.Inverted ? ClearInvert : Clear;

                if (result == null)
                {
                    int maskLayer = 1 << maskState.Layer;

                    result = new DepthStencilState()
                    {
                        DepthBufferEnable = false,

                        StencilEnable = true,

                        StencilFunction = CompareFunction.Never,

                        StencilMask = maskLayer,
                        StencilWriteMask = maskLayer,
                        ReferenceStencil = maskLayer,

                        StencilFail = !maskState.Inverted ? StencilOperation.Zero : StencilOperation.Replace
                    };

                    if (maskState.Inverted)
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

            public DepthStencilState GetDrawMaskState(MaskState maskState)
            {
                DepthStencilState result = maskState.Inverted ? DrawMaskInvert : DrawMask;

                if (result == null)
                {
                    int maskLayer = 1 << maskState.Layer;

                    result = new DepthStencilState()
                    {
                        DepthBufferEnable = false,

                        StencilEnable = true,

                        StencilFunction = CompareFunction.Never,

                        StencilMask = maskLayer,
                        StencilWriteMask = maskLayer,
                        ReferenceStencil = maskLayer,

                        StencilFail = !maskState.Inverted ? StencilOperation.Replace : StencilOperation.Zero,
                    };

                    if (maskState.Inverted)
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

            public DepthStencilState GetDrawContentState(MaskState maskState, bool depth)
            {
                DepthStencilState result = depth ? DrawContentDepth : DrawContent;

                if (result == null)
                {
                    int maskLayer = 1 << maskState.Layer;
                    int maskLayerL = maskLayer - 1;
                    int maskLayerLe = maskLayer | maskLayerL;

                    result = new DepthStencilState()
                    {
                        DepthBufferEnable = _maskSavedStencilStates[_maskLayer].DepthBufferEnable,

                        StencilEnable = true,

                        StencilMask = maskLayerLe,
                        StencilWriteMask = 0,
                        ReferenceStencil = maskLayerLe,

                        StencilFunction = CompareFunction.Equal,

                        StencilPass = StencilOperation.Keep,
                        StencilFail = StencilOperation.Keep,
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

        private static int _maskLayer = -1;
        private static bool _maskOnceLog = false;
        private static DepthStencilState[] _maskSavedStencilStates = new DepthStencilState[8];
        private static AlphaTestEffect _maskAlphaTest;
        private static MaskState[] _maskStates = new MaskState[8];
        private static MaskDepthStencilStateCacheEntry[] _maskStatesCache = new MaskDepthStencilStateCacheEntry[8];

        public static bool BeginDrawMask(bool inverted, float alphaTreshold)
        {
            if (_maskLayer + 1 == 8) //DepthFormat.Depth24Stencil8
            {
                if (_maskOnceLog)
                {
                    CCLog.Log(
                        @"Nesting more than 8 stencils is not supported. 
                        Everything will be drawn without stencil for this node and its childs."
                        );
                    _maskOnceLog = false;
                }
                return false;
            }

            _maskLayer++;

            var maskState = new MaskState() { Layer = _maskLayer, Inverted = inverted, AlphaTreshold = alphaTreshold };

            _maskStates[_maskLayer] = maskState;
            _maskSavedStencilStates[_maskLayer] = DepthStencilState;

            int maskLayer = 1 << _maskLayer;

            ///////////////////////////////////
            // CLEAR STENCIL BUFFER

            DepthStencilState = _maskStatesCache[_maskLayer].GetClearState(maskState);

            // draw a fullscreen solid rectangle to clear the stencil buffer
            var size = CCDirector.SharedDirector.WinSize;

            PushMatrix();
            SetIdentityMatrix();

            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawSolidRect(CCPoint.Zero, new CCPoint(size.Width, size.Height), new CCColor4B(255, 255, 255, 255));
            CCDrawingPrimitives.End();

            PopMatrix();

            ///////////////////////////////////
            // PREPARE TO DRAW MASK

            DepthStencilState = _maskStatesCache[_maskLayer].GetDrawMaskState(maskState);

            if (maskState.AlphaTreshold < 1)
            {
                if (_maskAlphaTest == null)
                {
                    _maskAlphaTest = new AlphaTestEffect(GraphicsDevice);
                    _maskAlphaTest.AlphaFunction = CompareFunction.Greater;
                }

                _maskAlphaTest.ReferenceAlpha = (byte)(255 * maskState.AlphaTreshold);

                PushEffect(_maskAlphaTest);
            }

            return true;
        }

        public static void EndDrawMask()
        {
            var maskState = _maskStates[_maskLayer];

            ///////////////////////////////////
            // PREPARE TO DRAW MASKED CONTENT

            if (maskState.AlphaTreshold < 1)
            {
                PopEffect();
            }

            DepthStencilState = _maskStatesCache[_maskLayer].GetDrawContentState(maskState, _maskSavedStencilStates[_maskLayer].DepthBufferEnable);
        }

        public static void EndMask()
        {
            DepthStencilState = _maskSavedStencilStates[_maskLayer];

            _maskLayer--;
        }
        
        #endregion
    }

    public enum CCResolutionPolicy
    {
        UnKnown,

        // The entire application is visible in the specified area without trying to preserve the original aspect ratio. 
        // Distortion can occur, and the application may appear stretched or compressed.
        ExactFit,
        // The entire application fills the specified area, without distortion but possibly with some cropping, 
        // while maintaining the original aspect ratio of the application.
        NoBorder,
        // The entire application is visible in the specified area without distortion while maintaining the original 
        // aspect ratio of the application. Borders can appear on two sides of the application.
        ShowAll,
        // The application takes the height of the design resolution size and modifies the width of the internal
        // canvas so that it fits the aspect ratio of the device
        // no distortion will occur however you must make sure your application works on different
        // aspect ratios
        FixedHeight,
        // The application takes the width of the design resolution size and modifies the height of the internal
        // canvas so that it fits the aspect ratio of the device
        // no distortion will occur however you must make sure your application works on different
        // aspect ratios
        FixedWidth
    }


    public class CCGraphicsResource : IDisposable
    {
        private static CCRawList<WeakReference> _createdResources = new CCRawList<WeakReference>();

        private bool _isDisposed;

        private WeakReference _wr;

        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        public CCGraphicsResource()
        {
            _wr = new WeakReference(this);

            lock (_createdResources)
            {
                _createdResources.Add(_wr);
            }
        }

        ~CCGraphicsResource()
        {
            if (!IsDisposed)
            {
                Dispose();
            }

            lock (_createdResources)
            {
                _createdResources.Remove(_wr);
            }
        }

        public virtual void Dispose()
        {
            _isDisposed = true;
        }

        public virtual void Reinit()
        {
        }

        internal static void ReinitAllResources()
        {
            lock (_createdResources)
            {
                var resources = _createdResources.Elements;
                for (int i = 0, count = _createdResources.Count; i < count; i++)
                {
                    if (resources[i].IsAlive)
                    {
                        ((CCGraphicsResource) resources[i].Target).Reinit();
                    }
                }
            }
        }

        internal static void DisposeAllResources()
        {
            lock (_createdResources)
            {
                var resources = _createdResources.Elements;
                for (int i = 0, count = _createdResources.Count; i < count; i++)
                {
                    if (resources[i].IsAlive)
                    {
                        ((CCGraphicsResource) resources[i].Target).Dispose();
                    }
                }
            }
        }
    }


    public class CCVertexBuffer<T> : CCGraphicsResource where T : struct, IVertexType
    {
        protected VertexBuffer _vertexBuffer;
        protected BufferUsage _usage;
        protected CCRawList<T> _data;

        internal VertexBuffer VertexBuffer
        {
            get { return _vertexBuffer; }
        }

        public CCRawList<T> Data
        {
            get { return _data; }
        }

        public int Count
        {
            get { return _data.Count; }
            set
            {
                Debug.Assert(value <= _data.Capacity);
                _data.count = value;
            }
        }

        public int Capacity
        {
            get { return _data.Capacity; }
            set
            {
                if (_data.Capacity != value)
                {
                    _data.Capacity = value;
                    Reinit();
                }
            }
        }

        public CCVertexBuffer(int vertexCount, BufferUsage usage)
        {
            _data = new CCRawList<T>(vertexCount);
            _usage = usage;
            Reinit();
        }
        
        public void UpdateBuffer()
        {
            UpdateBuffer(0, _data.Count);
        }

        public virtual void UpdateBuffer(int startIndex, int elementCount)
        {
            if (elementCount > 0)
            {
                _vertexBuffer.SetData(_data.Elements, startIndex, elementCount);
            }
        }

        public override void Reinit()
        {
            if (_vertexBuffer != null && !_vertexBuffer.IsDisposed)
            {
                _vertexBuffer.Dispose();
            }
            _vertexBuffer = new VertexBuffer(CCDrawManager.GraphicsDevice, typeof(T), _data.Capacity, _usage);
        }
    }

    public class CCQuadVertexBuffer : CCVertexBuffer<CCV3F_C4B_T2F_Quad>
    {
        public CCQuadVertexBuffer(int vertexCount, BufferUsage usage) : base(vertexCount, usage)
        {
        }

        public void UpdateBuffer(CCRawList<CCV3F_C4B_T2F_Quad> data, int startIndex, int elementCount)
        {
            //TODO: 
            var tmp = _data;
            _data = data;
            
            UpdateBuffer(startIndex, elementCount);

            _data = tmp;
        }

        public override void UpdateBuffer(int startIndex, int elementCount)
        {
            if (elementCount == 0)
            {
                return;
            }

            var quads = _data.Elements;

            var tmp = CCDrawManager._tmpVertices;

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

            _vertexBuffer.SetData(vertices, startIndex * 4, elementCount * 4);
        }

        public override void Reinit()
        {
            if (_vertexBuffer != null && !_vertexBuffer.IsDisposed)
            {
                _vertexBuffer.Dispose();
            }
            _vertexBuffer = new VertexBuffer(CCDrawManager.GraphicsDevice, typeof(CCV3F_C4B_T2F), _data.Capacity * 4, _usage);

            UpdateBuffer();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            if (_vertexBuffer != null && !_vertexBuffer.IsDisposed)
            {
                _vertexBuffer.Dispose();
            }
            
            _vertexBuffer = null;
        }
    }

    public class CCIndexBuffer<T> : CCGraphicsResource where T : struct
    {
        private IndexBuffer _indexBuffer;
        private BufferUsage _usage;
        private CCRawList<T> _data;

        internal IndexBuffer IndexBuffer
        {
            get { return _indexBuffer; }
        }

        public CCRawList<T> Data
        {
            get { return _data; }
        }

        public int Count
        {
            get { return _data.Count; }
            set
            {
                Debug.Assert(value <= _data.Capacity);
                _data.count = value;
            }
        }

        public int Capacity
        {
            get { return _data.Capacity; }
            set
            {
                if (_data.Capacity != value)
                {
                    _data.Capacity = value;
                    Reinit();
                }
            }
        }

        public CCIndexBuffer(int indexCount, BufferUsage usage)
        {
            _data = new CCRawList<T>(indexCount);
            _usage = usage;
            Reinit();
        }

        public override void Reinit()
        {
            if (_indexBuffer != null && !_indexBuffer.IsDisposed)
            {
                _indexBuffer.Dispose();
            }

            _indexBuffer = new IndexBuffer(CCDrawManager.GraphicsDevice, typeof(T), _data.Capacity, _usage);

            UpdateBuffer();
        }

        public void UpdateBuffer()
        {
            UpdateBuffer(0, _data.Count);
        }

        public void UpdateBuffer(int startIndex, int elementCount)
        {
            if (elementCount > 0)
            {
                _indexBuffer.SetData(_data.Elements, startIndex, elementCount);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_indexBuffer != null && !_indexBuffer.IsDisposed)
            {
                _indexBuffer.Dispose();
            }

            _indexBuffer = null;
        }
    }
}