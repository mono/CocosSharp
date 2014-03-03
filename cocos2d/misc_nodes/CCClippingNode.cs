using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCClippingNode : CCNode
    {
        public bool Inverted { get; set; }
        public CCNode Stencil { get; set; }
        public float AlphaThreshold { get; set; }



        #region Constructors

        public CCClippingNode() : this(null)
        {
        }

        public CCClippingNode(CCNode stencil)
        {
            Stencil = stencil;
            AlphaThreshold = 1;
            Inverted = false;
        }

        #endregion Constructors


        public override void OnEnter()
        {
            base.OnEnter();

            if (Stencil != null)
            {
                Stencil.OnEnter();
            }
        }

        public override void OnExit()
        {
            if (Stencil != null)
            {
                Stencil.OnExit();
            }
            base.OnExit();
        }

        public override void OnExitTransitionDidStart()
        {
            if (Stencil != null)
            {
                Stencil.OnExitTransitionDidStart();
            }

            base.OnExitTransitionDidStart();
        }

        public override void OnEnterTransitionDidFinish()
        {
            base.OnEnterTransitionDidFinish();

            if (Stencil != null)
            {
                Stencil.OnEnterTransitionDidFinish();
            }
        }

        public override void Visit()
        {
            if (Stencil == null || !Stencil.Visible)
            {
                if (Inverted)
                {
                    // draw everything
                    base.Visit();
                }
                return;
            }

            if (CCDrawManager.BeginDrawMask(Inverted, AlphaThreshold))
            {
                CCDrawManager.PushMatrix();
                Transform();
                
                Stencil.Visit();

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