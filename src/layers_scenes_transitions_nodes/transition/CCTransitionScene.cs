
using System.Diagnostics;

namespace CocosSharp
{
    public class CCTransitionScene : CCScene
    {
        #region Properties

        protected bool IsInSceneOnTop { get; set; }
        protected bool IsSendCleanupToScene { get; set; }
        protected float Duration { get; set; }
        protected CCScene InScene { get; private set; }
        protected CCScene OutScene { get; private set; }

        public override bool IsTransition
        {
            get { return true; }
        }

        public override CCScene Scene 
        { 
            get { return InScene; }
            internal set 
            {
                // Scene is dependent on InScene
            }
        }

        #endregion Properties


        #region Constructors

        public CCTransitionScene (float t, CCScene scene) 
            : base(scene.Window, scene.Camera, scene.Viewport, scene.Director)
        {
            InitCCTransitionScene(t, scene);
        }

        void InitCCTransitionScene(float t, CCScene scene)
        {
            Debug.Assert(scene != null, "Argument scene must be non-nil");

            Duration = t;

            // retain
            InScene = scene;
            OutScene = Director.RunningScene;
            if (OutScene == null)
            {
                // Creating an empty scene.
                OutScene = new CCScene(scene.Window, scene.Camera, scene.Viewport, scene.Director);
            }

            Debug.Assert(InScene != OutScene, "Incoming scene must be different from the outgoing scene");

            SceneOrder();
        }

        #endregion Constructors


        #region Scene callbacks

        protected virtual void AddedToNewScene()
        {
            base.AddedToNewScene();

            if (InScene != null && InScene.ContentSize == CCSize.Zero)
                InScene.ContentSize = Scene.VisibleBoundsWorldspace.Size;
        }

        #endregion Scene callbacks


        protected override void Draw()
        {
            base.Draw();

            if (IsInSceneOnTop)
            {
                OutScene.Visit();
                InScene.Visit();
            }
            else
            {
                InScene.Visit();
                OutScene.Visit();
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Disable events while transitioning
            EventDispatcherIsEnabled = false;

            // outScene should not receive the onEnter callback
            // only the onExitTransitionDidStart
            OutScene.OnExitTransitionDidStart();

            InScene.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();

            // Enable event after transitioning
            EventDispatcherIsEnabled = true;

            OutScene.OnExit();

            // m_pInScene should not receive the onEnter callback
            // only the onEnterTransitionDidFinish
            InScene.OnEnterTransitionDidFinish();
        }

        public override void Cleanup()
        {
            base.Cleanup();

            if (IsSendCleanupToScene)
                OutScene.Cleanup();
        }

        public virtual void Reset(float t, CCScene scene)
        {
            InitCCTransitionScene(t, scene);
        }

        public void Finish()
        {
            // clean up     
            InScene.Visible = true;
            InScene.Position = CCPoint.Zero;
            InScene.Scale = 1.0f;
            InScene.Rotation = 0.0f;

            OutScene.Visible = false;
            OutScene.Position = CCPoint.Zero;
            OutScene.Scale = 1.0f;
            OutScene.Rotation = 0.0f;

            Schedule(SetNewScene, 0);
        }

        public void HideOutShowIn()
        {
            InScene.Visible = true;
            OutScene.Visible = false;
        }

        protected virtual void SceneOrder()
        {
            IsInSceneOnTop = true;
        }

        private void SetNewScene(float dt)
        {
            Unschedule(SetNewScene);

            // Before replacing, save the "send cleanup to scene"
            IsSendCleanupToScene = Scene.Director.IsSendCleanupToScene;
            Scene.Director.ReplaceScene(InScene);

            // issue #267
            OutScene.Visible = true;
        }
    }
}