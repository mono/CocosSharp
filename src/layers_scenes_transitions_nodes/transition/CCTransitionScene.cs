
using System.Diagnostics;

namespace CocosSharp
{
    class CCTransitionSceneContainerNode: CCNode
    {
        public CCScene InnerScene { get; private set; }

        public CCTransitionSceneContainerNode(CCScene scene, CCSize contentSize) : base(contentSize)
        {
            InnerScene = scene;
        }

        protected override void Draw()
        {
            base.Draw();

            CCDrawManager drawManager = CCDrawManager.SharedDrawManager;

            if(this.Visible)
                InnerScene.Visit();
        }
    }

    class CCTransitionSceneContainerLayer : CCLayer
    {
        CCTransitionSceneContainerNode inSceneNodeContainer;
        CCTransitionSceneContainerNode outSceneNodeContainer;

        internal CCNode InSceneNodeContainer { get { return inSceneNodeContainer; } }
        internal CCNode OutSceneNodeContainer { get { return outSceneNodeContainer; } }
        internal CCScene InScene { get { return inSceneNodeContainer.InnerScene; } }
        internal CCScene OutScene { get { return outSceneNodeContainer.InnerScene; } }
        internal bool IsInSceneOnTop { get; set; }

        public CCTransitionSceneContainerLayer(CCScene inScene, CCScene outScene) 
            : base(new CCCamera(CCCameraProjection.Projection2D, outScene.VisibleBoundsScreenspace.Size))
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

        public override void Visit()
        {
            bool outSceneVisible = OutSceneNodeContainer.Visible;
            bool inSceneVisible = InSceneNodeContainer.Visible;

            CCDrawManager drawManager = CCDrawManager.SharedDrawManager;

            base.Visit();

            if (IsInSceneOnTop)
            {
                outSceneNodeContainer.Visit();
                inSceneNodeContainer.Visit();
            }
            else
            {
                inSceneNodeContainer.Visit();
                outSceneNodeContainer.Visit();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            outSceneNodeContainer.InnerScene.OnExit();
        }

        public override void OnExitTransitionDidStart()
        {
            base.OnExitTransitionDidStart();
            outSceneNodeContainer.InnerScene.OnExitTransitionDidStart();
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

        public CCTransitionScene (float duration, CCScene scene) : base(scene.Window, scene.Viewport)
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
                outScene = new CCScene(Window, Viewport, Director);
            }

            Debug.Assert(InScene != outScene, "Incoming scene must be different from the outgoing scene");

            OutScene = outScene;

            transitionSceneContainerLayer = new CCTransitionSceneContainerLayer(InScene, OutScene);
            AddChild(transitionSceneContainerLayer);

            SceneOrder();
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

            InScene.OnEnter();

            InitialiseScenes();

            OutSceneNodeContainer.OnEnter();
            InSceneNodeContainer.OnEnter();


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