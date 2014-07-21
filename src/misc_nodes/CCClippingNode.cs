using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCClippingNode : CCNode
    {
        CCNode stencil;

        public bool Inverted { get; set; }
        public float AlphaThreshold { get; set; }
        public CCNode Stencil 
        { 
            get { return stencil; }
            set 
            {
                stencil = value;
                if (stencil != null && Layer != null) 
                {
                    stencil.Layer = Layer;
                }
            }
        }

        public override CCScene Scene 
        { 
            get { return base.Scene; }
            internal set 
            {
                base.Scene = value;

                if (stencil != null)
                {
                    stencil.Scene = value;
                }
            }
        }

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

            if (Window.DrawManager.BeginDrawMask(Viewport.ViewportInPixels, Inverted, AlphaThreshold))
            {
                Window.DrawManager.PushMatrix();

                Transform();

                Stencil.Visit();

                Window.DrawManager.PopMatrix();

                Window.DrawManager.EndDrawMask();

                base.Visit();

                Window.DrawManager.EndMask();
            }
            else
            {
                base.Visit();
            }
        }
    }
 }