using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Sprite6 : SpriteTestDemo
    {
        int tagSpriteBatchNode = 1;

        CCSpriteBatchNode batch;

        CCRepeatForever action;
        CCScaleBy scale;
        CCFiniteTimeAction scale_back;
        CCRotateBy rotate;
        CCFiniteTimeAction rotate_back;

        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode transformation"; }
        }

        #endregion Properties


        #region Constructors

        public Sprite6()
        {
            // small capacity. Testing resizing
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, tagSpriteBatchNode);

            // SpriteBatchNode actions
            rotate = new CCRotateBy(5, 360);
            action = new CCRepeatForever(rotate);

            // SpriteBatchNode actions
            rotate_back = rotate.Reverse();

            scale = new CCScaleBy(5, 1.5f);
            scale_back = scale.Reverse();

            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = new CCSprite(batch.Texture, new CCRect(85 * i, 121 * 1, 85, 121));
                batch.AddChild(sprite, i);
            }
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            float step = windowSize.Width / 4;

            int i = 0;
            foreach(CCNode node in batch.Children)
            {
                CCSprite sprite = (CCSprite)node;
                sprite.Position = (new CCPoint((i + 1) * step, windowSize.Height / 2));
                sprite.RunAction(action);
                ++i;
            }

            batch.IgnoreAnchorPointForPosition = true;
            batch.AnchorPoint = new CCPoint(0.5f, 0.5f);
            batch.ContentSize = (new CCSize(windowSize.Width, windowSize.Height));

            batch.RepeatForever(scale, scale_back);
            batch.RepeatForever(rotate, rotate_back);
        }

        #endregion Setup content

    }
}
