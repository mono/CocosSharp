using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    public class CCClippingNode : CCNode
    {
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

            if (CCDrawManager.BeginDrawMask(m_bInverted, m_fAlphaThreshold))
            {
                CCDrawManager.PushMatrix();
                Transform();
                
                m_pStencil.Visit();

                CCDrawManager.PopMatrix();

                CCDrawManager.EndDrawMask();

                base.Visit();

                CCDrawManager.EndMask();
            }
            else
            {
                base.Visit();
            }
        }
    }
 }