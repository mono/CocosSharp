using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Xna.Framework;

namespace tests
{
    public class SpriteZOrder : SpriteTestDemo
    {
        int m_dir;
        public SpriteZOrder()
        {
            m_dir = 1;

            CCSize s = CCDirector.SharedDirector.WinSize;

            float step = s.Width / 11;
            for (int i = 0; i < 5; i++)
            {
                CCSprite sprite = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
                sprite.Position = (new CCPoint((i + 1) * step, s.Height / 2));
                AddChild(sprite, i);
            }

            for (int i = 5; i < 10; i++)
            {
                CCSprite sprite = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 0, 85, 121));
                sprite.Position = new CCPoint((i + 1) * step, s.Height / 2);
                AddChild(sprite, 14 - i);
            }

            CCSprite sprite1 = CCSprite.Create("Images/grossini_dance_atlas", new CCRect(85 * 3, 121 * 0, 85, 121));
            AddChild(sprite1, -1, (int)kTagSprite.kTagSprite1);
            sprite1.Position = (new CCPoint(s.Width / 2, s.Height / 2 - 20));
            sprite1.Scale = 6;
            sprite1.Color = new ccColor3B(Color.Red);

            Schedule(reorderSprite, 1);
        }

        public void reorderSprite(float dt)
        {
            CCSprite sprite = (CCSprite)(GetChildByTag((int)kTagSprite.kTagSprite1));

            int z = sprite.ZOrder;

            if (z < -1)
                m_dir = 1;
            if (z > 10)
                m_dir = -1;

            z += m_dir * 3;

            ReorderChild(sprite, z);
        }

        public override string title()
        {
            return "Sprite: Z order";
        }
    }
}
