using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeChildrenZ : SpriteTestDemo
    {
        CCSpriteBatchNode batch, batch2, batch3, batch4;
        CCSprite sprite1, sprite2, sprite3;
        CCSprite sprite4, sprite5, sprite6;
        CCSprite sprite7, sprite8, sprite9;
        CCSprite sprite10, sprite11, sprite12;

        #region Properties

        public override string Title
        {
            get { return "Sprite/BatchNode + child + scale + rot"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeChildrenZ()
        {
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("animations/grossini.plist");

            // test 1
            batch = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = new CCSprite("grossini_dance_01.png");
            sprite2 = new CCSprite("grossini_dance_02.png");
            sprite3 = new CCSprite("grossini_dance_03.png");

            batch.AddChild(sprite1);
            sprite1.AddChild(sprite2, 2);
            sprite1.AddChild(sprite3, -2);

            // test 2
            batch2 = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch2, 0, (int)kTags.kTagSpriteBatchNode);

            sprite4 = new CCSprite("grossini_dance_01.png");
            sprite5 = new CCSprite("grossini_dance_02.png");
            sprite6 = new CCSprite("grossini_dance_03.png");

            batch.AddChild(sprite1);
            sprite4.AddChild(sprite5, -2);
            sprite4.AddChild(sprite6, 2);

            // test 3
            batch3 = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch3, 0, (int)kTags.kTagSpriteBatchNode);

            sprite7 = new CCSprite("grossini_dance_01.png");
            sprite8 = new CCSprite("grossini_dance_02.png");
            sprite9 = new CCSprite("grossini_dance_03.png");

            batch3.AddChild(sprite7, 10);
            batch3.AddChild(sprite8, -10);
            batch3.AddChild(sprite9, -5);

            // test 4
            batch4 = new CCSpriteBatchNode("animations/grossini", 50);
            AddChild(batch4, 0, (int)kTags.kTagSpriteBatchNode);

            sprite10 = new CCSprite("grossini_dance_01.png");
            sprite11 = new CCSprite("grossini_dance_02.png");
            sprite12 = new CCSprite("grossini_dance_03.png");

            batch4.AddChild(sprite10, -10);
            batch4.AddChild(sprite11, -5);
            batch4.AddChild(sprite12, -2);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            sprite1.Position = (new CCPoint(windowSize.Width / 3, windowSize.Height / 2));
            sprite2.Position = (new CCPoint(20, 30));
            sprite3.Position = (new CCPoint(-20, 30));
            sprite4.Position = (new CCPoint(2 * windowSize.Width / 3, windowSize.Height / 2));
            sprite5.Position = (new CCPoint(20, 30));
            sprite6.Position = (new CCPoint(-20, 30));
            sprite7.Position = (new CCPoint(windowSize.Width / 2 - 90, windowSize.Height / 4));
            sprite8.Position = (new CCPoint(windowSize.Width / 2 - 60, windowSize.Height / 4));
            sprite9.Position = (new CCPoint(windowSize.Width / 2 - 30, windowSize.Height / 4));
            sprite10.Position = (new CCPoint(windowSize.Width / 2 + 30, windowSize.Height / 4));
            sprite11.Position = (new CCPoint(windowSize.Width / 2 + 60, windowSize.Height / 4));
            sprite12.Position = (new CCPoint(windowSize.Width / 2 + 90, windowSize.Height / 4));
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();
            CCApplication.SharedApplication.SpriteFrameCache.RemoveUnusedSpriteFrames();
        }
    }
}
