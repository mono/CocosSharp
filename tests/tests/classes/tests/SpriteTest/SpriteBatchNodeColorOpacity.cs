using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeColorOpacity : SpriteTestDemo
    {
        CCSprite sprite1;
        CCSprite sprite2;
        CCSprite sprite3;
        CCSprite sprite4;
        CCSprite sprite5;
        CCSprite sprite6;
        CCSprite sprite7;
        CCSprite sprite8;

        CCAction red;
        CCAction green;
        CCAction blue;
        CCAction fade;


        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode: Color & Opacity"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeColorOpacity()
        {
            // Small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = new CCSprite(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
            sprite2 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite3 = new CCSprite(batch.Texture, new CCRect(85 * 2, 121 * 1, 85, 121));
            sprite4 = new CCSprite(batch.Texture, new CCRect(85 * 3, 121 * 1, 85, 121));

            sprite5 = new CCSprite(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
            sprite6 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite7 = new CCSprite(batch.Texture, new CCRect(85 * 2, 121 * 1, 85, 121));
            sprite8 = new CCSprite(batch.Texture, new CCRect(85 * 3, 121 * 1, 85, 121));


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

            batch.AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);
            batch.AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);
            batch.AddChild(sprite3, 0, (int)kTagSprite.kTagSprite3);
            batch.AddChild(sprite4, 0, (int)kTagSprite.kTagSprite4);
            batch.AddChild(sprite5, 0, (int)kTagSprite.kTagSprite5);
            batch.AddChild(sprite6, 0, (int)kTagSprite.kTagSprite6);
            batch.AddChild(sprite7, 0, (int)kTagSprite.kTagSprite7);
            batch.AddChild(sprite8, 0, (int)kTagSprite.kTagSprite8);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
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
            CCSpriteBatchNode batch = (CCSpriteBatchNode)(GetChildByTag((int)kTags.kTagSpriteBatchNode));
            CCSprite sprite = (CCSprite)(batch.GetChildByTag((int)kTagSprite.kTagSprite5));

            batch.RemoveChild(sprite, false);
            batch.AddChild(sprite, 0, (int)kTagSprite.kTagSprite5);
        }
    }
}
