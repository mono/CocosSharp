using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeReorderIssue744 : SpriteTestDemo
    {
        CCSprite sprite;

        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode: reorder issue #744"; }
        }

        public override string Subtitle
        {
            get { return "Should not crash"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeReorderIssue744()
        {
            // Testing issue #744
            // http://code.google.com/p/cocos2d-iphone/issues/detail?id=744
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 15);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite = new CCSprite(batch.Texture, new CCRect(0, 0, 85, 121));
            batch.AddChild(sprite, 3);
            batch.ReorderChild(sprite, 1);
        }

        #endregion Constructors


        #region Setup content

        public void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            sprite.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height / 2));

        }

        #endregion Setup content
    }
}
