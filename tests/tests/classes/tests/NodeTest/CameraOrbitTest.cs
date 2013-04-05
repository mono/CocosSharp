using cocos2d;

namespace tests
{
    public class CameraOrbitTest : TestCocosNodeDemo
    {
        public CameraOrbitTest()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite p = new CCSprite(TestResource.s_back3);
            AddChild(p, 0);
            p.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            p.Opacity = 128;

            CCSprite sprite;
            CCOrbitCamera orbit;
            CCCamera cam;
            CCSize ss;

            // LEFT
            s = p.ContentSize;
            sprite = new CCSprite(TestResource.s_pPathGrossini);
            sprite.Scale = (0.5f);
            p.AddChild(sprite, 0);
            sprite.Position = (new CCPoint(s.Width / 4 * 1, s.Height / 2));
            cam = sprite.Camera;
            orbit = new CCOrbitCamera(2, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));

            // CENTER
            sprite = new CCSprite(TestResource.s_pPathGrossini);
            sprite.Scale = 1.0f;
            p.AddChild(sprite, 0);
            sprite.Position = new CCPoint(s.Width / 4 * 2, s.Height / 2);
            orbit = new CCOrbitCamera(2, 1, 0, 0, 360, 45, 0);
            sprite.RunAction(new CCRepeatForever (orbit));


            // RIGHT
            sprite = new CCSprite(TestResource.s_pPathGrossini);
            sprite.Scale = 2.0f;
            p.AddChild(sprite, 0);
            sprite.Position = new CCPoint(s.Width / 4 * 3, s.Height / 2);
            ss = sprite.ContentSize;
            orbit = new CCOrbitCamera(2, 1, 0, 0, 360, 90, -45);
            sprite.RunAction(new CCRepeatForever (orbit));


            // PARENT
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 90);
            p.RunAction(new CCRepeatForever (orbit));
            Scale = 1;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CCDirector.SharedDirector.Projection = (ccDirectorProjection.kCCDirectorProjection3D);
        }

        public override void OnExit()
        {
            CCDirector.SharedDirector.Projection = ccDirectorProjection.kCCDirectorProjection2D;
            base.OnExit();
        }

        public override string title()
        {
            return "Camera Orbit test";
        }
    }
}