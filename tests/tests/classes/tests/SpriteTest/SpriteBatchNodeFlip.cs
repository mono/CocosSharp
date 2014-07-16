using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;

namespace tests
{
    public class SpriteBatchNodeFlip : SpriteTestDemo
    {
        CCSprite sprite1;
        CCSprite sprite2;


        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode Flip X & Y"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeFlip()
        {
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 10);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            batch.AddChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            sprite2 = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            batch.AddChild(sprite2, 0, (int)kTagSprite.kTagSprite2);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            sprite1.Position = (new CCPoint(windowSize.Width / 2 - 100, windowSize.Height / 2));
            sprite2.Position = new CCPoint(windowSize.Width / 2 + 100, windowSize.Height / 2);

            Schedule(FlipSprites, 1);
        }

        #endregion Setup content


        void FlipSprites(float dt)
        {
            CCSpriteBatchNode batch = (CCSpriteBatchNode)(GetChildByTag((int)kTags.kTagSpriteBatchNode));

            bool x = sprite1.FlipX;
            bool y = sprite2.FlipY;

            CCLog.Log("Pre: {0}", sprite1.ContentSize.Height);
            sprite1.FlipX = !x;
            sprite2.FlipY = !y;
            CCLog.Log("Post: {0}", sprite1.ContentSize.Height);
        }
    }
}
