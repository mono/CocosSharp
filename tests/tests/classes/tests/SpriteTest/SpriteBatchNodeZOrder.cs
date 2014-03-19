using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{
    public class SpriteBatchNodeZOrder : SpriteTestDemo
    {
        int m_dir;
        public SpriteBatchNodeZOrder()
        {
            m_dir = 1;

            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 1);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.SharedDirector.WinSize;

            float step = s.Width / 11;
            for (int i = 0; i < 5; i++)
            {
                CCSprite sprite = new CCSprite(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
                sprite.Position = (new CCPoint((i + 1) * step, s.Height / 2));
                batch.AddChild(sprite, i);
            }

            for (int i = 5; i < 10; i++)
            {
                CCSprite sprite = new CCSprite(batch.Texture, new CCRect(85 * 1, 121 * 0, 85, 121));
                sprite.Position = new CCPoint((i + 1) * step, s.Height / 2);
                batch.AddChild(sprite, 14 - i);
            }

            CCSprite sprite1 = new CCSprite(batch.Texture, new CCRect(85 * 3, 121 * 0, 85, 121));
            batch.AddChild(sprite1, -1, (int)kTagSprite.kTagSprite1);
            sprite1.Position = (new CCPoint(s.Width / 2, s.Height / 2 - 20));
            sprite1.Scale = 6;
            sprite1.Color = CCColor3B.Red;

            Schedule(reorderSprite, 1);
        }

        public void reorderSprite(float dt)
        {
            CCSpriteBatchNode batch = (CCSpriteBatchNode)(GetChildByTag((int)kTags.kTagSpriteBatchNode));
            CCSprite sprite = (CCSprite)(batch.GetChildByTag((int)kTagSprite.kTagSprite1));

            int z = sprite.ZOrder;

            if (z < -1)
                m_dir = 1;
            if (z > 10)
                m_dir = -1;

            z += m_dir * 3;

            batch.ReorderChild(sprite, z);
        }

        public override string title()
        {
            return "SpriteBatchNode: Z order";
        }
    }
}
