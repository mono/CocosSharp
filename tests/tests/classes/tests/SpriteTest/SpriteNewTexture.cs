using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests
{
    public class SpriteNewTexture : SpriteTestDemo
    {
        bool m_usingTexture1;
        CCTexture2D m_texture1;
        CCTexture2D m_texture2;

        public SpriteNewTexture()
        {
			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();
			touchListener.OnTouchesEnded = onTouchesEnded;

			EventDispatcher.AddEventListener(touchListener, this);


            CCNode node = new CCNode ();
            AddChild(node, 0, (int)kTags.kTagSpriteBatchNode);

            m_texture1 = CCTextureCache.Instance.AddImage("Images/grossini_dance_atlas");
            m_texture2 = CCTextureCache.Instance.AddImage("Images/grossini_dance_atlas-mono");

            m_usingTexture1 = true;

            for (int i = 0; i < 30; i++)
                addNewSprite();
        }

        public void addNewSprite()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCPoint p = new CCPoint((float)(CCRandom.NextDouble() * s.Width), (float)(CCRandom.NextDouble() * s.Height));

            int idx = (int)(CCRandom.NextDouble() * 1400 / 100);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;


            CCNode node = GetChildByTag((int)kTags.kTagSpriteBatchNode);
            CCSprite sprite = new CCSprite(m_texture1, new CCRect(x, y, 85, 121));
            node.AddChild(sprite);

            sprite.Position = (new CCPoint(p.X, p.Y));

            CCActionInterval action;
            float random = (float)CCRandom.NextDouble();

            if (random < 0.20)
                action = new CCScaleBy(3, 2);
            else if (random < 0.40)
                action = new CCRotateBy (3, 360);
            else if (random < 0.60)
                action = new CCBlink (1, 3);
            else if (random < 0.8)
                action = new CCTintBy (2, 0, -255, -255);
            else
                action = new CCFadeOut  (2);

            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCActionInterval seq = (CCActionInterval)(new CCSequence(action, action_back));

            sprite.RunAction(new CCRepeatForever (seq));
        }

		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
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
