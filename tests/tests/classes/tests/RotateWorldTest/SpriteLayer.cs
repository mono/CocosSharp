using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteLayer : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";
        string s_pPathSister1 = "Images/grossinis_sister1";
        string s_pPathSister2 = "Images/grossinis_sister2";

        public override void OnEnter()
        {
            base.OnEnter();

            float x, y;

            CCSize size = CCDirector.SharedDirector.WinSize;
            x = size.Width;
            y = size.Height;

            CCSprite sprite = CCSprite.Create(s_pPathGrossini);
            CCSprite spriteSister1 = CCSprite.Create(s_pPathSister1);
            CCSprite spriteSister2 = CCSprite.Create(s_pPathSister2);

            sprite.Scale = (1.5f);
            spriteSister1.Scale = (1.5f);
            spriteSister2.Scale = (1.5f);

            sprite.Position = (new CCPoint(x / 2, y / 2));
            spriteSister1.Position = (new CCPoint(40, y / 2));
            spriteSister2.Position = (new CCPoint(x - 40, y / 2));

            CCAction rot = new CCRotateBy (16, -3600);

            AddChild(sprite);
            AddChild(spriteSister1);
            AddChild(spriteSister2);

            sprite.RunAction(rot);

            CCActionInterval jump1 = CCJumpBy.Create(4, new CCPoint(-400, 0), 100, 4);
            CCActionInterval jump2 = (CCActionInterval)jump1.Reverse();

            CCActionInterval rot1 = new CCRotateBy (4, 360 * 2);
            CCActionInterval rot2 = (CCActionInterval)rot1.Reverse();

            spriteSister1.RunAction(CCRepeat.Create(CCSequence.Create(jump2, jump1), 5));
            spriteSister2.RunAction(CCRepeat.Create(CCSequence.Create((CCFiniteTimeAction)(jump1.Copy()), (CCFiniteTimeAction)(jump2.Copy())), 5));

            spriteSister1.RunAction(CCRepeat.Create(CCSequence.Create(rot1, rot2), 5));
            spriteSister2.RunAction(CCRepeat.Create(CCSequence.Create((CCFiniteTimeAction)(rot2.Copy()), (CCFiniteTimeAction)(rot1.Copy())), 5));
        }

        public static new SpriteLayer node()
        {
            SpriteLayer node = new SpriteLayer();
            return node;
        }
    }
}
