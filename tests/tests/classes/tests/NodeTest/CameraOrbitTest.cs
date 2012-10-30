using cocos2d;

namespace tests
{
    public class CameraOrbitTest : TestCocosNodeDemo
    {
        public CameraOrbitTest()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite p = CCSprite.Create(TestResource.s_back3);
            AddChild(p, 0);
            p.Position = (new CCPoint(s.width / 2, s.height / 2));
            p.Opacity = 128;

            CCSprite sprite;
            CCOrbitCamera orbit;
            CCCamera cam;
            CCSize ss;

            // LEFT
            s = p.ContentSize;
            sprite = CCSprite.Create(TestResource.s_pPathGrossini);
            sprite.Scale = (0.5f);
            p.AddChild(sprite, 0);
            sprite.Position = (new CCPoint(s.width / 4 * 1, s.height / 2));
            cam = sprite.Camera;
            orbit = CCOrbitCamera.Create(2, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(CCRepeatForever.Create(orbit));

            // CENTER
            sprite = CCSprite.Create(TestResource.s_pPathGrossini);
            sprite.Scale = 1.0f;
            p.AddChild(sprite, 0);
            sprite.Position = new CCPoint(s.width / 4 * 2, s.height / 2);
            orbit = CCOrbitCamera.Create(2, 1, 0, 0, 360, 45, 0);
            sprite.RunAction(CCRepeatForever.Create(orbit));


            // RIGHT
            sprite = CCSprite.Create(TestResource.s_pPathGrossini);
            sprite.Scale = 2.0f;
            p.AddChild(sprite, 0);
            sprite.Position = new CCPoint(s.width / 4 * 3, s.height / 2);
            ss = sprite.ContentSize;
            orbit = CCOrbitCamera.Create(2, 1, 0, 0, 360, 90, -45);
            sprite.RunAction(CCRepeatForever.Create(orbit));


            // PARENT
            orbit = CCOrbitCamera.Create(10, 1, 0, 0, 360, 0, 90);
            p.RunAction(CCRepeatForever.Create(orbit));
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