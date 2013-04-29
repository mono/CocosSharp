
using System.Diagnostics;

namespace cocos2d
{
    public class CCTransitionScene : CCScene
    {
        protected bool m_bIsInSceneOnTop;
        protected bool m_bIsSendCleanupToScene;
        protected float m_fDuration;
        protected CCScene m_pInScene;
        protected CCScene m_pOutScene;

        // This can be taken out once all the transitions have been modified with constructors.
        protected CCTransitionScene() {}

        public CCTransitionScene(float t)
        {
            m_fDuration = t;
        }

        public CCTransitionScene (float t, CCScene scene)
        {
            InitWithDuration(t, scene);
        }
        

        public override bool IsTransition
        {
            get
            {
                return (true);
            }
        }

        public override void Draw()
        {
            base.Draw();

            if (m_bIsInSceneOnTop)
            {
                m_pOutScene.Visit();
                m_pInScene.Visit();
            }
            else
            {
                m_pInScene.Visit();
                m_pOutScene.Visit();
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // outScene_ should not receive the onExit callback
            // only the onExitTransitionDidStart
            m_pOutScene.OnExitTransitionDidStart();

            m_pInScene.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
            m_pOutScene.OnExit();

            // inScene should not receive the onExit callback
            // only the onEnterTransitionDidFinish
            m_pInScene.OnEnterTransitionDidFinish();
        }

        public override void Cleanup()
        {
            base.Cleanup();

            if (m_bIsSendCleanupToScene)
                m_pOutScene.Cleanup();
        }

        public virtual void Reset(float t, CCScene scene)
        {
            InitWithDuration(t, scene);
        }

        protected virtual bool InitWithDuration(float t, CCScene scene)
        {
            Debug.Assert(scene != null, "Argument scene must be non-nil");

            if (base.Init())
            {
                m_fDuration = t;

                // retain
                m_pInScene = scene;
                m_pOutScene = CCDirector.SharedDirector.RunningScene;
                if (m_pOutScene == null)
                {
                    // Creating an empty scene.
                    m_pOutScene = new CCScene();
                    m_pOutScene.Init();
                }

                Debug.Assert(m_pInScene != m_pOutScene, "Incoming scene must be different from the outgoing scene");

                // disable events while transitions
                CCDirector pDirector = CCDirector.SharedDirector;
                pDirector.TouchDispatcher.IsDispatchEvents = false;
                SceneOrder();

                return true;
            }
            return false;
        }

        public void Finish()
        {
            // clean up     
            m_pInScene.Visible = true;
            m_pInScene.Position = CCPoint.Zero;
            m_pInScene.Scale = 1.0f;
            m_pInScene.Rotation = 0.0f;
            m_pInScene.Camera.Restore();

            m_pOutScene.Visible = false;
            m_pOutScene.Position = CCPoint.Zero;
            m_pOutScene.Scale = 1.0f;
            m_pOutScene.Rotation = 0.0f;
            m_pOutScene.Camera.Restore();

            Schedule(SetNewScene, 0);
        }

        public void HideOutShowIn()
        {
            m_pInScene.Visible = true;
            m_pOutScene.Visible = false;
        }

        protected virtual void SceneOrder()
        {
            m_bIsInSceneOnTop = true;
        }

        private void SetNewScene(float dt)
        {
            // [self unschedule:_cmd]; 
            // "_cmd" is a local variable automatically defined in a method 
            // that contains the selector for the method
            Unschedule(SetNewScene);
            CCDirector director = CCDirector.SharedDirector;
            // Before replacing, save the "send cleanup to scene"
            m_bIsSendCleanupToScene = director.IsSendCleanupToScene();
            director.ReplaceScene(m_pInScene);
            // enable events while transitions
            director.TouchDispatcher.IsDispatchEvents = true;

            // issue #267
            m_pOutScene.Visible = true;
        }
    }
}