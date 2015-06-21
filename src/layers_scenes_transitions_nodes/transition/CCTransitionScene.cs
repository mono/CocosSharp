
using System.Diagnostics;

namespace CocosSharp
{
    class CCTransitionSceneContainerNode: CCNodeGrid
    {
        public CCScene InnerScene { get; private set; }

        public CCTransitionSceneContainerNode(CCScene scene, CCSize contentSize) : base(contentSize)
        {
            InnerScene = scene;
        }

        protected override void VisitRenderer(ref CCAffineTransform worldTransform)
        {
            if(this.Visible && InnerScene != null)
                InnerScene.Visit(ref worldTransform);
        }
    }

    class CCTransitionSceneContainerLayer : CCLayer
    {
        bool isInSceneOnTop;
        CCTransitionSceneContainerNode inSceneNodeContainer;
        CCTransitionSceneContainerNode outSceneNodeContainer;

        internal CCNode InSceneNodeContainer { get { return inSceneNodeContainer; } }
        internal CCNode OutSceneNodeContainer { get { return outSceneNodeContainer; } }
        internal CCScene InScene { get { return inSceneNodeContainer.InnerScene; } }
        internal CCScene OutScene { get { return outSceneNodeContainer.InnerScene; } }
        internal bool IsInSceneOnTop 
        { 
            get { return isInSceneOnTop; } 
            set 
            {
                isInSceneOnTop = value;

                if(isInSceneOnTop) 
                {
                    OutSceneNodeContainer.ZOrder = 0;
                    InSceneNodeContainer.ZOrder = 1;
                } 
                else 
                {
                    OutSceneNodeContainer.ZOrder = 1;
                    InSceneNodeContainer.ZOrder = 0;
                }
            }
        }

        public CCTransitionSceneContainerLayer(CCScene inScene, CCScene outScene) 
            : base(new CCCamera(CCCameraProjection.Projection3D, outScene.VisibleBoundsScreenspace.Size))
        {
            CCSize contentSize = outScene.VisibleBoundsScreenspace.Size;

            AnchorPoint = new CCPoint(0.5f, 0.5f);

            inSceneNodeContainer = new CCTransitionSceneContainerNode(inScene, contentSize);
            outSceneNodeContainer = new CCTransitionSceneContainerNode(outScene, contentSize);

            // The trick here is that we're not actually adding the InScene/OutScene as children
            // This keeps the scenes' original parents (if they have one) intact, so that there's no cleanup afterwards

            AddChild(InSceneNodeContainer);
            AddChild(OutSceneNodeContainer);

        }
    }

    public class CCTransitionScene : CCScene
    {
        CCTransitionSceneContainerLayer transitionSceneContainerLayer;

        #region Properties

        protected bool IsSendCleanupToScene { get; set; }
        protected float Duration { get; set; }

        public override bool IsTransition
        {
            get { return true; }
        }

        protected bool IsInSceneOnTop 
        { 
            get { return transitionSceneContainerLayer.IsInSceneOnTop; }
            set { transitionSceneContainerLayer.IsInSceneOnTop = value; }
        }

        public override CCLayer Layer
        {
            get { return transitionSceneContainerLayer; }

            internal set
            {
            }
        }

        protected CCNode InSceneNodeContainer
        {
            get { return transitionSceneContainerLayer.InSceneNodeContainer; }
        }

        protected CCNode OutSceneNodeContainer
        {
            get { return transitionSceneContainerLayer.OutSceneNodeContainer; }
        }

        protected virtual CCFiniteTimeAction InSceneAction
        {
            get { return null; }
        }

        protected virtual CCFiniteTimeAction OutSceneAction
        {
            get { return null; }
        }

        // Don't want subclasses do access scene - use node container instead
        CCScene InScene { get; set; }
        CCScene OutScene { get; set; }

        #endregion Properties


        #region Constructors

        public CCTransitionScene (float duration, CCScene scene) : base(scene.GameView, scene.Viewport)
        {
            InitCCTransitionScene(duration, scene);
        }

        void InitCCTransitionScene(float duration, CCScene scene)
        {
            Debug.Assert(scene != null, "Argument scene must be non-nil");

            Duration = duration;

            InScene = scene;

            CCScene outScene = Director.RunningScene;
            if (outScene == null)
            {
                // Creating an empty scene.
                outScene = new CCScene(GameView, Viewport);
            }

            Debug.Assert(InScene != outScene, "Incoming scene must be different from the outgoing scene");

            OutScene = outScene;

            transitionSceneContainerLayer = new CCTransitionSceneContainerLayer(InScene, OutScene);

            SceneOrder();

            AddChild(transitionSceneContainerLayer);
        }

        #endregion Constructors

        protected virtual void InitialiseScenes()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Disable events while transitioning
            EventDispatcherIsEnabled = false;

            // OutScene should not receive the OnEnter callback
            // only the OnExitTransitionDidStart
            OutScene.OnExitTransitionDidStart();
            InScene.OnEnter();

            InitialiseScenes();

            if(InSceneAction != null)
                InSceneNodeContainer.RunAction(InSceneAction);

            if (OutSceneAction != null)
                OutSceneNodeContainer.RunAction(new CCSequence(OutSceneAction, new CCCallFunc(Finish)));
            else
                OutSceneNodeContainer.RunAction(new CCSequence(new CCDelayTime(Duration), new CCCallFunc(Finish)));
        }

        public override void OnExit()
        {
            base.OnExit();

            // Enable event after transitioning
            EventDispatcherIsEnabled = true;

            OutScene.OnExit();

            // InScene should not receive the OnEnter callback
            // only the OnEnterTransitionDidFinish
            InScene.OnEnterTransitionDidFinish();
        }

        public virtual void Reset(float t, CCScene scene)
        {
            InitCCTransitionScene(t, scene);
        }

        public void Finish()
        {
            Schedule(SetNewScene, 0);
        }

        public void HideOutShowIn()
        {
            InSceneNodeContainer.Visible = true;
            OutSceneNodeContainer.Visible = false;
        }

        protected virtual void SceneOrder()
        {
            IsInSceneOnTop = true;
        }

        void SetNewScene(float dt)
        {
            Unschedule(SetNewScene);

            // Before replacing, save the "send cleanup to scene"
            IsSendCleanupToScene = Director.IsSendCleanupToScene;
            Director.ReplaceScene(transitionSceneContainerLayer.InScene);
        }
    }
}