using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteFrameAliasNameTest : SpriteTestDemo
    {
        CCSprite sprite;
        CCAnimation animation;

        #region Properties

        public override string Title
        {
            get { return "SpriteFrame Alias Name"; }
        }

        public override string Subtitle
        {
            get { return "SpriteFrames are obtained using the alias name"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteFrameAliasNameTest()
        {
            CCSpriteFrameCache cache = CCApplication.SharedApplication.SpriteFrameCache;
            cache.AddSpriteFrames("animations/grossini-aliases.plist", "animations/grossini-aliases");

            var animFrames = new List<CCSpriteFrame>(15);
            string str = "";
            for (int i = 1; i < 15; i++)
            {
                // Obtain frames by alias name
                str = string.Format("dance_{0:00}", i);
                CCSpriteFrame frame = cache[str];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.3f);

            sprite = new CCSprite("grossini_dance_01.png");

            CCSpriteBatchNode spriteBatch = new CCSpriteBatchNode("animations/grossini-aliases");
            spriteBatch.AddChild(sprite);
            AddChild(spriteBatch);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow (windowSize);

            sprite.Position = (new CCPoint(windowSize.Width * 0.5f, windowSize.Height * 0.5f));
            sprite.RunAction(new CCRepeatForever (new CCAnimate(animation)));
        }

        #endregion Setup content
    }
}
