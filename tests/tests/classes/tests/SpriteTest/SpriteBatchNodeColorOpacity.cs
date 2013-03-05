using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeColorOpacity : SpriteTestDemo
    {
        public SpriteBatchNodeColorOpacity()
        {
            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = CCSpriteBatchNode.Create("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSprite sprite1 = CCSprite.Create(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
            CCSprite sprite2 = CCSprite.Create(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            CCSprite sprite3 = CCSprite.Create(batch.Texture, new CCRect(85 * 2, 121 * 1, 85, 121));
            CCSprite sprite4 = CCSprite.Create(batch.Texture, new CCRect(85 * 3, 121 * 1, 85, 121));

            CCSprite sprite5 = CCSprite.Create(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
            CCSprite sprite6 = CCSprite.Create(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            CCSprite sprite7 = CCSprite.Create(batch.Texture, new CCRect(85 * 2, 121 * 1, 85, 121));
            CCSprite sprite8 = CCSprite.Create(batch.Texture, new CCRect(85 * 3, 121 * 1, 85, 121));


            CCSize s = CCDirector.SharedDirector.WinSize;
            sprite1.Position = new CCPoint((s.Width / 5) * 1, (s.Height / 3) * 1);
            sprite2.Position = new CCPoint((s.Width / 5) * 2, (s.Height / 3) * 1);
            sprite3.Position = new CCPoint((s.Width / 5) * 3, (s.Height / 3) * 1);
            sprite4.Position = new CCPoint((s.Width / 5) * 4, (s.Height / 3) * 1);
            sprite5.Position = new CCPoint((s.Width / 5) * 1, (s.Height / 3) * 2);
            sprite6.Position = new CCPoint((s.Width / 5) * 2, (s.Height / 3) * 2);
            sprite7.Position = new CCPoint((s.Width / 5) * 3, (s.Height / 3) * 2);
            sprite8.Position = new CCPoint((s.Width / 5) * 4, (s.Height / 3) * 2);

            CCActionInterval action = CCFadeIn.Create(2);
            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCAction fade = CCRepeatForever.Create((CCActionInterval)(CCSequence.Create(action, action_back)));

            CCActionInterval tintred = CCTintBy.Create(2, 0, -255, -255);
            CCActionInterval tintred_back = (CCActionInterval)tintred.Reverse();
            CCAction red = CCRepeatForever.Create((CCActionInterval)(CCSequence.Create(tintred, tintred_back)));

            CCActionInterval tintgreen = CCTintBy.Create(2, -255, 0, -255);
            CCActionInterval tintgreen_back = (CCActionInterval)tintgreen.Reverse();
            CCAction green = CCRepeatForever.Create((CCActionInterval)(CCSequence.Create(tintgreen, tintgreen_back)));

            CCActionInterval tintblue = CCTintBy.Create(2, -255, -255, 0);
            CCActionInterval tintblue_back = (CCActionInterval)tintblue.Reverse();
            CCAction blue = CCRepeatForever.Create((CCActionInterval)(CCSequence.Create(tintblue, tintblue_back)));


            sprite5.RunAction(red);
            sprite6.RunAction(green);
            sprite7.RunAction(blue);
            sprite8.RunAction(fade);

            // late add: test dirtyColor and dirtyPosition
            batch.AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);
            batch.AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);
            batch.AddChild(sprite3, 0, (int)kTagSprite.kTagSprite3);
            batch.AddChild(sprite4, 0, (int)kTagSprite.kTagSprite4);
            batch.AddChild(sprite5, 0, (int)kTagSprite.kTagSprite5);
            batch.AddChild(sprite6, 0, (int)kTagSprite.kTagSprite6);
            batch.AddChild(sprite7, 0, (int)kTagSprite.kTagSprite7);
            batch.AddChild(sprite8, 0, (int)kTagSprite.kTagSprite8);


            Schedule(removeAndAddSprite, 2);
        }

        public void removeAndAddSprite(float dt)
        {
            CCSpriteBatchNode batch = (CCSpriteBatchNode)(GetChildByTag((int)kTags.kTagSpriteBatchNode));
            CCSprite sprite = (CCSprite)(batch.GetChildByTag((int)kTagSprite.kTagSprite5));

            batch.RemoveChild(sprite, false);
            batch.AddChild(sprite, 0, (int)kTagSprite.kTagSprite5);
        }

        public override string title()
        {
            return "SpriteBatchNode: Color & Opacity";
        }
    }
}
