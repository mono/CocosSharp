using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    public class SpriteNewTexture : SpriteTestDemo
    {
        bool m_usingTexture1;
        CCTexture2D m_texture1;
        CCTexture2D m_texture2;

        public SpriteNewTexture()
        {
            base.TouchEnabled = true;

            CCNode node = CCNode.Create();
            AddChild(node, 0, (int)kTags.kTagSpriteBatchNode);

            m_texture1 = CCTextureCache.SharedTextureCache.AddImage("Images/grossini_dance_atlas");
            m_texture2 = CCTextureCache.SharedTextureCache.AddImage("Images/grossini_dance_atlas-mono");

            m_usingTexture1 = true;

            for (int i = 0; i < 30; i++)
                addNewSprite();
        }

        public void addNewSprite()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCPoint p = new CCPoint((float)(Random.NextDouble() * s.Width), (float)(Random.NextDouble() * s.Height));

            int idx = (int)(Random.NextDouble() * 1400 / 100);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;


            CCNode node = GetChildByTag((int)kTags.kTagSpriteBatchNode);
            CCSprite sprite = CCSprite.Create(m_texture1, new CCRect(x, y, 85, 121));
            node.AddChild(sprite);

            sprite.Position = (new CCPoint(p.x, p.y));

            CCActionInterval action;
            float random = (float)Random.NextDouble();

            if (random < 0.20)
                action = CCScaleBy.Create(3, 2);
            else if (random < 0.40)
                action = CCRotateBy.Create(3, 360);
            else if (random < 0.60)
                action = CCBlink.Create(1, 3);
            else if (random < 0.8)
                action = CCTintBy.Create(2, 0, -255, -255);
            else
                action = CCFadeOut.Create(2);

            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCActionInterval seq = (CCActionInterval)(CCSequence.Create(action, action_back));

            sprite.RunAction(CCRepeatForever.Create(seq));
        }

        public override void TouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
            base.TouchesEnded(touches, event_);
            CCNode node = GetChildByTag((int)kTags.kTagSpriteBatchNode);

            var children = node.Children;
            CCSprite sprite;
            if (m_usingTexture1)                          //--> win32 : Let's it make just simple sentence
            {
                foreach (var item in children)
                {
                    sprite = (CCSprite)item;
                    if (sprite == null)
                        break;

                    sprite.Texture = m_texture2;
                }

                m_usingTexture1 = false;
            }
            else
            {
                foreach (var item in children)
                {
                    sprite = (CCSprite)item;
                    if (sprite == null)
                        break;

                    sprite.Texture = m_texture1;
                }

                m_usingTexture1 = true;
            }
        }

        public override string title()
        {
            return "Sprite New texture (tap)";
        }
    }
}
