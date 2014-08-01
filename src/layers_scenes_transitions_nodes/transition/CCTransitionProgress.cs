namespace CocosSharp
{
    public abstract class CCTransitionProgress : CCTransitionScene
    {
        const int SceneRadial = 0xc001;
        protected float From;
        protected float To;
        protected CCScene SceneToBeModified;


        #region Constructors

        public CCTransitionProgress(float t, CCScene scene) : base(t, scene)
        {
        }

        #endregion Constructors


        public override void OnEnter()
        {
            if (Layer == null || Viewport == null)
                return;

            base.OnEnter();

            SetupTransition();

            // create a transparent color layer
            // in which we are going to add our rendertextures
            var bounds = Layer.VisibleBoundsWorldspace;
            CCRect viewportRect = Viewport.ViewportInPixels;

            // create the second render texture for outScene
            CCRenderTexture texture = new CCRenderTexture(bounds.Size, viewportRect.Size);
            texture.Scene = Scene;
            texture.Layer = Layer;
            texture.Sprite.AnchorPoint = CCPoint.AnchorMiddle;
            texture.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width / 2, bounds.Size.Height / 2);
            texture.AnchorPoint = CCPoint.AnchorMiddle;

            // render outScene to its texturebuffer
            texture.Clear(0, 0, 0, 1);
            texture.Begin();
            SceneToBeModified.Visit();
            texture.End();

            //    Since we've passed the outScene to the texture we don't need it.
            if (SceneToBeModified == OutScene)
            {
                HideOutShowIn();
            }

            //    We need the texture in RenderTexture.
            CCProgressTimer node = ProgressTimerNodeWithRenderTexture(texture);

            // create the blend action
            CCSequence layerAction = new CCSequence(
                new CCProgressFromTo(Duration, From, To),
                new CCCallFunc(Finish)
                );

            // run the blend action
            node.RunAction(layerAction);

            // add the layer (which contains our two rendertextures) to the scene
            AddChild(node, 2, SceneRadial);
        }

        // clean up on exit
        public override void OnExit()
        {
            // remove our layer and release all containing objects
            RemoveChildByTag(SceneRadial, true);
            base.OnExit();
        }

        protected abstract CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture);

        protected virtual void SetupTransition()
        {
            SceneToBeModified = OutScene;
            From = 100;
            To = 0;
        }

        protected override void SceneOrder()
        {
            IsInSceneOnTop = false;
        }
    }


    /** CCTransitionRadialCCW transition.
     A counter colock-wise radial transition to the next scene
     */

    public class CCTransitionProgressRadialCCW : CCTransitionProgress
    {
        public CCTransitionProgressRadialCCW(float t, CCScene scene) : base(t, scene)
        {
        }

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            var bounds = Layer.VisibleBoundsWorldspace;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Radial;

            //    Return the radial type that we want to use
            node.ReverseDirection = false;
            node.Percentage = 100;
            node.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width / 2, bounds.Size.Height / 2);
            node.AnchorPoint = CCPoint.AnchorMiddle;

            return node;
        }
    }


    /** CCTransitionRadialCW transition.
     A counter colock-wise radial transition to the next scene
    */

    public class CCTransitionProgressRadialCW : CCTransitionProgress
    {
        public CCTransitionProgressRadialCW(float t, CCScene scene) : base(t, scene)
        {
        }

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            var bounds = Layer.VisibleBoundsWorldspace;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Radial;

            //    Return the radial type that we want to use
            node.ReverseDirection = true;
            node.Percentage = 100;
            node.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width / 2, bounds.Size.Height / 2);
            node.AnchorPoint = CCPoint.AnchorMiddle;

            return node;
        }
    }

    /** CCTransitionProgressHorizontal transition.
     A  colock-wise radial transition to the next scene
     */

    public class CCTransitionProgressHorizontal : CCTransitionProgress
    {
        public CCTransitionProgressHorizontal(float t, CCScene scene) : base(t, scene)
        {
        }

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            var bounds = Layer.VisibleBoundsWorldspace;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Bar;

            node.Midpoint = new CCPoint(1, 0);
            node.BarChangeRate = new CCPoint(1, 0);

            node.Percentage = 100;
            node.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width / 2, bounds.Size.Height / 2);
            node.AnchorPoint = CCPoint.AnchorMiddle;

            return node;
        }
    }

    public class CCTransitionProgressVertical : CCTransitionProgress
    {
        //OLD_TRANSITION_CREATE_FUNC(CCTransitionProgressVertical)
        //TRANSITION_CREATE_FUNC(CCTransitionProgressVertical)
        public CCTransitionProgressVertical(float t, CCScene scene) : base(t, scene)
        {
        }

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            var bounds = Layer.VisibleBoundsWorldspace;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Bar;

            node.Midpoint = new CCPoint(0, 0);
            node.BarChangeRate = new CCPoint(0, 1);

            node.Percentage = 100;
            node.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width / 2, bounds.Size.Height / 2);
            node.AnchorPoint = CCPoint.AnchorMiddle;

            return node;
        }
    }

    public class CCTransitionProgressInOut : CCTransitionProgress
    {
        public CCTransitionProgressInOut(float t, CCScene scene) : base(t, scene)
        {
        }

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            var bounds = Layer.VisibleBoundsWorldspace;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Bar;

            node.Midpoint = new CCPoint(0.5f, 0.5f);
            node.BarChangeRate = new CCPoint(1, 1);

            node.Percentage = 0;
            node.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width / 2, bounds.Size.Height / 2);
            node.AnchorPoint = CCPoint.AnchorMiddle;

            return node;
        }

        protected override void SceneOrder()
        {
            IsInSceneOnTop = false;
        }

        protected override void SetupTransition()
        {
            SceneToBeModified = InScene;
            From = 0;
            To = 100;
        }
    }

    public class CCTransitionProgressOutIn : CCTransitionProgress
    {
        public CCTransitionProgressOutIn(float t, CCScene scene) : base(t, scene)
        {
        }

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            var bounds = Layer.VisibleBoundsWorldspace;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Bar;

            node.Midpoint = new CCPoint(0.5f, 0.5f);
            node.BarChangeRate = new CCPoint(1, 1);

            node.Percentage = 100;
            node.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width / 2, bounds.Size.Height / 2);
            node.AnchorPoint = CCPoint.AnchorMiddle;

            return node;
        }
    }
}