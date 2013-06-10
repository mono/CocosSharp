namespace Cocos2D
{
    public abstract class CCTransitionProgress : CCTransitionScene
    {
        private const int kCCSceneRadial = 0xc001;
        protected float m_fFrom;
        protected float m_fTo;
        protected CCScene m_pSceneToBeModified;

        public CCTransitionProgress()
        {
        }

        public CCTransitionProgress(float t, CCScene scene) : base(t, scene)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            SetupTransition();

            // create a transparent color layer
            // in which we are going to add our rendertextures
            CCSize size = CCDirector.SharedDirector.WinSize;

            // create the second render texture for outScene
            CCRenderTexture texture = new CCRenderTexture((int) size.Width, (int) size.Height);
            texture.Sprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
            texture.Position = new CCPoint(size.Width / 2, size.Height / 2);
            texture.AnchorPoint = new CCPoint(0.5f, 0.5f);

            // render outScene to its texturebuffer
            texture.Clear(0, 0, 0, 1);
            texture.Begin();
            m_pSceneToBeModified.Visit();
            texture.End();

            //    Since we've passed the outScene to the texture we don't need it.
            if (m_pSceneToBeModified == m_pOutScene)
            {
                HideOutShowIn();
            }

            //    We need the texture in RenderTexture.
            CCProgressTimer node = ProgressTimerNodeWithRenderTexture(texture);

            // create the blend action
            CCSequence layerAction = new CCSequence(
                new CCProgressFromTo(m_fDuration, m_fFrom, m_fTo),
                new CCCallFunc(Finish)
                );

            // run the blend action
            node.RunAction(layerAction);

            // add the layer (which contains our two rendertextures) to the scene
            AddChild(node, 2, kCCSceneRadial);
        }

        // clean up on exit
        public override void OnExit()
        {
            // remove our layer and release all containing objects
            RemoveChildByTag(kCCSceneRadial, true);
            base.OnExit();
        }

        protected abstract CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture);

        protected virtual void SetupTransition()
        {
            m_pSceneToBeModified = m_pOutScene;
            m_fFrom = 100;
            m_fTo = 0;
        }

        protected override void SceneOrder()
        {
            m_bIsInSceneOnTop = false;
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

        //OLD_TRANSITION_CREATE_FUNC(CCTransitionProgressRadialCCW)
        //TRANSITION_CREATE_FUNC(CCTransitionProgressRadialCCW)

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Radial;

            //    Return the radial type that we want to use
            node.ReverseDirection = false;
            node.Percentage = 100;
            node.Position = new CCPoint(size.Width / 2, size.Height / 2);
            node.AnchorPoint = new CCPoint(0.5f, 0.5f);

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
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Radial;

            //    Return the radial type that we want to use
            node.ReverseDirection = true;
            node.Percentage = 100;
            node.Position = new CCPoint(size.Width / 2, size.Height / 2);
            node.AnchorPoint = new CCPoint(0.5f, 0.5f);

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
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Bar;

            node.Midpoint = new CCPoint(1, 0);
            node.BarChangeRate = new CCPoint(1, 0);

            node.Percentage = 100;
            node.Position = new CCPoint(size.Width / 2, size.Height / 2);
            node.AnchorPoint = new CCPoint(0.5f, 0.5f);

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
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Bar;

            node.Midpoint = new CCPoint(0, 0);
            node.BarChangeRate = new CCPoint(0, 1);

            node.Percentage = 100;
            node.Position = new CCPoint(size.Width / 2, size.Height / 2);
            node.AnchorPoint = new CCPoint(0.5f, 0.5f);

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
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Bar;

            node.Midpoint = new CCPoint(0.5f, 0.5f);
            node.BarChangeRate = new CCPoint(1, 1);

            node.Percentage = 0;
            node.Position = new CCPoint(size.Width / 2, size.Height / 2);
            node.AnchorPoint = new CCPoint(0.5f, 0.5f);

            return node;
        }

        protected override void SceneOrder()
        {
            m_bIsInSceneOnTop = false;
        }

        protected override void SetupTransition()
        {
            m_pSceneToBeModified = m_pInScene;
            m_fFrom = 0;
            m_fTo = 100;
        }
    }

    public class CCTransitionProgressOutIn : CCTransitionProgress
    {
        public CCTransitionProgressOutIn(float t, CCScene scene) : base(t, scene)
        {
        }

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = new CCProgressTimer(texture.Sprite);

            // but it is flipped upside down so we flip the sprite
            //node.Sprite.IsFlipY = true;
            node.Type = CCProgressTimerType.Bar;

            node.Midpoint = new CCPoint(0.5f, 0.5f);
            node.BarChangeRate = new CCPoint(1, 1);

            node.Percentage = 100;
            node.Position = new CCPoint(size.Width / 2, size.Height / 2);
            node.AnchorPoint = new CCPoint(0.5f, 0.5f);

            return node;
        }
    }
}