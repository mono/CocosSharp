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
            // SpriteBatchNode actions
            rotate = new CCRotateBy(5, 360);
            action = new CCRepeatForever(rotate);

            // SpriteBatchNode actions
            rotate_back = rotate.Reverse();

            scale = new CCScaleBy(5, 1.5f);
            scale_back = scale.Reverse();

            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * i, 121 * 1, 85, 121));
                AddChild(sprite, i);
            }
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            float step = windowSize.Width / 4;

            int i = 0;
            foreach(CCNode node in Children)
            {
                CCSprite sprite = node as CCSprite;
                if (sprite != null)
                {
                    sprite.Position = (new CCPoint((i + 1) * step, windowSize.Height / 2));
                    sprite.RunAction(action);
                    ++i;
                }
            }
        }

        #endregion Setup content

    }
}
