using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteFrameAliasNameTest : SpriteTestDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize s = CCDirector.SharedDirector.WinSize;

            // IMPORTANT:
            // The sprite frames will be cached AND RETAINED, and they won't be released unless you call
            //     [[CCSpriteFrameCache sharedSpriteFrameCache] removeUnusedSpriteFrames];
            //
            // CCSpriteFrameCache is a cache of CCSpriteFrames
            // CCSpriteFrames each contain a texture id and a rect (frame).

            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.AddSpriteFramesWithFile("animations/grossini-aliases.plist", "animations/grossini-aliases");

            //
            // Animation using Sprite batch
            //
            // A CCSpriteBatchNode can reference one and only one texture (one .png file)
            // Sprites that are contained in that texture can be instantiatied as CCSprites and then added to the CCSpriteBatchNode
            // All CCSprites added to a CCSpriteBatchNode are drawn in one OpenGL ES draw call
            // If the CCSprites are not added to a CCSpriteBatchNode then an OpenGL ES draw call will be needed for each one, which is less efficient
            //
            // When you animate a sprite, CCAnimation changes the frame of the sprite using setDisplayFrame: (this is why the animation must be in the same texture)
            // When setDisplayFrame: is used in the CCAnimation it changes the frame to one specified by the CCSpriteFrames that were added to the animation,
            // but texture id is still the same and so the sprite is still a child of the CCSpriteBatchNode, 
            // and therefore all the animation sprites are also drawn as part of the CCSpriteBatchNode
            //

            CCSprite sprite = CCSprite.Create("grossini_dance_01.png");
            sprite.Position = (new CCPoint(s.Width * 0.5f, s.Height * 0.5f));

            CCSpriteBatchNode spriteBatch = CCSpriteBatchNode.Create("animations/grossini-aliases");
            spriteBatch.AddChild(sprite);
            AddChild(spriteBatch);

            var animFrames = new List<CCSpriteFrame>(15);
            string str = "";
            for (int i = 1; i < 15; i++)
            {
                // Obtain frames by alias name
                str = string.Format("dance_{0:00}", i);
                CCSpriteFrame frame = cache.SpriteFrameByName(str);
                animFrames.Add(frame);
            }

            CCAnimation animation = CCAnimation.Create(animFrames, 0.3f);
            // 14 frames * 1sec = 14 seconds
            sprite.RunAction(new CCRepeatForever (new CCAnimate (animation)));
        }

        public override string title()
        {
            return "SpriteFrame Alias Name";
        }

        public override string subtitle()
        {
            return "SpriteFrames are obtained using the alias name";
        }
    }
}
