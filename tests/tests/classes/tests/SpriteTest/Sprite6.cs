using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class Sprite6 : SpriteTestDemo
    {
        int kTagSpriteBatchNode = 1;
        public Sprite6()
        {
            // small capacity. Testing resizing
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, kTagSpriteBatchNode);
            batch.IgnoreAnchorPointForPosition = true;

            CCSize s = CCDirector.SharedDirector.WinSize;

            batch.AnchorPoint = new CCPoint(0.5f, 0.5f);
            batch.ContentSize = (new CCSize(s.Width, s.Height));


            // SpriteBatchNode actions
            CCActionInterval rotate = new CCRotateBy (5, 360);
            CCAction action = new CCRepeatForever (rotate);

            // SpriteBatchNode actions
            CCActionInterval rotate_back = (CCActionInterval)rotate.Reverse();
            CCActionInterval rotate_seq = (CCActionInterval)(new CCSequence(rotate, rotate_back));
            CCAction rotate_forever = new CCRepeatForever (rotate_seq);

            CCActionInterval scale = new CCScaleBy(5, 1.5f);
            CCActionInterval scale_back = (CCActionInterval)scale.Reverse();
            CCActionInterval scale_seq = (CCActionInterval)(new CCSequence(scale, scale_back));
            CCAction scale_forever = new CCRepeatForever (scale_seq);

            float step = s.Width / 4;

            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = new CCSprite(batch.Texture, new CCRect(85 * i, 121 * 1, 85, 121));
                sprite.Position = (new CCPoint((i + 1) * step, s.Height / 2));

                sprite.RunAction((CCAction)(action.Copy()));
                batch.AddChild(sprite, i);
            }

            batch.RunAction(scale_forever);
            batch.RunAction(rotate_forever);
        }

        public override string title()
        {
            return "SpriteBatchNode transformation";
        }
    }
}
