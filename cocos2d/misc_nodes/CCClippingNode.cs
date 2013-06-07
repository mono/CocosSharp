using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    public class CCClippingNode : CCNode
    {
        private static AlphaTestEffect _alphaTest;
        private static CCDrawNode _clearNode;
        
        private static int _layer = -1;

        protected CCNode m_pStencil;
        protected float m_fAlphaThreshold;
        protected bool m_bInverted;

        public bool Inverted
        {
            get { return m_bInverted; }
            set { m_bInverted = value; }
        }

        public CCNode Stencil
        {
            get { return m_pStencil; }
            set { m_pStencil = value; }
        }

        public float AlphaThreshold
        {
            get { return m_fAlphaThreshold; }
            set { m_fAlphaThreshold = value; }
        }

        public CCClippingNode()
        {
            Init(null);
        }

        public CCClippingNode(CCNode stencil)
        {
            Init(stencil);
        }

        public override bool Init()
        {
            return Init(null);
        }

        public bool Init(CCNode stencil)
        {
            m_pStencil = stencil;
            m_fAlphaThreshold = 1;
            m_bInverted = false;

            return true;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (m_pStencil != null)
            {
                m_pStencil.OnEnter();
            }
        }

        public override void OnExit()
        {
            if (m_pStencil != null)
            {
                m_pStencil.OnExit();
            }
            base.OnExit();
        }

        public override void OnExitTransitionDidStart()
        {
            if (m_pStencil != null)
            {
                m_pStencil.OnExitTransitionDidStart();
            }
            base.OnExitTransitionDidStart();
        }

        public override void OnEnterTransitionDidFinish()
        {
            base.OnEnterTransitionDidFinish();
            if (m_pStencil != null)
            {
                m_pStencil.OnEnterTransitionDidFinish();
            }
        }

        private static bool _once = true;
        
        public override void Visit()
        {
            if (m_pStencil == null || !m_pStencil.Visible)
            {
                if (m_bInverted)
                {
                    // draw everything
                    base.Visit();
                }
                return;
            }

            if (_layer + 1 == 8) //DepthFormat.Depth24Stencil8
            {
                if (_once)
                {
                    CCLog.Log(
                        "Nesting more than 8 stencils is not supported. Everything will be drawn without stencil for this node and its childs."
                        );
                    _once = false;
                }
                base.Visit();
                return;
            }

            _layer++;

            int maskLayer = 1 << _layer;
            int maskLayerL = maskLayer - 1;
            int maskLayerLe = maskLayer | maskLayerL;

            var saveDepthStencilState = CCDrawManager.DepthStencilState;

            ///////////////////////////////////
            // CLEAR STENCIL BUFFER

            var stencilState = new DepthStencilState()
                {
                    DepthBufferEnable = false,

                    StencilEnable = true,
                    
                    StencilFunction = CompareFunction.Never,

                    StencilMask = maskLayer,
                    StencilWriteMask = maskLayer,
                    ReferenceStencil = maskLayer,

                    StencilFail = !m_bInverted ? StencilOperation.Zero : StencilOperation.Replace
                };
            CCDrawManager.DepthStencilState = stencilState;

            // draw a fullscreen solid rectangle to clear the stencil buffer
            var size = CCDirector.SharedDirector.WinSize;

            CCDrawManager.PushMatrix();
            CCDrawManager.SetIdentityMatrix();

            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawSolidRect(CCPoint.Zero, new CCPoint(size.Width, size.Height), new CCColor4B(255, 255, 255, 255));
            CCDrawingPrimitives.End();

            CCDrawManager.PopMatrix();
            
            ///////////////////////////////////
            // DRAW CLIPPING STENCIL

            stencilState = new DepthStencilState()
                {
                    DepthBufferEnable = false,

                    StencilEnable = true,

                    StencilFunction = CompareFunction.Never,

                    StencilMask = maskLayer,
                    StencilWriteMask = maskLayer,
                    ReferenceStencil = maskLayer,

                    StencilFail = !m_bInverted ? StencilOperation.Replace : StencilOperation.Zero,
                };
            CCDrawManager.DepthStencilState = stencilState;
            
            if (m_fAlphaThreshold < 1)
            {
                if (_alphaTest == null)
                {
                    _alphaTest = new AlphaTestEffect(CCDrawManager.GraphicsDevice);
                    _alphaTest.AlphaFunction = CompareFunction.Greater;
                }

                _alphaTest.ReferenceAlpha = (byte)(255 * m_fAlphaThreshold);

                CCDrawManager.PushEffect(_alphaTest);
            }

            CCDrawManager.PushMatrix();
            Transform();
            m_pStencil.Visit();
            CCDrawManager.PopMatrix();

            if (m_fAlphaThreshold < 1)
            {
                CCDrawManager.PopEffect();
            }

            ///////////////////////////////////
            // DRAW CONTENT

            stencilState = new DepthStencilState()
            {
                DepthBufferEnable = saveDepthStencilState.DepthBufferEnable,

                StencilEnable = true,
                
                StencilMask = maskLayerLe,
                StencilWriteMask = 0,
                ReferenceStencil = maskLayerLe,

                StencilFunction = CompareFunction.Equal,

                StencilPass = StencilOperation.Keep,
                StencilFail = StencilOperation.Keep,
            };
            CCDrawManager.DepthStencilState = stencilState;
            
            
            base.Visit();

            //Restore DepthStencilState
            CCDrawManager.DepthStencilState = saveDepthStencilState;

            _layer--;
        }
    }
 }