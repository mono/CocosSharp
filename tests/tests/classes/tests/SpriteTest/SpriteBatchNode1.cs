using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;
using Random = Cocos2D.CCRandom;

namespace tests
{
    public class SpriteBatchNode1 : SpriteTestDemo
    {
        public SpriteBatchNode1()
        {
            TouchEnabled = true;

            CCSpriteBatchNode BatchNode = new CCSpriteBatchNode("Images/grossini_dance_atlas", 50);
            AddChild(BatchNode, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.SharedDirector.WinSize;
            addNewSpriteWithCoords(new CCPoint(s.Width / 2, s.Height / 2));
        }

        public void addNewSpriteWithCoords(CCPoint p)
        {
            CCSpriteBatchNode BatchNode = (CCSpriteBatchNode)GetChildByTag((int)kTags.kTagSpriteBatchNode);

            int idx = (int)(CCRandom.NextDouble() * 1400 / 100);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;


            CCSprite sprite = new CCSprite(BatchNode.Texture, new CCRect(x, y, 85, 121));
            BatchNode.AddChild(sprite);

            sprite.Position = (new CCPoint(p.X, p.Y));

            CCActionInterval action = null;
            float random = (float)CCRandom.NextDouble();

            if (random < 0.20)
                action = new CCScaleBy(3, 2);
            else if (random < 0.40)
                action = new CCRotateBy (3, 360);
            else if (random < 0.60)
                action = new CCBlink (1, 3);
            else if (random < 0.8)
                action = new CCTintBy (2, 0, -255, -255);
            else
                action = new CCFadeOut  (2);

            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCActionInterval seq = (CCActionInterval)(new CCSequence(action, action_back));

            sprite.RunAction(new CCRepeatForever (seq));
        }

        public override void TouchesEnded(List<CCTouch> touches)
        {
            foreach (CCTouch item in touches)
            {
                if (item == null)
                {
                    break;
                }
                CCPoint location = item.LocationInView;

                location = CCDirector.SharedDirector.ConvertToGl(location);

                addNewSpriteWithCoords(location);
            }
            base.TouchesEnded(touches);
        }

        public override string title()
        {
            return "SpriteBatchNode (tap screen)";
        }
    }
}
