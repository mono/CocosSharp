namespace cocos2d
{
    public abstract class CCTransitionProgress : CCTransitionScene
    {
        private const int kCCSceneRadial = 0xc001;
        protected float m_fFrom;
        protected float m_fTo;
        protected CCScene m_pSceneToBeModified;

        public override void OnEnter()
        {
            base.OnEnter();

            SetupTransition();

            // create a transparent color layer
            // in which we are going to add our rendertextures
            CCSize size = CCDirector.SharedDirector.WinSize;

            // create the second render texture for outScene
            CCRenderTexture texture = CCRenderTexture.Create((int) size.Width, (int) size.Height);
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
            CCSequence layerAction = CCSequence.Create(
                CCProgressFromTo.Create(m_fDuration, m_fFrom, m_fTo),
                CCCallFunc.Create(Finish)
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
            RemoveChildByTag(kCCSceneRadial, false);
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
        //OLD_TRANSITION_CREATE_FUNC(CCTransitionProgressRadialCCW)
        //TRANSITION_CREATE_FUNC(CCTransitionProgressRadialCCW)

        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = CCProgressTimer.Create(texture.Sprite);

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

        public new static CCTransitionProgressRadialCCW Create(float t, CCScene scene)
        {
            var ret = new CCTransitionProgressRadialCCW();
            ret.InitWithDuration(t, scene);
            return ret;
        }
    }


    /** CCTransitionRadialCW transition.
     A counter colock-wise radial transition to the next scene
    */

    public class CCTransitionProgressRadialCW : CCTransitionProgress
    {
        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = CCProgressTimer.Create(texture.Sprite);

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

        public new static CCTransitionProgressRadialCW Create(float t, CCScene scene)
        {
            var ret = new CCTransitionProgressRadialCW();
            ret.InitWithDuration(t, scene);
            return ret;
        }
    }

    /** CCTransitionProgressHorizontal transition.
     A  colock-wise radial transition to the next scene
     */

    public class CCTransitionProgressHorizontal : CCTransitionProgress
    {
        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = CCProgressTimer.Create(texture.Sprite);

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

        public new static CCTransitionProgressHorizontal Create(float t, CCScene scene)
        {
            var ret = new CCTransitionProgressHorizontal();
            ret.InitWithDuration(t, scene);
            return ret;
        }
    }

    public class CCTransitionProgressVertical : CCTransitionProgress
    {
        //OLD_TRANSITION_CREATE_FUNC(CCTransitionProgressVertical)
        //TRANSITION_CREATE_FUNC(CCTransitionProgressVertical)
        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = CCProgressTimer.Create(texture.Sprite);

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

        public new static CCTransitionProgressVertical Create(float t, CCScene scene)
        {
            var ret = new CCTransitionProgressVertical();
            ret.InitWithDuration(t, scene);
            return ret;
        }
    }

    public class CCTransitionProgressInOut : CCTransitionProgress
    {
        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = CCProgressTimer.Create(texture.Sprite);

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

        public new static CCTransitionProgressInOut Create(float t, CCScene scene)
        {
            var ret = new CCTransitionProgressInOut();
            ret.InitWithDuration(t, scene);
            return ret;
        }
    }

    public class CCTransitionProgressOutIn : CCTransitionProgress
    {
        protected override CCProgressTimer ProgressTimerNodeWithRenderTexture(CCRenderTexture texture)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;

            CCProgressTimer node = CCProgressTimer.Create(texture.Sprite);

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

        public new static CCTransitionProgressOutIn Create(float t, CCScene scene)
        {
            var ret = new CCTransitionProgressOutIn();
            ret.InitWithDuration(t, scene);
            return ret;
        }
    }
}