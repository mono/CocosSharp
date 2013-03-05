using cocos2d;

namespace tests
{
    public class ZwoptexGenericTest : ZwoptexTest
    {
        private static int spriteFrameIndex;
        protected int counter;
        protected CCSprite sprite1;
        protected CCSprite sprite2;

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("zwoptex/grossini.plist");
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile("zwoptex/grossini-generic.plist");

            CCLayerColor layer1 = CCLayerColor.Create(new ccColor4B(255, 0, 0, 255), 85, 121);
            layer1.Position = new CCPoint(s.Width / 2 - 80 - (85.0f * 0.5f), s.Height / 2 - (121.0f * 0.5f));
            AddChild(layer1);

            sprite1 = CCSprite.Create(CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName("grossini_dance_01.png"));
            sprite1.Position = (new CCPoint(s.Width / 2 - 80, s.Height / 2));
            AddChild(sprite1);

            sprite1.FlipX = false;
            sprite1.FlipY = false;

            CCLayerColor layer2 = CCLayerColor.Create(new ccColor4B(255, 0, 0, 255), 85, 121);
            layer2.Position = new CCPoint(s.Width / 2 + 80 - (85.0f * 0.5f), s.Height / 2 - (121.0f * 0.5f));
            AddChild(layer2);

            sprite2 = CCSprite.Create(CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName("grossini_dance_generic_01.png"));
            sprite2.Position = (new CCPoint(s.Width / 2 + 80, s.Height / 2));
            AddChild(sprite2);

            sprite2.FlipX = false;
            sprite2.FlipY = false;

            Schedule(startIn05Secs, 1.0f);

            counter = 0;
        }

        public void startIn05Secs(float dt)
        {
            Unschedule(startIn05Secs);
            Schedule(flipSprites, 0.5f);
        }

        public void flipSprites(float dt)
        {
            counter++;

            bool fx = false;
            bool fy = false;
            int i = counter % 4;

            switch (i)
            {
                case 0:
                    fx = false;
                    fy = false;
                    break;
                case 1:
                    fx = true;
                    fy = false;
                    break;
                case 2:
                    fx = false;
                    fy = true;
                    break;
                case 3:
                    fx = true;
                    fy = true;
                    break;
            }

            sprite1.FlipX = fx;
            sprite2.FlipX = fx;
            sprite1.FlipY = fy;
            sprite2.FlipY = fy;

            if (++spriteFrameIndex > 14)
            {
                spriteFrameIndex = 1;
            }

            string str1 = string.Format("grossini_dance_{0:00}.png", spriteFrameIndex);
            string str2 = string.Format("grossini_dance_generic_{0:00}.png", spriteFrameIndex);

            sprite1.DisplayFrame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(str1);
            sprite2.DisplayFrame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(str2);
        }

        public override string title()
        {
            return "Zwoptex Tests";
        }

        public override string subtitle()
        {
            return "Coordinate Formats, Rotation, Trimming, flipX/Y";
        }
    }
}