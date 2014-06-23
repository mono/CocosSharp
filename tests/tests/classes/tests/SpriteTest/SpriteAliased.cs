using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteAliased : SpriteTestDemo
    {
        CCSprite sprite1;
        CCSprite sprite2;

        CCSequence seq;


        #region Properties

        public override string Title
        {
            get { return "Sprite Aliased"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteAliased()
        {
            sprite1 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            sprite2 = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            var scale = new CCScaleBy(2, 5);
            var scale_back = scale.Reverse();
            seq = new CCSequence(scale, scale_back);

        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            sprite1.Position = new CCPoint(windowSize.Width / 2 - 100, windowSize.Height / 2);
            sprite2.Position = new CCPoint(windowSize.Width / 2 + 100, windowSize.Height / 2);

            sprite1.RepeatForever(seq);
            sprite2.RepeatForever(seq);
        }

        #endregion Setup content


        public override void OnEnter()
        {
            base.OnEnter();

            //
            // IMPORTANT:
            // This change will affect every sprite that uses the same texture
            // So sprite1 and sprite2 will be affected by this change
            //
            var sprite = (CCSprite)GetChildByTag((int)kTagSprite.kTagSprite1);
            sprite.IsAntialiased = false;
        }

        public override void OnExit()
        {
            // restore the tex parameter to AntiAliased.
            var sprite = (CCSprite)GetChildByTag((int)kTagSprite.kTagSprite1);
            sprite.IsAntialiased = true;
            base.OnExit();
        }
    }
}
