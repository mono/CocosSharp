using Cocos2D;

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
            sprite.Color = CCTypes.CCRed;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));


            // LEFT-BOTTOM
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0, 40);
            sprite.Position = (new CCPoint(s.Width / 5 * 1, s.Height / 5 * 4));
            sprite.Color = CCTypes.CCBlue;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));


            // RIGHT-TOP
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0);
            sprite.Position = (new CCPoint(s.Width / 5 * 4, s.Height / 5 * 1));
            sprite.Color = CCTypes.CCYellow;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));

            // RIGHT-BOTTOM
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0, 40);
            sprite.Position = (new CCPoint(s.Width / 5 * 4, s.Height / 5 * 4));
            sprite.Color = CCTypes.CCGreen;
            sprite.TextureRect = new CCRect(0, 0, 120, 50);
            orbit = new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0);
            sprite.RunAction(new CCRepeatForever (orbit));

            // CENTER
            sprite = new CCSprite("Images/white-512x512");
            AddChild(sprite, 0, 40);
            sprite.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            sprite.Color = CCTypes.CCWhite;
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
}