using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteColorOpacity : SpriteTestDemo
    {
        CCSprite sprite1;
        CCSprite sprite2;
        CCSprite sprite3;
        CCSprite sprite4;
        CCSprite sprite5;
        CCSprite sprite6;
        CCSprite sprite7;
        CCSprite sprite8;

        CCAction fade;
        CCAction red;
        CCAction green;
        CCAction blue;


        #region Properties

        public override string Title
        {
            get { return "Sprite: Color & Opacity"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteColorOpacity()
        {
            sprite1 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
            sprite2 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite3 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 2, 121 * 1, 85, 121));
            sprite4 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 3, 121 * 1, 85, 121));

            sprite5 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
            sprite6 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite7 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 2, 121 * 1, 85, 121));
            sprite8 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 3, 121 * 1, 85, 121));

            AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);
            AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);
            AddChild(sprite3, 0, (int)kTagSprite.kTagSprite3);
            AddChild(sprite4, 0, (int)kTagSprite.kTagSprite4);
            AddChild(sprite5, 0, (int)kTagSprite.kTagSprite5);
            AddChild(sprite6, 0, (int)kTagSprite.kTagSprite6);
            AddChild(sprite7, 0, (int)kTagSprite.kTagSprite7);
            AddChild(sprite8, 0, (int)kTagSprite.kTagSprite8);


            CCActionInterval action = new CCFadeIn  (2);
            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            fade = new CCRepeatForever ((CCActionInterval)(new CCSequence(action, action_back)));

            CCActionInterval tintred = new CCTintBy (2, 0, -255, -255);
            CCActionInterval tintred_back = (CCActionInterval)tintred.Reverse();
            red = new CCRepeatForever ((CCActionInterval)(new CCSequence(tintred, tintred_back)));

            CCActionInterval tintgreen = new CCTintBy (2, -255, 0, -255);
            CCActionInterval tintgreen_back = (CCActionInterval)tintgreen.Reverse();
            green = new CCRepeatForever ((CCActionInterval)(new CCSequence(tintgreen, tintgreen_back)));

            CCActionInterval tintblue = new CCTintBy (2, -255, -255, 0);
            CCActionInterval tintblue_back = (CCActionInterval)tintblue.Reverse();
            blue = new CCRepeatForever ((CCActionInterval)(new CCSequence(tintblue, tintblue_back)));
        }

        #endregion Constructors


        #region Setup content

        public void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            sprite1.Position = new CCPoint((windowSize.Width / 5) * 1, (windowSize.Height / 3) * 1);
            sprite2.Position = new CCPoint((windowSize.Width / 5) * 2, (windowSize.Height / 3) * 1);
            sprite3.Position = new CCPoint((windowSize.Width / 5) * 3, (windowSize.Height / 3) * 1);
            sprite4.Position = new CCPoint((windowSize.Width / 5) * 4, (windowSize.Height / 3) * 1);
            sprite5.Position = new CCPoint((windowSize.Width / 5) * 1, (windowSize.Height / 3) * 2);
            sprite6.Position = new CCPoint((windowSize.Width / 5) * 2, (windowSize.Height / 3) * 2);
            sprite7.Position = new CCPoint((windowSize.Width / 5) * 3, (windowSize.Height / 3) * 2);
            sprite8.Position = new CCPoint((windowSize.Width / 5) * 4, (windowSize.Height / 3) * 2);

            sprite5.RunAction(red);
            sprite6.RunAction(green);
            sprite7.RunAction(blue);
            sprite8.RunAction(fade);

            Schedule(RemoveAndAddSprite, 2);
        }

        #endregion Setup content


        void RemoveAndAddSprite(float dt)
        {
            CCSprite sprite = (CCSprite)(GetChildByTag((int)kTagSprite.kTagSprite5));

            RemoveChild(sprite, false);
            AddChild(sprite, 0, (int)kTagSprite.kTagSprite5);
        }

    }
}
