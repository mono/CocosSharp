using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteColorOpacity : SpriteTestDemo
    {
        public SpriteColorOpacity()
        {
            CCSprite sprite1 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
            CCSprite sprite2 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            CCSprite sprite3 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 2, 121 * 1, 85, 121));
            CCSprite sprite4 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 3, 121 * 1, 85, 121));

            CCSprite sprite5 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
            CCSprite sprite6 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            CCSprite sprite7 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 2, 121 * 1, 85, 121));
            CCSprite sprite8 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 3, 121 * 1, 85, 121));

            CCSize s = CCDirector.SharedDirector.WinSize;
            sprite1.Position = new CCPoint((s.Width / 5) * 1, (s.Height / 3) * 1);
            sprite2.Position = new CCPoint((s.Width / 5) * 2, (s.Height / 3) * 1);
            sprite3.Position = new CCPoint((s.Width / 5) * 3, (s.Height / 3) * 1);
            sprite4.Position = new CCPoint((s.Width / 5) * 4, (s.Height / 3) * 1);
            sprite5.Position = new CCPoint((s.Width / 5) * 1, (s.Height / 3) * 2);
            sprite6.Position = new CCPoint((s.Width / 5) * 2, (s.Height / 3) * 2);
            sprite7.Position = new CCPoint((s.Width / 5) * 3, (s.Height / 3) * 2);
            sprite8.Position = new CCPoint((s.Width / 5) * 4, (s.Height / 3) * 2);

            CCActionInterval action = new CCFadeIn  (2);
            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCAction fade = CCRepeatForever.Create((CCActionInterval)(CCSequence.FromActions(action, action_back)));

            CCActionInterval tintred = new CCTintBy (2, 0, -255, -255);
            CCActionInterval tintred_back = (CCActionInterval)tintred.Reverse();
            CCAction red = CCRepeatForever.Create((CCActionInterval)(CCSequence.FromActions(tintred, tintred_back)));

            CCActionInterval tintgreen = new CCTintBy (2, -255, 0, -255);
            CCActionInterval tintgreen_back = (CCActionInterval)tintgreen.Reverse();
            CCAction green = CCRepeatForever.Create((CCActionInterval)(CCSequence.FromActions(tintgreen, tintgreen_back)));

            CCActionInterval tintblue = new CCTintBy (2, -255, -255, 0);
            CCActionInterval tintblue_back = (CCActionInterval)tintblue.Reverse();
            CCAction blue = CCRepeatForever.Create((CCActionInterval)(CCSequence.FromActions(tintblue, tintblue_back)));

            sprite5.RunAction(red);
            sprite6.RunAction(green);
            sprite7.RunAction(blue);
            sprite8.RunAction(fade);

            // late add: test dirtyColor and dirtyPosition
            AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);
            AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);
            AddChild(sprite3, 0, (int)kTagSprite.kTagSprite3);
            AddChild(sprite4, 0, (int)kTagSprite.kTagSprite4);
            AddChild(sprite5, 0, (int)kTagSprite.kTagSprite5);
            AddChild(sprite6, 0, (int)kTagSprite.kTagSprite6);
            AddChild(sprite7, 0, (int)kTagSprite.kTagSprite7);
            AddChild(sprite8, 0, (int)kTagSprite.kTagSprite8);

            Schedule(removeAndAddSprite, 2);
        }

        public void removeAndAddSprite(float dt)
        {
            CCSprite sprite = (CCSprite)(GetChildByTag((int)kTagSprite.kTagSprite5));

            RemoveChild(sprite, false);
            AddChild(sprite, 0, (int)kTagSprite.kTagSprite5);
        }

        public override string title()
        {
            return "Sprite: Color & Opacity";
        }
    }
}
