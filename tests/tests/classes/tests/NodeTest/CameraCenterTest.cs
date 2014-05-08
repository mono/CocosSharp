using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{
    public class CameraCenterTest : TestCocosNodeDemo
    {
        public CameraCenterTest()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sprite;
            CCOrbitCamera orbit;

            // LEFT-TOP
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0);
            sprite.Position = new CCPoint(s.Width / 5 * 1, s.Height / 5 * 1);
            sprite.Color = CCColor3B.Red;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));


            // LEFT-BOTTOM
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0, 40);
            sprite.Position = (new CCPoint(s.Width / 5 * 1, s.Height / 5 * 4));
            sprite.Color = CCColor3B.Blue;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));


            // RIGHT-TOP
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0);
            sprite.Position = (new CCPoint(s.Width / 5 * 4, s.Height / 5 * 1));
            sprite.Color = CCColor3B.Yellow;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));

            // RIGHT-BOTTOM
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0, 40);
            sprite.Position = (new CCPoint(s.Width / 5 * 4, s.Height / 5 * 4));
            sprite.Color = CCColor3B.Green;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));

            // CENTER
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0, 40);
            sprite.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            sprite.Color = CCColor3B.White;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));
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
		public CameraTest1()
		{
			var s = CCDirector.SharedDirector.WinSize;

			var sprite1 = new CCSprite(TestResource.s_back3);
			AddChild (sprite1);
			sprite1.Position = new CCPoint (1 * s.Width / 4, s.Height / 2);
			sprite1.Scale = 0.5f;

			var sprite2 = new CCSprite(TestResource.s_back3);
			AddChild (sprite2);
			sprite2.Position = new CCPoint (3 * s.Width / 4, s.Height / 2);
			sprite2.Scale = 0.5f;

			var camera = new CCOrbitCamera(10, 0, 1, 0, 360, 0, 0);

			sprite1.RunAction( camera );
			sprite2.RunAction( camera );


		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			CCDirector.SharedDirector.Projection = CCDirectorProjection.Projection3D;
			CCDirector.SharedDirector.IsUseDepthTesting =  (true);
		}

		public override void OnExit ()
		{
			base.OnExit ();
			CCDirector.SharedDirector.Projection = CCDirectorProjection.Projection2D;
			CCDirector.SharedDirector.IsUseDepthTesting =  (false);
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
		public CameraTest2()
		{
			var s = CCDirector.SharedDirector.WinSize;

			var sprite1 = new CCSprite(TestResource.s_back3);
			AddChild (sprite1);
			sprite1.Position = new CCPoint (1 * s.Width / 4, s.Height / 2);
			sprite1.Scale = 0.5f;

			var sprite2 = new CCSprite(TestResource.s_back3);
			AddChild (sprite2);
			sprite2.Position = new CCPoint (3 * s.Width / 4, s.Height / 2);
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

			sprite1.AdditionalTransform = ct;
		}


		public override void OnEnter ()
		{
			base.OnEnter ();
			CCDirector.SharedDirector.Projection = CCDirectorProjection.Projection3D;
			CCDirector.SharedDirector.IsUseDepthTesting =  (true);
		}

		public override void OnExit ()
		{
			base.OnExit ();
			CCDirector.SharedDirector.Projection = CCDirectorProjection.Projection2D;
			CCDirector.SharedDirector.IsUseDepthTesting =  (false);
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