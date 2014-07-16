using CocosSharp;

namespace tests
{
    public class CameraZoomTest : TestCocosNodeDemo
    {
        private float m_z;

        public CameraZoomTest()
        {
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

			CCSprite sprite;
			CCCamera cam;

			// LEFT
			sprite = new CCSprite(TestResource.s_pPathGrossini);
			AddChild(sprite, 0);
			sprite.Position = (new CCPoint(s.Width / 4 * 1, s.Height / 2));
			cam = sprite.Camera;
//			cam.SetEyeXyz(0, 0, 415 / 2);
//			cam.SetCenterXyz(0, 0, 0);

			// CENTER
			sprite = new CCSprite(TestResource.s_pPathGrossini);
			AddChild(sprite, 0, 40);
			sprite.Position = (new CCPoint(s.Width / 4 * 2, s.Height / 2));

			// RIGHT
			sprite = new CCSprite(TestResource.s_pPathGrossini);
			AddChild(sprite, 0, 20);
			sprite.Position = (new CCPoint(s.Width / 4 * 3, s.Height / 2));

			m_z = 0;

			Schedule ();
		}

        public override void Update(float dt)
        {
            CCNode sprite;
            CCCamera cam;

            m_z += dt * 100;

            sprite = GetChildByTag(20);
            cam = sprite.Camera;
            //cam.SetEyeXyz(0, 0, m_z);

            sprite = GetChildByTag(40);
            cam = sprite.Camera;
            //cam.SetEyeXyz(0, 0, -m_z);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //Scene.Director.Projection = (CCDirectorProjection.Projection3D);
        }

        public override void OnExit()
        {
            //Scene.Director.Projection = (CCDirectorProjection.Projection2D);
            base.OnExit();
        }

        public override string title()
        {
            return "Camera Zoom test";
        }
    }
}