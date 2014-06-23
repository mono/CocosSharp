using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeAliased : SpriteTestDemo
    {
        CCSprite sprite1;
        CCSprite sprite2;

        CCRepeatForever repeat;

        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode Aliased"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeAliased()
        {
            var batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 10);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            batch.AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            sprite2 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            batch.AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            var scale = new CCScaleBy(2, 5);
            var scale_back = (CCActionInterval)scale.Reverse();

            repeat = new CCRepeatForever (scale, scale_back);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow (windowSize);

            sprite1.Position = new CCPoint(windowSize.Width / 2 - 100, windowSize.Height / 2);
            sprite2.Position = (new CCPoint(windowSize.Width / 2 + 100, windowSize.Height / 2));

            sprite1.RunAction(repeat);
            sprite2.RunAction(repeat);
        }

        #endregion Setup content


        public override void OnEnter()
        {
            base.OnEnter();
            CCSpriteBatchNode batch = (CCSpriteBatchNode)GetChildByTag((int)kTags.kTagSpriteBatchNode);
            batch.IsAntialiased = false;
        }

        public override void OnExit()
        {
            // restore the tex parameter to AntiAliased.
            CCSpriteBatchNode batch = (CCSpriteBatchNode)GetChildByTag((int)kTags.kTagSpriteBatchNode);
            batch.IsAntialiased = true;
            base.OnExit();
        }
    }
}
