using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{
    public class CameraCenterTest : TestCocosNodeDemo
    {
		//CCWindowProjection preProjection;

        public CameraCenterTest()
        {
            CCSprite sprite;
			CCOrbitCamera orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);;

            // LEFT-TOP
			sprite = new CCSprite("Images/white-512x512") { Tag = 100 };
			AddChild(sprite, 0);
            sprite.Color = CCColor3B.Red;

            sprite.TextureRectInPixels = new CCRect(0, 0, 120, 50);
            sprite.RepeatForever(orbit);

            // LEFT-BOTTOM
			sprite = new CCSprite("Images/white-512x512") { Tag = 101 };
			AddChild(sprite, 0);
            sprite.Color = CCColor3B.Blue;

            sprite.TextureRectInPixels = new CCRect(0, 0, 120, 50);
			sprite.RepeatForever(orbit);


            // RIGHT-TOP
			sprite = new CCSprite("Images/white-512x512") { Tag = 102 };
			AddChild(sprite, 0);
            sprite.Color = CCColor3B.Yellow;

            sprite.TextureRectInPixels = new CCRect(0, 0, 120, 50);
			sprite.RepeatForever(orbit);

            // RIGHT-BOTTOM
			sprite = new CCSprite("Images/white-512x512") { Tag = 103 };
			AddChild(sprite, 0);
            sprite.Color = CCColor3B.Green;

            sprite.TextureRectInPixels = new CCRect(0, 0, 120, 50);
			sprite.RepeatForever(orbit);

            // CENTER
			sprite = new CCSprite("Images/white-512x512") { Tag = 104 };
			AddChild(sprite, 0);
            sprite.Color = CCColor3B.White;

            sprite.TextureRectInPixels = new CCRect(0, 0, 120, 50);
			sprite.RepeatForever(orbit);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

			// Top Left
			this[100].Position = new CCPoint(s.Width / 5 * 1, s.Height / 5 * 1);

			// Left Bottom
			this[101].Position = new CCPoint(s.Width / 5 * 1, s.Height / 5 * 4);

			// Top Right
			this[102].Position = new CCPoint(s.Width / 5 * 4, s.Height / 5 * 1);

			// Bottom Right
			this[103].Position = new CCPoint(s.Width / 5 * 4, s.Height / 5 * 4);

			// Center
			this[104].Position = s.Center;
		}

		public override void OnEnter()
		{
			base.OnEnter();
//			preProjection = Director.Projection;
//			Director.Projection = CCDirectorProjection.Projection3D;
		}

		public override void OnExit()
		{
			base.OnExit();
//			Director.Projection = preProjection;
		}

        public override string title()
        {
            return "Camera Center test";
        }

        public override string subtitle()
        {
            return "Sprites should rotate at the same speed";
        }
    }

	public class CameraTest1 : TestCocosNodeDemo
	{

		CCSprite sprite1;
		CCSprite sprite2;

		public CameraTest1()
		{
			sprite1 = new CCSprite(TestResource.s_back3);
			AddChild (sprite1);
			sprite1.Scale = 0.5f;

			sprite2 = new CCSprite(TestResource.s_back3);
			AddChild (sprite2);
			sprite2.Scale = 0.5f;

			var camera = new CCOrbitCamera(10, 0, 1, 0, 360, 0, 0);

			sprite1.RunAction( camera );
			sprite2.RunAction( camera );


		}

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;
			sprite1.Position = new CCPoint (1 * s.Width / 4, s.Height / 2);
			sprite2.Position = new CCPoint (3 * s.Width / 4, s.Height / 2);
		}

		public override void OnEnter ()
		{
			base.OnEnter ();

			//Scene.Director.Projection = CCDirectorProjection.Projection3D;
            GameView.DepthTesting =  (true);
		}

		public override void OnExit ()
		{
			base.OnExit ();

            //Scene.Director.Projection = CCDirectorProjection.Projection2D;
            GameView.DepthTesting =  (false);
		}

		public override string title()
		{
			return "Camera Test 1";
		}

		public override string subtitle()
		{
			return "Both images should rotate with a 3D effect";
		}
	}

	// This test does not work right now because CCAffineTransform does not support 3D yet
	public class CameraTest2 : TestCocosNodeDemo
	{

		CCSprite sprite1;
		CCSprite sprite2;

		public CameraTest2()
		{
			sprite1 = new CCSprite(TestResource.s_back3);
			AddChild (sprite1);

			sprite1.Scale = 0.5f;

			sprite2 = new CCSprite(TestResource.s_back3);
			AddChild (sprite2);

			sprite2.Scale = 0.5f;

			Microsoft.Xna.Framework.Vector3 eye, center, up;

			eye = new Microsoft.Xna.Framework.Vector3 (150, 0, 200);
			center = Microsoft.Xna.Framework.Vector3.Zero;
			up = new Microsoft.Xna.Framework.Vector3 (0, 1, 0);

			var lookAt = Microsoft.Xna.Framework.Matrix.CreateLookAt (eye, center, up);

			CCAffineTransform ct = new CCAffineTransform (lookAt.M11, lookAt.M12, lookAt.M13, lookAt.M14, lookAt.Translation.X, lookAt.Translation.Y);
			ct = CCAffineTransform.Identity;

			// Setup our CCAffineTransfrom 
			ct.A = lookAt.M11;
			ct.C = lookAt.M21;
			ct.B = lookAt.M12;
			ct.D = lookAt.M22;
			ct.Tx = lookAt.M41;
			ct.Ty = lookAt.M42;

			//sprite1.AdditionalTransform = ct;
		}

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;
			sprite1.Position = new CCPoint (1 * s.Width / 4, s.Height / 2);
			sprite2.Position = new CCPoint (3 * s.Width / 4, s.Height / 2);
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			
            //Scene.Director.Projection = CCDirectorProjection.Projection3D;
            GameView.DepthTesting =  (true);
		}

		public override void OnExit ()
		{
			base.OnExit ();

            //Scene.Director.Projection = CCDirectorProjection.Projection2D;
            GameView.DepthTesting =  (false);
		}

		public override string title()
		{
			return "Camera Test 2";
		}

		public override string subtitle()
		{
			return "Both images should look the same";
		}
	}

}