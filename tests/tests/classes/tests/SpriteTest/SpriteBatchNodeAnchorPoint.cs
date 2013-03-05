using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeAnchorPoint : SpriteTestDemo
    {
        public SpriteBatchNodeAnchorPoint()
        {
            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = CCSpriteBatchNode.Create("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.SharedDirector.WinSize;


            CCActionInterval rotate = CCRotateBy.Create(10, 360);
            CCAction action = CCRepeatForever.Create(rotate);
            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = CCSprite.Create(batch.Texture, new CCRect(85 * i, 121 * 1, 85, 121));
                sprite.Position = (new CCPoint(s.Width / 4 * (i + 1), s.Height / 2));

                CCSprite point = CCSprite.Create("Images/r1");
                point.Scale = 0.25f;
                point.Position = sprite.Position;
                AddChild(point, 1);

                switch (i)
                {
                    case 0:
                        sprite.AnchorPoint = new CCPoint(0, 0);
                        break;
                    case 1:
                        sprite.AnchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprite.AnchorPoint = (new CCPoint(1, 1));
                        break;
                }

                point.Position = sprite.Position;

                CCAction copy = (CCAction)(action.Copy());
                sprite.RunAction(copy);
                batch.AddChild(sprite, i);
            }
        }

        public override string title()
        {
            return "SpriteBatchNode: anchor point";
        }
    }
}
