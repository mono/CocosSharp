
using System.Diagnostics;

namespace CocosSharp
{
    public class CCTransitionScene : CCScene
    {
		protected bool IsInSceneOnTop { get; set; }
		protected bool IsSendCleanupToScene { get; set; }
		protected float Duration { get; set; }
		protected CCScene InScene { get; set; }
		protected CCScene OutScene { get; set; }

        public override bool IsTransition
        {
            get
            {
                return (true);
            }
        }

        #region Constructors

        // This can be taken out once all the transitions have been modified with constructors.
//        protected CCTransitionScene() {}

        public CCTransitionScene (float t, CCScene scene) : base()
        {
            InitCCTransitionScene(t, scene);
        }

        private void InitCCTransitionScene(float t, CCScene scene)
        {
            Debug.Assert(scene != null, "Argument scene must be non-nil");

            Duration = t;

            // retain
            InScene = scene;
            OutScene = CCDirector.SharedDirector.RunningScene;
            if (OutScene == null)
            {
                // Creating an empty scene.
                OutScene = new CCScene();
            }

            Debug.Assert(InScene != OutScene, "Incoming scene must be different from the outgoing scene");

            SceneOrder();
        }

        #endregion Constructors


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
			EventDispatcher.IsEnabled = false;

            // outScene should not receive the onEnter callback
            // only the onExitTransitionDidStart
            OutScene.OnExitTransitionDidStart();
    
            InScene.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();

			// Enable event after transitioning
			EventDispatcher.IsEnabled = true;

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
            InScene.Camera.Restore();

            OutScene.Visible = false;
            OutScene.Position = CCPoint.Zero;
            OutScene.Scale = 1.0f;
            OutScene.Rotation = 0.0f;
            OutScene.Camera.Restore();

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
            CCDirector director = CCDirector.SharedDirector;
            IsSendCleanupToScene = director.IsSendCleanupToScene;
            director.ReplaceScene(InScene);

            // issue #267
            OutScene.Visible = true;
        }
    }
}